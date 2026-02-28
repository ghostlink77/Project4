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
        Debug.Log("슬롯 초기화 완료.");
        _name.text = "";
        Debug.Log("1.");
        _level.text = "";
        Debug.Log("2");
        _image.sprite = null;
        Debug.Log("3");
        _image.color = _nullColor;
        Debug.Log("4");
        _description.text = "";
        Debug.Log("5");
        _btn.interactable = false;
        Debug.Log("6");
    }

    public string GetName()
    {
        return _name.text;
    }
}
