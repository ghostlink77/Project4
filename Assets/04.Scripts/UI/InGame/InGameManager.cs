using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; private set; }

    public float PlayTime { get; private set; }

    protected override void Init()
    {
        IsDestroyOnLoad = true;

        base.Init();
    }

    private void Start()
    {
        InGameUIController = FindAnyObjectByType<InGameUIController>();
        
        if (InGameUIController == null)
        {
            Debug.Log("InGameUIController does not exist.");
            return;
        }

        PlayTime = 0;
        AudioManager.Instance.StopAll();
        AudioManager.Instance.Play(AudioType.BGM, "InGameBGM");
    }
    private void Update()
    {
        RecordPlayTime();
    }

    private void RecordPlayTime()
    {
        PlayTime += Time.deltaTime;
        InGameUIController.ShowPlayTime();
    }

    public void OpenLevelUpUI()
    {
        {
            if(InGameUIController != null)
            {
                InGameUIController.OpenLevelupUI();
            }
            else
            {
                Debug.LogError("UI 컨트롤러가 연결되지 않았습니다.");
            }
        }
    }
}
