using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public Health health;

    private void OnEnable()
    {
        health.OnDamaged += HandleDamage;
       // health.OnDeath += HandleDeath;
    }


    private void OnDisable()
    {
        health.OnDamaged -= HandleDamage;
       // health.OnDeath -= HandleDeath;
    }

    private void HandleDamage()
    {
        animator.SetTrigger("isDamaged");
    }
}
