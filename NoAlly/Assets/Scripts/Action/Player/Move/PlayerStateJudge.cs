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

    [Tooltip("")]
    Dictionary<PlayerPart, Transform> _playerPartPosList = new();
    [Tooltip("")]
    bool[] _isPlayerPart;


    void Start()
    {
        for (int i = 0; i < _playerPartPos.Length; i++)
        {
            _playerPartPosList.Add((PlayerPart)i, _playerPartPos[i]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputVector"></param>
    /// <returns></returns>
    Vector2 CurrentMoveVector(Vector2 inputVector)
    {
        float h = 0;
        float v = 0;
        if (inputVector.x > 0)
        {
            h = _playerCamera.transform.right.x;
        }
        else if (inputVector.x < 0)
        {
            h = _playerCamera.transform.right.x * -1;
        }
        if (inputVector.y > 0)
        {
            v = _playerCamera.transform.up.y;
        }

        return new Vector2(h, v);
    }

    /// <summary>
    /// 接壁判定
    /// </summary>
    /// <param name="walldistance"></param>
    /// <param name="currentNormal"></param>
    /// <param name="wallMask"></param>
    /// <returns></returns>
    public StateOfPlayer IsHitWall(float walldistance, Vector2 currentNormal,LayerMask wallMask)
    {
        for (int i = 0; i < _playerPartPos.Length; i++)//頭、胸、足からRayを飛ばし壁に当たっているか判定する
        {
            Ray isWallOnRay = new(_playerPartPos[i].transform.position, new(currentNormal.normalized.x, 1f, 0f));
            _isPlayerPart[i] = Physics.Raycast(isWallOnRay, out _, walldistance, wallMask);
        }
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
    StateOfPlayer IsGrounded(float radios, float distance, LayerMask groundMask, out RaycastHit hitObjInfo)
    {
        StateOfPlayer ofPlayer = StateOfPlayer.None;
        Vector3 isGroundCenter = _playerPartPos[2].transform.position; //Rayの原点
        Ray ray = new Ray(isGroundCenter, Vector3.down);　//Ray射出
        if (Physics.SphereCast(ray, radios, out hitObjInfo, distance, groundMask)) //Ray末端にSphereCast
        {
            ofPlayer = StateOfPlayer.OnGround;
        }
        else
        {
            ofPlayer = StateOfPlayer.InAri;
        }
        return ofPlayer; //接地判定をStateOfPlayerで返す
    }
}
