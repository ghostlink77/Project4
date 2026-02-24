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
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _hpAnimBar;

    [Header("Hp Bar Animation Value")]
    [SerializeField] private float _blendInTime;
    [SerializeField] private float _animSpeed;
    [SerializeField] private Coroutine _animCoroutine;

    [SerializeField] private Volume _inGameVolume;

    [Header("ItemSelectBtn")]
    [SerializeField] private ItemSlotData[] _itemSelectBtnDatas = new ItemSlotData[3];

    [Header("Inventory")]
    [SerializeField] private List<Inventory> _inventories = new List<Inventory>();

    public readonly string IMAGE_PATH = "Sprite";
    public const int FULL_FILL_AMOUNT = 1;

    [Header("LevelUpBtns")]
    [SerializeField] private Button[] _itemSelectBtns;

    [SerializeField] private DamageTextSpawner _damageTextSpawner;

    // ★ 새로운 랜덤 패시브 시스템을 위한 변수들
    [Header("New Passive Data Pool")]
    public List<LevelUpPassive> allPassives;

    private PlayerStatController _playerStat;
    private PlayerLevelControl _playerLevelControl;

    private void Awake()
    {
        Time.timeScale = 1f;

        _pauseUI.SetActive(false);
        _levelupUI.SetActive(false);
        _endGameUI.SetActive(false);
        _inGameUI.SetActive(true);
        _hpBar.fillAmount = FULL_FILL_AMOUNT;
        _hpAnimBar.fillAmount = FULL_FILL_AMOUNT;

        UpdateInventory();
    }

    private void Start()
    {
        _playerStat = FindAnyObjectByType<PlayerStatController>();
        _playerLevelControl = FindAnyObjectByType<PlayerLevelControl>();

        if (_playerLevelControl != null)
        {
            _playerLevelControl.OnLevelUp += OpenLevelupUI; // 종소리 구독
        }
    }

    private void OnDestroy()
    {
        if (_playerLevelControl != null)
        {
            _playerLevelControl.OnLevelUp -= OpenLevelupUI;
        }
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

    public void CloseLevelupUI()
    {
        if (_levelupUI != null) _levelupUI.SetActive(false);
        Time.timeScale = 1f;
    }

    // ★ 랜덤 패시브를 띄워주는 핵심 로직
    private void ShowRandomPassives()
    {
        if (_playerStat == null || allPassives == null) return;

        List<LevelUpPassive> availablePassives = new List<LevelUpPassive>();

        foreach (var p in allPassives)
        {
            if (p == null) continue;
            int currentLevel = _playerStat.GetCurrentPassiveLevel(p.passive);
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

                LevelUpPassive selectedData = availablePassives[i];
                int nextLevel = _playerStat.GetCurrentPassiveLevel(selectedData.passive) + 1;

                // UI 연결이 하나라도 빠져있어도 기절하지 않도록 방어 코드 추가
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

    private void OnPassiveSelected(LevelUpPassive data)
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

    // ★ 빈 씬에서 제일 에러가 많이 나던 경험치 바 함수 완벽 방어
    public void UpdateExpBar()
    {
        if (_expBar == null || DataTableManager.Instance == null || PlayerManager.Instance == null) return;

        int currentLevel = PlayerManager.Instance.PlayerStatController.CurrentLevel;
        float currentExp = PlayerManager.Instance.PlayerStatController.CurrentExp;
        float maxExp = DataTableManager.Instance.GetGameData<ExpData>().GetExpData(currentLevel);
        _expBar.fillAmount = currentExp / maxExp;
    }

    public void UpdateHpBar()
    {
        float currentHp = (float)PlayerManager.Instance.PlayerStatController.CurrentHp;
        float maxHp = (float)PlayerManager.Instance.PlayerStatController.MaxHp;
        _hpBar.fillAmount = currentHp / maxHp;

        if (_animCoroutine != null)
        {
            StopCoroutine(_animCoroutine);
            _animCoroutine = null;
        }
        _animCoroutine = StartCoroutine(PlayHpBarAnimation());

    }

    private IEnumerator PlayHpBarAnimation()
    {
        yield return new WaitForSeconds(_blendInTime);

        while (_hpAnimBar.fillAmount > _hpBar.fillAmount)
        {
            _hpAnimBar.fillAmount = Mathf.Lerp(
                _hpAnimBar.fillAmount,
                _hpBar.fillAmount,
                _animSpeed * Time.deltaTime
                );

            yield return null;
        }
    }

    // ---- [이하 기존 아이템/미니맵 관련 코드들도 안전하게 방어막 추가] ----
    private void UpdateSelectableItemInUI()
    {
        UpdateSelectableItemBtn<WeaponStatData>(0);
        UpdateSelectableItemBtn<PassiveStatData>(1);
        UpdateSelectableItemBtn<TurretData>(2);
    }

    private void UpdateSelectableItemBtn<T>(int index) where T : IItemStatData
    {
        if (DataTableManager.Instance == null) return;

        T newItemData = DataTableManager.Instance.GetSelectableItem<T>();
        if (EqualityComparer<T>.Default.Equals(newItemData, default(T)))
        {
            if (_itemSelectBtnDatas.Length > index)
            {
                if (_itemSelectBtnDatas[index].ItemNameText != null) _itemSelectBtnDatas[index].ItemNameText.text = "";
                if (_itemSelectBtnDatas[index].ItemImage != null)
                {
                    _itemSelectBtnDatas[index].ItemImage.sprite = null;
                    _itemSelectBtnDatas[index].ItemImage.color = new Color(1, 1, 1, 0);
                }
                if (_itemSelectBtnDatas[index].ItemLevelText != null) _itemSelectBtnDatas[index].ItemLevelText.text = "";
                if (_itemSelectBtnDatas[index].ItemDescriptionText != null) _itemSelectBtnDatas[index].ItemDescriptionText.text = "";
            }
            if (_itemSelectBtns.Length > index && _itemSelectBtns[index] != null) _itemSelectBtns[index].onClick.RemoveAllListeners();
            return;
        }

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
            if (_itemSelectBtnDatas[index].ItemDescriptionText != null) _itemSelectBtnDatas[index].ItemDescriptionText.text = "";
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
        if (_inGameUI != null) _inGameUI.SetActive(false);
        if (_endGameUI != null) _endGameUI.SetActive(true);
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