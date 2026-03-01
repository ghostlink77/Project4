using System.Collections.Generic;
using UnityEngine;

public class BlackHoleTurret : TurretBase
{
    [SerializeField] private LayerMask _expObjectLayer;

    private BlackHoleTurretData _blackHoleData;
    private float _effectTimer;

    private GameObject _mergedExpObject;

    private HashSet<ExpObject> _absorbingObjects = new HashSet<ExpObject>();

    public override void Initialize(int level)
    {
        base.Initialize(level);
        _blackHoleData = TurretData as BlackHoleTurretData;
    }

    private void Update()
    {
        _effectTimer += Time.deltaTime;
        if (_effectTimer >= _blackHoleData.EffectCooldown[_level - 1])
        {
            AbsorbExpObjects();
            _effectTimer = 0f;
        }
    }

    private void AbsorbExpObjects()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, TurretData.Range[_level - 1], _expObjectLayer);
        Debug.Log($"BlackHoleTurret absorbed {hits.Length} exp objects.");

        foreach (Collider2D hit in hits)
        {
            ExpObject expObj = hit.GetComponent<ExpObject>();
            if (expObj == null) continue;
            if (expObj.gameObject == _mergedExpObject) continue;
            if (_absorbingObjects.Contains(expObj)) continue;

            _absorbingObjects.Add(expObj);
            expObj.SetOnArrivedCallback(OnExpObjectArrived);
            expObj.CollectItem(transform);
        }
    }

    private void OnExpObjectArrived(ItemGroundedBase item)
    {
        ExpObject expObj = item as ExpObject;
        if (expObj == null) return;

        float exp = expObj.ExpAmount;
        _absorbingObjects.Remove(expObj);
        ExpObjectSpawner.Instance.ReturnToPool(expObj.gameObject);

        if (_mergedExpObject != null && _mergedExpObject.activeInHierarchy)
        {
            ExpObject mergedExp = _mergedExpObject.GetComponent<ExpObject>();
            mergedExp.ExpAmount += exp;
        }
        else
        {
            _mergedExpObject = ExpObjectSpawner.Instance.SpawnExpObject(transform.position, exp);
            ExpObject mergedExp = _mergedExpObject.GetComponent<ExpObject>();
            mergedExp.ExpAmount = exp;
            mergedExp.SetOnReturnedToPoolCallback(() => _mergedExpObject = null);
        }
    }
}
