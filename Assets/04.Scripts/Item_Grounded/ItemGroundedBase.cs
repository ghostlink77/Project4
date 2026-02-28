// NOTE: 바닥 아이템의 플레이어 흡수 이동 및 수집 처리를 담당하는 추상 베이스 클래스
using UnityEngine;

public abstract class ItemGroundedBase : MonoBehaviour, IItemGrounded
{
    [SerializeField] protected float _defaultSpeed = 5f;
    [SerializeField] protected float _acceleration = 10f;
    [SerializeField] private float _arrivalDistance = 0.2f;
    private float _currentSpeed = 0f;

    private bool _isMovingTo = false;
    private Transform _collectorTransform;
    private System.Action<ItemGroundedBase> _onArrived;
    private System.Action _onReturnedToPool;

    private void Update()
    {
        if (_isMovingTo && _collectorTransform != null)
        {
            MoveToCollector();
        }
    }

    public virtual void Initialize()
    {
        _collectorTransform = null;
        _currentSpeed = 0f;
        _isMovingTo = false;
        _onArrived = null;
        _onReturnedToPool = null;
    }

    public void SetOnArrivedCallback(System.Action<ItemGroundedBase> onArrived)
    {
        _onArrived = onArrived;
    }

    public void SetOnReturnedToPoolCallback(System.Action onReturnedToPool)
    {
        _onReturnedToPool = onReturnedToPool;
    }

    public void CollectItem(Transform collectorTransform)
    {
        if (_isMovingTo)
        {
            return;
        }
        _isMovingTo = true;
        _collectorTransform = collectorTransform;
        _currentSpeed = _defaultSpeed;
    }

    private void MoveToCollector()
    {
        _currentSpeed += _acceleration * Time.deltaTime;
        Vector3 direction = (_collectorTransform.position - transform.position).normalized;
        transform.position += direction * _currentSpeed * Time.deltaTime;

        float distance = Vector2.Distance(transform.position, _collectorTransform.position);
        if (distance <= _arrivalDistance && _onArrived != null)
        {
            _onArrived.Invoke(this);
            _isMovingTo = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollectedByPlayer(collision);
            _onReturnedToPool?.Invoke();
            ReturnToPool();
        }
    }

    protected abstract void OnCollectedByPlayer(Collider2D playerColl);

    protected abstract void ReturnToPool();
}
