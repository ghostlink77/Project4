using System.Collections.Generic;
using Game.Types;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(WeaponShootController))]
[RequireComponent(typeof(WeaponStatController))]
[RequireComponent(typeof(WeaponEventController))]
[RequireComponent(typeof(WeaponSoundController))]
public class WeaponManager : MonoBehaviour
{
    private IObjectPool<GameObject> _projectilePool;
    
    [Header("투사체")]
    [SerializeField]
    private GameObject _projectilePrefab;
    
    [Header("무기 기본 데이터")]
    [SerializeField]
    private WeaponStatData _baseStat;
    private CircleCollider2D _weaponRangeCollider;
    
    #region 스크립트 참조변수
    private WeaponShootController _weaponShootController;
    private BulletController _bulletController;
    private WeaponStatController _weaponStatController;
    private WeaponEventController _weaponEventController;
    private WeaponSoundController _weaponSoundController;
    #endregion
    
    #region 무기 스탯 변수
    private int _damage;
    private float _atkSpeed, _projectileSpeed;
    #endregion

    #region 유니티 생명주기 함수
    private void Awake()
    {
        _bulletController = InspectNullAndGetPrefabComponent();
        GetRequiredComponents();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        _weaponEventController.OnStatChanged += HandleStatChanged;
    }

    private void OnDisable()
    {
        _weaponEventController.OnStatChanged -= HandleStatChanged;
    }


    private void Update()
    {
        _weaponShootController.ShootProcedurePerUpdate(_damage, _atkSpeed, _projectileSpeed);
    }
    #endregion
    
    private void HandleStatChanged(WeaponStat type)
    {
        if (type == WeaponStat.AtkSpeed)
        {
            _atkSpeed = _weaponStatController.AtkSpeed;
        }
        else if (type == WeaponStat.Atk)
        {
            _damage = _weaponStatController.Atk;
        }
        else if (type == WeaponStat.ProjectileSpeed)
        {
            _projectileSpeed = _weaponStatController.ProjectileSpeed;
        }
    }

    private void GetRequiredComponents()
    {
        if (!TryGetComponent<WeaponShootController>(out _weaponShootController))
        Debug.Log($"{nameof(_weaponShootController)}가 null임");
        if (!TryGetComponent<WeaponStatController>(out _weaponStatController))
        Debug.Log($"{nameof(_weaponStatController)}가 null임");
        if (!TryGetComponent<WeaponEventController>(out _weaponEventController))
        Debug.Log($"{nameof(_weaponEventController)}가 null임");
        if (!TryGetComponent<CircleCollider2D>(out _weaponRangeCollider))
        Debug.Log($"{nameof(_weaponRangeCollider)}가 null임");
        if (!TryGetComponent<WeaponSoundController>(out _weaponSoundController))
        Debug.Log($"{nameof(_weaponSoundController)}가 null임");
        
        _weaponStatController.SetUp(_baseStat, _weaponRangeCollider);
        GetWeaponStats();
        
        _weaponShootController.SetUp(_projectilePrefab);
        _weaponSoundController.SetUp();

        _projectilePool = _weaponShootController.ReturnObjectPool();
    }
    
    private void GetWeaponStats()
    {
        _damage = _weaponStatController.Atk;
        _atkSpeed = _weaponStatController.AtkSpeed;
        _projectileSpeed = _weaponStatController.ProjectileSpeed;
    }

    private BulletController InspectNullAndGetPrefabComponent()
    {
        BulletController bulletController;
        if (_projectilePrefab == null)
        {
            Debug.LogError("총알 프리팹 할당되지 않음");
            return null;
        }
        _projectilePrefab.TryGetComponent<BulletController>(out bulletController);
        if (bulletController == null)
        {
            Debug.LogError("총알 프리팹에서 BulletController.cs 찾을 수 없음");
            return null;
        }
        return bulletController;
    }
}