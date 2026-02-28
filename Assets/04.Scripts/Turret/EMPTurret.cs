using UnityEngine;
using UnityEngine.Pool;

public class EMPTurret : TurretBase
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private GameObject _stunVFXPrefab;

    private EMPTurretData _empData;
    private float _effectTimer;

    static private ObjectPool<GameObject> _stunVFXPool;
    private const int MaxVFXCount = 16;
    private const int DefaultVFXCount = 4;

    public override void Initialize()
    {
        base.Initialize();
        _empData = TurretData as EMPTurretData;

        if (_stunVFXPool == null && _stunVFXPrefab != null)
        {
            _stunVFXPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(_stunVFXPrefab),
                actionOnGet: obj => obj.SetActive(true),
                actionOnRelease: obj =>
                {
                    obj.transform.SetParent(null);
                    obj.SetActive(false);
                },
                actionOnDestroy: obj => Destroy(obj),
                collectionCheck: false,
                defaultCapacity: DefaultVFXCount,
                maxSize: MaxVFXCount
            );
        }
    }

    private void Update()
    {
        _effectTimer += Time.deltaTime;
        if (_effectTimer >= _empData.EffectCooldown[_level - 1])
        {
            EmitEMP();
            _effectTimer = 0f;
        }
    }

    private void EmitEMP()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, TurretData.Range[_level - 1], _enemyLayer);

        float stunDuration = _empData.StunDuration[_level - 1];

        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null) continue;

            if (!enemy.IsStunned)
            {
                AttachStunVFX(enemy);
            }

            enemy.Stun(stunDuration);
        }
    }

    private void AttachStunVFX(Enemy enemy)
    {
        if (_stunVFXPool == null) return;

        GameObject vfx = _stunVFXPool.Get();
        vfx.transform.SetParent(enemy.transform);
        vfx.transform.localPosition = Vector3.zero;

        enemy.SetStunVFX(vfx, obj => _stunVFXPool.Release(obj));
    }
}
