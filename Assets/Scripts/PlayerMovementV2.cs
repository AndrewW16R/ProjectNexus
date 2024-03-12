using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    //How many jumps the player can do before needing to be grounded again
    [SerializeField] private int jumpsAvailable = 2;
    private int maxJumps;
    // Has the player used their first jump
    private bool initialJumpUsed = false;

    private float initialGravity;
    private bool fastFalling = false;
    [SerializeField] private float fastFallGravMultiplier = 1.5f;

    [SerializeField] private LayerMask jumpableGround;

    private bool isDashing = false;
    [SerializeField] private int dashesAvailable;
    private int maxDashes;
    private bool dashRefilling = false;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashSpeed;
    [SerializeField] private int timeToRefillOneDash = 1;


    //left/right input
    private float dirX = 0;


    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float jumpStrength = 14f;
    [SerializeField] private float doubleJumpStrength = 10f;

    //The direction tha player is facing. -1 = facing left  1 = facing right
    [SerializeField] private float facingDirection = 1;

    //The direction the player is holding -1 = facing left  1 = facing right
    [SerializeField] private float heldDirection;
    private enum MovementState { idle, walking, running, jumping, doubleJump, falling, dashGrounded, dashAirForward, dashAirBackward }
    private string currentAnim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        initialGravity = rb.gravityScale;
        maxJumps = jumpsAvailable;
        maxDashes = dashesAvailable;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Walking movement
        if (isDashing == false)
        {
            rb.velocity = new Vector2(dirX * movementSpeed, rb.velocity.y);

            if (dirX > 0 && IsGrounded())
            {
                facingDirection = 1;
            }
            else if (dirX < 0 && IsGrounded())
            {
                facingDirection = -1;
            }

        }

        //Checks for player's held so it can be referenced separately from the direction the player character is facing
        if (dirX > 0)
        {
            heldDirection = 1;
        }
        else if (dirX < 0)
        {
            heldDirection = -1;
        }

        //Refills player's available dashes
        if (dashesAvailable < maxDashes && dashRefilling == false)
        {
            StartCoroutine(RefillDash(timeToRefillOneDash));
        }

        //Debug.Log(currentAnim);

    }

    private void Update()
    {
        //Recieves horizontal input
        dirX = Input.GetAxisRaw("Horizontal");

        //Checks for dash input and executes dash if under proper conditions
        UpdateDash();
        //Checks for jump input and executes jump in under proper conditions
        UpdateJump();

        //Updates sprite animation
        UpdateAnimationUpdate();
    }

    private void UpdateJump()
    {
        //Checks if player is grounded
        if (IsGrounded())
        {
            rb.gravityScale = initialGravity;
            fastFalling = false;
            jumpsAvailable = maxJumps;
            initialJumpUsed = false;
        }

        //jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
            jumpsAvailable = jumpsAvailable - 1;
            initialJumpUsed = true;

        } //if player is already not grounded before the intial jump is used up, both the intial jump and the first addition jump are used up.
        else if (Input.GetButtonDown("Jump") && initialJumpUsed == false && jumpsAvailable > 0 && !IsGrounded())
        {
            rb.gravityScale = initialGravity;
            fastFalling = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpStrength);
            jumpsAvailable = jumpsAvailable - 2;
            initialJumpUsed = true;
        }
        else if (Input.GetButtonDown("Jump") && jumpsAvailable > 0 && !IsGrounded())
        {
            rb.gravityScale = initialGravity;
            fastFalling = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpStrength);
            jumpsAvailable = jumpsAvailable - 1;

        }
    }

    private void UpdateDash()
    {
        if (Input.GetButtonDown("Fire3") && isDashing == false && dashesAvailable > 0)
        {
            if (heldDirection != facingDirection && IsGrounded())
            {
                StartCoroutine(Dash(facingDirection));
            }
            else
            {
                StartCoroutine(Dash(heldDirection));
            }
        }

    }

    private void UpdateAnimationUpdate()
    {
        //Local variable to hold the animation state
        //MovementState state;

        if (dirX >= 0.5f && isDashing == false && IsGrounded())
        {
            //state = MovementState.running;
            SetAnimationState("Nexus_Running");
            sprite.flipX = false;
        }
        else if (dirX <= -0.5f && isDashing == false && IsGrounded())
        {
            //state = MovementState.running;
            SetAnimationState("Nexus_Running");
            sprite.flipX = true;
        }
        else if (dirX < 0.5f && dirX > 0f && isDashing == false && IsGrounded())
        {
            SetAnimationState("Nexus_Walking");
            sprite.flipX = false;
        }
        else if (dirX > -0.5f && dirX < 0f && isDashing == false && IsGrounded())
        {
            SetAnimationState("Nexus_Walking");
            sprite.flipX = true;
        }
        else if (dirX == 0f && isDashing == false && IsGrounded())
        {
            SetAnimationState("Nexus_Idle");
            if(facingDirection == 1)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
            
        }
        else if( isDashing == true && IsGrounded())
        {
            SetAnimationState("Nexus_GroundDash");
            if (facingDirection == 1)
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

        if (rb.velocity.y > .1f && isDashing == false && !IsGrounded() && jumpsAvailable == maxJumps -1) //First jump
        {
            if(currentAnim != "Nexus_Jumping")
            {
                SetAnimationState("Nexus_Jumping");
                if (facingDirection == 1)
                {
                    sprite.flipX = false;
                }
                else
                {
                    sprite.flipX = true;
                }
            }
            
        }
        else if (rb.velocity.y > .1f && isDashing == false && !IsGrounded() && jumpsAvailable < maxJumps - 1)
        {
            if (currentAnim != "Nexus_DoubleJump")
            {
                SetAnimationState("Nexus_DoubleJump");
                if (facingDirection == 1)
                {
                    sprite.flipX = false;
                }
                else
                {
                    sprite.flipX = true;
                }
            }

        }
        else if (rb.velocity.y < -.1f && isDashing == false && !IsGrounded())
        {
            //state = MovementState.falling;
            SetAnimationState("Nexus_Falling");
            if (facingDirection == 1)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }

            if (fastFalling == false)
            {
                rb.gravityScale = rb.gravityScale * fastFallGravMultiplier;
                fastFalling = true;
            }
        }
        else if (isDashing == true && !IsGrounded())
        {
          if(facingDirection == 1) //facing right
            {
                if (heldDirection == 1) //holding right
                {
                    SetAnimationState("Nexus_AirDashForward");
                    sprite.flipX = false;
                }
                else // holding left
                {
                    SetAnimationState("Nexus_AirDashBackward");
                    sprite.flipX = false;
                }
            }
          else //facing left
            {
                if (heldDirection == -1) //holding left
                {
                    SetAnimationState("Nexus_AirDashForward");
                    sprite.flipX = true;
                }
                else // holding right
                {
                    SetAnimationState("Nexus_AirDashBackward");
                    sprite.flipX = true;
                }
            }
        }


            //Sets value for state parameter
            // anim.SetInteger("state", (int)state);

    }

    //Checks if player is standing on a navigable ground tile
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }



    private IEnumerator Dash(float direction)
    {
        isDashing = true;
        //Debug.Log("dash detected");
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashSpeed * direction, 0f), ForceMode2D.Impulse);
       //below if statement implemented since above Added dash speed to move speed. It felt pretty good though
       
        /*
        if (rb.velocity.x != dashSpeed * direction)
        {
            rb.velocity = new Vector2(dashSpeed * direction, 0f);
        }
        */
        
        rb.gravityScale = 0;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.gravityScale = initialGravity;
        dashesAvailable = dashesAvailable - 1;
        
    }

    //Refills players available dashes
    private IEnumerator RefillDash(int amount)
    {
        dashRefilling = true;
        yield return new WaitForSeconds(timeToRefillOneDash);
        dashRefilling = false;
        dashesAvailable = dashesAvailable + 1;
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
           // Debug.Log(currentAnim);
        }
        
    }
}
