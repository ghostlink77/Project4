using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayableDirector _fadeAnim;
    [SerializeField] private TextMeshProUGUI _playTimeText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private List<Inventory> _itemInventories = new List<Inventory>();

    private void OnEnable()
    {
        if (_fadeAnim != null)
            _fadeAnim.Play();
        UpdateGameResultData();
    }

    private void OnDisable()
    {
        
    }

    private void UpdateGameResultData()
    {
        float time = InGameManager.Instance.PlayTime;
        string min = ((int)(time / 60)).ToString("D2");
        string sec = ((int)(time % 60)).ToString("D2");
        _playTimeText.text = $"{min} : {sec}";
        _levelText.text = PlayerManager.Instance.PlayerStatController.CurrentLevel.ToString();

        foreach (var inventory in _itemInventories)
            inventory.UpdateSlot();
    }
}
