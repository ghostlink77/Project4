using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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
    [SerializeField] private GameObject _inGameUI;
    [SerializeField] private TurretSelectUI _turretSelectUI;
    [SerializeField] private Image _playerHpBar;
    [SerializeField] private Image _playerHpAnimBar;
    [SerializeField] private Image _agitHpBar;
    [SerializeField] private Image _agitHpAnimBar;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private TextMeshProUGUI _scrapAmountText;
    [SerializeField] private GameObject _worldCanvas;

    [Header("Hp Bar Animation Value")]
    [SerializeField] private float _blendInTime;
    [SerializeField] private float _animSpeed;
    [SerializeField] private Coroutine _playerHpBarAnimCoroutine;
    [SerializeField] private Coroutine _agitHpBarAnimCoroutine;

    private Coroutine _messageTextCoroutine;

    [SerializeField] private Volume _inGameVolume;

    [Header("ItemSelectBtn")]
    [SerializeField] private ItemSlotData[] _itemSelectBtnDatas = new ItemSlotData[3];

    [Header("Inventory")]
    [SerializeField] private List<Inventory> _inventories = new List<Inventory>();

    public readonly string IMAGE_PATH = "Sprite";
    public const int FULL_FILL_AMOUNT = 1;
    public const int Null_AMOUNT = 0;

    [Header("LevelUpBtns")]
    [SerializeField] private Button[] _itemSelectBtns;

    [SerializeField] private string[] _selectedItemName = new string[3];

    [SerializeField] private DamageTextSpawner _damageTextSpawner;

    [Header("New Passive Data Pool")]
    public List<PassiveItemData> allPassives;

    private PlayerStatController _playerStat;
    private PlayerLevelControl _playerLevelControl;

    private void Awake()
    {
        Time.timeScale = 1f;

        _pauseUI.SetActive(false);
        _levelupUI.SetActive(false);
        _endGameUI.SetActive(false);
        _inGameUI.SetActive(true);
        _playerHpBar.fillAmount = FULL_FILL_AMOUNT;
        _playerHpAnimBar.fillAmount = FULL_FILL_AMOUNT;
        _agitHpBar.fillAmount = FULL_FILL_AMOUNT;
        _agitHpAnimBar.fillAmount = FULL_FILL_AMOUNT;
        _expBar.fillAmount = Null_AMOUNT;
        _messageText.text = "";
        _scrapAmountText.text = "";
  
    }

    private void Start()
    {
        _playerStat = FindAnyObjectByType<PlayerStatController>();
        _playerLevelControl = FindAnyObjectByType<PlayerLevelControl>();

        if (_playerLevelControl != null)
        {
            _playerLevelControl.OnLevelUp += OpenLevelupUI; // 종소리 구독
            _playerLevelControl.LevelUpEvent += OpenLevelupUI;
        }

        InGameManager.Instance.EndGameAction += EndGame;

        UpdateInventory();

    }

    private void OnDestroy()
    {
        if (_playerLevelControl != null)
        {
            _playerLevelControl.OnLevelUp -= OpenLevelupUI;
            _playerLevelControl.LevelUpEvent -= OpenLevelupUI;
        }

        InGameManager.Instance.EndGameAction -= EndGame;
    }

    private void Update()
    {
        HandleInput();
        //UpdateExpBar();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 테스트 씬에 UIManager가 없을 경우를 대비한 안전장치
            if (UIManager.Instance != null)
            {
                var frontUI = UIManager.Instance.GetFrontUI();
                if (frontUI != null)
                {
                    frontUI.OnClickCloseButton();
                    return;
                }
            }

            if (_pauseUI != null && _pauseUI.activeSelf == true)
            {
                _pauseUI.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                OnClickOpenPauseUI();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _turretSelectUI.IsSetting == false)
        {
            OpenTurretSelectUI();
        }
        else if (Input.GetMouseButtonDown(1) && _turretSelectUI.IsSetting == true)
        {
            _turretSelectUI.UnSetTurret();
        }
        else if (Input.GetMouseButtonDown(2) && _turretSelectUI.IsSetting == true)
        {
            _turretSelectUI.PlaceTurret();
        }
    }

    private void OpenTurretSelectUI()
    {
        if (_turretSelectUI != null)
        {
            if (!_turretSelectUI.IsActive)
            {
                _turretSelectUI.Show();
                _turretSelectUI.IsActive = true;
            }
            else
            {
                _turretSelectUI.Hide();
                _turretSelectUI.IsActive = false;
            }
        }
    }

    private void ShowQuitConfirmUI()
    {
        Application.Quit();
    }

    public void OnClickOpenPauseUI()
    {
        if (_pauseUI != null) _pauseUI.SetActive(true);
        if (AudioManager.Instance != null) AudioManager.Instance.Play(AudioType.SFX, "Button_Click");
        Time.timeScale = 0f;
    }

    public void OnClickClosePauseUI()
    {
        if (_pauseUI != null) _pauseUI.SetActive(false);
        if (AudioManager.Instance != null) AudioManager.Instance.Play(AudioType.SFX, "Button_Click_Close");
        Time.timeScale = 1f;
    }

    public void OnClickOpenConfigUI()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.Play(AudioType.SFX, "Button_Click");
        if (UIManager.Instance != null) UIManager.Instance.OpenUI<ConfigUI>();
    }

    public void OnClickRestartGame()
    {
        if (SceneLoader.Instance != null) SceneLoader.Instance.LoadScene(ESceneType.InGame);
    }

    public void OnClickGoLobby()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.Play(AudioType.SFX, "Button_Click_Close");
        if (SceneLoader.Instance != null) SceneLoader.Instance.LoadScene(ESceneType.Lobby);
    }

    public void OnClickExitGame()
    {
        Application.Quit();
    }

    public void OpenLevelupUI(int level)
    {
        if (_levelupUI != null) _levelupUI.SetActive(true);
        ShowRandomPassives();
        Time.timeScale = 0f;
    }

    public void OpenLevelupUI()
    {
        if (_levelupUI != null) _levelupUI.SetActive(true);
        UpdateSelectableItemInUI();
        Time.timeScale = 0f;
    }

    public void CloseLevelupUI()
    {
        if (_levelupUI != null) _levelupUI.SetActive(false);
        Time.timeScale = 1f;
        UpdateExpBar();
    }

    private void ShowRandomPassives()
    {
        if (_playerStat == null || allPassives == null) return;

        List<PassiveItemData> availablePassives = new List<PassiveItemData>();

        foreach (var p in allPassives)
        {
            if (p == null) continue;

            int currentLevel = _playerStat.GetCurrentPassiveLevel(p.passiveType);
            if (currentLevel < p.maxLevel)
            {
                availablePassives.Add(p);
            }
        }

        availablePassives = availablePassives.OrderBy(x => UnityEngine.Random.value).ToList();

        for (int i = 0; i < _itemSelectBtns.Length; i++)
        {
            if (_itemSelectBtns[i] == null) continue;

            if (i < availablePassives.Count)
            {
                _itemSelectBtns[i].gameObject.SetActive(true);

                PassiveItemData selectedData = availablePassives[i];

                int nextLevel = _playerStat.GetCurrentPassiveLevel(selectedData.passiveType) + 1;

                if (_itemSelectBtnDatas.Length > i)
                {
                    if (_itemSelectBtnDatas[i].ItemImage != null)
                    {
                        _itemSelectBtnDatas[i].ItemImage.sprite = selectedData.icon;
                        _itemSelectBtnDatas[i].ItemImage.color = Color.white;
                    }
                    if (_itemSelectBtnDatas[i].ItemNameText != null)
                        _itemSelectBtnDatas[i].ItemNameText.text = selectedData.itemName;
                    if (_itemSelectBtnDatas[i].ItemDescriptionText != null)
                        _itemSelectBtnDatas[i].ItemDescriptionText.text = selectedData.GetDescription(nextLevel);
                    if (_itemSelectBtnDatas[i].ItemLevelText != null)
                        _itemSelectBtnDatas[i].ItemLevelText.text = (nextLevel == 1) ? "New!" : $"Lv.{nextLevel}";
                }

                _itemSelectBtns[i].onClick.RemoveAllListeners();
                _itemSelectBtns[i].onClick.AddListener(() => OnPassiveSelected(selectedData));
            }
            else
            {
                _itemSelectBtns[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnPassiveSelected(PassiveItemData data)
    {
        if (_playerStat != null) _playerStat.LevelUpPassiveStat(data);
        CloseLevelupUI();
    }

    public void OpenEndgameUI()
    {
        if (_endGameUI != null) _endGameUI.SetActive(true);
    }

    public void ShowPlayTime()
    {
        if (InGameManager.Instance == null || _playTimeUI == null) return;

        float time = InGameManager.Instance.PlayTime;
        string min = ((int)(time / 60)).ToString("D2");
        string sec = ((int)(time % 60)).ToString("D2");

        _playTimeUI.text = $"{min} : {sec}";
    }

    public void UpdateExpBar()
    {
        if (_expBar == null || DataTableManager.Instance == null || PlayerManager.Instance == null) return;

        int currentLevel = PlayerManager.Instance.PlayerStatController.CurrentLevel;
        float currentExp = PlayerManager.Instance.PlayerStatController.CurrentExp;
        //float maxExp = DataTableManager.Instance.GetGameData<ExpData>().GetExpData(currentLevel);
        float maxExp = _playerLevelControl.requiredXP;
        _expBar.fillAmount = currentExp / maxExp;
    }


    public void UpdatePlayerHpBar()
    {
        float currentHp = (float)PlayerManager.Instance.PlayerStatController.CurrentHp;
        float maxHp = (float)PlayerManager.Instance.PlayerStatController.MaxHp;
        _playerHpBar.fillAmount = currentHp / maxHp;

        if (_playerHpBarAnimCoroutine != null)
        {
            StopCoroutine(_playerHpBarAnimCoroutine);
            _playerHpBarAnimCoroutine = null;
        }
        _playerHpBarAnimCoroutine = StartCoroutine(PlayHpBarAnimation(true));
    }

    public void UpdateAgitHpBar(float currentHp, float maxHp)
    {
        _agitHpBar.fillAmount = currentHp / maxHp;

        if (_agitHpBarAnimCoroutine != null)
        {
            StopCoroutine(_agitHpBarAnimCoroutine);
            _agitHpBarAnimCoroutine = null;
        }
        _agitHpBarAnimCoroutine = StartCoroutine(PlayHpBarAnimation(false));
    }

    private IEnumerator PlayHpBarAnimation(bool isPlayer)
    {
        Image animBar = isPlayer ? _playerHpAnimBar : _agitHpAnimBar;
        Image bar = isPlayer ? _playerHpBar : _agitHpBar;
        yield return new WaitForSeconds(_blendInTime);

        while (animBar.fillAmount > bar.fillAmount)
        {
            animBar.fillAmount = Mathf.Lerp(
                animBar.fillAmount,
                bar.fillAmount,
                _animSpeed * Time.deltaTime
                );

            yield return null;
        }
    }

    public void PrintMessge(string text)
    {
        _messageText.text = text;
        if (_messageTextCoroutine != null) StopCoroutine(_messageTextCoroutine);
        _messageTextCoroutine = StartCoroutine(ShowMessageText());
    }

    private IEnumerator ShowMessageText(float duration = 1.0f)
    {
        Color color = _messageText.color;
        color.a = 1;
        _messageText.color = color;

        float currentTime = 0f;

        while(currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            
            float alpha = Mathf.Lerp(1, 0, currentTime / duration);
            color.a = alpha;
            _messageText.color = color;
            yield return null;
        }
    }

    public void UpdateScrapAmountText(int amount)
    {
        _scrapAmountText.text = amount.ToString();
    }

    // ---- [이하 기존 아이템/미니맵 관련 코드들도 안전하게 방어막 추가] ----
    private void UpdateSelectableItemInUI()
    {
        Array.Clear(_selectedItemName, 0, _selectedItemName.Length);

        for (int index = 0; index< _itemSelectBtns.Length; index++)
        {
            float itemTypeIndex = UnityEngine.Random.Range(0, _itemSelectBtns.Length);
            switch (itemTypeIndex)
            {
                case 0:
                    UpdateSelectableItemBtn<WeaponStatData>(index);
                    break;
                case 1:
                    UpdateSelectableItemBtn<PassiveItemData>(index);
                    break;
                case 2:
                    UpdateSelectableItemBtn<TurretData>(index);
                    break;
                default:
                    break;
            }
        }
        /*UpdateSelectableItemBtn<WeaponStatData>(0);
        UpdateSelectableItemBtn<PassiveStatData>(1);
        UpdateSelectableItemBtn<TurretData>(2);*/
    }

    private void UpdateSelectableItemBtn<T>(int index) where T : IItemStatData
    {
        if (DataTableManager.Instance == null) return;

        T newItemData = DataTableManager.Instance.GetSelectableItem<T>(_selectedItemName);
        if (EqualityComparer<T>.Default.Equals(newItemData, default(T)))
        {
            _itemSelectBtns[index].gameObject.SetActive(false);
            _itemSelectBtns[index].onClick.RemoveAllListeners();
            return;
        }

        _itemSelectBtns[index].gameObject.SetActive(true);

        _selectedItemName[index] = newItemData.GetName();
        _itemSelectBtns[index].interactable = true;

        if (PlayerManager.Instance == null) return;
        int currentItemLevel = PlayerManager.Instance.PlayerItemController.GetItemLevelInSlot<T>(newItemData);

        int ItemLevel = 1;
        if (currentItemLevel != -1)
        {
            ItemLevel = currentItemLevel + 1;
        }

        if (_itemSelectBtnDatas.Length > index)
        {
            if (_itemSelectBtnDatas[index].ItemNameText != null) _itemSelectBtnDatas[index].ItemNameText.text = newItemData.GetName();
            if (_itemSelectBtnDatas[index].ItemImage != null) _itemSelectBtnDatas[index].ItemImage.sprite = newItemData.GetIcon();
            if (_itemSelectBtnDatas[index].ItemLevelText != null) _itemSelectBtnDatas[index].ItemLevelText.text = ItemLevel.ToString();
            if (_itemSelectBtnDatas[index].ItemDescriptionText != null) _itemSelectBtnDatas[index].ItemDescriptionText.text = newItemData.GetDescription(ItemLevel);
        }

        if (_itemSelectBtns.Length > index && _itemSelectBtns[index] != null)
        {
            _itemSelectBtns[index].onClick.RemoveAllListeners();
            _itemSelectBtns[index].onClick.AddListener(() => OnClickItemSelectBtn<T>(newItemData));
        }
    }

    private void UpdateInventory()
    {
        foreach (var inventory in _inventories)
        {
            if (inventory != null) inventory.UpdateSlot();
        }
    }

    private void OnClickItemSelectBtn<T>(T newItemData) where T : IItemStatData
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.PlayerItemController.AddItemToSlot<T>(newItemData);
            UpdateInventory();
        }
    }

    public void ShowDamageText(Vector3 position, float damage)
    {
        if (_damageTextSpawner != null) _damageTextSpawner.ShowDamageText(damage, position);
    }

    public void AddTracedEnemyInMinimap(Transform transform) { if (_minimap != null) _minimap.AddTracedEnemy(transform); }
    public void RemoveTracedEnemyInMinimap(Transform transform) { if (_minimap != null) _minimap.RemoveTracedEnemy(transform); }
    public void ShowMinimapWarning() { if (_minimap != null) _minimap.PlayWarningAnim(); }
    public void HideMinimapWarning() { if (_minimap != null) _minimap.StopWarningAnim(); }

    public void ShowHealthLessWarning()
    {
        if (_inGameVolume != null && _inGameVolume.profile.TryGet<Vignette>(out var vignette)) vignette.intensity.value = 1;
    }

    public void HideHealthLessWarning()
    {
        if (_inGameVolume != null && _inGameVolume.profile.TryGet<Vignette>(out var vignette)) vignette.intensity.value = 0;
    }

    public void ShowEndGameUI()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.StopAll();
        if (_endGameUI != null) _endGameUI.SetActive(true);
    }

    private void EndGame()
    {
        _inGameUI?.SetActive(false);
        _worldCanvas?.SetActive(false);
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    [Header("TestObject")]
    [SerializeField] private List<GameObject> _testEnemies;

    public void ShowDamageTextDebug()
    {
        if (_testEnemies == null || _damageTextSpawner == null) return;
        foreach (var obj in _testEnemies)
        {
            if (obj != null) _damageTextSpawner.ShowDamageText(10110, obj.transform.position);
        }
    }

    public void AddTracedEnemiesInMinimapDebug()
    {
        if (_testEnemies == null || _minimap == null) return;
        foreach (var obj in _testEnemies)
        {
            if (obj != null) _minimap.AddTracedEnemy(obj.transform);
        }
    }

    public void RemoveTracedEnemiesInMinimapDebug()
    {
        if (_testEnemies == null || _minimap == null) return;
        foreach (var obj in _testEnemies)
        {
            if (obj != null) _minimap.RemoveTracedEnemy(obj.transform);
        }
    }
#endif
}