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
        PlayerManager.Instance.PlayerEventController.CallScrapCollected();
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
