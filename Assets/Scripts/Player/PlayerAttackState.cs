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
            Debug.Log("Move input detected, transitioning to MoveState");
            player.ChangeState(player.moveState);
        }
         else
        {
            Debug.Log("No move input detected, transitioning to IdleState");
            player.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exiting AttackState");
        anim.SetBool("isAttacking", false);
    }
}
