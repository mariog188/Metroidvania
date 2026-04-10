using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    public PlayerInput playerInput;

    [Header("Movement Variables")]
    public float speed = 5;
    public float jumpForce = 20;
    public float jumpCutMultiplier = .5f;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;

    public int facingDirection;

    // Inputs
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpReleased;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;

    private void Start()
    {
        rigidbody2D.gravityScale = normalGravity;
    }

    private void Update()
    {
        Flip();
    }

    private void FixedUpdate()
    {
        ApplyVariableGravity();
        CheckGrounded();
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float targetSpeed = moveInput.x * speed;
        rigidbody2D.linearVelocity = new Vector2(targetSpeed, rigidbody2D.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, jumpForce);
            jumpPressed = false;
            jumpReleased = false;
        }
        if (jumpReleased)
        {
            if (rigidbody2D.linearVelocity.y > 0) // its goind up
            {
                rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, rigidbody2D.linearVelocity.y * jumpCutMultiplier);
            }
            jumpReleased = false;
        }
    }

    private void ApplyVariableGravity()
    {
        if (rigidbody2D.linearVelocity.y < -0.1f) // falling
        {
            rigidbody2D.gravityScale = fallGravity;
        } 
        else if (rigidbody2D.linearVelocity.y > 0.1) // rising
        {
            rigidbody2D.gravityScale = jumpGravity;
        } else
        {
            rigidbody2D.gravityScale = normalGravity;
        }
    }

    void Flip()
    {
        if (moveInput.x > .1f)
        {
            facingDirection = 1;
        }
        else if (moveInput.x < -.1f)
        {
            facingDirection = -1;
        }
        transform.localScale = new Vector3(facingDirection, 1, 1);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpPressed = true;
            jumpReleased = false;
        } else
        {
            jumpReleased = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
