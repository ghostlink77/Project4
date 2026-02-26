using System.Collections.Generic;
using UnityEngine;

public class AttackTurretBase : TurretBase
{
    [SerializeField] private Transform _firePoint;

    private AttackTurretData _attackTurretData;
    private List<Transform> _enemiesInRange = new List<Transform>();
    private string _projectileKey;

    private float _fireTimer;
    private Transform _target;

    protected override void Initialize()
    {
        base.Initialize();
        _attackTurretData = TurretData as AttackTurretData;
        _projectileKey = _attackTurretData.GetProjectileKey();

        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider2D[] initialEnemies =
            Physics2D.OverlapCircleAll(transform.position, TurretData.range[_level], enemyLayer);
        foreach (Collider2D enemy in initialEnemies)
        {
            _enemiesInRange.Add(enemy.transform);
        }
    }

    private void Update()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _attackTurretData.fireRate[_level])
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
        if (collision.CompareTag("Enemy"))
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

        TurretProjectile projectile =
            TurretProjectileSpawner.Instance.SpawnProjectile(_projectileKey, _firePoint.position);
        if (projectile != null)
        {
            projectile.Initialize(_target, _attackTurretData.projectileSpeed, _attackTurretData.damage[_level], _projectileKey);
        }
        else
        {
            Debug.LogError($"Failed to spawn projectile with key {_projectileKey}");
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

    private Vector2 GetDirectionVector(Vector2 startPos, Vector2 endPos) => endPos - startPos;
}
