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

    [SerializeField] private float _gameClearConditionTime;
    public float GameClearConditionTime { get { return _gameClearConditionTime; }}

    private PlayerLevelControl _playerLevelControl;

    public GameStat GameStat { get; private set; }

    public Action AgitGameOverAction;
    public Action PlayerGameOverAction;
    public Action PlayerWinAction;
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

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAll();
            AudioManager.Instance.Play(AudioType.BGM, "InGameBGM");
        }

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
        if (GameStat == GameStat.Play)
            RecordPlayTime();
    }

    private void RecordPlayTime()
    {
        PlayTime += Time.deltaTime;
        InGameUIController.ShowPlayTime();

        if (PlayTime >= _gameClearConditionTime)
        {
            Debug.Log("게임 우승!!");
            Time.timeScale = .2f;
            PlayerWinAction?.Invoke();
            EnemySpawner.Instance.DestroyAll();
            GameStat = GameStat.End;
        }
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

    public void EndGame(bool isPlayerDead)
    {
        GameStat = GameStat.End;
        Time.timeScale = 0f;
        AudioManager.Instance.StopAll();

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
