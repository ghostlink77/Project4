using UnityEngine;

public class PlasmaTurretProjectile : TurretProjectile
{
    [SerializeField] private float _explosionRadius = 1.5f;
    [SerializeField] private LayerMask _enemyLayer;

    Vector2 _direction;

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
            _isActive = false;
            Explode();
        }
    }

    private void Explode()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            IDamageable damageableEnemy = enemy.GetComponent<IDamageable>();
            if (damageableEnemy != null)
            {
                DealDamage(damageableEnemy);
            }
        }
        Debug.Log($"Plasma explosion : {hitEnemies.Length}");
        Destroy(gameObject);
    }
}
