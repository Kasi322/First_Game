using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageZone : MonoBehaviour
{
    [Header("Урон и интервал")]
    public float damagePerTick = 10f;
    public float tickInterval = 1f;

    private Coroutine _damageCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Пытаемся получить любой компонент, реализующий IDamageable
        if (other.TryGetComponent<IDamageable>(out var dmg))
        {
            // Запускаем корутину, если её ещё нет
            if (_damageCoroutine == null)
                _damageCoroutine = StartCoroutine(DamageOverTime(dmg));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Если выходящий объект — тот же, что мы травим, останавливаем урон
        if (_damageCoroutine != null && other.TryGetComponent<IDamageable>(out _))
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;
        }
    }

    private IEnumerator DamageOverTime(IDamageable target)
    {
        while (true)
        {
            target.takeDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
    }
}