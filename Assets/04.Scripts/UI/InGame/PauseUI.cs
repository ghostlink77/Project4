using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;
    [SerializeField] private PlayableAsset _fadeInAsset;
    [SerializeField] private PlayableAsset _fadeOutAsset;

    [SerializeField] private GameObject _firstBtn;

    private void OnEnable()
    {
        if (_director == null)
        {
            Debug.LogError("Pause UI's PlayableDirector is null.");
            return;
        }

        if (_fadeInAsset == null)
        {
            Debug.LogError("Pause UI's FadeOut PlayableAsset is null.");
            return;
        }
        _director.playableAsset = _fadeInAsset;
        _director.Play();
    }

    public void OnClickContinueBtn()
    {
        InGameManager.Instance.ContinueGame();

        if (AudioManager.Instance != null) AudioManager.Instance.Play(AudioType.UISFX, "Button_Click");

        if (_director == null)
        {
            Debug.LogError("Pause UI's PlayableDirector is null.");
            return;
        }

        if (_fadeInAsset == null)
        {
            Debug.LogError("Pause UI's FadeOut PlayableAsset is null.");
            return;
        }
        _director.playableAsset = _fadeOutAsset;
        _director.Play();
    }

    public void OnClickConfigBtn()
    {
        if (UIManager.Instance == null) Debug.LogError("UIManager Instance is null");
        else UIManager.Instance.SaveCurrentBtn(EventSystem.current.currentSelectedGameObject);

        if (AudioManager.Instance != null) AudioManager.Instance.Play(AudioType.UISFX, "Button_Click");

        if (InGameManager.Instance == null)
        {
            Debug.LogError("InGameManager Instance is null.");
            return;
        }
        InGameManager.Instance.InGameUIController.OpenConfigUI();
    }

    public void OnClickExitGame()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.Play(AudioType.UISFX, "Button_Click");

        if (InGameManager.Instance == null)
        {
            Debug.LogError("InGameManager Instance is null.");
            return;
        }
        InGameManager.Instance.ExitGame();
        InGameManager.Instance.InGameUIController.ShowFadeOutAnim();
    }

    public void EndFadeIn()
    {
        EventSystem.current.SetSelectedGameObject(_firstBtn);
    }
}
