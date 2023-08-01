//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateJudge : MonoBehaviour
{
    [SerializeField, Header("接地、接触判定のposition")]
    Transform[] _playerPartPos = new Transform[3];
    [SerializeField, Header("プレイヤーカメラ")]
    Camera _playerCamera = null;
    [SerializeField, Header("")]
    Vector3 _rayDirection = Vector3.zero;

    [Tooltip("")]
    Dictionary<PlayerPart, Transform> _playerPartPosList = new();
    [Tooltip("")]
    bool[] _isPlayerPart;
    [Tooltip("")]
    RaycastHit[] _raycastHits = null;

    public Transform[] playerPartPos => _playerPartPos;

    public void Initialize()
    {
        _isPlayerPart = new bool[_playerPartPos.Length];
        _raycastHits = new RaycastHit[_playerPartPos.Length];
        for (int i = 0; i < _playerPartPos.Length; i++)
        {
            _playerPartPosList.Add((PlayerPart)i, _playerPartPos[i]);
        }
    }

    /// <summary>
    /// 現在の入力ベクトル
    /// </summary>
    /// <param name="h"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector2 CurrentMoveVectorNormal(float h, float v)
    {
        if (h > 0)
        {
            h = _playerCamera.transform.right.x;
        }
        else if (h < 0)
        {
            h = _playerCamera.transform.right.x * -1;
        }
        return new Vector2(h, v).normalized;
    }
    /// <summary>
    /// プレイヤーのロケーション
    /// </summary>
    /// <param name="ableJump"></param>
    /// <param name="actorParamater"></param>
    /// <param name="currentNormal"></param>
    /// <param name="hitInfo"></param>
    /// <returns></returns>
    public StateOfPlayer ActorCurrentLocation(ActorParamater actorParamater, Vector2 currentNormal, out RaycastHit hitInfo)
    {
        if (IsGrounded(_playerPartPos[2], actorParamater.isGroundRengeRadios, actorParamater.graundDistance, actorParamater.groundMask, out hitInfo))
        {
            return StateOfPlayer.OnGround;
        }
        else
        {
            StateOfPlayer result = IsHitWall(_playerPartPos, actorParamater.walldistance, currentNormal, actorParamater.wallMask, out RaycastHit[] hitInfos);
            if (result != StateOfPlayer.None)
            {
                foreach (var info in hitInfos)
                {
                    if (info.collider)
                    {
                        hitInfo = info;
                        return result;
                    }
                }
            }
        }

        return StateOfPlayer.InAir;
    }
    /// <summary>
    /// 接地判定
    /// </summary>
    /// <param name="radios"></param>
    /// <param name="distance"></param>
    /// <param name="groundMask"></param>
    /// <param name="hitObjInfo"></param>
    /// <returns></returns>
    bool IsGrounded(Transform playerPartPos, float radios, float distance, LayerMask groundMask, out RaycastHit hitInfo)
    {
        Vector3 rayCenter = playerPartPos.transform.position; //Rayの原点
        Ray rayUnderPlayer = new Ray(rayCenter, Vector3.down); //Ray射出

        return (Physics.SphereCast(rayUnderPlayer, radios, out hitInfo, distance, groundMask)); //接地判定をStateOfPlayerで返す
    }
    /// <summary>
    /// 接壁判定
    /// </summary>
    /// <param name="walldistance"></param>
    /// <param name="currentNormal"></param>
    /// <param name="wallMask"></param>
    /// <returns></returns>
    StateOfPlayer IsHitWall(Transform[] playerPartPos, float walldistance, Vector2 currentNormal, LayerMask wallMask, out RaycastHit[] hitInfo)
    {
        for (int i = 0; i < playerPartPos.Length; i++)//頭、胸、足からRayを飛ばし壁に当たっているか判定する
        {
            Ray isWallOnRay = new(playerPartPos[i].transform.position, new(currentNormal.normalized.x, 1f, 0f));
            _isPlayerPart[i] = Physics.Raycast(isWallOnRay, out RaycastHit hit, walldistance, wallMask);
            _raycastHits[i] = hit;
        }
        hitInfo = _raycastHits;
        return HitMaps.HitObjMapToWall(_isPlayerPart);
    }

    public Vector3 GroundNormalChack(Vector2 currntMoveVec, float distance, LayerMask groundMask)
    {
        var rayDirectionX = _rayDirection.x * currntMoveVec.x;
        var rayDirection = (_playerPartPos[2].transform.localPosition + new Vector3(rayDirectionX, _rayDirection.y)).normalized;
        Ray rayUnderPlayer = new Ray(_playerPartPos[2].transform.localPosition, rayDirection); //Ray射出
        if(Physics.Raycast(rayUnderPlayer, out RaycastHit hitInfo, distance, groundMask))
        {
            Vector3 onPlane = Vector3.ProjectOnPlane(new Vector3(currntMoveVec.x, 0f, 0f), hitInfo.normal);
            if(Mathf.Abs(onPlane.y) <= 0.01f)
            {
                return onPlane.normalized;
            }
        }
        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var rayCenter1 = new Vector3(_playerPartPos[2].transform.localPosition.x + _rayDirection.x, _playerPartPos[2].transform.localPosition.y + _rayDirection.y);
        var rayCenter2 = new Vector3(-(_playerPartPos[2].transform.localPosition.x + _rayDirection.x), _playerPartPos[2].transform.localPosition.y + _rayDirection.y);

        Ray rayUnderPlayer1 = new Ray(_playerPartPos[2].transform.position, rayCenter1);
        Ray rayUnderPlayer2 = new Ray(_playerPartPos[2].transform.position, rayCenter2);
        Gizmos.DrawRay(rayUnderPlayer1);
        Gizmos.DrawRay(rayUnderPlayer2);
    }
}
