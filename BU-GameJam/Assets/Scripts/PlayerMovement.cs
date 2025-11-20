using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;             // Reference to the Rigidbody2D component
    public LayerMask groundLayer;       // LayerMask to identify ground objects
    
    public float speed = 5f, JumpPower = 10f, gravity = 2f;

    [SerializeField] private Animator animator;

    public float horizontal;    // Movement speed and jump power
    public bool isFacingRight = true;  // To track the player's facing direction

    public Vector2 boxSize;             // Size of the box for ground detection
    public float castDist;              // Distance for the boxcast

    public InputAction playerControls;  // Input action for player controls

    Vector2 moveDirection = Vector2.zero;   // Movement direction vector

    private Transform currentPlatform;    // Reference to the current platform the player is on
    private Vector3 lastPlatformPosition; // Last position of the current platform
    private Vector2 currentPlatformVelocity = Vector2.zero; // Velocity of the current platform

    private const float fixedInterval = 0.01f;

    // Timer to track elapsed time since the last action



    public bool cantGetHurt = false;
    public float cantGetHurtTimer = 0f;
    //private bool isOnPlatform = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;
    }

    private void Update()
    {
        if (!isFacingRight && horizontal > 0f) Flip();
        else if(isFacingRight && horizontal <0f) Flip();

        animator.SetBool("isGrounded", isGrounded());
    }

    private void FixedUpdate()  // Called at a fixed interval for physics updates
    {        


        if(currentPlatform != null) // If on a moving platform, calculate its velocity
        {
            Vector3 platformPos = currentPlatform.position;
            currentPlatformVelocity = ((Vector2)(platformPos - lastPlatformPosition)) / Time.fixedDeltaTime;
            lastPlatformPosition = platformPos;
        }
        else
        {
            currentPlatformVelocity = Vector2.zero;
        }

        float combinedHorizontal = moveDirection.x *speed + currentPlatformVelocity.x; // Combine player input with platform velocity
        float combinedVertical = rb.linearVelocity.y + (isGrounded() ? currentPlatformVelocity.y : 0f); // Combine vertical velocity with platform velocity

        rb.linearVelocity = new Vector2(combinedHorizontal, combinedVertical);   // Set horizontal velocity based on input
                                                                                 // 
        cantGetHurtTimer -= Time.fixedDeltaTime;
        if (cantGetHurtTimer <= 0) cantGetHurt = false;

    }
    
    public bool isGrounded()    // Check if the player is grounded using a boxcast
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0f, -transform.up, castDist, groundLayer);        
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
            AudioManager.Instance.Play(AudioManager.SoundType.Jump);
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        }
    }

    public void Flip() // Flip the player's facing direction
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
        if (horizontal != 0) animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);
        
    }

    // Platform detection and tracking
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f) // contact from above
            {
                // Look up the MovingPlatform on this object or its parents
                var mp = collision.collider.GetComponentInParent<MovingPlatform>();
                if (mp != null)
                {
                    currentPlatform = mp.platform != null ? mp.platform : mp.transform;
                    lastPlatformPosition = currentPlatform.position;
                    //isOnPlatform = true;
                }
                else
                {
                    //isOnPlatform = false;
                }
                break;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                var mp = collision.collider.GetComponentInParent<MovingPlatform>();
                if (mp != null)
                {
                    if (currentPlatform == null)
                    {
                        currentPlatform = mp.platform != null ? mp.platform : mp.transform;
                        lastPlatformPosition = currentPlatform.position;
                    }
                    //isOnPlatform = true;
                }
                else
                {
                    //isOnPlatform = false;
                }
                break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var mp = collision.collider.GetComponentInParent<MovingPlatform>();
        if (mp == null) return;

        var trackedPlatform = mp.platform != null ? mp.platform : mp.transform;
        if (currentPlatform == trackedPlatform || (currentPlatform != null && currentPlatform.IsChildOf(mp.transform)))
        {
            currentPlatform = null;
            //isOnPlatform = false;
        }
    }


    public Animator GetAnimator()
    {
        return animator;
    }

}
