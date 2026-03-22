using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
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
    [SerializeField] private PauseUI _pauseUIComponent;
    [SerializeField] private GameObject _endGameUI;
    [SerializeField] private GameObject _levelupUI;
    [SerializeField] private TextMeshProUGUI _playTimeUI;
    [SerializeField] private Image _expBar;
    [SerializeField] private Minimap _minimap;
    [SerializeField] private GameObject _inGameUI;
    [SerializeField] private TurretSelectUI _turretSelectUI;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private TextMeshProUGUI _scrapAmountText;
    [SerializeField] private GameObject _worldCanvas;
    [SerializeField] private HpUI _playerHpUI;
    [SerializeField] private HpUI _agitHpUI;
    [SerializeField] private PlayableDirector _director;
    [SerializeField] private PlayableAsset _fadeInAsset;
    [SerializeField] private PlayableAsset _fadeOutAsset;

    [SerializeField] private CanvasGroup _canvasGroup;

    private Dictionary<Transform, HpUI> _turretHpBars = new Dictionary<Transform, HpUI>();

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


    [SerializeField] private DamageTextSpawner _damageTextSpawner;
    [SerializeField] private HpUISpawner _hpUISpawner;

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
        _inGameUI.SetActive(false);
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

        InGameManager.Instance.AgitGameOverAction += EndGame;
        InGameManager.Instance.PlayerGameOverAction += EndGame;
        InGameManager.Instance.PlayerWinAction += PlayerWinEndGame;

        UpdateInventory();

    }

    private void OnDestroy()
    {
        if (_playerLevelControl != null)
        {
            _playerLevelControl.OnLevelUp -= OpenLevelupUI;
            _playerLevelControl.LevelUpEvent -= OpenLevelupUI;
        }

        InGameManager.Instance.AgitGameOverAction -= EndGame;
        InGameManager.Instance.PlayerGameOverAction -= EndGame;
        InGameManager.Instance.PlayerWinAction -= PlayerWinEndGame;
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

            if (_pauseUI != null && _pauseUI.activeSelf == true && InGameManager.Instance.GameStat == GameStat.Pause)
            {
                _pauseUIComponent.OnClickContinueBtn();
            }
            else if (InGameManager.Instance.GameStat == GameStat.Play)
            {
                OnClickOpenPauseUI();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _turretSelectUI.IsSetting == false && InGameManager.Instance.GameStat == GameStat.Play)
        {
            OpenTurretSelectUI();
        }
        else if (Input.GetMouseButtonDown(1) && _turretSelectUI.IsSetting == true && InGameManager.Instance.GameStat == GameStat.Play)
        {
            _turretSelectUI.UnSetTurret();
        }
        else if (Input.GetMouseButtonDown(2) && _turretSelectUI.IsSetting == true && InGameManager.Instance.GameStat == GameStat.Play)
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
        InGameManager.Instance.PauseGame();

        if (_pauseUI != null) _pauseUI.SetActive(true);
        if (AudioManager.Instance != null) AudioManager.Instance.Play(AudioType.UISFX, "Button_Click");
        Time.timeScale = 0f;
    }

    public void ClosePauseUI()
    {
        if (_pauseUI != null) _pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenConfigUI()
    {
        if (UIManager.Instance != null) UIManager.Instance.OpenUI<ConfigUI>();
    }

    public void OnClickRestartGame()
    {
        if (SceneLoader.Instance != null) SceneLoader.Instance.LoadScene(ESceneType.InGame);
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
        _playerHpUI.UpdateHpBar(currentHp, maxHp);
    }

    public void UpdateAgitHpBar(float currentHp, float maxHp)
    {
        _agitHpUI.UpdateHpBar(currentHp, maxHp);
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

    private void UpdateSelectableItemInUI()
    {
        if (DataTableManager.Instance == null || PlayerManager.Instance == null) return;

        var excludedNames = new HashSet<string>();
        List<IItemStatData> availableItems = DataTableManager.Instance.GetAllSelectableItems(excludedNames);

        // NOTE: 셔플 후 버튼 수만큼 꺼내기
        for (int i = availableItems.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (availableItems[i], availableItems[randomIndex]) = (availableItems[randomIndex], availableItems[i]);
        }

        for (int i = 0; i < _itemSelectBtns.Length; i++)
        {
            if (_itemSelectBtns[i] == null) continue;

            if (i < availableItems.Count)
            {
                SetupItemSelectBtn(i, availableItems[i]);
            }
            else
            {
                _itemSelectBtns[i].gameObject.SetActive(false);
                _itemSelectBtns[i].onClick.RemoveAllListeners();
            }
        }
    }

    private void SetupItemSelectBtn(int index, IItemStatData itemData)
    {
        _itemSelectBtns[index].gameObject.SetActive(true);
        _itemSelectBtns[index].interactable = true;

        int currentLevel = PlayerManager.Instance.PlayerItemController.GetItemLevelInSlot(itemData);
        int displayLevel = (currentLevel == -1) ? 1 : currentLevel + 1;

        if (_itemSelectBtnDatas.Length > index)
        {
            if (_itemSelectBtnDatas[index].ItemNameText != null)
            {
                _itemSelectBtnDatas[index].ItemNameText.text = itemData.GetName();
            }

            if (_itemSelectBtnDatas[index].ItemImage != null)
            {
                _itemSelectBtnDatas[index].ItemImage.sprite = itemData.GetIcon();
                _itemSelectBtnDatas[index].ItemImage.preserveAspect = true;
            }

            if (_itemSelectBtnDatas[index].ItemLevelText != null)
            {
                _itemSelectBtnDatas[index].ItemLevelText.text = displayLevel.ToString();
            }

            if (_itemSelectBtnDatas[index].ItemDescriptionText != null)
            {
                _itemSelectBtnDatas[index].ItemDescriptionText.text = itemData.GetDescription(displayLevel);
            }
        }

        _itemSelectBtns[index].onClick.RemoveAllListeners();
        _itemSelectBtns[index].onClick.AddListener(() => OnClickItemSelectBtn(itemData));
    }

    private void UpdateInventory()
    {
        foreach (var inventory in _inventories)
        {
            if (inventory != null) inventory.UpdateSlot();
        }
    }

    private void OnClickItemSelectBtn(IItemStatData itemData)
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.PlayerItemController.AddItemToSlot(itemData);
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

    private void PlayerWinEndGame()
    {
        _playTimeUI.text = "Player Win!!";
    }
    public void ShowFadeInAnim()
    {
         if (_director != null && _fadeInAsset != null)
        {
            _director.playableAsset = _fadeInAsset;
            _director.Play();
        }
    }

    public void EndFadeIn()
    {
        if (InGameManager.Instance != null)
        {
            InGameManager.Instance.StartGame();
            _inGameUI.SetActive(true);
        }
    }

    public void ShowFadeOutAnim()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.sendNavigationEvents = false;

        if (_director != null && _fadeOutAsset != null)
        {
            _director.playableAsset = _fadeOutAsset;
            _director.Play();
        }
    }

    public void EndFadeOut()
    {
        if (SceneLoader.Instance != null) SceneLoader.Instance.LoadScene(ESceneType.Lobby);
    }
    
    public void CreateTurretHpBar(Transform transform)
    {
        HpUI hpUI = _hpUISpawner.CreateHpUI(transform.position);
        _turretHpBars[transform] = hpUI;
    }

    public void RemoveTurretHpBar(Transform transform)
    {
        if (_turretHpBars.TryGetValue(transform, out HpUI hpUI))
        {
            _hpUISpawner.RemoveHpUI(hpUI);
            _turretHpBars.Remove(transform);
        }
        
    }

    public void UpdateTurretHpBar(Transform transform, float currentHp, float maxHp)
    {
        if (_turretHpBars.TryGetValue(transform, out HpUI hpUI))
            hpUI.UpdateHpBar(currentHp, maxHp);
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