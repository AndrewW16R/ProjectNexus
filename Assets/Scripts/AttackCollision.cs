using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    public float AttackDamage;

    GameObject otherObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.tag == "PlayerHitBox" && other.tag == "EnemyHurtBox") //if the player hits an enemy with an attack
        {
            //Enemy takes damage
            EnemyTakeDamage(other.gameObject);
        }
        else if (gameObject.tag == "EnemyHitBox" && other.tag == "PlayerHurtBox") //If an enemy hits the player with an attack
        {
            //Player takes damage
            PlayerTakeDamage(other.gameObject); //passes in with gameobject that the player hurtbox is connected to
        }
        else return;
    }


    void EnemyTakeDamage(GameObject other)
    {
        otherObject = other.transform.parent.gameObject;
        Debug.Log("Enemy Takes Damage");
        Debug.Log(otherObject);
    }

    void PlayerTakeDamage(GameObject other)
    {
        otherObject = other.transform.parent.gameObject; //Gameobject other is set to the parent gameobject of the player hurtbox (the player gameobject at the top of its hierarchy)
        Debug.Log("Player Takes Damage");
        Debug.Log(otherObject);
    }
}
