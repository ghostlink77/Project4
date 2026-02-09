/*
플레이어의 애니메이션을 관리하는 스크립트.
플레이어의 애니메이션 작동은 이곳에서 관리한다.
*/
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;

    public void SetUp()
    {
        _animator = PlayerManager.Instance.Animator;
    }
}
