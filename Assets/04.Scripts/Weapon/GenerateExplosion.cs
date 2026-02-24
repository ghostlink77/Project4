using Unity.VisualScripting;
using UnityEngine;

public class GenerateExplosion : MonoBehaviour
{
    private Collider2D _bulletCollider;
    private BulletController _bulletController;
    [SerializeField]
    private GameObject explosionPrefab;

    private void Awake()
    {
        if (!gameObject.TryGetComponent<Collider2D>(out _bulletCollider)) Debug.LogError("총알에 Collider2D 없음");
        if (!gameObject.TryGetComponent<BulletController>(out _bulletController)) Debug.LogError("총알에 BulletController 컴포넌트 없음");
    }

    private void OnEnable()
    {
        if (_bulletController == null) Debug.LogWarning("이이");
        _bulletController.OnHit += GenerateExplosionOnBulletPosition;
    }

    private void OnDisable()
    {
        _bulletController.OnHit -= GenerateExplosionOnBulletPosition;
    }

    private void GenerateExplosionOnBulletPosition()
    {
        Vector2 bulletPosision = gameObject.transform.position;
        GameObject createdExplosion = Instantiate(explosionPrefab, bulletPosision, Quaternion.identity);
        createdExplosion.GetComponent<EffectController>().Init(_bulletController.ProjectileDmg);
    }
}
