using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject LoadingTextTimeline;
    [SerializeField] Texture2D _mouseCursorImage;

    private void Start()
    {
        AudioManager.Instance.SyncUserSettings();
        DataTableManager.Instance.SetData();

        Cursor.SetCursor(_mouseCursorImage, new Vector2(_mouseCursorImage.width / 2, _mouseCursorImage.height / 2), CursorMode.Auto);

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
    
