// NOTE: 바닥 아이템의 플레이어 흡수 이동 및 수집 처리를 담당하는 추상 베이스 클래스
using UnityEngine;

public abstract class ItemGroundedBase : MonoBehaviour, IItemGrounded
{
    [SerializeField] protected float _defaultSpeed = 5f;
    [SerializeField] protected float _acceleration = 10f;
    private float _currentSpeed = 0f;

    private bool _isMovingToPlayer = false;
    private Transform _collectorTransform;

    private void Update()
    {
        if (_isMovingToPlayer && _collectorTransform != null)
        {
            MoveToCollector();
        }
    }

    public virtual void Initialize()
    {
        _collectorTransform = null;
        _currentSpeed = 0f;
        _isMovingToPlayer = false;
    }

    public void CollectItem(Transform collectorTransform)
    {
        if (_isMovingToPlayer)
        {
            return;
        }
        _isMovingToPlayer = true;
        _collectorTransform = collectorTransform;
        _currentSpeed = _defaultSpeed;
    }

    private void MoveToCollector()
    {
        _currentSpeed += _acceleration * Time.deltaTime;
        Vector3 direction = (_collectorTransform.position - transform.position).normalized;
        transform.position += direction * _currentSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollectedByPlayer(collision);
            ReturnToPool();
        }
    }

    protected abstract void OnCollectedByPlayer(Collider2D playerColl);

    protected abstract void ReturnToPool();
}
