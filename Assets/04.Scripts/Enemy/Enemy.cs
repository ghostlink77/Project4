// NOTE: 적 유닛의 이동, 전투, 사망 처리를 담당하는 스크립트
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int _damage;
    [SerializeField] private float _damageDelay;
    private float _currentDamageDelay;

    [SerializeField] private float _speed;
    [SerializeField] private int _maxHp;
    [SerializeField] private EnemyType _enemyType;
    private int _currentHp;
    private bool _isLive;

    private EnemySpawner _spawner;

    private Rigidbody2D _rigid;
    private CapsuleCollider2D _collider;
    private Rigidbody2D _target;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private static readonly int DeadHash = Animator.StringToHash("Dead");

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _spawner = EnemySpawner.Instance;
        _collider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (_isLive)
        {
            ElapseTime();
        }
    }

    private void FixedUpdate()
    {
        TrackTarget();
        _rigid.linearVelocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (_target != null && _isLive)
        {
            _spriteRenderer.flipX = _target.position.x < _rigid.position.x;
        }
    }

    public void Initialize()
    {
        _currentHp = _maxHp;
        _isLive = true;
        _collider.enabled = true;

        _animator.Play("Walk", 0, 0f);
    }

    private void TrackTarget()
    {
        if (_target != null && _isLive)
        {
            Vector2 direction = (_target.position - _rigid.position).normalized;
            Vector2 moveAmount = direction * _speed * Time.fixedDeltaTime;
            _rigid.MovePosition(_rigid.position + moveAmount);
        }
    }

    public void SetTarget(Rigidbody2D targetRigidbody)
    {
        _target = targetRigidbody;
    }

    // NOTE: 일정 시간마다 데미지 입히기 위한 시간 경과 처리
    private void ElapseTime()
    {
        if (!_isLive)
            return;

        if (_currentDamageDelay > 0f)
        {
            _currentDamageDelay -= Time.deltaTime;
        }
    }

    // NOTE: 적과 닿아있으면 지속적으로 데미지 입히기
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!_isLive)
            return;

        if (collision.collider.CompareTag("Agit") || collision.collider.CompareTag("Player"))
        {
            if (_currentDamageDelay <= 0f)
            {
                _currentDamageDelay = _damageDelay;
                collision.collider.GetComponent<IDamageable>()?.TakeDamage(_damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0 && _isLive)
        {
            Die();
        }
    }

    private void Die()
    {
        _isLive = false;
        _collider.enabled = false;
        _rigid.linearVelocity = Vector2.zero;
        _animator.SetTrigger(DeadHash);
    }

    // NOTE: 애니메이션이 끝난 후 Animation Event로 호출
    public void OnDeathAnimationEnd()
    {
        _spawner.ReturnToPool(_enemyType.ToString(), gameObject);
        DropExpObject();
    }

    private void DropExpObject()
    {
        ExpObjectSpawner.Instance.SpawnExpObject(transform.position);
    }
}
