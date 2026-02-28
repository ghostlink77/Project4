using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _name;
    [SerializeField] protected TextMeshProUGUI _level;
    [SerializeField] protected Image _image;
    [SerializeField] protected TextMeshProUGUI _description;
    [SerializeField] protected Button _btn;

    public readonly string IMAGE_PATH = "Sprite";
    protected readonly Color _nullColor = new Color(1, 1, 1, 0);

    public void SetSlot(string name, int level, string description)
    {
        _name.text = name;
        _level.text = level.ToString();
        _image.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/{name}");
        _image.color = Color.white;
        _description.text = description;
        _btn.interactable = true;
    }

    public virtual void SetSlot(string name, int level, string description, int cost)
    {
        SetSlot(name, level, description);
    }

    public virtual void ResetSlot()
    {
        _name.text = "";
        _level.text = "";
        _image.sprite = null;
        _image.color = _nullColor;
        _description.text = "";
        _btn.interactable = false;
    }

    public string GetName()
    {
        return _name.text;
    }
}
