using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class BulletAnimationController : MonoBehaviour
{
    #region 변수들
    [Header("총알 애니메이션")]
    [SerializeField]
    private AnimationClip _bulletAnimationClip;
    public AnimationClip BulletAnimationClip {get => _bulletAnimationClip; private set => _bulletAnimationClip = value;}
    
    private BulletController _bulletController;
    private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool _deleteAfterAnimation;
    #endregion
    
    #region 이벤트
    private void AddEvent()
    {
        _bulletController.OnHit += OnEventHit;
    }
    
    private void RemoveEvent()
    {
        _bulletController.OnHit -= OnEventHit;

    }
    
    private void OnEventHit()
    {
        HideBulletSprite();
    }
    
    private void HideBulletSprite()
    {
        Debug.Log("총알 끔");
        _spriteRenderer.enabled = false;
        _collider2D.enabled = false;
        gameObject.SetActive(false);
    }
    
    private void ShowBulletSprite()
    {
        Debug.Log("총알 보임");
        _spriteRenderer.enabled = true;
        _collider2D.enabled = true;
        gameObject.SetActive(true);
    }
    
    private void OnBulletGenerate()
    {
        if (_bulletAnimationClip == null)
        {
            Debug.Log("애니메이션 클립 없음");
            return;
        }
        ShowBulletSprite();
        
        if(_animator != null) _animator.Play(_bulletAnimationClip.name, 0, 0f);
    }
    
    private void OnBulletRemove()
    {
        HideBulletSprite();
    }
    #endregion

    #region 유니티 생명주기 메서드
    private void Awake()
    {
        if (!TryGetComponent<BulletController>(out _bulletController)) Debug.Log($"{nameof(_bulletController)}가 null임");
        if (!TryGetComponent<Collider2D>(out _collider2D)) Debug.Log($"{nameof(_collider2D)}가 null임");
        if (!TryGetComponent<SpriteRenderer>(out _spriteRenderer)) Debug.Log($"{nameof(_spriteRenderer)}가 null임");
        if (!TryGetComponent<Animator>(out _animator)) Debug.Log($"{nameof(_animator)}가 null임");
        if (_deleteAfterAnimation && _bulletAnimationClip != null)
        _deleteAfterAnimation = _bulletController.DeleteAfterAnimation;
        _bulletController.SetLifeTime(_bulletAnimationClip.length);
    }

    private void OnEnable()
    {
        OnBulletGenerate();
        AddEvent();
    }

    private void OnDisable()
    {
        RemoveEvent();
        OnBulletRemove();
    }
    #endregion
}
