using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyAttack : MonoBehaviour
{
    [Header("Урон и интервал")]
    public float damagePerTick = 10f;
    public float tickInterval  = 1f;

    [Header("Фильтр слоев")]
    [Tooltip("Только объекты из этих слоёв будут получать урон")]
    public LayerMask targetLayers;

    [Header("Анимация атаки")]
    [SerializeField] private Animator animator;
    [Tooltip("Имя триггера в Animator для атаки")]
    [SerializeField] private string attackTrigger = "isAttack";

    private Coroutine   _damageCoroutine;
    private IDamageable _currentTarget;

    private void Start()
    {
        // подхватим Animator, если забыли в инспекторе
        if (animator == null)
            animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // фильтруем по слоям
        if (((1 << other.gameObject.layer) & targetLayers) == 0)
            return;

        // проверяем, что объект реалазует IDamageable
        if (other.TryGetComponent<IDamageable>(out var dmg))
        {
            _currentTarget = dmg;
            if (_damageCoroutine == null)
                _damageCoroutine = StartCoroutine(DamageOverTime());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // если тот же самый объект вышел – останавливаем урон
        if (_damageCoroutine != null 
         && other.TryGetComponent<IDamageable>(out var dmg) 
         && dmg == _currentTarget)
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine  = null;
            _currentTarget    = null;
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (true)
        {
            // запустить анимацию атаки
            if (animator != null)
                animator.SetTrigger(attackTrigger);

            // нанести урон
            _currentTarget.takeDamage(damagePerTick);
            
            yield return new WaitForSeconds(tickInterval);
        }
    }
}

