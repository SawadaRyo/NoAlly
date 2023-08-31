//日本語コメント可
using UnityEngine;

[CreateAssetMenu(fileName = "ActorParamater", menuName = "ScriptableObjects/ActorParamater/Player", order = 1)]

public class ActorParamater : ScriptableObject
{
    [Header("Ground")]
    [SerializeField, Header("プレイヤーの移動速度")]
    public float speed = 5f;
    [SerializeField, Header("ダッシュ時の加算速度")]
    public float dashSpeed = 10f;
    [SerializeField, Header("ダッシュしたときのクールダウン")]
    public float dashIntarval = 0.5f;
    [SerializeField, Header("プレイヤーの振り向き速度")]
    public float turnSpeed = 25f;

    [Header("Jump")]
    [SerializeField, Header("プレイヤーのジャンプ力")]
    public float jumpPower = 7f;
    [SerializeField, Header("プレイヤーの落下速度")]
    public float fallSpeed = 90f;
    [SerializeField, Header("接地判定のRayの射程")]
    public float graundDistance = 0.25f;
    [SerializeField, Header("接地判定のRayの射程")]
    public float graundNormalDistance = 0.25f;
    [SerializeField, Header("接地判定のSphierCastの半径")]
    public float isGroundRengeRadios = 0.2f;
    [SerializeField, Header("接地判定のLayerMask")]
    public LayerMask groundMask = ~0;

    [Header("WallJump")]
    [SerializeField, Header("プレイヤーの壁キックの力")]
    public float wallJumpPower = 10f;
    [SerializeField, Header("壁をずり落ちる速度")]
    public float wallSlideSpeed = 1.5f;
    [SerializeField, Header("次の壁キックに掛かる時間")]
    public float wallKickInterval = 0.3f;
    [SerializeField, Header("壁の接触判定のRayの射程")]
    public float walldistance = 0.3f;
    [SerializeField, Header("壁の接触判定のRayの射程")]
    public float wallRadios = 0.3f;
    [SerializeField, Header("壁接触時のコライダーの位置")]
    public Vector3[] colliderPointOnWall = new Vector3[2];
    [SerializeField, Header("壁の接触判定")]
    public LayerMask wallMask;

    public ActorParamater(ActorParamater orignal)
    {
        speed = orignal.speed;
        dashSpeed = orignal.dashSpeed;
        dashIntarval = orignal.dashIntarval;
        turnSpeed = orignal.turnSpeed;
        jumpPower = orignal.jumpPower;
        fallSpeed = orignal.fallSpeed;
        graundDistance = orignal.graundDistance;
        graundNormalDistance = orignal.graundNormalDistance;
        isGroundRengeRadios = orignal.isGroundRengeRadios;
        groundMask = orignal.groundMask;
        wallJumpPower = orignal.wallJumpPower;
        wallSlideSpeed = orignal.wallSlideSpeed;
        wallKickInterval = orignal.wallKickInterval;
        walldistance = orignal.walldistance;
        wallRadios = orignal.wallRadios;
        colliderPointOnWall = orignal.colliderPointOnWall;
        wallMask = orignal.wallMask;
    }
}
