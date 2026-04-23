using UnityEngine;

public class PlayerSlideState : PlayerState
{
    private float sliderTimer;
    private float slideStoptimer;
    public PlayerSlideState(Player player) : base(player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sliderTimer = player.slideDuration;
        slideStoptimer = 0;

        player.SetColliderSlide();
        anim.SetBool("isSliding", true);
    }

    public override void Update()
    {
        base.Update();

        if (sliderTimer > 0)
        {
            sliderTimer -= Time.deltaTime;
        }
        else if (slideStoptimer <= 0)
        {
            slideStoptimer = player.slideStopDuration;
        } else
        {
            slideStoptimer -= Time.deltaTime;
            if (slideStoptimer <= 0)
            {
                if (player.CheckForCeiling() || MoveInput.y <= -0.1f)
                {
                    player.ChangeState(player.crouchState);
                } else
                {
                    player.ChangeState(player.idleState);
                }
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (sliderTimer > 0)
        {
            rigidbody2D.linearVelocity = new Vector2(player.slideSpeed * player.facingDirection, rigidbody2D.linearVelocity.y);
        }
        else
        {
            rigidbody2D.linearVelocity = new Vector2(0, rigidbody2D.linearVelocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetColliderNormal();
        anim.SetBool("isSliding", false);
    }
}
