/*
무기의 사격을 관리하는 스크립트
*/
using System;
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
    
    int _dmg;
    float _atkCoolTime, _projectileSpeed = 0f;

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
    
    // 초기화 메서드
    public void SetUp(GameObject bulletPrefab)
    {
        _bulletPrefab = bulletPrefab;
    }

    // 오브젝트 풀을 반환하는 메서드
    public IObjectPool<GameObject> ReturnObjectPool() => _projectilePool;

    // 공격 범위에 들어오면
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) enemiesInRange.Add(collision.gameObject);
    }

    // 공격 범위에서 나가면
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) enemiesInRange.Remove(collision.gameObject);
    }

    // 프레임당 실행해야 하는 사격 매커니즘
    public void ShootProcedurePerUpdate(int dmg, float atkSpeed, float projectileSpeed)
    {
        if (atkSpeed <= 0.01f)
        {
            Debug.LogError("공격속도가 지나치게 낮음");
            return; // 공격속도가 지나치게 낮으면 게임 터지는걸 막기 위해서 강제 종료
        }
        _atkCoolTime += Time.deltaTime;
        if (enemiesInRange.Count >= 1 && _atkCoolTime >= atkSpeed)
        {
            _atkCoolTime = 0f;
            Shoot(dmg, projectileSpeed);
        }
    }

    void Shoot(int dmg, float projSpeed)
    {
        _dmg = dmg;
        _projectileSpeed = projSpeed;
        GameObject bullet = _projectilePool.Get();
    }
    
    // 플레이어와 위치가 가장 가까운 타겟의 위치 벡터 구하는 방법
    /*
    결과가 정상적이지 않을 경우 Vector2.zero를 출력하도록 코드를 작성했다.
    따라서 거리가 0이라면 사격을 실행하지 않도록 하는 코드를 사격 코드에 추가해야 할 것이다.
    */
    Vector2 FindClosestTargetVector(Vector2 playerPos)
    {
        if (enemiesInRange.Count == 0) return Vector2.zero;
        
        int smallestIndex = 0;
        float smallestDistance = float.MaxValue;
        
        Vector2 targetPos = Vector2.zero;
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
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

    // 매개변수 시작 위치에서 매개변수 도착 위치 방향의 벡터 구하는 메서드
    Vector2 GetDirectionVector(Vector2 startPos, Vector2 endPos) => endPos - startPos;

    // 여기 아래부터는 오브젝트 풀링 관련 함수들 정리
    // 총알을 생성해야 할 때
    GameObject OnCreateBullet()
    {
        GameObject obj = Instantiate(_bulletPrefab);
        BulletController bulletController = obj.GetComponent<BulletController>();
        bulletController.SetDmg(_dmg);
        bulletController.SetProjectileSpeed(_projectileSpeed);
        if (bulletController != null) bulletController.SetProjectilePool(_projectilePool);
        return obj;
    }
    
    // 오브젝트 풀링에서 꺼낼 때
    void OnGetBullet(GameObject obj)
    {
        Vector2 playerPos = transform.position;
        obj.transform.position = playerPos;
        
        // 가장 가까운 타겟 찾기
        Vector2 targetPos = FindClosestTargetVector(playerPos);
        
        // 오류가 뜬 경우
        if (targetPos == Vector2.zero)
        {
            Debug.LogError("적 발견 불가");
            _projectilePool.Release(obj);
            return;
        }
        
        // 플레이어 위치를 시작점으로 하는 적 방향의 위치 벡터.
        Vector2 direction = GetDirectionVector(playerPos, targetPos).normalized;
        obj.transform.right = direction;
        
        // 총알 오브젝트 활성화
        obj.SetActive(true);
        
        // 총알 스탯 결정
        BulletController bulletController = obj.GetComponent<BulletController>();
        bulletController.SetDmg(_dmg);
        bulletController.SetProjectileSpeed(_projectileSpeed);
    }

    // 총알 없앨 때 적용할 메서드(오브젝트 풀로 다시 넣을 때)
    void OnReleaseBullet(GameObject obj)
    {
        obj.SetActive(false);
        obj.GetComponent<BulletController>().StopAllCoroutines();
    }
    
    // 총알 삭제할 때
    void OnDestroyBullet(GameObject obj) => Destroy(obj);
}