using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class LevelUpBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _selectedImage;
    [SerializeField] private ParticleSystem _selectedParticle;

    private RectTransform _rect;
    private Camera _uiCamera;

    [SerializeField] private float _selectedSize = 1.3f;
    [SerializeField] private float _deSelectedSize = 1.0f;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            Debug.Log(canvas.gameObject.name);
            _uiCamera = canvas.worldCamera;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(CheckInitialMousePos());
    }

    private IEnumerator CheckInitialMousePos()
    {
        // 생명주기 때문에 Rect값을 변경해도 Unity 상에서 Ui요소들을 화면에 배치하기 위해 내부적으로 레이아웃을 초기화시키기 때문에 효과가 보이지 않는다고 하네요 ㅇㅇ;;
        yield return null;

        if (_rect == null || _uiCamera == null)
        {
            Debug.Log("Rect Transform 또는 UICamera가 없습니다.");
            yield break;
        }

            Vector2 mousePos = Input.mousePosition;

        if (RectTransformUtility.RectangleContainsScreenPoint(_rect, mousePos, _uiCamera))
        {
            ShowSelectAnim();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowSelectAnim();
    }

    private void ShowSelectAnim()
    {
        //_selectedImage.SetActive(true);
        _selectedParticle.Play();

        _rect.localScale = Vector3.one * _selectedSize;
        AudioManager.Instance.Play(AudioType.UISFX, "ItemSelectBtn_PointEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _selectedImage.SetActive(false);
        _selectedParticle.Stop();
        _selectedParticle.Clear();

        _rect.localScale = Vector3.one * _deSelectedSize;
    }
}
