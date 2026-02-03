using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject LoadingTextTimeline;

    private void Start()
    {
        AudioManager.Instance.SyncUserSettings();

    }

    public void StartLoading()
    {
        LoadingTextTimeline.SetActive(true);
        LoadingTextTimeline.GetComponent<PlayableDirector>().Play();
        StartCoroutine(LoadingSequence());
    }

    private IEnumerator LoadingSequence()
    {
        Debug.Log($"{GetType()}::{nameof(LoadingSequence)}");


        var loadingOperation = SceneLoader.Instance.LoadSceneAsync(ESceneType.Lobby);
        if (loadingOperation == null)
        {
            Debug.Log($"Fail to load {ESceneType.Lobby} scene.");
            yield break;
        }

        loadingOperation.allowSceneActivation = false;

        yield return new WaitForSeconds(1f);

        loadingOperation.allowSceneActivation = true;
        
    }
}
    
