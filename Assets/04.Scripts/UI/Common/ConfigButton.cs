using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfigButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _selectedImage;
    [SerializeField] private Image _btnImage;
    [SerializeField] private Animator _selectedImageAnim;
    public Slider Slider;

    private void OnEnable()
    {
        ChangeButtonEffect(false);
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
        _selectedImage.enabled = isSelected;
        _btnImage.enabled = isSelected;

        _selectedImageAnim.enabled = isSelected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //EventSystem.current.SetSelectedGameObject(gameObject);
        ChangeButtonEffect(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeButtonEffect(false);
    }
}
