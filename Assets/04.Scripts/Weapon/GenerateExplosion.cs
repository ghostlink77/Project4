using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class GenerateExplosion : MonoBehaviour
{
    private Collider2D _bulletCollider;
    private BulletController _bulletController;
    [SerializeField]
    private GameObject explosionPrefab;
    private IObjectPool<GameObject> _projectilePool;
    public IObjectPool<GameObject> ProjectilePool {get => _projectilePool;}

    private void Awake()
    {
        if (!gameObject.TryGetComponent<Collider2D>(out _bulletCollider)) Debug.LogError("총알에 Collider2D 없음");
        if (!gameObject.TryGetComponent<BulletController>(out _bulletController)) Debug.LogError("총알에 BulletController 컴포넌트 없음");
        if (_bulletCollider != null) _projectilePool = _bulletController.ProjectilePool;
    }

    private void OnEnable()
    {
        if (_bulletController == null) Debug.LogWarning("bulletController가 null임");
        _bulletController.OnHit += GenerateExplosionOnBulletPosition;
    }

    private void OnDisable()
    {
        _bulletController.OnHit -= GenerateExplosionOnBulletPosition;
    }

    private void GenerateExplosionOnBulletPosition(Collider2D hitTarget)
    {
        Vector2 bulletPosision = gameObject.transform.position;
        GameObject createdExplosion = Instantiate(explosionPrefab, bulletPosision, Quaternion.identity);
        EffectController effectController = createdExplosion.GetComponent<EffectController>();
    }
}
