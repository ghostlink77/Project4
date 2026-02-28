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
    private LayerMask _enemyLayer;
    private SpriteRenderer _spriteRenderer;

    public override void Initialize()
    {
        base.Initialize();

        _attackTurretData = TurretData as AttackTurretData;
        _projectileKey = _attackTurretData.GetProjectileKey();

        _enemyLayer = LayerMask.GetMask("Enemy");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateEnemiesInRange();

        Debug.Log($"Initializing turret: {gameObject.name} with data: {_attackTurretData.name}");

    }

    private void Update()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _attackTurretData.FireRate[_level - 1])
        {
            Fire();
            _fireTimer = 0f;
        }
    }

    private void Fire()
    {
        UpdateEnemiesInRange();
        if (_enemiesInRange.Count == 0)
        {
            return;
        }
        UpdateTarget();

        if (_target.position.x < transform.position.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }

        TurretProjectile projectile =
            TurretProjectileSpawner.Instance.SpawnProjectile(_projectileKey, _firePoint.position);
        if (projectile != null)
        {
            projectile.Initialize(_target, _attackTurretData.ProjectileSpeed, _attackTurretData.Damage[_level - 1], _projectileKey);
        }
        else
        {
            Debug.LogError($"Failed to spawn projectile with key {_projectileKey}");
        }
    }

    private void UpdateEnemiesInRange()
    {
        _enemiesInRange.RemoveAll(enemy => enemy == null || !enemy.gameObject.activeSelf);
        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(transform.position, TurretData.Range[_level - 1], _enemyLayer);
        Debug.Log($"Enemies detected in range: {enemies.Length}");

        foreach (Collider2D enemy in enemies)
        {
            if (_enemiesInRange.Contains(enemy.transform)) continue;
            _enemiesInRange.Add(enemy.transform);
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
