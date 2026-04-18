using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    public PlayerState currentState;
    public PlayerIdleState idleState;
    public PlayerJumpState jumpState;
    public PlayerMoveState moveState;
    public PlayerCrouchState crouchState;

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
    public Vector2 moveInput;
    public bool runPressed;
    public bool jumpPressed;
    public bool jumpReleased;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isGrounded;

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

    private void Awake()
    {
        idleState = new PlayerIdleState(this);
        jumpState = new PlayerJumpState(this);
        moveState = new PlayerMoveState(this);
        crouchState = new PlayerCrouchState(this);
    }

    private void Start()
    {
        rigidbody2D.gravityScale = normalGravity;

        ChangeState(idleState);
    }

    private void Update()
    {
        if (currentState != null)
            currentState.Update();

        if (!isSliding)
            Flip();
        HandleAnimations();
        HandleSlide();
    }

    private void FixedUpdate()
    {
        if (currentState != null)
            currentState.FixedUpdate();

        CheckGrounded();
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
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

    public void SetColliderSlide()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, slideHeight);
        playerCollider.offset = slideOffset;
    }

    public void SetColliderNormal()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, normalHeight);
        playerCollider.offset = normalOffset;
    }


    public void ApplyVariableGravity()
    {
        if (rigidbody2D.linearVelocity.y < -0.1) // falling
        {
            rigidbody2D.gravityScale = fallGravity;
        }
        else if (rigidbody2D.linearVelocity.y > 0.1) // rising
        {
            rigidbody2D.gravityScale = jumpGravity;
        }
        else
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

    public bool CheckForCeiling()
    {
        return Physics2D.OverlapCircle(headCheck.position, headCheckRadius, groundLayer);
    }

    void HandleAnimations()
    {
        bool isCrouching = animator.GetBool("isCrouching");

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isSliding", isSliding);

        animator.SetFloat("yVelocity", rigidbody2D.linearVelocity.y);
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
        }
        else
        {
            jumpReleased = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(headCheck.position, headCheckRadius);
    }
}
