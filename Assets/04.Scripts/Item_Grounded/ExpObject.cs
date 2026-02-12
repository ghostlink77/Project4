using UnityEngine;

public class ExpObject : ItemGroundedBase
{
    [SerializeField] private int _expAmount;

    protected override void OnCollectedByPlayer()
    {
        Debug.Log($"Exp 획득: {_expAmount}");
    }

    protected override void ReturnToPool()
    {
        ExpObjectSpawner.Instance.ReturnToPool(gameObject);
    }
}
