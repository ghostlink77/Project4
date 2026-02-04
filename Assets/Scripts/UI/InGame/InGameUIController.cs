using System;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

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

    [Header("PlayerStat")]
    [SerializeField] private PlayerStatus _playerStatus;

    [Header("LevelUpBtns")]
    [SerializeField] private Button[] _itemSelectBtns;

    private void Update()
    {
        HandleInput();
        ShowPlayTime();
        UpdateExpBar();
        UpdateInventory();
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
        int currentLevel = _playerStatus.currentLevel;
        float currentExp = _playerStatus.currentExp;
        float maxExp = DataTableManager.Instance.GetGameData<ExpData>().GetExpData(currentLevel);
        _expBar.fillAmount = currentExp / maxExp;
    }

    private void UpdateSelectableItemInUI()
    {
        while (true)
        {
            WeaponDataStruct randomWeapon = DataTableManager.Instance.GetGameData<WeaponData>().GetRandomWeaponData();

            const int maxLevel = 10;
            int currentWeaponLevel = 1;
            bool isItemMaxLevel = false;

            for (int i = 0; i < _playerStatus.Items.Length; i++)
            {
                if (_playerStatus.Items[i].ItemName == randomWeapon.WeaponName)
                {
                    if (_playerStatus.Items[i].ItemLevel >= maxLevel)
                    {
                        isItemMaxLevel = true;
                    }
                    else
                    {
                        currentWeaponLevel = _playerStatus.Items[i].ItemLevel + 1;
                    }
                }
            }

            if (!isItemMaxLevel)
            {
                _itemSelectBtnDatas[0].ItemNameText.text = randomWeapon.WeaponName;
                _itemSelectBtnDatas[0].ItemImage.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/{randomWeapon.WeaponName}");
                _itemSelectBtnDatas[0].ItemLevelText.text = currentWeaponLevel.ToString();
                //_itemSelectBtnDatas[0].ItemDescriptionText.text = randomWeapon.Description;
                _itemSelectBtnDatas[0].ItemDescriptionText.text = "";
                _itemSelectBtns[0].onClick.AddListener(() => OnClickItemSelectBtn(
                    randomWeapon.WeaponName,
                    currentWeaponLevel,
                    "",
                    "Weapon"
                    ));

                break;
            }
        }
    }

    private void UpdateInventory()
    {
        for (int index = 0; index < _playerStatus.Items.Length; index++)
        {
            Item item = _playerStatus.Items[index];
            if (item.ItemLevel == 0) break;

            ItemSlotData itemSlot = _inventorySlot[index];
            itemSlot.ItemImage.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/{item.ItemName}");
            itemSlot.ItemImage.color = Color.white;
            itemSlot.ItemLevelText.text = item.ItemLevel.ToString();
            itemSlot.ItemDescriptionText.text = item.Description;
        }
    }

    private void OnClickItemSelectBtn(string Name, int level, string Description, string itemType)
    {
        Debug.Log($"클릭된 버튼 정보 -> Name : {Name} Level : {level} Desc : {Description}");

        switch(itemType)
        {
            case "Weapon":
                AddWeaponItem(Name, level, Description);
                CloseLevelupUI();
                break;
            default:
                Debug.Log("Doesn't Contains ItemType.");
                break;
        }

        /*Item item = new Item();
        item.ItemLevel = 
        _playerStatus.Items.*/

    }

    private void AddWeaponItem(string Name, int level, string Description)
    {
        for(int i=0; i< _playerStatus.Items.Length; i++)
        {
            if (_playerStatus.Items[i].ItemName == Name)
            {
                _playerStatus.Items[i].ItemLevel = level;
                return;
            }
            else if (_playerStatus.Items[i].ItemLevel == 0)
            {
                _playerStatus.Items[i].ItemName = Name;
                _playerStatus.Items[i].ItemLevel = level;
                _playerStatus.Items[i].Description = Description;
                return;
            }
        }
    }
}
