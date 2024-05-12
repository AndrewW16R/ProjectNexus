using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyMaxHealth;
    public float enemyCurrentHealth;

    public bool inHitstun;
    public bool inKnockback;
    public bool inKnockdown;

    public float remainingHitstunTime;
    public float remainingKnockbackTime;
    public float remainingKnockdownTime;

    public float recievedKnockbackPower;


    Rigidbody2D rb;
    EnemyAI enemyAI;

    // Start is called before the first frame update
    void Start()
    {
        if (enemyMaxHealth == 0)
        {
            enemyMaxHealth = 100;
        }
        if (enemyCurrentHealth == 0)
        {
            enemyCurrentHealth = enemyMaxHealth;
        }

        inHitstun = false;
        inKnockdown = false;

        rb = GetComponent<Rigidbody2D>();
        enemyAI = GetComponent<EnemyAI>();
    }

    void Update()
    {
        UpdateHitstun();
        UpdateKnockback();
        UpdateKnockdown();
    }


    public void TakeDamage(float damageRecieved)
    {
        enemyCurrentHealth = enemyCurrentHealth - damageRecieved;

        //Check if health is at or below zero here
    }

    public void HealEnemy(float healAmount)
    {
        if (healAmount > (enemyMaxHealth - enemyCurrentHealth)) //If adding the heal amount to the enemy's current healt would cause overheal, the enemy's health will be set to their maximum possible health
        {
            enemyCurrentHealth = enemyMaxHealth;
        }
        else
        {
            enemyCurrentHealth = enemyCurrentHealth + healAmount;
        }
    }

    public void ApplyHitstun(float hitstunDuration)
    {
        inHitstun = true; //Is set to be in hitstun
        remainingHitstunTime = hitstunDuration;
    }

    public void ApplyKnockback(float knockbackPower, float knockbackDuration)
    {
        recievedKnockbackPower = knockbackPower;
        remainingKnockbackTime = knockbackDuration;
        inKnockback = true;
    }

    public void ApplyKnockdown(float knockdownDuration)
    {
        remainingKnockdownTime = knockdownDuration;

        inKnockdown = true;
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
            enemyAI.preventMovement = false;
            //MAKE SURE TO INCLUDE ATTACKING / BLOCKING WHEN YOU GET TO IT
        }

        if (inHitstun == true)
        {
            enemyAI.preventMovement = true;
            //Stop Enemy Ai script from making the enemy move towards/pathfind to player and doing any other actions such as attacking or blocking
            //MAKE SURE TO INCLUDE ATTACKING / BLOCKING WHEN YOU GET TO IT
        }
    }

    public void UpdateKnockback()
    {
        if (inKnockback == true && remainingKnockbackTime > 0)
        {
            ApplyKnockbackForce(recievedKnockbackPower); //funtion added to this script which applies the knockback force
            remainingKnockbackTime = remainingKnockbackTime - 1;
        }
        else
        {
            inKnockback = false;
        }
    }

    public void UpdateKnockdown()
    {
        if (inKnockdown == true && remainingKnockdownTime > 0 ) //Knockdown duration will not count down unless player is grounded
        {

            remainingKnockdownTime = remainingKnockdownTime - 1; //knockdown duration counts down
            enemyAI.preventMovement = true;
            //Stop Enemy Ai script from making the enemy move towards/pathfind to player and doing any other actions such as attacking or blocking
            //MAKE SURE TO INCLUDE ATTACKING / BLOCKING WHEN YOU GET TO IT
        }
        else if (inKnockdown == true) //No longer in knowdown on remainingKnockdowntime runs out.
        {
            inKnockdown = false;
            enemyAI.preventMovement = false;
            //Allow enemy AI to to be actionable again, MAKE SURE TO INCLUDE ATTACKING/BLOCKING WHEN YOU GET TO IT
        }
    }

    public void ApplyKnockbackForce(float horizontalKnockback)
    {
        //if (playerFacingDirection == 1) //needs to be coded in once I can figure out how to pass in Player gameobject data through AttackCollision script for when the enemy gets hit
        {
            rb.AddForce(transform.right * horizontalKnockback);
            
        }
       // else if (playerFacingDirection == -1)
        {
            rb.AddForce(transform.right * (-1 * horizontalKnockback));
        }
    }


}
