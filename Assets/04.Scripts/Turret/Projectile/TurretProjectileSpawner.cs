using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class TurretProjectileSpawner : SingletonBehaviour<TurretProjectileSpawner>
{
    private const int MaxSize = 18;
    private const int InitSize = 6;

    private Dictionary<string, ObjectPool<TurretProjectile>> _projectilePools = new Dictionary<string, ObjectPool<TurretProjectile>>();

    private void CreatePool(string projectileKey, GameObject prefab)
    {
        var pool = new ObjectPool<TurretProjectile>(
            createFunc: () => Instantiate(prefab).GetComponent<TurretProjectile>(),
            actionOnGet: ActivateProjectile,
            actionOnRelease: DisableProjectile,
            actionOnDestroy: DestroyProjectile,
            collectionCheck: false,
            defaultCapacity: InitSize,
            maxSize: MaxSize
        );
        _projectilePools[projectileKey] = pool;
    }

    public async Task LoadProjectilePrefab(string projectileKey)
    {
        AsyncOperationHandle<GameObject> handle =
            Addressables.LoadAssetAsync<GameObject>($"Prefabs/Turret/Projectiles/{projectileKey}");

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"투사체 프리팹 로드 완료: {projectileKey}");
            CreatePool(projectileKey, handle.Result);
        }
        else
        {
            Debug.LogError($"투사체 프리팹 로드 실패: {projectileKey}");
        }
    }

    public TurretProjectile SpawnProjectile(string projectileKey, Vector3 position)
    {
        if (_projectilePools.TryGetValue(projectileKey, out var pool))
        {
            TurretProjectile projectile = pool.Get();
            projectile.transform.position = position;
            return projectile;
        }
        else
        {
            Debug.LogError($"투사체 풀을 찾을 수 없습니다: {projectileKey}");
            return null;
        }
    }

    public void ReturnToPool(string projectileKey, TurretProjectile projectile)
    {
        if (_projectilePools.TryGetValue(projectileKey, out var pool))
        {
            pool.Release(projectile);
        }
        else
        {
            Debug.LogError($"투사체 풀을 찾을 수 없습니다: {projectileKey}");
            Destroy(projectile.gameObject);
        }
    }

    private void ActivateProjectile(TurretProjectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }
    private void DisableProjectile(TurretProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }
    private void DestroyProjectile(TurretProjectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
