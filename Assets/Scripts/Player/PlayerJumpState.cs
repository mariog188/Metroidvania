using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player) : base(player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        anim.SetBool("isJumping", true);

        rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, player.jumpForce);

        JumpPressed = false;
        JumpReleased = false;
    }

    public override void Update()
    {
        base.Update();

        if (!player.isGrounded && player.isTouchingWall && MoveInput.x == player.facingDirection && rigidbody2D.linearVelocity.y < 0)
        { 
            player.ChangeState(player.wallSlideState);
        } else if (JumpPressed && player.isTouchingWall) 
        { 
            player.ChangeState(player.wallJumpState);
        }
        else if (player.isGrounded && rigidbody2D.linearVelocity.y <= 0.1)
        {
            player.ChangeState(player.idleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.ApplyVariableGravity();

        if (JumpReleased && rigidbody2D.linearVelocity.y > 0)
        {
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, rigidbody2D.linearVelocity.y * player.jumpCutMultiplier);
            JumpReleased = false;
        }

        float speed = RunPressed ? player.runSpeed : player.walkSpeed;
        float targetSpeed = speed * MoveInput.x;
        rigidbody2D.linearVelocity = new Vector2(targetSpeed, rigidbody2D.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("isJumping", false);
    }
}