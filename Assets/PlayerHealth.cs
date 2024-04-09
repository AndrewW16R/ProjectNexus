using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public bool inHitstun;
    public float remainingHitstun;

    PlayerMovementV2 playerMovement;
    PlayerAnimation playerAnimation;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovementV2>();
        playerAnimation = gameObject.GetComponent<PlayerAnimation>();

        if (maxHealth == null)
        {
            maxHealth = 100;
        }
        if (currentHealth == null)
        {
            currentHealth = maxHealth;
        }
        inHitstun = false;
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
            ApplyHitstun(10);
        }

        if (inHitstun == true && remainingHitstun > 0)
        {
            remainingHitstun = remainingHitstun - 1;
        }
        else //if hitstunDuration runs out, no longer in hitstun
        {
            inHitstun = false;
        }
    }

    public void ApplyHitstun(float hitstunDuration)
    {
        inHitstun = true; //Is set to be in hitstun
        remainingHitstun = hitstunDuration;

       
    }
}
