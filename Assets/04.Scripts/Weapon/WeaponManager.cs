using System.Collections.Generic;
using Game.Types;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponManager : MonoBehaviour
{
    IObjectPool<GameObject> _projectilePool;
    
    [Header("투사체")]
    [SerializeField]
    private GameObject _projectilePrefab;
    
    [Header("무기 기본 데이터")]
    [SerializeField]
    WeaponStatData _baseStat;
    CircleCollider2D _weaponRangeCollider;
    
    #region 스크립트 참조변수
    // 스크립트 참조 변수
    WeaponShootController _weaponShootController;
    BulletController _bulletController;
    WeaponStatController _weaponStatController;
    WeaponEventController _weaponEventController;
    #endregion
    
    #region 무기 스탯 변수

    // 무기 스탯 변수
    int _dmg;
    float _atkSpeed, _projectileSpeed;
    #endregion

    #region 유니티 생명주기 함수
    void Awake()
    {
        _bulletController = InspectNullAndGetPrefabComponent();
        GetRequiredComponents();
    }

    void Start()
    {
    }

    void OnEnable()
    {
        _weaponEventController.OnStatChanged += HandleStatChanged;
    }

    void OnDisable()
    {
        _weaponEventController.OnStatChanged -= HandleStatChanged;
    }


    void Update()
    {
        _weaponShootController.ShootProcedurePerUpdate(_dmg, _atkSpeed, _projectileSpeed);
    }
    #endregion
    
    void HandleStatChanged(WeaponStat type)
    {
        if (type == WeaponStat.AtkSpeed)
        {
            _atkSpeed = _weaponStatController.AtkSpeed;
        }
        else if (type == WeaponStat.Atk)
        {
            _dmg = _weaponStatController.Atk;
        }
        else if (type == WeaponStat.ProjectileSpeed)
        {
            _projectileSpeed = _weaponStatController.ProjectileSpeed;
        }
    }

    // 필요한 스크립트들 참조하는 메서드
    void GetRequiredComponents()
    {
        _weaponShootController = GetComponent<WeaponShootController>();
        _weaponStatController = GetComponent<WeaponStatController>();
        _weaponEventController = GetComponent<WeaponEventController>();
        _weaponRangeCollider = GetComponent<CircleCollider2D>();

        
        // 스탯 먼저 초기화
        _weaponStatController.SetUp(_baseStat, _weaponRangeCollider);
        GetWeaponStats();
        
        // ShootController 초기화
        _weaponShootController.SetUp(_projectilePrefab);

        _projectilePool = _weaponShootController.ReturnObjectPool();
    }
    
    void GetWeaponStats()
    {
        _dmg = _weaponStatController.Atk;
        _atkSpeed = _weaponStatController.AtkSpeed;
        _projectileSpeed = _weaponStatController.ProjectileSpeed;
    }

    // 총알 프리팹이 있는지 확인하고, 안에 BulletController 스크립트까지 있는지 확인하는 메서드
    BulletController InspectNullAndGetPrefabComponent()
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