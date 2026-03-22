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

        // NOTE: ë‹¤ë¥¸ ì»´í¬ë„ŒíŠ¸ì˜ Startì—ì„œ ì°¸ì¡°í•  ìˆ˜ ìˆë„ë¡ Awake ë‹¨ê³„(Init)ì—ì„œ ì„¸íŒ…
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

        // ¼öÁ¤ : 10ºĞ ÇÃ·¹ÀÌ ÈÄ º¸½º ÃâÇöÀ» À§ÇØ Å¬¸®¾î Á¶°Ç Á¦°Å
        /*
        if (PlayTime >= _gameClearConditionTime)
        {
            Debug.Log("ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½!!");
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
            Debug.LogError("UI ï¿½ï¿½Æ®ï¿½Ñ·ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ê¾Ò½ï¿½ï¿½Ï´ï¿½.");
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
