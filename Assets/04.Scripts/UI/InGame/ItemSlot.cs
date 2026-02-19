using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _description;

    public readonly string IMAGE_PATH = "Sprite";

    public void SetSlot(string name, int level, string description)
    {
        _name.text = name;
        _level.text = level.ToString();
        _image.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/{name}");
        _image.color = Color.white;
        _description.text = description;
    }
}
