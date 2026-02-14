using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _selectedImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _selectedImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _selectedImage.SetActive(false);
    }
}
