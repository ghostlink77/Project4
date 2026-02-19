using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; private set; }

    public float PlayTime { get; private set; }

    private PlayerLevelControl _playerLevelControl;

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
        // AudioManager.Instance.StopAll();
        // AudioManager.Instance.Play(AudioType.BGM, "InGameBGM");

        _playerLevelControl = FindAnyObjectByType<PlayerLevelControl>();
        if (_playerLevelControl != null)
        {
            _playerLevelControl.OnLevelUp += OpenLevelUpUI;
        }
    }

    private void OnDestroy()
    {
        if (_playerLevelControl != null)
        {
            _playerLevelControl.OnLevelUp -= OpenLevelUpUI;
        }
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

    public void OpenLevelUpUI(int level)
    {
        if (InGameUIController != null)
        {
            InGameUIController.OpenLevelupUI();
        }
        else
        {
            Debug.LogError("UI 컨트롤러가 연결되지 않았습니다.");
        }
    }
}
