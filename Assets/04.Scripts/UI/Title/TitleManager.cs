using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadingTextTimeline;
    [SerializeField] private Texture2D _mouseCursorImage;

    private void Start()
    {
        AudioManager.Instance.SyncUserSettings();
        DataTableManager.Instance.SetData();

        if (_mouseCursorImage != null)
            Cursor.SetCursor(_mouseCursorImage, Vector2.zero, CursorMode.Auto);

    }

    public void StartLoading()
    {
        _loadingTextTimeline.SetActive(true);
        _loadingTextTimeline.GetComponent<PlayableDirector>().Play();
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
    
