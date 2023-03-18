using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "ScriptableObjects/PlayerState", order = 2)]
public class PlayerState : ScriptableObject
{
    [Header("Ground")]
    [SerializeField, Header("�v���C���[�̈ړ����x")]
    float _speed = 5f;
    [SerializeField, Header("�_�b�V���̔{��")]
    float _dashSpeed = 10f;
    [SerializeField, Header("�v���C���[�̐U��������x")]
    float _turnSpeed = 25f;

    [Header("WallJump")]
    [SerializeField, Header("�v���C���[�̕ǃL�b�N�̗�")]
    float[] _wallJumpPower = new float[2] { 15f, 22f };
    [SerializeField, Header("�ǂ����藎���鑬�x")]
    float _wallSlideSpeed = 0.8f;
    [SerializeField, Header("�ǂ̐ڐG�����Ray�̎˒�")]
    float _wallDistance = 0.1f;
    [SerializeField, Header("�ǂ̐ڐG����")]
    LayerMask[] _wallLayer;

    [Header("Wall")]
    [SerializeField, Header("")]
    float _wallKickInterval = 0.3f;

    public float Speed => _speed;
    public float DashSpeed => _dashSpeed;
    public float TurnSpeed => _turnSpeed;
    public float[] WallJumpPower => _wallJumpPower;
    public float WallSlideSpeed => _wallSlideSpeed;
    public float WallDistance => _wallDistance;
    public LayerMask[] WallLayer => _wallLayer;
    public float WallKickINterval => _wallKickInterval;
}
