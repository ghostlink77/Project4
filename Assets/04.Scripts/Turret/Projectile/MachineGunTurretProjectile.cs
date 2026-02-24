using UnityEngine;

public class MachineGunTurretProjectile : TurretProjectile
{

    protected override void Move()
    {
        transform.position += (Vector3)_direction * _speed * Time.deltaTime;
    }

    private void Start()
    {
        if (_target != null)
        {
            _direction = (_target.position - transform.position).normalized;

            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && _isActive)
        {
            IDamageable enemy = collision.GetComponent<IDamageable>();
            DealDamage(enemy);
            DisableProjectile();
        }
    }
}
