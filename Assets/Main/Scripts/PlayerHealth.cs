using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // на эти события подписывается UI, так префабы не ссылаются друг на друга
    public static event Action<int> HealthChanged;
    public static event Action Died;

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invulnerableTime = 1f; // после удара какое-то время урон не проходит
    [SerializeField] private float restartDelay = 2f;
    [SerializeField] private float fallLimit = -30f; // если упал ниже - считаем что умер

    private int health;
    private float lastHitTime = -100f;
    private bool dead;

    void Start()
    {
        health = maxHealth;
        HealthChanged?.Invoke(health);
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

        HealthChanged?.Invoke(health);

        if (health == 0)
            Die();
    }

    void Die()
    {
        dead = true;
        GetComponent<PlayerMovement>().enabled = false;
        Died?.Invoke();
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
