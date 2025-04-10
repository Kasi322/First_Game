using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;

    [Header("Attack Settings")]
    [SerializeField, Range(0f, 15f)] private float attackRange = 0.5f;
    [SerializeField] public LayerMask enemyLayerMask;
    [Range(0f, 100f)] public float atk = 10f;
    [Range(0f, 20f)] public float attackRate = 2f;
    [Range(0f, 20f)] public float nextAttackTime = 2f;

    private static readonly int PunchTrigger = Animator.StringToHash("isPunch");

    private void Start()
    {
    }

    private void Update()
    {
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
        animator.SetTrigger(PunchTrigger);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayerMask);
        foreach (var enemy in hitEnemies)
        {
            enemy.GetComponent<Target>().TakeDamage(atk);
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