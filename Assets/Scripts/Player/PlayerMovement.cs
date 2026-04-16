using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; // Horizontal wall jump force
    [SerializeField] private float wallJumpY; // Vertical wall jump force
    [SerializeField] private float wallSlideSpeed = 2f; // Speed at which player slides down the wall
    [SerializeField] private float wallJumpDuration = 0.2f; // Time player can't control movement after wall jump
    [SerializeField] private float wallStickTime = 0.25f; // Time player sticks to wall before starting to slide

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [Header("Jump sound")]
    [SerializeField] private AudioClip JumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private float wallStickCounter;
    private bool isWallSliding;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Wall jump cooldown
        if (wallJumpCooldown > 0.2f)
        {
            // Flip player when moving left-right (only when not in wall jump cooldown)
            if (horizontalInput > 0.01f)
                transform.localScale = Vector3.one;
            else if (horizontalInput < -0.01f)
                transform.localScale = new Vector3(-1, 1, 1);
        }

        // Set animator parameters
        anim.SetBool("Walk", horizontalInput != 0 && isGrounded() && wallJumpCooldown > 0.2f);
        anim.SetBool("Grounded", isGrounded());

        // Handle wall sliding logic
        HandleWallSliding();

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        // Handle movement
        HandleMovement();

        // Update timers
        wallJumpCooldown += Time.deltaTime;
    }

    private void HandleWallSliding()
    {
        bool touchingWall = onWall();
        bool pushingWall = (horizontalInput > 0 && transform.localScale.x > 0) || 
                          (horizontalInput < 0 && transform.localScale.x < 0);

        if (touchingWall && !isGrounded() && pushingWall)
        {
            isWallSliding = true;
            
            // Wall stick time - player sticks to wall briefly before sliding
            if (wallStickCounter > 0)
            {
                wallStickCounter -= Time.deltaTime;
                body.velocity = new Vector2(body.velocity.x, 0);
            }
            else
            {
                // Wall sliding - controlled descent
                if (body.velocity.y < -wallSlideSpeed)
                    body.velocity = new Vector2(body.velocity.x, -wallSlideSpeed);
            }

            // Set wall animation
            anim.SetBool("Wall", true);
        }
        else
        {
            isWallSliding = false;
            wallStickCounter = wallStickTime; // Reset stick counter
            anim.SetBool("Wall", false);
        }
    }

    private void HandleMovement()
    {
        if (isWallSliding)
        {
            // Don't apply horizontal movement while wall sliding
            return;
        }

        if (wallJumpCooldown > 0.2f)
        {
            // Normal movement (only when not in wall jump cooldown)
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        }

        // Handle ground/air state for jumps
        if (isGrounded())
        {
            coyoteCounter = coyoteTime;
            jumpCounter = extraJumps;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        SoundManager.instance.PlaySound(JumpSound);
        if (isWallSliding)
        {
            WallJump();
        }
        else
        {
            // Regular jump logic
            if (coyoteCounter <= 0 && jumpCounter <= 0) return;

            if (isGrounded())
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                anim.SetTrigger("Jump");
            }
            else
            {
                if (coyoteCounter > 0)
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    anim.SetTrigger("Jump");
                }
                else if (jumpCounter > 0)
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    jumpCounter--;
                    anim.SetTrigger("Jump");
                }
            }

            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        // Direction opposite to wall
        Vector2 wallJumpDirection = new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY);
        
        // Apply wall jump force
        body.velocity = wallJumpDirection;
        
        // Reset wall jump cooldown to prevent immediate input
        wallJumpCooldown = 0;
        
        // Reset wall stick counter
        wallStickCounter = wallStickTime;
        
        // Trigger jump animation
        anim.SetTrigger("Jump");
        
        // Optional: Flip player to face jump direction
        if (wallJumpDirection.x > 0)
            transform.localScale = Vector3.one;
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}