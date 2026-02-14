using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfigUI : BaseUI
{
    [SerializeField] private GameObject BGMSlider;
    [SerializeField] private GameObject SFXSlider;
    [SerializeField] private Slider BGM;
    [SerializeField] private Slider SFX;

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
        if (EventSystem.current.currentSelectedGameObject.name == BGMSlider.name)
        {
            if (isUp)
                BGM.value += _defaultSoundValueScale;
            else
                BGM.value -= _defaultSoundValueScale;
        }
        else if (EventSystem.current.currentSelectedGameObject.name == SFXSlider.name)
        {
            if (isUp)
                SFX.value += _defaultSoundValueScale;
            else
                SFX.value -= _defaultSoundValueScale;
        }
    }



    public void SetVolume()
    {
        Debug.Log($"BGM value : {BGM.value}, SFX value : {SFX.value}");

        AudioManager.Instance.SetVolume(AudioType.BGM, BGM.value);
        AudioManager.Instance.SetVolume(AudioType.SFX, SFX.value);
    }

    public void EndFadeIn()
    {
        EventSystem.current.SetSelectedGameObject(BGMSlider);
    }
    public void EndFadeOut()
    {
        EventSystem.current.SetSelectedGameObject(LastSelectedBtn);
        Close();
    }
}
