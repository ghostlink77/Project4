using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTurretBase : TurretBase
{
    [SerializeField] private Transform _firePoint;

    private AttackTurretData _attackTurretData;
    private List<Transform> _enemiesInRange = new List<Transform>();
    private List<Transform> _sortedTargets = new List<Transform>();
    private string _projectileKey;

    private float _fireTimer;
    private LayerMask _enemyLayer;
    private SpriteRenderer _spriteRenderer;

    private bool _isFiring;
    private static readonly WaitForSeconds FireDelay = new WaitForSeconds(0.1f);

    public override void Initialize(int level)
    {
        base.Initialize(level);

        _attackTurretData = TurretData as AttackTurretData;
        _projectileKey = _attackTurretData.GetProjectileKey();

        _enemyLayer = LayerMask.GetMask("Enemy");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateEnemiesInRange();
    }

    private void Update()
    {
        if (_isFiring) return;

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
        if (_enemiesInRange.Count == 0) return;

        UpdateSortedTargets();
        if (_sortedTargets.Count == 0) return;

        int numProjectile = _attackTurretData.NumProjectile[_level - 1];

        if (numProjectile <= 1)
        {
            FlipSprite(_sortedTargets[0]);
            FireProjectileAt(_sortedTargets[0]);
        }
        else
        {
            StartCoroutine(FireMultipleProjectiles(numProjectile));
        }
    }

    private IEnumerator FireMultipleProjectiles(int count)
    {
        _isFiring = true;

        for (int i = 0; i < count; i++)
        {
            if (_sortedTargets.Count == 0) break;

            // NOTE: 타겟 인덱스가 범위를 초과하면 마지막 유효 타겟을 재사용
            int targetIndex = Mathf.Min(i, _sortedTargets.Count - 1);
            Transform target = _sortedTargets[targetIndex];

            if (target == null || !target.gameObject.activeSelf)
            {
                UpdateEnemiesInRange();
                UpdateSortedTargets();
                if (_sortedTargets.Count == 0) break;
                targetIndex = Mathf.Min(i, _sortedTargets.Count - 1);
                target = _sortedTargets[targetIndex];
            }

            FlipSprite(target);
            FireProjectileAt(target);

            if (i < count - 1)
            {
                yield return FireDelay;
            }
        }

        _isFiring = false;
    }

    private void FireProjectileAt(Transform target)
    {
        TurretProjectile projectile =
            TurretProjectileSpawner.Instance.SpawnProjectile(_projectileKey, _firePoint.position);
        if (projectile != null)
        {
            projectile.Initialize(target, _attackTurretData.ProjectileSpeed, _attackTurretData.Damage[_level - 1], _projectileKey);
        }
    }

    private void FlipSprite(Transform target)
    {
        _spriteRenderer.flipX = target.position.x < transform.position.x;
    }

    private void UpdateEnemiesInRange()
    {
        _enemiesInRange.RemoveAll(enemy => enemy == null || !enemy.gameObject.activeSelf);
        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(transform.position, TurretData.Range[_level - 1], _enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            if (_enemiesInRange.Contains(enemy.transform)) continue;
            if (!enemy.gameObject.activeSelf) continue;
            _enemiesInRange.Add(enemy.transform);
        }
    }

    private void UpdateSortedTargets()
    {
        _sortedTargets.Clear();
        Vector2 pos = transform.position;
        foreach (Transform enemy in _enemiesInRange)
        {
            if (enemy == null || !enemy.gameObject.activeSelf) continue;
            _sortedTargets.Add(enemy);
        }
        _sortedTargets.Sort((a, b) =>
        {
            float distA = ((Vector2)a.position - pos).sqrMagnitude;
            float distB = ((Vector2)b.position - pos).sqrMagnitude;
            return distA.CompareTo(distB);
        });
    }

}
