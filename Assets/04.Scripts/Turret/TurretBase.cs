using System;
using UnityEngine;

public abstract class TurretBase : MonoBehaviour, IDamageable, IItemStatController
{
    [SerializeField] private TurretData _turretData;
    public TurretData TurretData => _turretData;

    protected float _currentHp;
    private const int StartLevel = 1;
    protected int _level = 1;
    private bool _isInitialized;

    public event Action DestroyTurret;

    protected virtual void Start()
    {
        if (!_isInitialized)
        {
            Initialize(StartLevel);
        }
    }

    public virtual void Initialize(int level)
    {
        _level = level;
        _currentHp = _turretData.MaxHp[_level - 1];
        _isInitialized = true;
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;

        InGameManager.Instance.InGameUIController.UpdateTurretHpBar(transform.parent, _currentHp, (float)_turretData.MaxHp[_level-1]);
        
        if (_currentHp <= 0)
        {
            OnDestroyed();
            InGameManager.Instance.InGameUIController.RemoveTurretHpBar(transform.parent);
        }
    }

    protected virtual void OnDestroyed()
    {
        // TODO: 파괴 효과, 사운드 등 추가
        if (TurretManager.Instance != null)
        {
            TurretManager.Instance.UnregisterPlacedTurret(_turretData.GetName(), this);
        }
        //Destroy(gameObject);
        DestroyTurret?.Invoke();

    }

    public int GetLevel()
    {
        return _level;
    }

    public virtual void LevelUp()
    {
        if(_level >= _turretData.MaxLevel)
        {
            Debug.Log("최대 레벨에 도달했습니다.");
            return;
        }
        _currentHp = _turretData.MaxHp[_level];
        _level++;
    }

    public virtual void SetLevel(int level)
    {
        if (level < 1 || level > _turretData.MaxLevel)
        {
            Debug.LogWarning($"SetLevel: 유효하지 않은 레벨 {level} (max: {_turretData.MaxLevel})");
            return;
        }
        _level = level;
        _currentHp = _turretData.MaxHp[_level - 1];
    }

    public int GetCost()
    {
        return _turretData.ScrapCost;
    }
}