using UnityEngine;

public class Scrap : ItemGroundedBase
{
    private ScrapSpawner _spawner;

    public void Initialize(ScrapSpawner scrapSpawner)
    {
        Initialize();
        _spawner = scrapSpawner;
    }

    protected override void OnCollectedByPlayer()
    {
        // TODO: 고철 재화 획득
        Debug.Log("고철 획득");
    }

    protected override void ReturnToPool()
    {
        if(_spawner == null)
        {
            return;
        }
        _spawner.ReturnToPool(gameObject);
    }
}
