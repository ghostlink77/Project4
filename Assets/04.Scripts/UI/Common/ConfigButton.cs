using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum BtnType
{
    BGMSoundBtn,
    MasterSoundBtn,
    UISoundBtn,
    CharSoundBtn,
}



public class ConfigButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _selectedImage;
    [SerializeField] private Animator _selectedImageAnim;
    [SerializeField] private TextMeshProUGUI _btnNameText;
    [SerializeField] private TextMeshProUGUI _sliderValueText;
    public Slider Slider;

    [SerializeField] private bool _isFirst = false;
    [SerializeField] ConfigUI _configUIObject;

    [SerializeField] private BtnType _btnType;
    public BtnType BtnType { get { return _btnType; } }

    [SerializeField] private Color _OnSelectColor;
    [SerializeField] private Color _deSelectColor;

    private void Awake()
    {
        _configUIObject = GetComponentInParent<ConfigUI>();
        if (Slider != null)
            Slider.value = 1f;
    }

    private void Update()
    {
        _sliderValueText.text = ((int)(Slider.value * 10)).ToString();
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
    }

    private void ChangeButtonEffect(bool isSelected)
    {
        if (isSelected)
        {
            _selectedImage.enabled = true;
            _selectedImageAnim.Play("ShowBtn");
            _btnNameText.color = _OnSelectColor;
        }
        else
        {
            _selectedImage.enabled = false;
            _btnNameText.color = _deSelectColor;
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnDeselect(eventData);
    }
}
