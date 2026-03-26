/*
 * 포탑 건설 선택 UI
 * UI오브젝트에 부착
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretSelectUI : MonoBehaviour
{
    [SerializeField] private TurretPlacer turretPlacer;
    [SerializeField] private Image _setImage;
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _settingKeyExplaneObj;
    private Camera _mainCamera;
    private Animator _anim;

    public const string FADEIN = "TurretSelectUIShow";
    public const string FADEOUT = "TurretSelectUIHide";
    public const string PATH = "Sprite";
    public const float CAMERA_DISTANCE = 0f;

    public bool IsActive = false;
    public bool IsSetting = false;

    private Button[] _turretBtns;

    private string _selectedTurretName;
    private Vector3 _mouseWorldPos = Vector3.zero;


    private void Awake()
    {
        _mainCamera = Camera.main;
        _setImage?.gameObject.SetActive(false);
        _settingKeyExplaneObj?.SetActive(false);
        gameObject.SetActive(true);
        _panel?.SetActive(false);
        _anim = GetComponent<Animator>();
        _turretBtns = GetComponentsInChildren<Button>();
        foreach (var btn in _turretBtns)
        {
            btn.onClick.RemoveAllListeners();

            if (btn.TryGetComponent<ItemSlot>(out var component))
            {
                btn.onClick.AddListener(() => SetTurret(component.GetName()));
            }
            else
            {
                Debug.Log("ItemSlot 컴포넌트가 없습니다.");
            }
        }
    }

    private void Start()
    {
        PlayerManager.Instance.PlayerEventController.ScrapCollected += UpdateScrapAmountEffect;
        turretPlacer.UseScrap += UpdateScrapAmountEffect;
        turretPlacer.EndPlaceTurret += UnSetTurret;
    }

    private void Update()
    {
        if (IsSetting)
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = CAMERA_DISTANCE;
            _mouseWorldPos = _mainCamera.ScreenToWorldPoint(mouseScreenPos);
            _mouseWorldPos.z = 0f;
            _setImage.gameObject.transform.position = _mouseWorldPos;
        }
        
    }

    public int GetScrapAmount()
    {
        return turretPlacer.GetScrapAmount();
    }
    private void UpdateScrapAmountEffect()
    {
        foreach(var btn in _turretBtns)
        {
            if (btn.TryGetComponent<TurretItemSlot>(out var component))
            {
                component.UpdateScrapValue(turretPlacer.GetScrapAmount());
            }
        }
    }
    private void SetTurret(string name)
    {
        Cursor.visible = false;

        Time.timeScale = 0f;
        _panel?.SetActive(true);
        if (_setImage != null)
        {
            _setImage.sprite = Resources.Load<Sprite>($"{PATH}/{name}");
            _setImage.gameObject.SetActive(true);
        }
        StartCoroutine(DelaySetting());

        _selectedTurretName = name;
        _settingKeyExplaneObj?.SetActive(true);
    }

    private IEnumerator DelaySetting()
    {
        yield return null;

        IsSetting = true;
    }

    public void PlaceTurret()
    {

        if (turretPlacer == null)
        {
            Debug.Log("turretPlacer가 TurretSelectUI에 없습니다.");
            return;
        }
        turretPlacer.SetTurret(_selectedTurretName, _mouseWorldPos);

    }

    public void UnSetTurret()
    {
        Cursor.visible = true;
        Time.timeScale = 1f;
        _panel?.SetActive(false);
        _setImage?.gameObject.SetActive(false);
        IsSetting = false;
        _settingKeyExplaneObj?.SetActive(false);
    }

    public void Show()
    {
        _anim.Play(FADEIN);
        AudioManager.Instance.Play(AudioType.UISFX, FADEIN);
    }
    public void Hide()
    {
        _anim.Play(FADEOUT);
        AudioManager.Instance.Play(AudioType.UISFX, FADEOUT);
    }

}
