/*
무기의 사격을 관리하는 스크립트
*/
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponShootController : MonoBehaviour
{
    private IObjectPool<GameObject> _projectilePool;
    private GameObject _bulletPrefab;
    private CircleCollider2D _weaponRangeCollider;
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private WeaponStatController _weaponStatController;
    private WeaponEventController _weaponEventController;
    
    private int _weaponDamage, _projectileCount;
    private float _atkCoolTime, _projectileSpeed = 0f;
    [SerializeField]
    private float _dispersionAngle = 10f;


    private void Awake()
    {   
        _projectilePool = new ObjectPool<GameObject>(
            createFunc: OnCreateBullet,
            actionOnGet: OnGetBullet,
            actionOnRelease: OnReleaseBullet,
            actionOnDestroy: OnDestroyBullet,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 50
        );
    }
    
    public void SetUp(GameObject bulletPrefab)
    {
        _bulletPrefab = bulletPrefab;
        _weaponStatController = GetComponent<WeaponStatController>();
        _weaponEventController = GetComponent<WeaponEventController>();
    }

    public IObjectPool<GameObject> ReturnObjectPool() => _projectilePool;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) enemiesInRange.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) enemiesInRange.Remove(collision.gameObject);
    }

    public void ShootProcedurePerUpdate(int weaponDamage, float atkSpeed, float projectileSpeed)
    {
        if (atkSpeed <= 0.01f)
        {
            Debug.LogError("공격속도가 지나치게 낮음");
            return;
        }
        _atkCoolTime += Time.deltaTime;
        if (enemiesInRange.Count >= 1 && _atkCoolTime >= atkSpeed)
        {
            _atkCoolTime = 0f;
            Shoot(weaponDamage, projectileSpeed);
        }
    }

    private void Shoot(int weaponDamage, float projSpeed)
    {
        _weaponDamage = weaponDamage;
        _projectileSpeed = projSpeed;
        _projectileCount = (int)_weaponStatController.ProjectileCount;
        for (int i = 0; i < _projectileCount; i++)
        {
            GameObject bullet = _projectilePool.Get();
            if (_projectileCount >= 2)
            {
                float addedAngle = UnityEngine.Random.Range(-_dispersionAngle/2, _dispersionAngle/2);
                bullet.transform.right = Quaternion.Euler(0, 0, addedAngle) * bullet.transform.right;
            }
            bullet.SetActive(true);
        }
    }
    
    private Vector2 FindClosestTargetVector(Vector2 playerPos)
    {
        if (enemiesInRange.Count == 0) return Vector2.zero;
        
        int smallestIndex = 0;
        float smallestDistance = float.MaxValue;
        
        Vector2 targetPos = Vector2.zero;
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            if (enemiesInRange[i] == null) continue;
            Vector2 targetCandidatePos = enemiesInRange[i].transform.position;
            float oneEnemyDistance = GetDirectionVector(playerPos,targetCandidatePos).sqrMagnitude;
            if (smallestDistance > oneEnemyDistance)
            {
                smallestIndex = i;
                targetPos = targetCandidatePos;
            }
        }
        return targetPos;
    }

    private Vector2 GetDirectionVector(Vector2 startPos, Vector2 endPos) => endPos - startPos;

    private GameObject OnCreateBullet()
    {
        GameObject obj = Instantiate(_bulletPrefab);
        return obj;
    }
    
    private void OnGetBullet(GameObject obj)
    {
        Vector2 playerPos = transform.position;
        obj.transform.position = playerPos;
        
        Vector2 targetPos = FindClosestTargetVector(playerPos);
        
        if (targetPos == Vector2.zero)
        {
            Debug.LogError("적 발견 불가");
            _projectilePool.Release(obj);
            return;
        }
        
        Vector2 direction = GetDirectionVector(playerPos, targetPos).normalized;
        obj.transform.right = direction;
        
        if (obj.TryGetComponent<BulletController>(out var bulletController))
        {
            bulletController.GetNeededVariableForAttack(_weaponDamage, _projectileSpeed, _projectilePool, _weaponEventController);
        }
    }

    private void OnReleaseBullet(GameObject obj)
    {
        obj.SetActive(false);
    }
    
    private void OnDestroyBullet(GameObject obj) => Destroy(obj);
}