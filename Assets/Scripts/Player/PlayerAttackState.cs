using System.Diagnostics.Tracing;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(Player player) : base(player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        anim.SetBool("isAttacking", true);
        rigidbody2D.linearVelocity = new Vector2(0, rigidbody2D.linearVelocity.y); // Stop horizontal movement when attacking
    }

    public override void AnimationFinished()
    {
        if (Mathf.Abs(MoveInput.x) > .1f)
        {
            player.ChangeState(player.moveState);
        }
         else
        {
            player.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("isAttacking", false);
    }
}
