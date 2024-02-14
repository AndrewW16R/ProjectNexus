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
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashEndSpeed;
    [SerializeField] private float dashEndTime;


    //left/right input
    private float dirX = 0;


    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float jumpStrength = 14f;
    [SerializeField] private float doubleJumpStrength = 10f;

    //The direction tha player is facing. -1 = facing left  1 = facing right
    private float facingDirection;

    //The direction the player is holding -1 = facing left  1 = facing right
    private float heldDirection;
    private enum MovementState { idle, running, jumping, falling }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        initialGravity = rb.gravityScale;
        maxJumps = jumpsAvailable;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Walking movement
        dirX = Input.GetAxisRaw("Horizontal");
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
        

        
        if (Input.GetButtonDown("Fire3") && isDashing == false)
        {
            if (dirX > 0)
            {
                heldDirection = 1;
            }
            else if (dirX < 0)
            {
                heldDirection = -1;
            }
            StartCoroutine(Dash(heldDirection));
        }
        



    }

    private void Update()
    {
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

    private void UpdateAnimationUpdate()
    {
        //Local variable to hold the animation state
        MovementState state;

        if (dirX > 0f && isDashing == false)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f && isDashing == false)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
            if (fastFalling == false)
            {
                rb.gravityScale = rb.gravityScale * fastFallGravMultiplier;
                fastFalling = true;
            }
        }

        //Sets value for state parameter
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }


    /*
    private IEnumerator StartDash(Vector2 dir)
    {
        float dashStartTime = Time.time;

        dashesAvailable = dashesAvailable - 1;
        isDashing = true;


        rb.gravityScale = 0;

        while (Time.time - dashStartTime <= dashDuration)
        {
            // rb.velocity = dir.normalized * dashSpeed;
            rb.AddForce(new Vector2(dashSpeed * 1, 0f), ForceMode2D.Impulse);

            yield return null;
        }

        dashStartTime = Time.time;

        isDashing = false;

        rb.gravityScale = initialGravity;

        rb.velocity = movementSpeed * dir.normalized;

        while (Time.time - dashStartTime <= dashEndTime )
        {
            yield return null;
        }

        isDashing = false;

    }
    */

    private IEnumerator Dash(float direction)
    {
        isDashing = true;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashSpeed * direction, 0f), ForceMode2D.Impulse);
        rb.gravityScale = 0;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.gravityScale = initialGravity;
    }
}
