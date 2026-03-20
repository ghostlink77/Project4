using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfigUI : BaseUI
{
    [SerializeField] private GameObject MasterVolumeObj;

    [Header("Sliders")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _uiSFXSlider;
    [SerializeField] private Slider _gameSFXSlider;


    private readonly float _defaultSoundValueScale = 0.01f;


    private void Update()
    {
        InputHandle();
    }

    private void InputHandle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            base.OnClickCloseButton();
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

    private void SetupSound(bool isUp)
    {
        if (EventSystem.current.currentSelectedGameObject.TryGetComponent<ConfigButton>(out ConfigButton component))
        {
            ConfigButton currentBtn = component;

            if (currentBtn.BtnType == BtnType.BGMSoundBtn)
            {
                if (isUp)
                    _bgmSlider.value += _defaultSoundValueScale;
                else
                    _bgmSlider.value -= _defaultSoundValueScale;
            }
            else if (currentBtn.BtnType == BtnType.MasterSoundBtn)
            {
                if (isUp)
                    _masterSlider.value += _defaultSoundValueScale;
                else
                    _masterSlider.value -= _defaultSoundValueScale;
            }
            else if (currentBtn.BtnType == BtnType.UISoundBtn)
            {
                if (isUp)
                    _uiSFXSlider.value += _defaultSoundValueScale;
                else
                    _uiSFXSlider.value -= _defaultSoundValueScale;
            }
            else if (currentBtn.BtnType == BtnType.CharSoundBtn)
            {
                if (isUp)
                    _gameSFXSlider.value += _defaultSoundValueScale;
                else
                    _gameSFXSlider.value -= _defaultSoundValueScale;
            }
            SetVolume();
        }  
    }



    public void SetVolume()
    {
        AudioManager.Instance.SetVolumeMixer(_gameSFXSlider.value, "SFX");
        AudioManager.Instance.SetVolumeMixer(_masterSlider.value, "Master");
        AudioManager.Instance.SetVolumeMixer(_bgmSlider.value, "BGM");
        AudioManager.Instance.SetVolumeMixer(_uiSFXSlider.value, "UISFX");
    }

    public void EndFadeIn()
    {
        EventSystem.current.SetSelectedGameObject(MasterVolumeObj);
    }
    public void EndFadeOut()
    {
        if (UIManager.Instance == null) Debug.Log("UIManager Instance is null.");
        else UIManager.Instance.LoadCurrentBtn();
            Close();
    }
}
