using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;             // Reference to the Rigidbody2D component
    public LayerMask groundLayer;       // LayerMask to identify ground objects

    private float speed = 5f, horizontal, JumpPower = 5f;   // Movement speed and jump power
    private bool isFacingRight = true;  // To track the player's facing direction

    public Vector2 boxSize;             // Size of the box for ground detection
    public float castDist;              // Distance for the boxcast

    public InputAction playerControls;  // Input action for player controls

    Vector2 moveDirection = Vector2.zero;   // Movement direction vector

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {        
        if (!isFacingRight && horizontal > 0f)      // Player is moving right but facing left
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)  // Player is moving left but facing right
        {
            Flip();
        }
    }

    private void FixedUpdate()  // Called at a fixed interval for physics updates
    {        
        rb.linearVelocity = new Vector2(moveDirection.x * speed, rb.linearVelocityY);   // Set horizontal velocity based on input

        if (isGrounded())
        {
            rb.gravityScale = 0f;   // Disable gravity when grounded (to stop sliding on slopes)
        }
        else
        {
            rb.gravityScale = 1f;   // Enable gravity when in the air
        }
    }
    
    public bool isGrounded()    // Check if the player is grounded using a boxcast
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDist, groundLayer))    // Perform boxcast to check for ground
        {
            return true;
        }else
            return false;
    }
    
    private void OnDrawGizmos() //to see the boxcast in the scene view
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position - transform.up * castDist, boxSize);
    }

    public void Jump(InputAction.CallbackContext context)   // Handle jump input
    {
        if (context.performed && isGrounded())
        {
            rb.AddForce(new Vector2(0f, JumpPower), ForceMode2D.Impulse);
        }/*
        else
        {
            Debug.Log("Jump not performed or player not grounded.");
        }
        */

        /* // if we want variable jump height (hold to jump higher)
        if (context.canceled && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
        */
    }

    private void Flip() // Flip the player's facing direction
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)   // Handle movement input
    {
        moveDirection = context.ReadValue<Vector2>();
        horizontal = moveDirection.x;
    }
}
