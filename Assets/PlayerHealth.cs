using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public bool inHitstun;
    public float remainingHitstunTime;
    public float remainingKnockdownTime;

    public bool inKnockdown;
    public bool inKnockback;

    PlayerMovementV2 playerMovement;
    PlayerAnimation playerAnimation;
    [HideInInspector] public PlayerAttack playerAttack;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovementV2>();
        playerAnimation = gameObject.GetComponent<PlayerAnimation>();
        playerAttack = gameObject.GetComponent<PlayerAttack>();

        if (maxHealth == 0)
        {
            maxHealth = 100;
        }
        if (currentHealth == 0)
        {
            currentHealth = maxHealth;
        }
        inHitstun = false;
        inKnockdown = false;
    }

    // Update is called once per frame
    void Update()
    {
        HitAnimTest();
    }


    public void TakeDamage(float damageRecieved)
    {
        currentHealth = currentHealth - damageRecieved;

        //Check if health is at or below zero here
    }

    public void HealPlayer(float healAmount)
    {
        if (healAmount > (maxHealth - currentHealth)) //If adding the heal amount to the player's current healt would cause overheal, the player's health will be set to their maximum possible health
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = currentHealth + healAmount;
        }
    }

    public void HitAnimTest()
    { 
        if(Input.GetButtonDown("Fire2") && playerMovement.isBlocking == false) //when top face button is pressed, simulates getting hit by attack
        {
            ApplyHitstun(20); //Value to change depending on what attack player is hit by
            ApplyKnockback(100, 5);
        }

        if (Input.GetButtonDown("Fire3") && playerMovement.isBlocking == false && playerMovement.IsGrounded()) //when right face button is pressed, simulates getting hit by attack which causes knockdown
        {
            ApplyHitstun(60); //technically not needed as long as code for knockdown also applies invincibility to player
            ApplyKnockdown(60); //Value to change depending on what attack player is hit by, value is duration of knockdown in frames
            ApplyKnockback(100, 10); //Value to change depending on what attack player is hit by, value is duration of knockback in frames
        }

        UpdateHitstun();
        UpdateKnockdown();
        UpdateKnockback();

    }

    public void ApplyHitstun(float hitstunDuration)
    {
        inHitstun = true; //Is set to be in hitstun
        remainingHitstunTime = hitstunDuration;
    }

    public void ApplyKnockdown(float knockdownDuration)
    {
        remainingKnockdownTime = knockdownDuration;
        
        inKnockdown = true;
    }

    public void ApplyKnockback(float knockbackPower, float knockbackDuration)
    {
        playerMovement.hitKnockbackDuration = knockbackDuration; //sets knockback duration value in movement script
        playerMovement.hitKnockbackForce = knockbackPower; //The exact value which would be passed in should vary depending what the player is getting hit by
        inKnockback = true;
    }

    public void UpdateHitstun()
    {
        if (inHitstun == true && remainingHitstunTime > 0)
        {
            remainingHitstunTime = remainingHitstunTime - 1;//lowers hitstun duration
        }
        else //if hitstunDuration runs out, no longer in hitstun
        {
            inHitstun = false;
   
            
            if (playerAttack.isAttacking == false) //if not attacking and not in hitstun, movement and input preventions will be set to false
            {
                if (playerAttack.stopHorizontalInput == true)
                {
                    Debug.Log("setting horz input ok");
                    playerAttack.UpdateHorizontalInputPrevention(false);
                }

                if (playerAttack.stopHorizontalVel == true)
                {
                    playerAttack.UpdateHorizontalVelocityPrevention(false);
                }

                if (playerAttack.stopDashing == true)
                {
                    playerAttack.UpdateDashingPrevention(false);
                }

                if (playerAttack.stopJumpInput == true)
                {
                    playerAttack.UpdateJumpInputPrevention(false);
                }
            }
        }

        if(inHitstun == true && playerMovement.IsGrounded())
        {
            playerAttack.UpdateHorizontalVelocityPrevention(true);
            playerAttack.UpdateHorizontalInputPrevention(true);
            playerAttack.UpdateDashingPrevention(true);
            playerAttack.UpdateJumpInputPrevention(true);
        }
    }

    public void UpdateKnockdown()
    {
        if(inKnockdown == true && remainingKnockdownTime > 0 && playerMovement.IsGrounded()) //Knockdown duration will not count down unless player is grounded
        {
            remainingKnockdownTime = remainingKnockdownTime - 1; //knockdown duration counts down
            playerAttack.UpdateHorizontalVelocityPrevention(true);
            playerAttack.UpdateHorizontalInputPrevention(true);
            playerAttack.UpdateDashingPrevention(true);
            playerAttack.UpdateJumpInputPrevention(true);
        }
        else if (inKnockdown == true && playerMovement.IsGrounded()) //No longer in knowdown on remainingKnockdowntime runs out.
        {
            inKnockdown = false;
            playerAttack.UpdateHorizontalVelocityPrevention(false);
            playerAttack.UpdateHorizontalInputPrevention(false);
            playerAttack.UpdateDashingPrevention(false);
            playerAttack.UpdateJumpInputPrevention(false);
        }
    }

    public void UpdateKnockback()
    {
        if(inKnockback == true && playerMovement.hitKnockbackDuration > 0)
        {
            playerMovement.ApplyKnockbackForce(playerMovement.hitKnockbackForce);
            playerMovement.hitKnockbackDuration = playerMovement.hitKnockbackDuration - 1;
        }
        else
        {
            inKnockback = false;
        }
    }
}
