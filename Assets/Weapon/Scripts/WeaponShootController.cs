/*
무기의 사격을 관리하는 스크립트
*/
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponShootController : MonoBehaviour
{
    private IObjectPool<GameObject> _projectilePool;
    GameObject bulletPrefab;
    GetEnemyAngle getEnemyAngle;
    
    // 스크립터블 오브젝트
    [Header("스크립터블 오브젝트")]
    [SerializeField]
    WeaponStatData weaponStatData;


    void Awake()
    {
        _projectilePool = new ObjectPool<GameObject>(
            createFunc: OnCreateBullet,         // 객체 생성 로직
            actionOnGet: OnGetBullet,           // 풀에서 꺼낼 때
            actionOnRelease: OnReleaseBullet,   // 풀에 반납할 때
            actionOnDestroy: OnDestroyBullet,   // 풀이 가득 찼을 때 파괴
            collectionCheck: true,              // 중복 반납 체크(에디터용)
            defaultCapacity: 10,                // 기본 크기
            maxSize: 50                         // 최대 크기
        );
    }

    void Start()
    {
        bulletPrefab = weaponStatData.ProjectilePrefab;
        getEnemyAngle = GetComponent<GetEnemyAngle>();
    }
    
    public void Shoot()
    {
        GameObject bullet = _projectilePool.Get();
    }
    
    // 여기 아래부터는 오브젝트 풀링 관련 함수들 정리
    private GameObject OnCreateBullet() => Instantiate(bulletPrefab);
    private void OnGetBullet(GameObject obj)
    {
        obj.SetActive(true);
        Vector2 projectileDirection = getEnemyAngle.FindCLosestEnemyDirection();
        obj.transform.right = projectileDirection;
    }
    private void OnReleaseBullet(GameObject obj) => obj.SetActive(false);
    private void OnDestroyBullet(GameObject obj) => Destroy(obj);
}