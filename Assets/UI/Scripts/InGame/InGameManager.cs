using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; private set; }

    protected override void Init()
    {
        IsDestroyOnLoad = false;

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

        AudioManager.Instance.StopAll();
        //AudioManager.Instance.Play("InGameBGM");
    }
}
