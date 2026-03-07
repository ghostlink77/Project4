// 스크립트 사용과 관련해 문제가 발생했으며, 어떻게 고쳐야 할지 모르곘다.
// 인공지능은 Physics2D.OverlapCircleAll 이걸 사용해보라고 하는데, 이게 뭔지 모르기 때문에 조사하고 수정을 시도해봐야겠다.
/*
이 스크립트는 총알 게임 오브젝트에 붙어야 함.
현재 구상 중인 구현 방식은 다음과 같음:
활성화와 동시에 총알 관통을 활성화함.

먼저 가장 가까운 적을 찾도록 함.
적을 인식하면 도탄 횟수를 점검함.
이후 적에게 발사하기 위해 총알을 생성함.
생성한 총알에 공격력, 투사체 속도 스탯 주입.
생성한 총알의 ricochetController를 변수화해서 현재 도탄 횟수에 -1 적용
총알 이동 시작

일단 이 방법에서 예상되는 문제로는
- 총알이 생성되자마자 생성된 위치의 적에게 데미지를 입히는 문제
가 있다.

그래서 차라리 모양이 똑같이 생긴 "도탄 전용 투사체 프리팹"을 따로 구현하는 것도 좋지 않을까 하는 생각도 든다.
*/
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyFinder))]
public class RicochetController : MonoBehaviour
{
    [Header("최대 도탄 횟수")]
    [SerializeField]
    private int _maxRicochetNumber;
    public int MaxRicochetNumber {get => _maxRicochetNumber; set => _maxRicochetNumber = value;}
    
    private int _currentRicochetNumber;
    private BulletController _bulletController;
    
    
    [Header("도탄 사거리")]
    [SerializeField]
    private float _ricochetRange = 5f;
    
    private void Awake()
    {
        _bulletController = GetComponent<BulletController>();
    }

    private void OnEnable()
    {
        _bulletController.Penetratable = true;
        _currentRicochetNumber = _maxRicochetNumber;
        if (_currentRicochetNumber > 0) _bulletController.OnHit += Ricochet;
    }

    private void OnDisable()
    {
        _bulletController.OnHit -= Ricochet;
    }
    
    private void Ricochet(Collider2D hitTarget)
    {
        EnemyFinder enemyFinder;
        if (!TryGetComponent<EnemyFinder>(out enemyFinder))
        {
            Debug.LogError("EnemyFinder가 존재하지 않음");
            return;
        }
        enemyFinder.HitTarget = hitTarget;
        Transform closestEnemyTransform = enemyFinder.GetClosestEnemy(_ricochetRange);
        if (closestEnemyTransform == null)
        {
            Debug.LogError("주변에 적 없으므로 도탄 중지");
            return;
        }
        Debug.LogError($"주변에 가장 가까운 적 위치: {closestEnemyTransform.position}");

        Vector2 enemyDirection = (closestEnemyTransform.position - transform.position).normalized;
        Debug.LogError($"기존 / 바뀔 방향: {transform.right} / {enemyDirection}");
        gameObject.transform.right = enemyDirection;

        if (_currentRicochetNumber <= 1) _bulletController.Penetratable = false;
        _currentRicochetNumber--;
    }
}
