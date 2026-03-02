using System.Threading;
using UnityEngine;

public abstract class TurretProjectile : MonoBehaviour
{
    protected Transform _target;
    protected float _damage;
    protected float _speed;
    protected string _projectileKey;

    protected bool _isActive;
    protected Vector2 _direction;

    [SerializeField] protected float _lifetime = 8f;
    private float _timer;

    public virtual void Initialize(Transform target, float speed, float damage, string key)
    {
        _target = target;
        _speed = speed;
        _damage = damage;
        _isActive = true;
        _projectileKey = key;
        _timer = 0f;
        SetDirectionAndRotation();
    }

    private void Start()
    {
        SetDirectionAndRotation();
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        Move();
        _timer += Time.deltaTime;
        if (_timer >= _lifetime)
        {
            DisableProjectile();
        }
    }

    private void SetDirectionAndRotation()
    {
        if (_target != null)
        {
            _direction = (_target.position - transform.position).normalized;

            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    protected abstract void Move();

    protected virtual void DealDamage(IDamageable target)
    {
        if (_target != null)
        {
            target.TakeDamage(_damage);
        }
    }

    protected virtual void DisableProjectile()
    {
        _isActive = false;
        TurretProjectileSpawner.Instance.ReturnToPool(_projectileKey, this);
    }
}
