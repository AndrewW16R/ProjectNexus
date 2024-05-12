using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    public float attackDamage;
    public float attackHitstunDuration;
    public float attackKnockbackPower;
    public float attackKnockbackDuration;
    public bool appliesKnockdown;
    public float attackKnockdownDuration;

    GameObject otherObject;
    PlayerHealth playerState;
    PlayerMovementV2 playerMovement;

    EnemyHealth enemyState;

    private void OnTriggerEnter2D(Collider2D other) //in this context "other" refers to the collider which the script is on
    {
        if (gameObject.tag == "PlayerHitBox" && other.tag == "EnemyHurtBox") //if the player hits an enemy with an attack
        {
            //Enemy takes damage
            EnemyTakeDamage(other.gameObject);

            //How do I access the player gameobject in this situation where the enemy ("other" collider) is hit by the player?
        }
        else if (gameObject.tag == "EnemyHitBox" && other.tag == "PlayerHurtBox") //If an enemy hits the player with an attack
        {
            //Player takes damage
            PlayerTakeDamage(other.gameObject); //passes in with gameobject that the player hurtbox is connected to
            //How do I access the enemy gameobject in this situation where the player ("other" collider) is hit by the enemy?
        }
        else return;
    }


    void EnemyTakeDamage(GameObject other)
    {
        otherObject = other.transform.parent.gameObject;
        enemyState = otherObject.GetComponent<EnemyHealth>();

        enemyState.ApplyHitstun(attackHitstunDuration);
        enemyState.ApplyKnockback(attackKnockbackPower, attackKnockdownDuration);

        if (appliesKnockdown == true)
        {
            enemyState.ApplyKnockdown(attackKnockdownDuration);
        }

        enemyState.TakeDamage(attackDamage);

        Debug.Log("Enemy Takes Damage");
        Debug.Log(otherObject);
    }

    void PlayerTakeDamage(GameObject other)
    {
        otherObject = other.transform.parent.gameObject; //Gameobject other is set to the parent gameobject of the player hurtbox (the player gameobject at the top of its hierarchy)
        playerState = otherObject.GetComponent<PlayerHealth>();
        playerMovement = otherObject.GetComponent<PlayerMovementV2>();
        playerState.ApplyHitstun(attackHitstunDuration);
        playerState.ApplyKnockback(attackKnockbackPower, attackKnockdownDuration);

        if(appliesKnockdown == true)
        {
            playerState.ApplyKnockdown(attackKnockdownDuration);
        }

        playerState.TakeDamage(attackDamage);

        Debug.Log("Player Takes Damage");
        Debug.Log(otherObject);
    }
}
