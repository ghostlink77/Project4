// NOTE: 고철 아이템, 플레이어 수집 시 터렛 건설 재화로 사용
using UnityEngine;

public class Scrap : ItemGroundedBase
{
    private ScrapSpawner _spawner;
    private float _lifetime;
    private Coroutine _despawnCoroutine;

    public void Initialize(ScrapSpawner scrapSpawner, float lifetime)
    {
        Initialize();
        _spawner = scrapSpawner;
        _lifetime = lifetime;

        if (_despawnCoroutine != null)
        {
            StopCoroutine(_despawnCoroutine);
        }
        _despawnCoroutine = StartCoroutine(DespawnAfterLifetime());
    }

    private System.Collections.IEnumerator DespawnAfterLifetime()
    {
        yield return new WaitForSeconds(_lifetime);
        _despawnCoroutine = null;
        ReturnToPool();
    }

    protected override void OnCollectedByPlayer(Collider2D playerColl)
    {
        if (_despawnCoroutine != null)
        {
            StopCoroutine(_despawnCoroutine);
            _despawnCoroutine = null;
        }
        PlayerManager.Instance.PlayerEventController.CallScrapCollected();
    }

    protected override void ReturnToPool()
    {
        if (_spawner == null)
        {
            return;
        }
        if (_despawnCoroutine != null)
        {
            StopCoroutine(_despawnCoroutine);
            _despawnCoroutine = null;
        }
        _spawner.ReturnToPool(gameObject);
    }
}
