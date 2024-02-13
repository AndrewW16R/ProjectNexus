using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    //left/right input
    private float dirX = 0;

    [SerializeField]private float movementSpeed = 6f;
    [SerializeField]private float jumpStrength =14f;
    private enum MovementState { idle, running, jumping, falling }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Walking movement
         dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * movementSpeed, rb.velocity.y);

        //jumping
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        }

        //Updates sprite animation
        UpdateAnimationUpdate();
       
    }

    private void UpdateAnimationUpdate()
    {
        //Local variable to hold the animation state
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
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
        }

        //Sets value for state parameter
        anim.SetInteger("state", (int)state);
    }

}
