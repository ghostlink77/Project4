// NOTE: 고철 아이템, 플레이어 수집 시 터렛 건설 재화로 사용
using UnityEngine;

public class Scrap : ItemGroundedBase
{
    private ScrapSpawner _spawner;

    public void Initialize(ScrapSpawner scrapSpawner)
    {
        Initialize();
        _spawner = scrapSpawner;
    }

    protected override void OnCollectedByPlayer(Collider2D playerColl)
    {
        // TODO: 고철 재화 획득
        TurretPlacer turretPlacer = playerColl.GetComponent<TurretPlacer>();
        turretPlacer.CollectScrap();
    }

    protected override void ReturnToPool()
    {
        if (_spawner == null)
        {
            return;
        }
        _spawner.ReturnToPool(gameObject);
    }
}
