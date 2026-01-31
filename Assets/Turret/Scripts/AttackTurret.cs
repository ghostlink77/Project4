using UnityEngine;
using UnityEngine.Pool;

public class AttackTurret : MonoBehaviour
{
    IObjectPool<GameObject> _projectilePool;

    [SerializeField] 
    private GameObject _projectilePrefab;
    [SerializeField]
    private TurretData _turretStat;

    private CircleCollider2D _turretRangeCollider;

    private WeaponShootController _weaponShootController;
    private BulletController _bulletController;

    int _dmg;
    float _atkSpeed, _projectileSpeed;

    void Awake()
    {
        _bulletController = InspectNullAndGetPrefabComponent();
        GetRequiredComponents();
    }


    // Update is called once per frame
    void Update()
    {
        ProjectBullet();
    }

    public void UpdateTurretStat(TurretData turretData)
    {
        _turretStat = turretData;
    }

    private void ProjectBullet()
    {
        _dmg = _turretStat.damage;
        _atkSpeed = _turretStat.fireRate;
        _projectileSpeed = _turretStat.projectileSpeed;

        _weaponShootController.ShootProcedurePerUpdate(_dmg, _atkSpeed, _projectileSpeed);
    }

    private void GetRequiredComponents()
    {
        _weaponShootController = GetComponent<WeaponShootController>();
        _turretRangeCollider = GetComponent<CircleCollider2D>();


        // ShootController 초기화
        _weaponShootController.SetUp(_projectilePrefab);

        _projectilePool = _weaponShootController.ReturnObjectPool();
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
