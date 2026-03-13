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

    private GameObject LastSelectedBtn;

    public void SelectBtn()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            LastSelectedBtn = EventSystem.current.currentSelectedGameObject;
    }

    private void KeepSelected()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (LastSelectedBtn != null) 
                EventSystem.current.SetSelectedGameObject(LastSelectedBtn);
        }
    }

    private void Update()
    {
        InputHandle();
        SetVolume();
        KeepSelected();
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
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        if (EventSystem.current.currentSelectedGameObject.name == MasterVolumeObj.name)
        {
            if (isUp)
                BGMSlider.value += _defaultSoundValueScale;
            else
                BGMSlider.value -= _defaultSoundValueScale;
        }
        else if (EventSystem.current.currentSelectedGameObject.name == BGMVolumeObj.name)
        {
            if (isUp)
                BGMSlider.value += _defaultSoundValueScale;
            else
                BGMSlider.value -= _defaultSoundValueScale;
        }
    }



    public void SetVolume()
    {
        AudioManager.Instance.SetVolume(AudioType.BGM, BGMSlider.value);
        AudioManager.Instance.SetVolume(AudioType.SFX, UISFXSlider.value);
    }

    public void EndFadeIn()
    {
        EventSystem.current.SetSelectedGameObject(MasterVolumeObj);
    }
    public void EndFadeOut()
    {
        EventSystem.current.SetSelectedGameObject(LastSelectedBtn);
        Close();
    }
}
