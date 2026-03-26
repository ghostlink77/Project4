using UnityEngine;
using UnityEngine.Pool;

public class PlasmaTurretProjectile : TurretProjectile
{
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private GameObject _explosionVFXPrefab;

    private static ObjectPool<GameObject> _vfxPool;
    private static GameObject _cachedVFXPrefab;


    private void Awake()
    {
        if (_vfxPool == null && _explosionVFXPrefab != null)
        {
            _cachedVFXPrefab = _explosionVFXPrefab;
            _vfxPool = new ObjectPool<GameObject>(
                createFunc: () => Object.Instantiate(_cachedVFXPrefab),
                actionOnGet: obj => obj.SetActive(true),
                actionOnRelease: obj => obj.SetActive(false),
                actionOnDestroy: obj => Object.Destroy(obj),
                collectionCheck: false,
                defaultCapacity: 4,
                maxSize: 8
            );
        }
    }

    protected override void Move()
    {
        transform.position += (Vector3)_direction * _speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && _isActive)
        {
            DisableProjectile();
        }
    }

    private void Explode()
    {
        SpawnExplosionVFX();

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
    }

    private void SpawnExplosionVFX()
    {
        if (_vfxPool == null) return;

        GameObject vfx = _vfxPool.Get();
        vfx.transform.position = transform.position;

        VFXAutoReturn autoReturn = vfx.GetComponent<VFXAutoReturn>();
        if (autoReturn != null)
        {
            autoReturn.Play(obj => _vfxPool.Release(obj));
        }
    }

    protected override void DisableProjectile()
    {
        base.DisableProjectile();
        Explode();
    }
}
