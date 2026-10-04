using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // на это событие подписывается UI, так префабы не ссылаются друг на друга
    public static event Action Died;

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invulnerableTime = 1f; // после удара какое-то время урон не проходит
    [SerializeField] private float fallLimit = -30f; // если упал ниже - считаем что умер
    [SerializeField] private HealthText healthText;

    private int health;
    private float lastHitTime = -100f;
    private bool dead;

    void Start()
    {
        health = maxHealth;
        healthText.Show(health, 0);
    }

    void Update()
    {
        if (!dead && transform.position.y < fallLimit)
            Die();
    }

    public void TakeDamage(int damage)
    {
        if (dead || Time.time - lastHitTime < invulnerableTime)
            return;

        lastHitTime = Time.time;
        health -= damage;
        if (health < 0)
            health = 0;

        healthText.Show(health, damage);

        if (health == 0)
            Die();
    }

    void Die()
    {
        dead = true;
        GetComponent<PlayerMovement>().enabled = false;
        Died?.Invoke();
    }
}
