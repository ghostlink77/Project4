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
    [SerializeField] private float _expDropAmount = 1f;
    private int _currentHp;
    private bool _isLive;

    private Rigidbody2D _rigid;
    private Collider2D _collider;
    private Rigidbody2D _target;
    private EnemyTargetSetter _targetSetter;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    // 기절 관련
    private bool _isStunned;
    private float _stunTimer;
    private GameObject _currentStunVFX;
    private System.Action<GameObject> _onStunVFXReturn;

    public bool IsStunned => _isStunned;

    private static readonly int DeadHash = Animator.StringToHash("Dead");

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _targetSetter = GetComponentInChildren<EnemyTargetSetter>();
    }

    private void Update()
    {
        if (!_isLive) return;

        if (_isStunned)
        {
            _stunTimer -= Time.deltaTime;
            if (_stunTimer <= 0f)
            {
                EndStun();
            }
            return;
        }

        ElapseTime();
    }

    private void FixedUpdate()
    {
        if (_isStunned) return;

        TrackTarget();
        _rigid.linearVelocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (_target != null && _isLive && !_isStunned)
        {
            _spriteRenderer.flipX = _target.position.x < _rigid.position.x;
        }
    }

    public void Initialize(Rigidbody2D agitRigidbody)
    {
        _currentHp = _maxHp;
        _isLive = true;
        _collider.enabled = true;
        _isStunned = false;
        _stunTimer = 0f;
        ClearStunVFX();

        _animator.Play("Walk", 0, 0f);

        _targetSetter.Initialize(agitRigidbody);
        SetTarget(agitRigidbody);
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

    private void ElapseTime()
    {
        if (!_isLive)
            return;

        if (_currentDamageDelay > 0f)
        {
            _currentDamageDelay -= Time.deltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!_isLive || _isStunned)
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

    public void Stun(float duration)
    {
        if (!_isLive) return;

        _stunTimer = duration;

        if (!_isStunned)
        {
            _isStunned = true;
            _rigid.linearVelocity = Vector2.zero;
            _animator.speed = 0f;
        }
    }

    private void EndStun()
    {
        _isStunned = false;
        _animator.speed = 1f;
        ClearStunVFX();
    }

    public void SetStunVFX(GameObject vfx, System.Action<GameObject> onReturn)
    {
        _currentStunVFX = vfx;
        _onStunVFXReturn = onReturn;
    }

    private void ClearStunVFX()
    {
        if (_currentStunVFX != null)
        {
            _currentStunVFX.transform.SetParent(null);
            _onStunVFXReturn?.Invoke(_currentStunVFX);
            _currentStunVFX = null;
            _onStunVFXReturn = null;
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        InGameManager.Instance.InGameUIController.ShowDamageText(transform.position, damage);
        if (_currentHp <= 0 && _isLive)
        {
            Die();
        }
    }

    private void Die()
    {
        _isLive = false;
        _isStunned = false;
        _collider.enabled = false;
        _rigid.linearVelocity = Vector2.zero;
        _animator.speed = 1f;
        _animator.SetTrigger(DeadHash);
        ClearStunVFX();
    }

    // NOTE: 애니메이션이 끝난 후 Animation Event로 호출
    public void OnDeathAnimationEnd()
    {
        DropExpObject();
        EnemySpawner.Instance.ReturnToPool(_enemyType.ToString(), gameObject);
    }

    private void DropExpObject()
    {
        ExpObjectSpawner.Instance.SpawnExpObject(transform.position, _expDropAmount);
    }
}
