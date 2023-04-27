using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveDirection;
    public float fallGravityScale = 2f;
    private float defaultGravityScale = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
        Move();
        Debug.Log(isGrounded);
        

       
    
        
    }
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("jump");
            Jump();
        }
        CalculateGravity();
    }

    private void CalculateGravity()
    {
        //if player is falling, add gravity to make the jump feel less floaty
        if (isGrounded)
        {   
            rb.gravityScale  = defaultGravityScale;
        }
        else if (rb.velocity.y > 0)
        {
            rb.gravityScale = defaultGravityScale;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallGravityScale;
        }
       
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }
    
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}