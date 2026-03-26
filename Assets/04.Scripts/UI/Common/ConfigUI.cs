using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfigUI : BaseUI
{
    [SerializeField] private GameObject MasterVolumeObj;

    [Header("Config Buttons")]
    [SerializeField] private ConfigButton _masterSoundBtn;
    [SerializeField] private ConfigButton _bgmSoundBtn;
    [SerializeField] private ConfigButton _uiSFXSoundBtn;
    [SerializeField] private ConfigButton _gameSFXSoundBtn;

    [SerializeField] private CanvasGroup _canvasGroup;


    private readonly float _defaultSoundValueScale = 0.01f;

    private void Start()
    {
        _canvasGroup.blocksRaycasts = false;
    }
    private void OnEnable()
    {
        EventSystem.current.sendNavigationEvents = false;
    }
    private void Update()
    {
        InputHandle();
    }

    private void InputHandle()
    {
        if (EventSystem.current.sendNavigationEvents == false) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickCloseButton();
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            SetupSound(true);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            SetupSound(false);
        }
    }

    public override void OnClickCloseButton()
    {
        _canvasGroup.blocksRaycasts = false;
        EventSystem.current.sendNavigationEvents = false;

        base.OnClickCloseButton();

    }
    private void SetupSound(bool isUp)
    {
        if (EventSystem.current.currentSelectedGameObject == null) return;

        if (EventSystem.current.currentSelectedGameObject.TryGetComponent<ConfigButton>(out ConfigButton component))
        {
            ConfigButton currentBtn = component;

            if (currentBtn.BtnType == BtnType.BGMSound)
            {
                _bgmSoundBtn.SetValue(isUp);
            }
            else if (currentBtn.BtnType == BtnType.MasterSound)
            {
                _masterSoundBtn.SetValue(isUp);
            }
            else if (currentBtn.BtnType == BtnType.UISFXSound)
            {
                _uiSFXSoundBtn.SetValue(isUp);
            }
            else if (currentBtn.BtnType == BtnType.SFXSound)
            {
                _gameSFXSoundBtn.SetValue(isUp);
            }
        }  
    }

    public void EndFadeIn()
    {
        EventSystem.current.SetSelectedGameObject(MasterVolumeObj);

        _canvasGroup.blocksRaycasts = true;
        EventSystem.current.sendNavigationEvents = true;
    }
    public void EndFadeOut()
    {
        EventSystem.current.sendNavigationEvents = true;

        if (UIManager.Instance == null) Debug.Log("UIManager Instance is null.");
        else UIManager.Instance.LoadCurrentBtn();
            Close();
    }
}
