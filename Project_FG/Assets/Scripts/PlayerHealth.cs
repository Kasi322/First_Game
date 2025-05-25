using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Player health")]
    public float maxHealth = 100f;
    private float currentHealth;
    public bool isDead { get; private set;}

    private Rigidbody2D rb2d;
    private Animator animator;
    private Collider2D collider2d;
    public Image healthBar;

    [Header("Respawn")]
    public Vector2 spawnPoint = new Vector2(11f, 11f);

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        collider2d = GetComponent<Collider2D>();
        currentHealth = maxHealth;
    }

    public void takeDamage(float damage)
    {
        if (isDead) return;

        animator.SetTrigger("isHit");
        currentHealth -= damage;
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        // Воспроизводим анимацию смерти
        animator.SetBool("isDead", true);

        // Отключаем физику и столкновения
        rb2d.linearVelocity = Vector2.zero;
        rb2d.simulated = false;
        collider2d.enabled = false;

        // Отключаем управление персонажем (если логика в этом компоненте)
        this.enabled = false;
    }

    /* public void Revive(Vector2 newSpawnPoint)
    {
        if (!isDead) return;

        isDead = false;

        transform.position = newSpawnPoint;
        rb2d.simulated = true;
        rb2d.linearVelocity = Vector2.zero;
        collider2d.enabled = true;
        this.enabled = true;
        
        currentHealth = maxHealth;
    }

    public void RespawnAtSavedPoint()
    {
        Revive(spawnPoint);
    }
    */
}

