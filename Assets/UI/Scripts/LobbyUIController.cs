using UnityEngine;
using UnityEngine.Rendering;

public class LobbyUIController : MonoBehaviour
{
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
                frontUI.Close();
            }
            else
            {
                ShowQuitConfirmUI();
            }
        }
    }

    private void ShowQuitConfirmUI()
    {
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

    public void OnClickSettingsButton()
    {
        //UIManager.Instance.OpenUI<SettingsUI>();
    }

    public void OnClickStartButton()
    {
        //SceneLoader.Instance.LoadScene()
    }

    public void OnClickExitButton()
    {
        ShowQuitConfirmUI();
    }
}
