using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player) : base(player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("o IdleState");
        anim.SetBool("isIdle", true);
        rigidbody2D.linearVelocity = new Vector2(0, rigidbody2D.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (SpellcastPressed && magic.CanCast)
        {
            player.ChangeState(player.spellcastState);
        }
        else if (AttackPressed && combat.canAttack)
        {
            player.ChangeState(player.attackState);
        }
        else if (JumpPressed)
        {
            JumpPressed = false;
            player.ChangeState(player.jumpState);
        }
        else if (Mathf.Abs(MoveInput.x) > .1f)
        {
            player.ChangeState(player.moveState);
        }
        else if (MoveInput.y < -.1f)
        {
            player.ChangeState(player.crouchState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("isIdle", false);
    }
}
