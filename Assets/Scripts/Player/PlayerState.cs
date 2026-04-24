using UnityEngine;

public abstract class PlayerState
{
    protected Player player;
    protected Animator anim;
    protected Rigidbody2D rigidbody2D;
    protected Combat combat;
    protected Magic magic;

    protected bool JumpPressed { get => player.jumpPressed; set => player.jumpPressed = value; }
    protected bool JumpReleased { get => player.jumpReleased; set => player.jumpReleased = value; }
    protected bool RunPressed => player.runPressed;
    protected bool AttackPressed => player.attackPressed;
    protected bool SpellcastPressed => player.spellcastPressed;
    protected Vector2 MoveInput => player.moveInput;

    protected PlayerState(Player player)
    {
        this.player = player;
        this.anim = player.animator;
        this.rigidbody2D = player.rigidbody2D;
        this.combat = player.combat;
        this.magic = player.magic;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void AnimationFinished()
    {

    }
}
