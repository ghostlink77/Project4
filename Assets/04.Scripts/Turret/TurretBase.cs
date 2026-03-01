using UnityEngine;

public abstract class TurretBase : MonoBehaviour, IDamageable, IItemStatController
{
    [SerializeField] private TurretData _turretData;
    public TurretData TurretData => _turretData;

    protected int _currentHp;
    private const int StartLevel = 1;
    protected int _level = 1;
    private bool _isInitialized;

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

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;

        if (_currentHp <= 0)
        {
            OnDestroyed();
        }
    }

    protected virtual void OnDestroyed()
    {
        if (TurretManager.Instance != null)
        {
            TurretManager.Instance.UnregisterPlacedTurret(_turretData.GetName(), this);
        }
        Destroy(gameObject);
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