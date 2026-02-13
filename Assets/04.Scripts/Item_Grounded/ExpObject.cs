using UnityEngine;

public class ExpObject : ItemGroundedBase
{
    [SerializeField] private int _expAmount;

    protected override void OnCollectedByPlayer(Collider2D playerColl)
    {
        // TODO: 플레이어에게 Exp 추가 로직
        Debug.Log($"Exp 획득: {_expAmount}");
    }

    protected override void ReturnToPool()
    {
        ExpObjectSpawner.Instance.ReturnToPool(gameObject);
    }
}
