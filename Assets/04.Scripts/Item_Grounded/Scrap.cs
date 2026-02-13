using UnityEngine;

public class Scrap : ItemGroundedBase
{
    private ScrapSpawner _spawner;

    public void Initialize(ScrapSpawner scrapSpawner)
    {
        Initialize();
        _spawner = scrapSpawner;
    }

    protected override void OnCollectedByPlayer(Collider2D playercoll)
    {
        // TODO: 고철 재화 획득
        TurretPlacer turretPlacer = playercoll.GetComponent<TurretPlacer>();
        turretPlacer.CollectScrap();

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
