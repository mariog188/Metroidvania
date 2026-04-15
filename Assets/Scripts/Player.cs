using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rigidbody2D;
    public PlayerInput playerInput;
    public Animator animator;
    public CapsuleCollider2D playerCollider;

    [Header("Movement Variables")]
    public float walkSpeed = 5;
    public float runSpeed = 9;
    public float jumpForce = 20;
    public float jumpCutMultiplier = .5f;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;

    public int facingDirection;

    // Inputs
    private Vector2 moveInput;
    private bool runPressed;
    private bool jumpPressed;
    private bool jumpReleased;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Slide Settings")]
    public Transform headCheck;
    public float headCheckRadius = .2f;

    [Header("Slide Settings")]
    public float slideDuration = .6f;
    public float slideSpeed = 12;
    public float slideStopDuration = .15f;

    public float slideHeight;
    public Vector2 slideOffset;
    public float normalHeight;
    public Vector2 normalOffset;

    private bool isSliding;
    private bool slideInputLocked;
    private float sliderTimer;
    private float slideStoptimer;


    private void Start()
    {
        rigidbody2D.gravityScale = normalGravity;
    }

    private void Update()
    {
        if (!isSliding)
            Flip();
        HandleAnimations();
        HandleSlide();
    }

    private void FixedUpdate()
    {
        ApplyVariableGravity();
        CheckGrounded();
        if (!isSliding)
            HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float currentSpeed = runPressed ? runSpeed : walkSpeed;
        float targetSpeed = moveInput.x * currentSpeed;
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

    private void HandleSlide()
    {
        if (isSliding)
        {
            sliderTimer -= Time.deltaTime;
            rigidbody2D.linearVelocity = new Vector2(slideSpeed * facingDirection, rigidbody2D.linearVelocity.y);

            // if we are done sliding
            if (sliderTimer <= 0)
            {
                isSliding = false;
                slideStoptimer = slideStopDuration;
                SetColliderNormal();
            }
        }

        if (slideStoptimer > 0)
        {
            slideStoptimer -= Time.deltaTime;
            rigidbody2D.linearVelocity = new Vector2(0, rigidbody2D.linearVelocity.y);
        }

        // start the slide
        if (isGrounded && runPressed && moveInput.y < -.1f && !isSliding && !slideInputLocked)
        {
            isSliding = true;
            slideInputLocked = true;
            sliderTimer = slideDuration;
            SetColliderSlide();
        }

        if (slideStoptimer < 0 && moveInput.y >= -.1f)
        {
            slideInputLocked = false;
        }
    }

    private void SetColliderSlide()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, slideHeight);
        playerCollider.offset = slideOffset;
    }

    private void SetColliderNormal()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, normalHeight);
        playerCollider.offset = normalOffset;
    }

    void TryStandUp()
    {

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

    void HandleAnimations()
    {
        animator.SetBool("isJumping", rigidbody2D.linearVelocity.y > .1f);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isSliding", isSliding);

        animator.SetFloat("yVelocity", rigidbody2D.linearVelocity.y);

        bool isMoving = Mathf.Abs(moveInput.x) > .1f && isGrounded;
        animator.SetBool("isIdle", !isMoving && isGrounded && !isSliding);
        animator.SetBool("isWalking", isMoving && !runPressed && !isSliding);
        animator.SetBool("isRunning", isMoving && runPressed && !isSliding);
    }

    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }

    public void OnRun(InputValue value)
    {
        runPressed = value.isPressed;
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
