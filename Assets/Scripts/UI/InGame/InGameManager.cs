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
        //AudioManager.Instance.Play("InGameBGM");
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
}
