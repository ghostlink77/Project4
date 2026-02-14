using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfigButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _selectedImage;
    [SerializeField] private Image _btnImage;
    [SerializeField] private Animator _selectedImageAnim;
    public Slider Slider;

    [SerializeField] private bool _isFirst = false;
    [SerializeField] ConfigUI _configUIObject;

    private void Awake()
    {
        _configUIObject = GetComponentInParent<ConfigUI>();
    }

    private void OnEnable()
    {
        if (_isFirst) EventSystem.current.SetSelectedGameObject(gameObject);
        else ChangeButtonEffect(false);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        ChangeButtonEffect(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        ChangeButtonEffect(true);
        if (_configUIObject != null)
            _configUIObject.SelectBtn();
        _selectedImageAnim.enabled = true;
    }

    private void ChangeButtonEffect(bool isSelected)
    {
        _selectedImage.enabled = isSelected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _btnImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _btnImage.enabled = false;
    }
}
