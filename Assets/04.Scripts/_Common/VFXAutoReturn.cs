using System;
using UnityEngine;

public class VFXAutoReturn : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;

    private Action<GameObject> _onComplete;
    private float _timer;
    private bool _isPlaying;

    public void Play(Action<GameObject> onComplete)
    {
        _onComplete = onComplete;
        _timer = _duration;
        _isPlaying = true;
    }

    private void Update()
    {
        if (!_isPlaying) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _isPlaying = false;
            _onComplete?.Invoke(gameObject);
        }
    }
}
