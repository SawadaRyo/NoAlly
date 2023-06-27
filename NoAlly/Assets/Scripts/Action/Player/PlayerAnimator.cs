//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    Animator _animator = null;

    public void Initialize(PlayerMoveInput moveInput)
    {
        AnimationState(moveInput);
    }

    public void AnimationState(PlayerMoveInput moveInput)
    {
        moveInput
    }
}
