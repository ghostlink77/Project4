using UnityEngine;
using UnityEngine.Playables;

public class LevelUPUI : MonoBehaviour
{
    [SerializeField] private PlayableDirector _showAnimation;
    [SerializeField] private PlayableDirector _hideAnimation;
    private void OnEnable()
    {
        if (_showAnimation != null)
            _showAnimation.Play();
    }

    public void PlayHideAnimation()
    {
        if (_hideAnimation != null)
            _hideAnimation.Play();
    }
}
