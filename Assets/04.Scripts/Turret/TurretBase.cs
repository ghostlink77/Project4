using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour, IDamageable
{
    [SerializeField] private TurretData _turretData;

    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private CircleCollider2D _rangeCollider;

    private List<Transform> _enemiesInRange = new List<Transform>();

    private float _fireTimer;
    private int _currentHp;
    private Transform _target;

    private void Start()
    {
        _currentHp = _turretData.maxHp;
        _rangeCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _turretData.fireRate)
        {
            Fire();
            _fireTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _enemiesInRange.Add(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            _enemiesInRange.Remove(collision.transform);
        }
    }

    private void Fire()
    {
        if (_enemiesInRange.Count == 0)
        {
            return;
        }
        UpdateTarget();

        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        TurretProjectile projScript = projectile.GetComponent<TurretProjectile>();
        if (projScript != null)
        {
            projScript.Initialize(_target, _turretData.projectileSpeed, _turretData.damage);
        }
    }

    private void UpdateTarget()
    {
        if (_enemiesInRange.Count == 0)
        {
            _target = null;
            return;
        }

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (Transform enemy in _enemiesInRange)
        {
            float distance = GetDirectionVector(transform.position, enemy.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        _target = closestEnemy;
    }

    Vector2 GetDirectionVector(Vector2 startPos, Vector2 endPos) => endPos - startPos;

    public void LevelUp(TurretData newData)
    {
        _turretData = newData;
        _rangeCollider.radius = _turretData.range;
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;

        if (_currentHp <= 0)
        {
            OnDestroyed();
        }
    }

    private void OnDestroyed()
    {
        // TODO: 터렛이 파괴될 때의 로직 (예: 애니메이션, 사운드 등)
        Destroy(gameObject);
    }

}
