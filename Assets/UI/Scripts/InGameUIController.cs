using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _endGameUI;
    [SerializeField] private GameObject _levelupUI;

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
}
