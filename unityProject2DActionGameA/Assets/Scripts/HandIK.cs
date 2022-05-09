using UnityEngine;

/// <summary>
/// 手の IK を制御する。
/// </summary>
[RequireComponent(typeof(Animator))]
public class HandIK : MonoBehaviour
{
    /// <summary>右手のターゲット</summary>
    [SerializeField] Transform _rightTarget = default;
    /// <summary>左手のターゲット</summary>
    [SerializeField] Transform _leftTarget = default;
    /// <summary>右手の Position に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _rightPositionWeight = 0;
    /// <summary>右手の Rotation に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _rightRotationWeight = 0;
    /// <summary>左手の Position に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _leftPositionWeight = 0;
    /// <summary>左手の Rotation に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _leftRotationWeight = 0;
    Animator m_animator = default;

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        // 右手に対して IK を設定する
        m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightPositionWeight);
        m_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _rightRotationWeight);
        m_animator.SetIKPosition(AvatarIKGoal.RightHand, _rightTarget.position);
        m_animator.SetIKRotation(AvatarIKGoal.RightHand, _rightTarget.rotation);
        // 左手に対して IK を設定する
        m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftPositionWeight);
        m_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _leftRotationWeight);
        m_animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftTarget.position);
        m_animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftTarget.rotation);
    }
}
