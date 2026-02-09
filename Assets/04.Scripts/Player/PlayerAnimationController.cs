using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;

    public void SetUp()
    {
        _animator = PlayerManager.Instance.Animator;
    }
}
