using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    private PlayerHealth playerHealth;

    [Header("Attack Settings")]
    [SerializeField, Range(0f, 15f)] private float attackRange = 0.5f;
    [SerializeField] public LayerMask enemyLayerMask;
    [Range(0f, 100f)] public float atk = 10f;
    [Range(0f, 20f)] public float attackRate = 2f;
    [Range(0f, 20f)] public float nextAttackTime = 2f;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    { 
        if (playerHealth.isDead) return; 
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

    }

    private void attack()
    {
        animator.SetTrigger("isPunch");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayerMask);
        foreach (var enemy in hitEnemies)
        {
            var t = enemy.GetComponent<Target>();
            if (t != null)
                t.takeDamage(atk);
            else
                Debug.LogWarning($"У {enemy.name} нет компонента Target!");
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}