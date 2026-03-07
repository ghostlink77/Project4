/*
무기의 사격을 관리하는 스크립트
*/
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class WeaponShootController : MonoBehaviour
{
    private IObjectPool<GameObject> _projectilePool;
    private GameObject _bulletPrefab;
    private CircleCollider2D _weaponRangeCollider;
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private WeaponStatController _weaponStatController;
    private WeaponEventController _weaponEventController;

    private float _weaponDamage;
    private float _projectileCount;
    private float _atkCoolTime, _projectileSpeed = 0f;
    [SerializeField]
    private float _dispersionAngle = 10f;
    
    [Header("공격할 적 레이어")]
    [SerializeField]
    private LayerMask _enemyLayer;
    
    // 공격 방향을 확인하기 위한 임시 코드. 추후 삭제 필요
    #region 임시 추가 코드
    [SerializeField]
    private LineRenderer _lineRenderer;
    private Vector3 _firepoint;
    private float _range = 25f;
    
    private void Update()
    {
        UpdateAimLine();
    }
    
    private void UpdateAimLine()
    {
        _lineRenderer.SetPosition(0, gameObject.transform.position);
        _lineRenderer.SetPosition(1, _firepoint);
    }
    #endregion

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
        if (((1 << collision.gameObject.layer) & _enemyLayer) != 0)
        {
            enemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _enemyLayer) != 0)
        {
            enemiesInRange.Remove(collision.gameObject);
        }
    }

    public void ShootProcedurePerUpdate(float weaponDamage, float atkSpeed, float projectileSpeed)
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

    private void Shoot(float weaponDamage, float projSpeed)
    {
        _weaponDamage = weaponDamage;
        _projectileSpeed = projSpeed;
        _projectileCount = (int)_weaponStatController.ProjectileCount;
        for (int i = 0; i < _projectileCount; i++)
        {
            GameObject bullet = _projectilePool.Get();
            if (bullet.TryGetComponent<RicochetController>(out RicochetController ricochetController))
            {
                bullet.GetComponent<BulletController>().Penetratable = true;
            }
            if (_projectileCount >= 2)
            {
                float addedAngle = UnityEngine.Random.Range(-_dispersionAngle/2, _dispersionAngle/2);
                bullet.transform.right = Quaternion.Euler(0, 0, addedAngle) * bullet.transform.right;
            }
            bullet.SetActive(true);
        }
        _weaponEventController.CallOnShoot();
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
        // 공격 방향을 확인하기 위한 임시 코드. 추후 삭제 필요
        #region 임시 추가 코드
        _firepoint = targetPos;
        #endregion
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