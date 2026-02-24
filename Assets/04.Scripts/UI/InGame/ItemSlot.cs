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
    private readonly Color _nullColor = new Color(1, 1, 1, 0);

    public void SetSlot(string name, int level, string description)
    {
        _name.text = name;
        _level.text = level.ToString();
        _image.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/{name}");
        _image.color = Color.white;
        _description.text = description;
    }

    public void ResetSlot()
    {
        _name.text = "";
        _level.text = "";
        _image.sprite = null;
        _image.color = _nullColor;
        _description.text = "";
    }
}
