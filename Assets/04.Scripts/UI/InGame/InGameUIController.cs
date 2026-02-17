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
    [SerializeField] private Minimap _minimap;

    [Header("ItemSelectBtn")]
    [SerializeField] private ItemSlotData[] _itemSelectBtnDatas = new ItemSlotData[3];

    [Header("Inventory")]
    [SerializeField] private List<Inventory> _inventories = new List<Inventory>();

    public readonly string IMAGE_PATH = "Sprite";

    [Header("LevelUpBtns")]
    [SerializeField] private Button[] _itemSelectBtns;

    [SerializeField] private DamageTextSpawner _damageTextSpawner;


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
        UpdateSelectableItemBtn<WeaponStatData>(0);
        UpdateSelectableItemBtn<PassiveStatData>(1);
        UpdateSelectableItemBtn<TurretData>(2);
    }

    private void UpdateSelectableItemBtn<T>(int index) where T : IItemStatData
    {
        T newItemData = DataTableManager.Instance.GetSelectableItem<T>();
        if (EqualityComparer<T>.Default.Equals(newItemData, default(T)))
        {
            Debug.Log("No Selectable Item.");
            _itemSelectBtnDatas[index].ItemNameText.text = "";
            _itemSelectBtnDatas[index].ItemImage.sprite = null;
            _itemSelectBtnDatas[index].ItemImage.color = new Color(1, 1, 1, 0);
            _itemSelectBtnDatas[index].ItemLevelText.text = "";
            _itemSelectBtnDatas[index].ItemDescriptionText.text = "";
            _itemSelectBtns[index].onClick.RemoveAllListeners();
            return;
        }
        int currentItemLevel = PlayerManager.Instance.PlayerItemController.GetItemLevelInSlot<T>(newItemData);

        int ItemLevel = 1;
        if (currentItemLevel != -1)
        {
            ItemLevel = currentItemLevel + 1;
        }
        _itemSelectBtnDatas[index].ItemNameText.text = newItemData.GetName();
        _itemSelectBtnDatas[index].ItemImage.sprite = newItemData.GetIcon();
        _itemSelectBtnDatas[index].ItemLevelText.text = ItemLevel.ToString();
        _itemSelectBtnDatas[index].ItemDescriptionText.text = "";
        _itemSelectBtns[index].onClick.RemoveAllListeners();
        _itemSelectBtns[index].onClick.AddListener(() => OnClickItemSelectBtn<T>(newItemData));
    }

    private void UpdateInventory()
    {
        foreach (var inventory in _inventories)
        {
            inventory.UpdateSlot();
        }
    }
    private void OnClickItemSelectBtn<T>(T newItemData) where T : IItemStatData
    {
        PlayerManager.Instance.PlayerItemController.AddItemToSlot<T>(newItemData);
        UpdateInventory();
    }

    public void ShowDamageText(Vector3 position, float damage)
    {
        if (_damageTextSpawner == null)
        {
            Debug.Log("No DamageTextSpanwer.");
            return;
        }
        _damageTextSpawner.ShowDamageText(damage, position);
    }

    public void AddTracedEnemyInMinimap(Transform transform)
    {
        _minimap.AddTracedEnemy(transform);
    }

    public void RemoveTracedEnemyInMinimap(Transform transform)
    {
        _minimap.RemoveTracedEnemy(transform);
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    [Header("TestObject")]
    [SerializeField] private List<GameObject> _testEnemies;

    public void ShowDamageTextDebug()
    {
        if (_testEnemies == null)
        {
            Debug.Log("No testObj.");
            return;
        }
        foreach (var obj in _testEnemies)
        {
            _damageTextSpawner.ShowDamageText(10110, obj.transform.position);
        }
    }

    public void AddTracedEnemiesInMinimapDebug()
    {
        if (_testEnemies == null)
        {
            Debug.Log("No testObj.");
            return;
        }
        foreach (var obj in _testEnemies)
        {
            _minimap.AddTracedEnemy(obj.transform);
        }
    }

    public void RemoveTracedEnemiesInMinimapDebug()
    {
        if (_testEnemies == null)
        {
            Debug.Log("No testObj.");
            return;
        }
        foreach (var obj in _testEnemies)
        {
            _minimap.RemoveTracedEnemy(obj.transform);
        }
    }
#endif
}
