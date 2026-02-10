using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LobbyButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _buttonImage;
    private Animator Anim;

    [SerializeField] private bool _isFirstSelected;

    //private readonly Color _selectedTextColor = new Color(46, 0, 0, 255);
    private readonly Color _selectedTextColor = Color.gold;
    private readonly Color _deselectedTextColor = Color.cyan;

    private void Awake()
    {
        Anim = GetComponent<Animator>();

        //ChangeButtonEffect(false);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //ChangeButtonEffect(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        //ChangeButtonEffect(true);
    }

    private void ChangeButtonEffect(bool isSelected)
    {
        Color textColor = isSelected? _selectedTextColor : _deselectedTextColor;

        _text.color = textColor;
        _buttonImage.enabled = isSelected;

        Anim.enabled = isSelected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeButtonEffect(true);
        //EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeButtonEffect(false);
        //throw new System.NotImplementedException();
    }
}