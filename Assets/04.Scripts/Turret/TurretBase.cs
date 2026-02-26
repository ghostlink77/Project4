using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour, IDamageable, IItemStatController
{
    [SerializeField] private TurretData _turretData;
    public TurretData TurretData => _turretData;

    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private CircleCollider2D _rangeCollider;

    private List<Transform> _enemiesInRange = new List<Transform>();

    private float _fireTimer;
    private int _currentHp;
    private Transform _target;

    private int _level = 1;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _currentHp = _turretData.maxHp;
        _rangeCollider.radius = _turretData.range;

        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider2D[] initialEnemies =
            Physics2D.OverlapCircleAll(transform.position, _turretData.range, enemyLayer);
        foreach (Collider2D enemy in initialEnemies)
        {
            _enemiesInRange.Add(enemy.transform);
        }
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

        TurretProjectile projectile = 
            TurretProjectileSpawner.Instance.SpawnProjectile(_turretData.GetprojectileKey(), _firePoint.position);
        if (projectile != null)
        {
            Debug.Log($"Firing projectile {_turretData.GetprojectileKey()}");
            projectile.Initialize(_target, _turretData.projectileSpeed, _turretData.damage, TurretData.GetprojectileKey());
        }
        else
        {
            Debug.LogError($"Failed to spawn projectile with key {_turretData.GetprojectileKey()}");
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
            if (enemy != null)
            {
                float distance = GetDirectionVector(transform.position, enemy.position).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
            
        }
        _target = closestEnemy;
    }

    Vector2 GetDirectionVector(Vector2 startPos, Vector2 endPos) => endPos - startPos;

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

    public int GetLevel()
    {
        return _level;
    }

    public void LevelUp()
    {
        _level++;
    }

    public int GetCost()
    {
        return _turretData.scrapCost;
    }
}