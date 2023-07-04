//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStateJudge : MonoBehaviour
{
    [SerializeField, Header("接地、接触判定のposition")]
    Transform[] _playerPartPos = new Transform[3];
    [SerializeField, Header("プレイヤーカメラ")]
    Camera _playerCamera = null;

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
    public Vector2 CurrentMoveVector(float h, float v)
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

    public StateOfPlayer ActorCurrentLocation(bool ableJump, ActorParamater actorParamater, Vector2 currentNormal, out RaycastHit hitInfo)
    {

        if (IsGrounded(actorParamater.isGroundRengeRadios, actorParamater.graundDistance, actorParamater.groundMask, out hitInfo) && ableJump)
        {
            return StateOfPlayer.OnGround;
        }
        else
        {
            StateOfPlayer result = IsHitWall(actorParamater.walldistance, currentNormal, actorParamater.wallMask, out RaycastHit[] hitInfos);
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
    /// 接壁判定
    /// </summary>
    /// <param name="walldistance"></param>
    /// <param name="currentNormal"></param>
    /// <param name="wallMask"></param>
    /// <returns></returns>
    StateOfPlayer IsHitWall(float walldistance, Vector2 currentNormal, LayerMask wallMask, out RaycastHit[] hitInfo)
    {
        for (int i = 0; i < _playerPartPos.Length; i++)//頭、胸、足からRayを飛ばし壁に当たっているか判定する
        {
            Ray isWallOnRay = new(_playerPartPos[i].transform.position, new(currentNormal.normalized.x, 1f, 0f));
            _isPlayerPart[i] = Physics.Raycast(isWallOnRay, out RaycastHit hit, walldistance, wallMask);
            _raycastHits[i] = hit;
        }
        hitInfo = _raycastHits;
        return HitMaps.HitObjMapToWall(_isPlayerPart);
    }

    /// <summary>
    /// 接地判定
    /// </summary>
    /// <param name="radios"></param>
    /// <param name="distance"></param>
    /// <param name="groundMask"></param>
    /// <param name="hitObjInfo"></param>
    /// <returns></returns>
    bool IsGrounded(float radios, float distance, LayerMask groundMask, out RaycastHit hitInfo)
    {
        Vector3 isGroundCenter = _playerPartPos[2].transform.position; //Rayの原点
        Ray ray = new Ray(isGroundCenter, Vector3.down); //Ray射出

        return (Physics.SphereCast(ray, radios, out hitInfo, distance, groundMask)); //接地判定をStateOfPlayerで返す
    }
}
