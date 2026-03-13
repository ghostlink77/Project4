/*
이펙트의 애니메이션 재생과 수명을 관리하는 스크립트.
애니메이션 재생이 끝나면 자동으로 파괴된다.
*/
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class EffectController : MonoBehaviour
{
    private Coroutine _releaseCoroutine;
    private Animator _animator;
    [SerializeField] private string _hitClipName;

    [Header("Settings")]
    [SerializeField] private float _defaultLifeTime = 1f;

    private WaitForSeconds _waitForSecondsUntilDelete;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        AudioManager.Instance.Play(AudioType.BulletSFX, _hitClipName);
    }

    private void OnEnable()
    {
        _releaseCoroutine = StartCoroutine(ReleaseRoutine());
    }

    private void OnDisable()
    {
        if (_releaseCoroutine != null)
        {
            StopCoroutine(_releaseCoroutine);
            _releaseCoroutine = null;
        }
    }

    private IEnumerator ReleaseRoutine()
    {
        // NOTE: 애니메이터가 상태를 완전히 인지할 때까지 한 프레임 대기
        yield return null;
        if (_waitForSecondsUntilDelete == null)
        {
            float length = _animator.GetCurrentAnimatorStateInfo(0).length;
            if (length <= 0) length = _defaultLifeTime;
            _waitForSecondsUntilDelete = new WaitForSeconds(length);
        }
        yield return _waitForSecondsUntilDelete;
        Destroy(gameObject);
    }
}
