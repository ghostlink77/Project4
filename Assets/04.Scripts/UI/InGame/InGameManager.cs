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

        // NOTE: ?ㅻⅨ 而댄룷?뚰듃??Start?먯꽌 李몄“?????덈룄濡?Awake ?④퀎(Init)?먯꽌 ?명똿
        InGameUIController = FindAnyObjectByType<InGameUIController>();
    }

    private void Start()
    {
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
        if (GameStat == GameStat.Play)
            RecordPlayTime();
    }

    private void RecordPlayTime()
    {
        PlayTime += Time.deltaTime;
        InGameUIController.ShowPlayTime();

        // 수정 : 10분 플레이 후 보스 출현을 위해 클리어 조건 제거
        /*
        if (PlayTime >= _gameClearConditionTime)
        {
            Debug.Log("占쏙옙占쏙옙 占쏙옙占?!");
            Time.timeScale = .2f;
            PlayerWinAction?.Invoke();
            EnemySpawner.Instance.DestroyAll();
            GameStat = GameStat.End;
        }
        */
    }

    public void OpenLevelUpUI(int level)
    {
        if (InGameUIController != null)
        {
            InGameUIController.OpenLevelupUI(level);
        }
        else
        {
            Debug.LogError("UI 占쏙옙트占싼뤄옙占쏙옙 占쏙옙占쏙옙占쏙옙占?占십았쏙옙占싹댐옙.");
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

    public void StopGameIfClear()
    {
        GameStat = GameStat.End;
        Time.timeScale = 0.2f;
        EnemySpawner.Instance.DestroyAll();
    }
}
