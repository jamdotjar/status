using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Script Toggle")] 
    [SerializeField] private bool scriptEnabled = true;
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 11f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.4f;
    [SerializeField] private float accelerationSpeed = 0.5f;
    [SerializeField] private float decelerationSpeed = 0.4f;
    [SerializeField] private float lowJumpGravityScale = 4f;
    [SerializeField] private float defaultGravityScale = 2f;
    [SerializeField] private float fallGravityScale = 6f;
    [SerializeField] private float airAccelerationSpeed = 0.2f;
    [SerializeField] private float airDecelerationSpeed = 0.06f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveDirection;
    public 
    
    bool shouldJump;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
   
    
    void Update()
    {
        CheckGrounded();
        moveDirection = Input.GetAxisRaw("Horizontal");
        Move();
        

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            shouldJump = true;
        }
    }
    void FixedUpdate()
{
    if (shouldJump)
    {
        Jump();
        shouldJump = false;
    }
    CalculateGravity();
}
    
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }
    private void CalculateGravity()
    {
        if (isGrounded)
        {
            rb.gravityScale = defaultGravityScale;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // If the player is ascending and releases the jump button
        {
            rb.gravityScale = lowJumpGravityScale;
        }
        else if (rb.velocity.y > 0) // If the player is ascending and still holding the jump button
        {
            rb.gravityScale = defaultGravityScale;
        }
        else if (rb.velocity.y < 0) // If the player is falling
        {
            rb.gravityScale = fallGravityScale;
        }
    }
    private void Move()
    {
        float targetVelocityX = moveDirection * moveSpeed;
        float speed;

        if (isGrounded)
        {
            speed = moveDirection == 0 ? decelerationSpeed : accelerationSpeed;
        }
        else
        {
            speed = moveDirection == 0 ? airDecelerationSpeed : airAccelerationSpeed;
        }

        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, targetVelocityX, speed * Time.fixedDeltaTime), rb.velocity.y);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }
}