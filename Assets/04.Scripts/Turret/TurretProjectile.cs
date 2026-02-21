using UnityEngine;

public abstract class TurretProjectile : MonoBehaviour
{
    protected Transform _target;
    protected int _damage;
    protected float _speed;

    protected bool _isActive;

    [SerializeField] protected float _lifetime = 8f;

    public void Initialize(Transform target, float speed, int damage)
    {
        _target = target;
        _speed = speed;
        _damage = damage;
        _isActive = true;
        Destroy(gameObject, _lifetime);
    }

    protected abstract void Move();

    protected virtual void DealDamage()
    {
        if (_target != null)
        {
            IDamageable enemy = _target.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
            }
        }
    }
}
