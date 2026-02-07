using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ItemSlotData
{
    public Image ItemImage;
    public TextMeshProUGUI ItemNameText;
    public TextMeshProUGUI ItemLevelText;
    public TextMeshProUGUI ItemDescriptionText;
}


public class InGameUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _endGameUI;
    [SerializeField] private GameObject _levelupUI;
    [SerializeField] private TextMeshProUGUI _playTimeUI;
    [SerializeField] private Image _expBar;

    [Header("ItemSelectBtn")]
    [SerializeField] private ItemSlotData[] _itemSelectBtnDatas = new ItemSlotData[3];

    [Header("Inventory")]
    [SerializeField] private ItemSlotData[] _inventorySlot = new ItemSlotData[7];

    public readonly string IMAGE_PATH = "Sprite";

    [Header("LevelUpBtns")]
    [SerializeField] private Button[] _itemSelectBtns;

    private void Update()
    {
        HandleInput();
        UpdateExpBar();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //AudioManager.Instance.Play();

            var frontUI = UIManager.Instance.GetFrontUI();
            if (frontUI != null)
            {
                frontUI.OnClickCloseButton();
            }
            else if (_pauseUI.activeSelf == true)
            {
                _pauseUI.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                OnClickOpenPauseUI();
            }
        }
    }

    private void ShowQuitConfirmUI()
    {
        Application.Quit();
        /*var data = new ConfirmUIData()
        {
            ConfirmType = 
             TitleText = 
            DescriptionText =
            OkButtonText = 
            CancleButtonText = 
            ActionOnClickOkButton = () => Application.Quit()
        };
        UIManager.Instance.OpenUI<ConfirmUI>(data);*/
    }

    public void OnClickOpenPauseUI()
    {
        _pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void OnClickClosePauseUI()
    {
        _pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnClickOpenConfigUI()
    {
        UIManager.Instance.OpenUI<ConfigUI>();
    }

    public void OnClickRestartGame()
    {
        SceneLoader.Instance.LoadScene(ESceneType.InGame);
    }

    public void OnClickGoLobby()
    {
        SceneLoader.Instance.LoadScene(ESceneType.Lobby);
    }

    public void OnClickExitGame()
    {
        Application.Quit();
    }

    public void OpenLevelupUI()
    {
        _levelupUI.SetActive(true);
        UpdateSelectableItemInUI();
        Time.timeScale = 0f;
    }

    public void CloseLevelupUI()
    {
        _levelupUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenEndgameUI()
    {
        _endGameUI.SetActive(true);
    }

    public void ShowPlayTime()
    {
        float time = InGameManager.Instance.PlayTime;
        string min = ((int)(time / 60)).ToString("D2");
        string sec = ((int)(time % 60)).ToString("D2");

        _playTimeUI.text = $"{min} : {sec}";
    }

    public void UpdateExpBar()
    {
        int currentLevel = PlayerManager.Instance.PlayerStatController.CurrentLevel;
        float currentExp = PlayerManager.Instance.PlayerStatController.CurrentExp;
        float maxExp = DataTableManager.Instance.GetGameData<ExpData>().GetExpData(currentLevel);
        _expBar.fillAmount = currentExp / maxExp;
    }

    private void UpdateSelectableItemInUI()
    {
        WeaponStatData newWeaponData = DataTableManager.Instance.GetSelectableWeapon();
        if (newWeaponData == null)
        {
            Debug.Log("No Selectable Weapon.");
            _itemSelectBtnDatas[0].ItemNameText.text = "";
            _itemSelectBtnDatas[0].ItemImage.sprite = null;
            _itemSelectBtnDatas[0].ItemImage.color = new Color(1, 1, 1, 0);
            _itemSelectBtnDatas[0].ItemLevelText.text = "";
            //_itemSelectBtnDatas[0].ItemDescriptionText.text = randomWeapon.Description;
            _itemSelectBtnDatas[0].ItemDescriptionText.text = "";
            _itemSelectBtns[0].onClick.RemoveAllListeners();
            return;
        }
        int currentWeaponLevel = PlayerManager.Instance.PlayerItemController.GetWeaponLevelInSlot(newWeaponData);

        int weaponLevel = 1;
        if (currentWeaponLevel != -1)
        {
            weaponLevel = currentWeaponLevel + 1;
        }
            
        _itemSelectBtnDatas[0].ItemNameText.text = newWeaponData.WeaponName;
        _itemSelectBtnDatas[0].ItemImage.sprite = newWeaponData.Icon;
        _itemSelectBtnDatas[0].ItemLevelText.text = weaponLevel.ToString();
        //_itemSelectBtnDatas[0].ItemDescriptionText.text = randomWeapon.Description;
        _itemSelectBtnDatas[0].ItemDescriptionText.text = "";
        _itemSelectBtns[0].onClick.RemoveAllListeners();
        _itemSelectBtns[0].onClick.AddListener(() => OnClickItemSelectBtn(
            newWeaponData,
            "Weapon"
            ));
    }

    private void UpdateInventory()
    {
        IReadOnlyList<GameObject> weaponSlot = PlayerManager.Instance.PlayerItemController.WeaponSlot;
        if (weaponSlot == null) return;
        for (int index = 0; index < weaponSlot.Count; index++)
        {
            if (weaponSlot[index] == null) return;
            ItemSlotData itemSlot = _inventorySlot[index];
            itemSlot.ItemImage.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/{weaponSlot[index].name}");
            itemSlot.ItemImage.color = Color.white;
            itemSlot.ItemLevelText.text = weaponSlot[index].GetComponent<WeaponStatController>().Level.ToString();
            itemSlot.ItemDescriptionText.text = "";
        }
    }

    private void OnClickItemSelectBtn(WeaponStatData newWeaponData, string itemType)
    {
        switch(itemType)
        {
            case "Weapon":
                PlayerManager.Instance.PlayerItemController.AddWeaponToSlot(newWeaponData);
                CloseLevelupUI();
                break;
            default:
                Debug.Log("Doesn't Contains ItemType.");
                break;
        }

        UpdateInventory();
    }
}
