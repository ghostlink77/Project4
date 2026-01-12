using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Slider _progressBar;
    [SerializeField] TextMeshProUGUI _progressBarText;

    private void Start()
    {
        AudioManager.Instance.SyncUserSettings();

        StartCoroutine(LoadingSequence());
    }

    private IEnumerator LoadingSequence()
    {
        Debug.Log($"{GetType()}::{nameof(LoadingSequence)}");

        // 타이틀 애니메이션 타임라인 재생

        var loadingOperation = SceneLoader.Instance.LoadSceneAsync(ESceneType.Lobby);
        if (loadingOperation == null)
        {
            Debug.Log($"Fail to load {ESceneType.Lobby} scene.");
            yield break;
        }

        loadingOperation.allowSceneActivation = false;

        _progressBar.value = 0.5f;
        _progressBarText.text = $"{(int)(_progressBar.value * 100.0f)}%";
        yield return new WaitForSeconds(0.5f);

        while(true)
        {
            if (loadingOperation.isDone)
            {
                break;
            }

            _progressBar.value = loadingOperation.progress;
            _progressBarText.text = $"{(int)(_progressBar.value * 100.0f)}%";

            if (_progressBar.value >= 0.9f)
            {
                loadingOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
    
