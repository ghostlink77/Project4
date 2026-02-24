using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretSlotBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image _btnImage;

    private SelectedBtnControl _selectedBtnControl;  

    [Header("Effect Variable")]
    [SerializeField] private Color _pointerEnterColor = Color.white;
    [SerializeField] private Color _pointerExitColor = Color.white;
    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeEffect(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeEffect(false);
    }

    protected virtual void ChangeEffect(bool isTrue)
    {
        _btnImage.color = isTrue ? _pointerEnterColor : _pointerExitColor;
    }

    private void Awake()
    {
        _btnImage = GetComponent<Image>();
        _btnImage.color = _pointerExitColor;


    }
}
