using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;
    [SerializeField] private TimelineAsset _playerDeathCutScene;
    [SerializeField] private TimelineAsset _agitDeathCutScene;

    private void Start()
    {
        InGameManager.Instance.PlayerGameOverAction += ShowPlayerDeathAction;
    }

    private void OnDestroy()
    {
        InGameManager.Instance.PlayerGameOverAction -= ShowPlayerDeathAction;
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
}
