using UnityEngine;

public class ExpObject : MonoBehaviour, IItemGrounded
{
    [SerializeField] private int _expAmount;

    [SerializeField] private float _defaultSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;
    private float _currentSpeed = 0f;

    private bool _isMovingToPlayer = false;
    private ExpObjectSpawner _spawner;
    private Transform _playerTransform;

    private void Update()
    {
        if (_isMovingToPlayer && _playerTransform != null)
        {
            MoveToPlayer();
        }
    }
    public void Initialize(ExpObjectSpawner expObjectSpawner)
    {
        _spawner = expObjectSpawner;
        _playerTransform = null;
        _currentSpeed = _defaultSpeed;
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

    }

    private void MoveToPlayer()
    {
        _currentSpeed += _acceleration * Time.deltaTime;
        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        transform.position += direction * _currentSpeed * Time.deltaTime;
    }


    // TODO: 플레이어 경험치 증가
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _spawner.ReturnToPool(gameObject);
            Debug.Log($"Player gained {_expAmount} EXP.");
        }
    }
}
