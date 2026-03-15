using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfigUI : BaseUI
{
    [SerializeField] private GameObject MasterVolumeObj;
    [SerializeField] private GameObject BGMVolumeObj;
    [SerializeField] private GameObject UISFXVolumeObj;
    [SerializeField] private GameObject CharSFXVolumeObj;

    [Header("Sliders")]
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider UISFXSlider;
    [SerializeField] private Slider CharSFXSlider;


    private readonly float _defaultSoundValueScale = 0.01f;


    private void Update()
    {
        InputHandle();
        SetVolume();
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
                    BGMSlider.value += _defaultSoundValueScale;
                else
                    BGMSlider.value -= _defaultSoundValueScale;
            }
            else if (currentBtn.BtnType == BtnType.MasterSoundBtn)
            {
                if (isUp)
                    MasterSlider.value += _defaultSoundValueScale;
                else
                    MasterSlider.value -= _defaultSoundValueScale;
            }
            else if (currentBtn.BtnType == BtnType.UISoundBtn)
            {
                if (isUp)
                    UISFXSlider.value += _defaultSoundValueScale;
                else
                    UISFXSlider.value -= _defaultSoundValueScale;
            }
            else if (currentBtn.BtnType == BtnType.CharSoundBtn)
            {
                if (isUp)
                    CharSFXSlider.value += _defaultSoundValueScale;
                else
                    CharSFXSlider.value -= _defaultSoundValueScale;
            }
        }  
    }



    public void SetVolume()
    {
        AudioManager.Instance.SetVolume(AudioType.BGM, BGMSlider.value * MasterSlider.value);
        AudioManager.Instance.SetVolume(AudioType.UISFX, UISFXSlider.value * MasterSlider.value);
        AudioManager.Instance.SetVolume(AudioType.CharSFX, CharSFXSlider.value * MasterSlider.value);
        AudioManager.Instance.SetVolume(AudioType.EnemySFX, CharSFXSlider.value * MasterSlider.value);
        AudioManager.Instance.SetVolume(AudioType.BulletSFX, CharSFXSlider.value * MasterSlider.value);
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
