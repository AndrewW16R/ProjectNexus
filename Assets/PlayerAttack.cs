using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool isAttacking;
    public bool attackDurationUpdateQued;
    private string currentAttackName; //this variable is not currently utilized but could be implemented to indicate which attack is being used
    [SerializeField] private int currentAttackDuration;

    public GameObject hitboxGroup;
    public GameObject attackHitbox_LGN;
    public int prevAttackDir;

    PlayerMovementV2 playerMovement;
    PlayerAnimation playerAnimation;
    public Vector3 currentHitboxScale;

    // Start is called before the first frame update
    void Start()
    {
        //Gets playerMovementV2 script
        playerMovement = gameObject.GetComponent<PlayerMovementV2>();
        playerAnimation = gameObject.GetComponent<PlayerAnimation>();
        attackDurationUpdateQued = false;
        isAttacking = false;

        prevAttackDir = 0;
}

    // Update is called once per frame
    void Update()
    {
        UpdateAttack();
    }

    private void FixedUpdate()
    {
        UpdateAttackHitbox();
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
            UpdateHitboxDirection();
            attackDurationUpdateQued = true;
    
        }
        else
        {
            isAttacking = false;
            currentAttackName = "";
        }

       
    }

    private void UpdateAttackHitbox()
    {
        if (currentAttackName == "L_Grounded_Neutral")
        {
            if (currentAttackDuration <= 6 && currentAttackDuration >= 4)
            {
                
                //set hitbox active
                attackHitbox_LGN.gameObject.SetActive(true);
            }
            else
            {
                //set hitbox inactive
                attackHitbox_LGN.gameObject.SetActive(false);
            }
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

    private void UpdateHitboxDirection() //flips attack hitbox gameobjects depend on facingDirection, does not yet account for back air
    {
        if(playerMovement.facingDirection == 1 && prevAttackDir == 0)
        {
            prevAttackDir = 1;
        }
        else if(playerMovement.facingDirection == -1 && prevAttackDir == 0) //has not attacked before, is facing left
        {
            hitboxGroup.transform.localScale = new Vector3(-1, 1, 1);
            prevAttackDir = -1;
        }
        else if (playerMovement.facingDirection == 1 && prevAttackDir == -1) //is facing right, prev attack was facing left
        {
            hitboxGroup.transform.localScale = new Vector3(1, 1, 1);
            prevAttackDir = 1;
        }
        else if (playerMovement.facingDirection == -1 && prevAttackDir == 1) //is facing left, prev attack was facing right
        {
            hitboxGroup.transform.localScale = new Vector3(-1, 1, 1);
            prevAttackDir = -1;
        }
        Debug.Log(prevAttackDir);
        
    }

}
