using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool isAttacking;
    public bool attackDurationUpdateQued;
    private string currentAttackName; //this variable is not currently utilized but could be implemented to indicate which attack is being used
    [SerializeField] private int currentAttackDuration;

    PlayerMovementV2 playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        //Gets playerMovementV2 script
        playerMovement = gameObject.GetComponent<PlayerMovementV2>();
        attackDurationUpdateQued = false;
        isAttacking = false;
}

    // Update is called once per frame
    void Update()
    {
        UpdateAttack();
    }

    private void FixedUpdate()
    {
        UpdateAttackDuration();
    }

    private void UpdateAttack()
    {
        if (Input.GetButtonDown("Fire1") && playerMovement.isDashing == false && playerMovement.IsGrounded() && isAttacking == false) //This is currently exclusive coded for L_Grounded_Neutral for testing purposes
        {
            isAttacking = true;
            currentAttackDuration = 9;
            currentAttackName = "L_Grounded_Neutral";
        }
        else if (currentAttackDuration > 0) //if the attack animation is currently playing, wait subtract one from its duration
        {
            attackDurationUpdateQued = true;
    
        }
        else
        {
            isAttacking = false;
            currentAttackName = "";
        }

       
    }
    private void UpdateAttackDuration()
    {
        if (attackDurationUpdateQued == true)
        {
            currentAttackDuration = currentAttackDuration - 1;
            attackDurationUpdateQued = false;
        }
        
    }

}
