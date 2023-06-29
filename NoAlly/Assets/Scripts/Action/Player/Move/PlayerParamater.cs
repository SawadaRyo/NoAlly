//日本語コメント可
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParamater", menuName = "ScriptableObjects/PlayerParamater", order = 1)]

public class PlayerParamater : ScriptableObject
{
    [Header("Ground")]
    [SerializeField, Header("プレイヤーの移動速度")]
    float _speed = 5f;
    [SerializeField, Header("ダッシュ時の加算速度")]
    float _dashSpeed = 10f;
    [SerializeField, Header("ダッシュしたときのクールダウン")]
    float _dashIntarval = 0.5f;
    [SerializeField, Header("プレイヤーの振り向き速度")]
    float _turnSpeed = 25f;

    [Header("Jump")]
    [SerializeField, Header("プレイヤーのジャンプ力")]
    float _jumpPower = 7f;
    [SerializeField, Header("接地判定のRayの射程")]
    float _graundDistance = 0.25f;
    [SerializeField, Header("接地判定のSphierCastの半径")]
    float _isGroundRengeRadios = 0.2f;
    [SerializeField, Header("接地判定のLayerMask")]
    LayerMask _groundMask = ~0;

    [Header("WallJump")]
    [SerializeField, Header("プレイヤーの壁キックの力")]
    float _wallJump = 10f;
    [SerializeField, Header("壁をずり落ちる速度")]
    float _wallSlideSpeed = 1.5f;
    [SerializeField, Header("次の壁キックに掛かる時間")]
    float _wallKickInterval = 0.3f;
    [SerializeField, Header("壁の接触判定のRayの射程")]
    float _walldistance = 0.3f;
    [SerializeField, Header("壁の接触判定のRayの射程")]
    float _wallRadios = 0.3f;
    [SerializeField, Header("壁接触時のコライダーの位置")]
    Vector3[] _colliderPointOnWall = new Vector3[2];
    [SerializeField, Header("壁の接触判定")]
    LayerMask _wallMask;

    public float speed => _speed;
    public float dashSpeed => _dashSpeed;
    public float turnSpeed => _turnSpeed;
    public float dashInterval => _dashIntarval;

    public float jumpPower => _jumpPower;
    public float graundDistance => _graundDistance;
    public float isGroundRengeRadios => _isGroundRengeRadios;
    public LayerMask groundMask => _groundMask;

    public float wallJump => _wallJump;
    public float wallSlideSpeed => _wallSlideSpeed;
    public float walldistance => _walldistance;
    public float wallKickInterval => _wallKickInterval;
    public float wallRadios => _wallRadios;
    public Vector3[] colliderPointOnWall => _colliderPointOnWall;
    public LayerMask wallMask => _wallMask;
}
