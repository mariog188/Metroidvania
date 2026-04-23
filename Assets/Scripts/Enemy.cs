using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public Health health;

    [Header("Death FX")]
    [SerializeField] private GameObject[] deathParts;
    [SerializeField] private float spawnForce = 5;
    [SerializeField] private float torque = 5;
    [SerializeField] private float lifetime = 2;

    private void OnEnable()
    {
        health.OnDamaged += HandleDamage;
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDamaged -= HandleDamage;
        health.OnDeath -= HandleDeath;
    }

    private void HandleDamage()
    {
        animator.SetTrigger("isDamaged");
    }

    private void HandleDeath()
    {
        foreach (GameObject prefab in deathParts)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0.5f, 1));
            GameObject part = Instantiate(prefab, transform.position, rotation);

            Rigidbody2D rb = part.GetComponent<Rigidbody2D>();

            Vector2 randomDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0.5f, 1f)).normalized;
            rb.linearVelocity = randomDirection * spawnForce;
            rb.AddTorque(UnityEngine.Random.Range(-torque, torque), ForceMode2D.Impulse);
            Destroy(part, lifetime);
        }

        Destroy(gameObject);
    }
}
