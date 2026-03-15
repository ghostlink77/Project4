using UnityEngine;
using UnityEngine.EventSystems;

public class PauseBtn : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Animator _anim;

    private readonly string SHOW = "Show";
    private readonly string HIDE = "Hide";

    public bool isFirst;

    private void OnEnable()
    {
        if (_anim != null && isFirst)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }     
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("버튼 비활성화.");
        _anim.Play(HIDE);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.delta.sqrMagnitude > 0.1f)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("버튼 활성화.");
        _anim.Play(SHOW);
    }
}
