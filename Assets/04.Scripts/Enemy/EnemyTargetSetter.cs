using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetSetter : MonoBehaviour
{
    private Rigidbody2D _agitRigidbody;
    private Enemy _enemy;
    private HashSet<Rigidbody2D> _damageablesInRange = new HashSet<Rigidbody2D>();
    private Rigidbody2D _currentTarget;
    private Coroutine _updateTargetCoroutine;
    private const float FindTargetInterval = 0.1f;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnDisable()
    {
        if (_updateTargetCoroutine != null)
        {
            StopCoroutine(_updateTargetCoroutine);
            _updateTargetCoroutine = null;
        }
    }

    public void Initialize(Rigidbody2D agit)
    {
        _agitRigidbody = agit;
        _currentTarget = agit;
        _damageablesInRange.Clear();
        _updateTargetCoroutine = StartCoroutine(UpdateTargetRoutine());
    }

    private void UpdateTarget()
    {
        _damageablesInRange.RemoveWhere(d => d == null || !d.gameObject.activeSelf);

        if (_damageablesInRange.Count == 0)
        {
            if (_currentTarget != _agitRigidbody)
            {
                _currentTarget = _agitRigidbody;
                _enemy.SetTarget(_agitRigidbody);
            }
            return;
        }

        Rigidbody2D closestDamageable = null;
        float closestDistance = float.MaxValue;
        foreach (Rigidbody2D damageable in _damageablesInRange)
        {
            float distance = (damageable.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestDamageable = damageable;
            }
        }

        if (closestDamageable != null && closestDamageable != _currentTarget)
        {
            _currentTarget = closestDamageable;
            _enemy.SetTarget(_currentTarget);
        }
    }

    private IEnumerator UpdateTargetRoutine()
    {
        while (true)
        {
            UpdateTarget();
            yield return new WaitForSeconds(FindTargetInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Agit")) return;
        if (!collision.gameObject.activeSelf) return;

        if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb) && rb.GetComponent<IDamageable>() != null)
        {
            _damageablesInRange.Add(rb);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Agit")) return;

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            _damageablesInRange.Remove(rb);
        }
    }
}
