// NOTE: 플레이어 주변 아이템 자동 수집 범위를 담당하는 스크립트
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private float _collectRadius = 1f;
    private CircleCollider2D _collider;

    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IItemGrounded>(out IItemGrounded item))
        {
            item.CollectItem(transform);
        }
    }

    public void SetCollectRadius(float radius)
    {
        _collectRadius = radius;
        if (_collider != null)
        {
            _collider.radius = _collectRadius;
        }
    }
}
