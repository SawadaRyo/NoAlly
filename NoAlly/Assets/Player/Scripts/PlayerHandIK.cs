//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandIK : MonoBehaviour
{
    [SerializeField, Header("IKを有効にするか")]
    bool _isActive = false;
    [SerializeField, Header("プレイヤーのアニメーション")]
    Animator _playerAnimator = null;
    [SerializeField,Header("左手のターゲット")]
    Transform _leftTarget = default;
    [SerializeField, Range(0f, 1f), Header("左手の Position に対するウェイト")] 
    float _leftPositionWeight = 0;
    [SerializeField, Range(0f, 1f),Header("左手の Rotation に対するウェイト")] 
    float _leftRotationWeight = 0;

    void OnAnimatorIK()
    {
        if (_isActive)
        {
            // 左手に対して IK を設定する
            _playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftPositionWeight);
            _playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _leftRotationWeight);
            _playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, _leftTarget.position);
            _playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, _leftTarget.rotation);
        }
    }
}
