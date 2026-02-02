using TMPro;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _endGameUI;
    [SerializeField] private GameObject _levelupUI;
    [SerializeField] private TextMeshProUGUI _playTimeUI;

    private void Update()
    {
        HandleInput();
        ShowPlayTime();
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
            else if (_pauseUI.activeSelf == true)
            {
                _pauseUI.SetActive(false);
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

    public void OnClickOpenPauseUI()
    {
        _pauseUI.SetActive(true);
    }
    
    public void OnClickClosePauseUI()
    {
        _pauseUI.SetActive(false);
    }

    public void OnClickOpenConfigUI()
    {
        UIManager.Instance.OpenUI<ConfigUI>();
    }

    public void OnClickRestartGame()
    {
        SceneLoader.Instance.LoadScene(ESceneType.InGame);
    }

    public void OnClickGoLobby()
    {
        SceneLoader.Instance.LoadScene(ESceneType.Lobby);
    }

    public void OnClickExitGame()
    {
        Application.Quit();
    }

    public void OpenLevelupUI()
    {
        _levelupUI.SetActive(true);
    }

    public void CloseLevelupUI()
    {
        _levelupUI.SetActive(false);
    }

    public void OpenEndgameUI()
    {
        _endGameUI.SetActive(true);
    }

    public void ShowPlayTime()
    {
        float time = InGameManager.Instance.PlayTime;
        string min = ((int)(time / 60)).ToString("D2");
        string sec = ((int)(time % 60)).ToString("D2");

        _playTimeUI.text = $"{min} : {sec}";
    }
}
