using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    [SerializeField] private PlayableDirector _fadeOutObj;
    [SerializeField] private GameObject _firstBtn;

    private void Update()
    {
        HandleInput();
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
            else
            {
                ShowQuitConfirmUI();
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

    public void OnClickConfigButton()
    {
        if (UIManager.Instance == null) Debug.LogError("UIManager Instance is null.");
        else UIManager.Instance.SaveCurrentBtn(EventSystem.current.currentSelectedGameObject);
        
        AudioManager.Instance.Play(AudioType.UISFX, "Button_Click");
        UIManager.Instance.OpenUI<ConfigUI>();
    }

    public void OnClickStartButton()
    {
        AudioManager.Instance.Play(AudioType.UISFX, "Button_Click");
        _fadeOutObj.Play();
    }

    public void OnClickExitButton()
    {
        ShowQuitConfirmUI();
    }

    public void EndFadeOut()
    {
        SceneLoader.Instance.LoadScene(ESceneType.InGame);
    }

    public void EndFadeIn()
    {
        EventSystem.current.sendNavigationEvents = true;
        AudioManager.Instance.Play(AudioType.BGM, "LobbyBGM");
        EventSystem.current.SetSelectedGameObject(_firstBtn);
    }
}
