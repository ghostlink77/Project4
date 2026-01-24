/*
무기의 사격을 관리하는 스크립트
*/
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponShootController : MonoBehaviour
{
    IObjectPool<GameObject> _projectilePool;
    GameObject _bulletPrefab;
    CircleCollider2D _weaponRangeCollider;
    List<GameObject> enemiesInRange = new List<GameObject>();
    WeaponStatController _weaponStatController;
    
    float _atkSpeed = 0f;
    float _atkCoolTime = 0f;
    
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
        _weaponStatController = GetComponent<WeaponStatController>();
        _weaponRangeCollider = GetComponent<CircleCollider2D>();
        _bulletPrefab = _weaponStatController.baseStat.ProjectilePrefab;
    }

    void Update()
    {
        if (_atkSpeed != _weaponStatController.AtkSpeed) SyncShootDuration();
        _atkCoolTime += Time.deltaTime;
        if (enemiesInRange.Count >= 1 && _atkCoolTime >= _atkSpeed) Shoot();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) enemiesInRange.Add(collision.gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) enemiesInRange.Remove(collision.gameObject);
    }

    void SyncShootDuration() => _atkSpeed = _weaponStatController.AtkSpeed;

    public void Shoot()
    {
        GameObject bullet = _projectilePool.Get();
        _atkCoolTime = 0f;
    }
    
    /*
    결과가 정상적이지 않을 경우 Vector2.zero를 출력하도록 코드를 작성했다.
    따라서 거리가 0이라면 사격을 실행하지 않도록 하는 코드를 사격 코드에 추가해야 할 것이다.
    */
    public Vector2 FindClosestTargetVector(Vector2 playerPos)
    {
        int length = enemiesInRange.Count;
        if (length == 0) return Vector2.zero;
        
        int smallestIndex = 0;
        float smallestDistance = float.MaxValue;
        
        Vector2 targetPos = Vector2.zero;
        for (int i = 0; i < length; i++)
        {
            Vector2 targetCandidatePos = enemiesInRange[i].transform.position;
            float oneEnemyDistance = GetDirectionVector(
                playerPos,
                targetCandidatePos
            ).sqrMagnitude;
            if (smallestDistance > oneEnemyDistance)
            {
                smallestIndex = i;
                targetPos = targetCandidatePos;
            }
        }
        return targetPos;
    }
    
    Vector2 GetGameObjectPositionVector(GameObject gameObj)
    {
        return gameObj.transform.position;
    }

    Vector2 GetDirectionVector(Vector2 startPos, Vector2 endPos)
    {
        return endPos - startPos;
    }

    // 여기 아래부터는 오브젝트 풀링 관련 함수들 정리
    private GameObject OnCreateBullet()
    {
        GameObject obj = Instantiate(_bulletPrefab);
        BulletController bulletController = obj.GetComponent<BulletController>();
        bulletController.SetDmg(_weaponStatController.Atk);
        if (bulletController != null) bulletController.SetProjectilePool(_projectilePool);
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
        obj.SetActive(true);
        // Debug.Log($"오브젝트 개수: {(_projectilePool as ObjectPool<GameObject>).CountAll}");
        BulletController bulletController = obj.GetComponent<BulletController>();
        bulletController.ProjectileSpeed = _weaponStatController.ProjectileSpeed;
    }

    private void OnReleaseBullet(GameObject obj)
    {
        obj.SetActive(false);
        obj.GetComponent<BulletController>().StopAllCoroutines();
    }
    private void OnDestroyBullet(GameObject obj) => Destroy(obj);
}