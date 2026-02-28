using UnityEngine;

public abstract class TurretBase : MonoBehaviour, IDamageable, IItemStatController
{
    [SerializeField] private TurretData _turretData;
    public TurretData TurretData => _turretData;

    [SerializeField] protected CircleCollider2D _rangeCollider;

    protected int _currentHp;
    protected int _level = 1;

    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        _currentHp = _turretData.MaxHp[_level - 1];
        if (_rangeCollider != null)
        { 
            _rangeCollider.radius = _turretData.Range[_level - 1];
        }
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
        // TODO: 터렛이 파괴될 때의 로직 (예: 애니메이션, 사운드 등)
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
        _rangeCollider.radius = _turretData.Range[_level];
        _level++;
    }
}