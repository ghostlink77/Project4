using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurretItemSlot : ItemSlot
{
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private GameObject _xImage;
    protected CanvasGroup _canvasGroup;

    private int _cost = -1;

    protected override void Awake()
    {
        base.Awake();
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public override void SetSlot(string name, int level, string description, int cost)
    {
        base.SetSlot(name, level, description, cost);
        _costText.text = cost.ToString();
        _cost = cost;
    }

    public override void ResetSlot()
    {
        base.ResetSlot();
        _costText.text = "";

        HideAnim();
    }

    public void UpdateScrapValue(int currentScrapAmount)
    {
        if (currentScrapAmount >= _cost) ShowAnim();
        else HideAnim();
    }

    private void ShowAnim()
    {
        _xImage.SetActive(false);
        _canvasGroup.alpha = 1;
        _btn.interactable = true;
    }

    private void HideAnim()
    {
        _xImage.SetActive(true);
        _canvasGroup.alpha = 0.5f;
        _btn.interactable = false;
    }
}
