using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    [HideInInspector]public Rigidbody2D rb;
    private BoxCollider2D coll;

    //How many jumps the player can do before needing to be grounded again
    [SerializeField] public int jumpsAvailable = 2;
    //is assigned the number of jumps available from frame 1, used to determine how many jumps the player gets when jumps refrsh
    public int maxJumps;
    // Has the player used their first jump
    private bool initialJumpUsed = false;

    private float initialGravity;
    public bool fastFalling = false;
    [SerializeField] public float fastFallGravMultiplier = 1.5f;

    [SerializeField] private LayerMask jumpableGround;

    [HideInInspector] public bool isDashing = false;
    [SerializeField] public int dashesAvailable;
    private int maxDashes;
    private bool dashRefilling = false;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashSpeed;
    [SerializeField] private int timeToRefillOneDash = 1;
    public float airDashDir; //Stores value of which driection player is air dashing to inform animator which air dash animation to play


    //left/right input
    [HideInInspector]public float dirX = 0;


    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float jumpStrength = 14f;
    [SerializeField] private float doubleJumpStrength = 10f;

    //The direction tha player is facing. -1 = facing left  1 = facing right
    [SerializeField] public float facingDirection = 1;

    //The direction the player is holding -1 = facing left  1 = facing right
    [SerializeField] private float heldDirection;
    private enum MovementState { idle, walking, running, jumping, doubleJump, falling, dashGrounded, dashAirForward, dashAirBackward }

    [HideInInspector] public PlayerAttack playerAttack;
  

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        playerAttack = gameObject.GetComponent<PlayerAttack>();

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

    }

    private void Update()
    {
        //Recieves horizontal input
        dirX = Input.GetAxisRaw("Horizontal");

        //Checks for dash input and executes dash if under proper conditions
        UpdateDash();
        //Checks for jump input and executes jump in under proper conditions
        UpdateJump();
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

        if (rb.velocity.y < -.1f && isDashing == false && !IsGrounded() && fastFalling == false)
        {
            rb.gravityScale = rb.gravityScale * fastFallGravMultiplier;
            fastFalling = true;
        }
    }

    private void UpdateDash()
    {
        if (Input.GetButtonDown("Fire3") && isDashing == false && dashesAvailable > 0)
        {
            if (IsGrounded())
            {
                if(heldDirection != facingDirection)
                {
                    StartCoroutine(Dash(facingDirection));
                }
                else
                {
                   StartCoroutine(Dash(heldDirection));
                }
                
            }
            else
            {
                if (heldDirection != facingDirection && dirX == 0)
                {
                    airDashDir = facingDirection;
                    StartCoroutine(Dash(facingDirection));
                }
                else
                {
                    airDashDir = heldDirection;
                    StartCoroutine(Dash(heldDirection));
                }
                
            }
        }

    }

    //Checks if player is standing on a navigable ground tile
    public bool IsGrounded()
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

}
