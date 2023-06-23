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
    [Tooltip("接触しているオブジェクトの情報")]
    Dictionary<PlayerPart, RaycastHit> _hitObjInfo;

    Dictionary<bool[], PlayerWallState> _hitObjMap = new()
    {   
        //壁に掴まっている
        {new bool[] { true,true,true },PlayerWallState.Griping},//全ての部位が当たっている
        {new bool[] { true,true,false },PlayerWallState.Griping},//頭、胴が当たっている
        {new bool[] { false,true,false },PlayerWallState.Griping},//胴のみが当たっている

        //よじ登っている
        {new bool[] { false,true,true },PlayerWallState.GripingEdge},//胴、足が当たっている

        //足をかけて登っている
        {new bool[]{ false,false,true },PlayerWallState.HangingEgde}//足のみ当たっている
    };


    void Start()
    {
        for (int i = 0; i < _playerPartPos.Length; i++)
        {
            _playerPartPosList.Add((PlayerPart)i, _playerPartPos[i]);
        }
    }

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
    public void IsHitWall(float walldistance, Vector2 currentNormal,out Vector3 wallVec, LayerMask wallMask, out RaycastHit hitObjInfo)
    {
        for (int i = 0; i < _playerPartPos.Length; i++)//頭、胸、足からRayを飛ばし壁に当たっているか判定する
        {
            Ray isWallOnRay = new(_playerPartPos[i].transform.position, new(currentNormal.normalized.x, 1f, 0f));
            _isPlayerPart[i] = Physics.Raycast(isWallOnRay, out _, walldistance, wallMask);
        }
        WallNormal(_hitObjMap[_isPlayerPart], currentNormal, out wallVec, out hitObjInfo);
    }
    public PlayerWallState WallNormal(PlayerWallState isHits, Vector2 currentNormal, out Vector3 wallVec, out RaycastHit hitObjInfo)
    {
        hitObjInfo = new();
        wallVec = Vector3.zero;

        if ()
        {
            wallVec = new Vector3(currentNormal.x, 0f, 0f).normalized * -1f;
            return PlayerWallState.Griping;
        }
        else if (!_isPlayerPart[0] && _isPlayerPart[1] && _isPlayerPart[2]
              || !_isPlayerPart[0] && !_isPlayerPart[1] && _isPlayerPart[2])
        {
            return PlayerWallState.GripingEdge;
        }
        return PlayerWallState.None;
    }
    /// <summary>
    /// 接地判定
    /// </summary>
    StateOfPlayer IsGrounded(float radios, float distance, LayerMask groundMask, out RaycastHit hitObjInfo)
    {
        StateOfPlayer ofPlayer = StateOfPlayer.None;
        Vector3 isGroundCenter = _playerPartPos[2].transform.position; //Rayの原点
        Ray ray = new Ray(isGroundCenter, Vector3.down);　//Ray射出
        if (Physics.SphereCast(ray, radios, out hitObjInfo, distance, groundMask)) //Ray末端にSphereCast
        {
            ofPlayer = StateOfPlayer.Ground;
        }
        else
        {
            ofPlayer = StateOfPlayer.Ari;
        }
        return ofPlayer; //接地判定をStateOfPlayerで返す
    }
}
