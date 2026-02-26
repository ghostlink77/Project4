// NOTE: 경험치 오브젝트, 플레이어 수집 시 경험치 추가
using UnityEngine;

public class ExpObject : ItemGroundedBase
{
    [SerializeField] private int _expAmount;
    public int ExpAmount
    {
        get => _expAmount;
        set => _expAmount = value;
    }

    protected override void OnCollectedByPlayer(Collider2D playerColl)
    {
        if (playerColl.TryGetComponent<PlayerLevelControl>(out PlayerLevelControl playerLevelControl))
        {
            playerLevelControl.AddXP(_expAmount);
            ReturnToPool();
        }
    }

    protected override void ReturnToPool()
    {
        ExpObjectSpawner.Instance.ReturnToPool(gameObject);
    }
}
