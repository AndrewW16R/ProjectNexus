using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Animator anim;
    private PlayerMovementV2 playerMovement;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;
    private string currentAnim;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerMovement = gameObject.GetComponent<PlayerMovementV2>();
        playerAttack = gameObject.GetComponent<PlayerAttack>();
        playerHealth = gameObject.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationUpdate();
    }

    public void UpdateAnimationUpdate()
    {
        //Local variable to hold the animation state
        //MovementState state;
        if(playerMovement.IsGrounded() && playerMovement.isBlocking == true)
        {
            SetAnimationState("Nexus_Block");
            if (playerMovement.facingDirection == 1)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        } //Player is blocking
        else if(playerMovement.IsGrounded() && playerHealth.inHitstun == true && playerHealth.inKnockdown == false)
        {
            SetAnimationState("Nexus_HitStandard");
            if (playerMovement.facingDirection == 1)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }
        else if (playerMovement.IsGrounded() && playerHealth.inKnockdown == true)
        {
            SetAnimationState("Nexus_Knockdown");
            if (playerMovement.facingDirection == 1)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }
        else if (playerMovement.dirX == 0 && playerMovement.isDashing == false && playerMovement.IsGrounded() && playerAttack.isAttacking == true && playerMovement.isBlocking == false) //Anim Attack L Grounded Neutral
        {
            SetAnimationState("Nexus_Attack_L_Grounded_Neutral");
            if (playerMovement.facingDirection == 1)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }
        else if (playerMovement.dirX >= 0.5f && playerMovement.isDashing == false && playerMovement.IsGrounded() && playerAttack.isAttacking == false && playerMovement.isBlocking == false) //Anim Running Right
        {
            //state = MovementState.running;
            SetAnimationState("Nexus_Running");
            sprite.flipX = false;
        }
        else if (playerMovement.dirX <= -0.5f && playerMovement.isDashing == false && playerMovement.IsGrounded() && playerAttack.isAttacking == false && playerMovement.isBlocking == false) //Anim Running Left
        {
            //state = MovementState.running;
            SetAnimationState("Nexus_Running");
            sprite.flipX = true;
        }
        else if (playerMovement.dirX < 0.5f && playerMovement.dirX > 0f && playerMovement.isDashing == false && playerMovement.IsGrounded() && playerAttack.isAttacking == false && playerMovement.isBlocking == false) //Anim Walking Right
        {
            SetAnimationState("Nexus_Walking");
            sprite.flipX = false;
        }
        else if (playerMovement.dirX > -0.5f && playerMovement.dirX < 0f && playerMovement.isDashing == false && playerMovement.IsGrounded() && playerAttack.isAttacking == false && playerMovement.isBlocking == false) //Anim Walking Left
        {
            SetAnimationState("Nexus_Walking");
            sprite.flipX = true;
        }
        else if (playerMovement.dirX == 0f && playerMovement.isDashing == false && playerMovement.IsGrounded() && playerAttack.isAttacking == false && playerMovement.isBlocking == false) //Anim Idle
        {
            SetAnimationState("Nexus_Idle");
            if (playerMovement.facingDirection == 1)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }

        }
        else if (playerMovement.isDashing == true && playerMovement.IsGrounded() && playerAttack.isAttacking == false && playerMovement.isBlocking == false) //Anim Ground Dash
        {
            SetAnimationState("Nexus_GroundDash");
            if (playerMovement.facingDirection == 1)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }
        else
        {
            //state = MovementState.idle;
            SetAnimationState(currentAnim);
            //sprite.flipX = false;
        }

        if (playerMovement.rb.velocity.y > .1f && playerMovement.isDashing == false && !playerMovement.IsGrounded() && playerMovement.jumpsAvailable == playerMovement.maxJumps - 1) //First jump
        {
            if (currentAnim != "Nexus_Jumping")
            {
                SetAnimationState("Nexus_Jumping");
                if (playerMovement.facingDirection == 1)
                {
                    sprite.flipX = false;
                }
                else
                {
                    sprite.flipX = true;
                }
            }

        }
        else if (playerMovement.rb.velocity.y > .1f && playerMovement.isDashing == false && !playerMovement.IsGrounded() && playerMovement.jumpsAvailable < playerMovement.maxJumps - 1)
        {
            if (currentAnim != "Nexus_DoubleJump")
            {
                SetAnimationState("Nexus_DoubleJump");
                if (playerMovement.facingDirection == 1)
                {
                    sprite.flipX = false;
                }
                else
                {
                    sprite.flipX = true;
                }
            }

        }
        else if (playerMovement.rb.velocity.y < -.1f && playerMovement.isDashing == false && !playerMovement.IsGrounded())
        {

            SetAnimationState("Nexus_Falling");
            if (playerMovement.facingDirection == 1)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }

        }
        else if (playerMovement.isDashing == true && !playerMovement.IsGrounded())
        {

            if (playerMovement.facingDirection == 1) //facing right
            {
                if (playerMovement.airDashDir == 1) //dashing right
                {
                    SetAnimationState("Nexus_AirDashForward");
                    sprite.flipX = false;
                }
                else // dashing left
                {
                    SetAnimationState("Nexus_AirDashBackward");
                    sprite.flipX = false;
                }
            }
            else //facing left
            {
                if (playerMovement.airDashDir == -1) //dashing left
                {
                    SetAnimationState("Nexus_AirDashForward");
                    sprite.flipX = true;
                }
                else // dashing right
                {
                    SetAnimationState("Nexus_AirDashBackward");
                    sprite.flipX = true;
                }
            }
        }

    }

    void SetAnimationState(string newState)
    {
        if (newState == currentAnim)
        {
            return;
        }
        else
        {
            currentAnim = newState;
            anim.Play(newState);
        }

    }
}
