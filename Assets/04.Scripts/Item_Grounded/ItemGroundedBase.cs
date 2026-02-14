// NOTE: 바닥 아이템의 플레이어 흡수 이동 및 수집 처리를 담당하는 추상 베이스 클래스
using UnityEngine;

public abstract class ItemGroundedBase : MonoBehaviour, IItemGrounded
{
    [SerializeField] protected float _defaultSpeed = 5f;
    [SerializeField] protected float _acceleration = 10f;
    private float _currentSpeed = 0f;

    private bool _isMovingToPlayer = false;
    private Transform _playerTransform;

    private void Update()
    {
        if (_isMovingToPlayer && _playerTransform != null)
        {
            MoveToPlayer();
        }
    }

    public virtual void Initialize()
    {
        _playerTransform = null;
        _currentSpeed = 0f;
        _isMovingToPlayer = false;
    }

    public void CollectItem(Transform playerTransform)
    {
        if (_isMovingToPlayer)
        {
            return;
        }
        _isMovingToPlayer = true;
        _playerTransform = playerTransform;
        _currentSpeed = _defaultSpeed;
    }

    private void MoveToPlayer()
    {
        _currentSpeed += _acceleration * Time.deltaTime;
        Vector3 direction = (_playerTransform.position - transform.position).normalized;
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
