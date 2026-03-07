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

    [Header("Owner")]
    [SerializeField] private GameObject _UIowner;

    [Header("UI Position Value")]
    [SerializeField] private Vector3 _customPosition;
    [SerializeField] private bool _isFocus;
    private Vector3 _position;
    
    private void Awake()
    {
        _hpBar.fillAmount = FULL_AMOUNT;
        _hpAnimBar.fillAmount = FULL_AMOUNT;
    }
    private void Update()
    {
        if (!_isFocus) return;
        _position = _UIowner.transform.position;
        _position += _customPosition;
        transform.position = _position;
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
        yield return new WaitForSeconds(_blendInTime);

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
