using System;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public Player player;
    public SpellSO currentSpell;

    [Header("Spark Variables")]
    public GameObject sparkFXPrefab;
    public int damage;
    public float damageRadius = 5;
    public LayerMask enemyLayer;

    public bool CanCast => Time.time >= nextCastTime;
    public float nextCastTime;

    public void AnimationFinished()
    {
        player.Animationfinished();
        CastSpell();
    }

    private void CastSpell()
    {
        if (!CanCast || currentSpell == null)
            return;

        currentSpell.Cast(player);

        nextCastTime = Time.time + currentSpell.cooldown;
    }
}
