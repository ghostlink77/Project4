using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _hpAnimBar;

    [Header("Anim Value")]
    [SerializeField] private float _blendInTime;
    [SerializeField] private float _animSpeed;
    private Coroutine _animCoroutine;
    private const float FULL_AMOUNT = 1f;
    private WaitForSeconds _waitForBlendInTime;

    [Header("Owner")]
    [SerializeField] private GameObject _UIOwner;

    [Header("UI Position Value")]
    [SerializeField] private Vector3 _customPosition;
    [SerializeField] private bool _isFocus;
    
    private void Awake()
    {
/*        _hpBar.fillAmount = FULL_AMOUNT;
        _hpAnimBar.fillAmount = FULL_AMOUNT;*/
        _waitForBlendInTime = new WaitForSeconds(_blendInTime);

    }

    private void OnEnable()
    {
        _hpBar.fillAmount = FULL_AMOUNT;
        _hpAnimBar.fillAmount = FULL_AMOUNT;
    }
    private void Update()
    {
        if (!_isFocus || !_UIOwner) return;
        FollowOwner();
    }

    private void FollowOwner()
    {
        transform.position = _UIOwner.transform.position + _customPosition;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position + _customPosition;
    }

    public void UpdateHpBar(float currentHp, float maxHp)
    {
        _hpBar.fillAmount = currentHp / maxHp;

        if (_animCoroutine != null)
        {
            StopCoroutine(_animCoroutine);
            _animCoroutine = null;
        }
        _animCoroutine = StartCoroutine(PlayHpBarAnimation());
    }

    private IEnumerator PlayHpBarAnimation()
    {
        Image animBar = _hpAnimBar;
        Image bar = _hpBar;
        yield return _waitForBlendInTime;

        while (animBar.fillAmount > bar.fillAmount)
        {
            animBar.fillAmount = Mathf.Lerp(
                animBar.fillAmount,
                bar.fillAmount,
                _animSpeed * Time.deltaTime
                );

            yield return null;
        }
    }
}
