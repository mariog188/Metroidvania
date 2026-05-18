using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    private float horizontalJumpPercentage = 0.5f;
    public PlayerWallJumpState(Player player) : base(player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        anim.SetTrigger("isWallJumping");

        rigidbody2D.linearVelocity = Vector2.zero;
        rigidbody2D.linearVelocity = new Vector2(-player.facingDirection * horizontalJumpPercentage, 1f) * player.jumpForce;

        JumpPressed = false;
        JumpReleased = false;
    }

    public override void Update()
    {
        if (JumpPressed && player.isTouchingWall)
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
        player.ApplyVariableGravity();
        if (JumpReleased && rigidbody2D.linearVelocity.y > 0)
        {
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, rigidbody2D.linearVelocity.y * player.jumpCutMultiplier);
            JumpReleased = false;
        }
    }

    public override void Exit()
    {
        anim.SetBool("isWallJumping", false);
    }
}
