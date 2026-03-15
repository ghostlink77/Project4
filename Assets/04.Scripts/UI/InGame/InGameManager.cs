using System;
using UnityEngine;

public enum GameStat
{
    Play,
    Pause,
    End
}


public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; private set; }

    public float PlayTime { get; private set; }

    private PlayerLevelControl _playerLevelControl;

    public GameStat GameStat { get; private set; }

    public Action AgitGameOverAction;
    public Action PlayerGameOverAction;
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

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAll();
        }

        _playerLevelControl = FindAnyObjectByType<PlayerLevelControl>();
        if (_playerLevelControl != null)
        {
            _playerLevelControl.OnLevelUp += OpenLevelUpUI;
        }

        GameStat = GameStat.Pause;
        Time.timeScale = 0f;
        InGameUIController.ShowFadeInAnim();
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
            InGameUIController.OpenLevelupUI(level);
        }
        else
        {
            Debug.LogError("UI 컨트롤러가 연결되지 않았습니다.");
        }
    }

    public void StartGame()
    {
        GameStat = GameStat.Play;
        Time.timeScale = 1f;
        PlayTime = 0;
        if (AudioManager.Instance != null) AudioManager.Instance.Play(AudioType.BGM, "InGameBGM");
    }

    public void PauseGame()
    {
        GameStat = GameStat.Pause;
    }

    public void ContinueGame()
    {
        GameStat = GameStat.Play;
    }

    public void ExitGame()
    {
        GameStat = GameStat.End;
    }

    public void EndGame(bool isPlayerDead)
    {
        GameStat = GameStat.End;
        Time.timeScale = 0f;

        if (isPlayerDead)
        {
            PlayerGameOverAction?.Invoke();
        }
        else
        {
            AgitGameOverAction?.Invoke();
        }
            
    }
}
