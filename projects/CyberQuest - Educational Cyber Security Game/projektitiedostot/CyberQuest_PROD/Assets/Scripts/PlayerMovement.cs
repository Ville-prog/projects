using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    public float moveSpeed = 15f;
    bool isFacingRight = true;
    public float jumpPower = 7f; // Lowered to a reasonable value
    
    public Transform groundCheck;  // Empty GameObject below the player for ground detection
    public LayerMask groundLayer;  // Assign the "Ground" layer in Unity Inspector

    Rigidbody2D rb;
    Animator animator;
    
    
    bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            FlipSprite();
        }

        // Check if player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 1.5f, groundLayer);
        animator.SetBool("isJumping", !isGrounded);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower); // FIXED!
        }
    }

    private void FixedUpdate()
    {   
        /*
        if (isGrounded){

            Vector2 moveDirection = GetSlopeDirection();

                rb.linearVelocity = new Vector2(moveDirection.x * horizontalInput * moveSpeed, moveDirection.y *horizontalInput * moveSpeed);


            }

        */
        
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);

    }

    void FlipSprite()
    {
        if ((horizontalInput > 0 && !isFacingRight) || (horizontalInput < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    /**
    Work In Progress to handle slopes better

    */
    private Vector2 GetSlopeDirection(){

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);

        if (hit.collider != null)
    {
        Vector2 slopeDirection = Vector2.Perpendicular(hit.normal).normalized;
        return (slopeDirection.y < 0) ? slopeDirection : -slopeDirection;
    }

    return Vector2.right; // Default to normal movement
    }
    
}
