// NOTE: 경험치 오브젝트, 플레이어 수집 시 경험치 추가
using UnityEngine;

public class ExpObject : ItemGroundedBase
{
    public enum ExpTier
    {
        Small,
        Medium,
        Large
    }

    [Header("경험치 설정")]
    [SerializeField] private float _expAmount;

    [Header("등급별 경험치 임계값")]
    [SerializeField] private float _mediumThreshold = 5f;
    [SerializeField] private float _largeThreshold = 15f;

    [Header("등급별 색상")]
    [SerializeField] private Color _smallColor = new Color(0f, 1f, 0.9f, 1f);
    [SerializeField] private Color _mediumColor = new Color(0.6f, 0.3f, 1f, 1f);
    [SerializeField] private Color _largeColor = new Color(1f, 0.85f, 0.2f, 1f);

    [Header("Glow 이펙트")]
    [SerializeField] private float _glowSpeed = 3f;
    [SerializeField] private float _glowMinIntensity = 0.6f;
    [SerializeField] private float _glowMaxIntensity = 1.2f;

    private SpriteRenderer _spriteRenderer;
    private Color _baseColor;
    private Vector3 _spawnPosition;
    private float _glowTimer;
    private ExpTier _currentTier;

    public float ExpAmount
    {
        get => _expAmount;
        set => _expAmount = value;
    }

    public ExpTier CurrentTier => _currentTier;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(float amount)
    {
        base.Initialize();
        _expAmount = amount;
        _glowTimer = 0f;
        _spawnPosition = transform.position;

        DetermineExpTier(amount);
        ApplyTierColor();
    }

    private void DetermineExpTier(float amount)
    {
        if (amount >= _largeThreshold)
        {
            _currentTier = ExpTier.Large;
        }
        else if (amount >= _mediumThreshold)
        {
            _currentTier = ExpTier.Medium;
        }
        else
        {
            _currentTier = ExpTier.Small;
        }
    }

    private void ApplyTierColor()
    {
        if (_spriteRenderer == null)
        {
            return;
        }

        _baseColor = _currentTier switch
        {
            ExpTier.Large => _largeColor,
            ExpTier.Medium => _mediumColor,
            _ => _smallColor
        };

        _spriteRenderer.color = _baseColor;
    }

    // NOTE: LateUpdate를 사용하여 ItemGroundedBase의 Update(자석 이동)와 충돌 방지
    private void LateUpdate()
    {
        UpdateGlow();
    }

    private void UpdateGlow()
    {
        if (_spriteRenderer == null)
        {
            return;
        }

        _glowTimer += Time.deltaTime * _glowSpeed;
        float intensity = Mathf.Lerp(
            _glowMinIntensity,
            _glowMaxIntensity,
            (Mathf.Sin(_glowTimer) + 1f) * 0.5f
        );
        _spriteRenderer.color = _baseColor * intensity;
    }

    protected override void OnCollectedByPlayer(Collider2D playerColl)
    {
        if (playerColl.TryGetComponent<PlayerLevelControl>(out PlayerLevelControl playerLevelControl))
        {
            playerLevelControl.AddXP(_expAmount);
        }
    }

    protected override void ReturnToPool()
    {
        ExpObjectSpawner.Instance.ReturnToPool(gameObject);
    }
}
