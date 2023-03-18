using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "ScriptableObjects/PlayerState", order = 2)]
public class PlayerState : ScriptableObject
{
    [Header("Ground")]
    [SerializeField, Header("プレイヤーの移動速度")]
    float _speed = 5f;
    [SerializeField, Header("ダッシュの倍率")]
    float _dashSpeed = 10f;
    [SerializeField, Header("プレイヤーの振り向き速度")]
    float _turnSpeed = 25f;

    [Header("WallJump")]
    [SerializeField, Header("プレイヤーの壁キックの力")]
    float[] _wallJumpPower = new float[2] { 15f, 22f };
    [SerializeField, Header("壁をずり落ちる速度")]
    float _wallSlideSpeed = 0.8f;
    [SerializeField, Header("壁の接触判定のRayの射程")]
    float _wallDistance = 0.1f;
    [SerializeField, Header("壁の接触判定")]
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
