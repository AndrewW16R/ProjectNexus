using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public bool inHitstun;
    public float remainingHitstun;

    public bool inKnockdown;

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

    public void HitAnimTest()
    { 
        if(Input.GetButtonDown("Fire2") && playerMovement.isBlocking == false) //when top face button is pressed, simulates getting hit by attack
        {
            ApplyHitstun(60);
        }

        if (Input.GetButtonDown("Fire3") && playerMovement.isBlocking == false) //when top face button is pressed, simulates getting hit by attack
        {
            ApplyHitstun(90);
            applyKnockdown();
        }

        UpdateHitstun();
        
    }

    public void ApplyHitstun(float hitstunDuration)
    {
        inHitstun = true; //Is set to be in hitstun
        remainingHitstun = hitstunDuration;
    }

    public void applyKnockdown()
    {
        inKnockdown = true;
    }

    public void UpdateHitstun()
    {
        if (inHitstun == true && remainingHitstun > 0)
        {
            remainingHitstun = remainingHitstun - 1;//lowers hitstun duration
        }
        else //if hitstunDuration runs out, no longer in hitstun
        {
            inHitstun = false;
            
            if(inKnockdown == true)
            {
                inKnockdown = false;
            }
            
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
}
