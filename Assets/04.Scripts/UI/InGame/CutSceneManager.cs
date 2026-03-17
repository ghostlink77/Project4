using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;
    [SerializeField] private TimelineAsset _playerDeathCutScene;
    [SerializeField] private TimelineAsset _agitDeathCutScene;
    [SerializeField] private TimelineAsset _playerWinCutScene;

    private void Start()
    {
        InGameManager.Instance.PlayerGameOverAction += ShowPlayerDeathAction;
        InGameManager.Instance.PlayerWinAction += ShowPlayerWinAction;
    }

    private void OnDestroy()
    {
        InGameManager.Instance.PlayerGameOverAction -= ShowPlayerDeathAction;
        InGameManager.Instance.PlayerWinAction -= ShowPlayerWinAction;
    }

    public void ShowGameOverUI()
    {
        InGameManager.Instance.InGameUIController.ShowEndGameUI();
    }

    private void ShowPlayerDeathAction()
    {
        if (_playerDeathCutScene != null)
        {
            _director.playableAsset = _playerDeathCutScene;
            _director.Play();
        }
    }

    public void ShowAgitDeathAction()
    {
        if (_agitDeathCutScene != null)
        {
            _director.playableAsset = _agitDeathCutScene;
            _director.Play();
        }
    }

    private void ShowPlayerWinAction()
    {
        if (_playerWinCutScene != null)
        {
            _director.playableAsset = _playerWinCutScene;
            _director.Play();
        }
    }

    public void StopSlow()
    {
        Time.timeScale = 1.0f;
    }

    public void StopTime()
    {
        Time.timeScale = 0f;
    }
}
