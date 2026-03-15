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



public class ConfigButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    [SerializeField] private Image _selectedImage;
    [SerializeField] private Animator _selectedImageAnim;
    [SerializeField] private TextMeshProUGUI _btnNameText;
    [SerializeField] private TextMeshProUGUI _sliderValueText;
    public Slider Slider;

    [SerializeField] private bool _isFirst;

    [SerializeField] private BtnType _btnType;
    public BtnType BtnType { get { return _btnType; } }

    [SerializeField] private Color _OnSelectColor;
    [SerializeField] private Color _deSelectColor;

    private static readonly int SHOW_BTN = Animator.StringToHash("showBtn");
    private void Awake()
    {
        if (Slider != null)
            Slider.value = 1f;

        SetValueText();
    }

    public void SetValueText()
    {
        _sliderValueText.text = ((int)(Slider.value * 10)).ToString();
    }

    private void OnEnable()
    {
        ChangeButtonEffect(false);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        ChangeButtonEffect(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        ChangeButtonEffect(true);
    }

    private void ChangeButtonEffect(bool isSelected)
    {
        if (isSelected)
        {
            _selectedImage.enabled = true;
            _selectedImageAnim.Play(SHOW_BTN);
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
        if (eventData.delta.sqrMagnitude > 0.1f)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
