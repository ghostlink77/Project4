/*
투사체 충돌 시 폭발 이펙트를 생성하고 범위 데미지를 적용하는 스크립트.
폭발 범위는 explosionPrefab에 부착된 CircleCollider2D의 radius를 기준으로 한다.
*/
using UnityEngine;

public class GenerateExplosion : MonoBehaviour
{
    private BulletController _bulletController;

    [SerializeField]
    private GameObject explosionPrefab;

    [Header("폭발 피해를 받는 레이어")]
    [SerializeField]
    private LayerMask _damageableLayer;

    [Header("플레이어 폭발 데미지 배율")]
    [SerializeField]
    private float _playerDamageMultiplier = 0.6f;

    private float _explosionRadius;

    private void Awake()
    {
        if (!gameObject.TryGetComponent<BulletController>(out _bulletController))
        {
            Debug.LogError("총알에 BulletController 컴포넌트 없음");
            return;
        }

        CacheExplosionRadius();
    }

    private void CacheExplosionRadius()
    {
        if (explosionPrefab == null)
        {
            Debug.LogError("explosionPrefab이 할당되지 않음");
            return;
        }

        if (explosionPrefab.TryGetComponent<CircleCollider2D>(out var collider))
        {
            // NOTE: CircleCollider2D.radius는 로컬 스페이스 값이므로 프리팹의 Scale을 반영해야 한다
            float scale = Mathf.Max(explosionPrefab.transform.localScale.x, explosionPrefab.transform.localScale.y);
            _explosionRadius = collider.radius * scale;
        }
        else
        {
            Debug.LogError("explosionPrefab에 CircleCollider2D가 없음");
        }
    }

    private void OnEnable()
    {
        if (_bulletController == null) return;
        _bulletController.OnHit += HandleExplosion;
    }

    private void OnDisable()
    {
        if (_bulletController == null) return;
        _bulletController.OnHit -= HandleExplosion;
    }

    private void HandleExplosion()
    {
        Vector2 explosionPosition = transform.position;
        ApplyAreaDamage(explosionPosition, _bulletController.ProjectileDmg);
        SpawnExplosionEffect(explosionPosition);
    }

    private void ApplyAreaDamage(Vector2 center, float damage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, _explosionRadius, _damageableLayer);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                if (hit.TryGetComponent<IDamageable>(out var target))
                {
                    target.TakeDamage(damage);
                    Debug.Log($"적이 폭발에 휘말림, damage: {damage}");
                }
            }
            else if (hit.CompareTag("Player"))
            {
                if (hit.TryGetComponent<PlayerStatController>(out var player))
                {
                    float reducedDamage = damage * _playerDamageMultiplier;
                    player.TakeDamage(reducedDamage);
                    Debug.Log($"플레이어가 폭발에 휘말림, damage: {reducedDamage}");
                }
            }
        }
    }

    private void SpawnExplosionEffect(Vector2 position)
    {
        Instantiate(explosionPrefab, position, Quaternion.identity);
    }
}
