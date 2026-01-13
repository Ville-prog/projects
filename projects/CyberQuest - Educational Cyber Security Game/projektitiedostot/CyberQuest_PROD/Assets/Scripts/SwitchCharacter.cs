/*
 * SwitchCharacter.cs
 * This script handles the movement and jumping of a character in Unity.
 * It also allows for switching between different character colors using AnimatorControllers.
 * Authors: Salla Valio and Meri Välimäki
 */

using System;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    float horizontalInput; // Stores horizontal movement input
    public float moveSpeed = 5f; // Speed at which the character moves
    bool isFacingRight = true; // Keeps track of the character's facing direction
    public float jumpPower = 7f; // Lowered to a reasonable value

    public Transform groundCheck;  // Empty GameObject below the player for ground detection
    public LayerMask groundLayer;  // Assign the "Ground" layer in Unity Inspector

    private SpriteRenderer spriteRenderer;
    public RuntimeAnimatorController[] colorAnimators; // Assign 8 different AnimatorControllers
    public static int currentColorIndex = 0;

    Rigidbody2D rb; // Rigidbody component for physics
    Animator animator; // Animator component for animations

    bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Set animator based on the current color index
        if (colorAnimators.Length > 0)
        {
        animator.runtimeAnimatorController = colorAnimators[currentColorIndex];
        }
    }

    void Update()
    {
        // Get horizontal input (A/D, Left/Right arrow keys)
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            FlipSprite();
        }

        // Check if character is grounded using a small circle under the player
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        animator.SetBool("isJumping", !isGrounded);

        // Jump if the player presses the jump button and is on the ground
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower); // FIXED!
        }

        //function that makes it that when button c is pressed, you can manually change character color
        //HandleColorChange();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FlipSprite()
    {
        // Flip the character's facing direction when changing movement direction
        if ((horizontalInput > 0 && !isFacingRight) || (horizontalInput < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
void HandleColorChange()
    {
        // This function would change the character's color (animation controller),
        // but the logic is currently commented out.
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentColorIndex = (currentColorIndex + 1) % colorAnimators.Length;
            UpdateAnimator();
        }
    }

    public void UpdateAnimator()
    {
        // Update the Animator with the selected color/style
        if (colorAnimators.Length > 0)
        {
            animator.runtimeAnimatorController = colorAnimators[currentColorIndex];
        }
    }
}