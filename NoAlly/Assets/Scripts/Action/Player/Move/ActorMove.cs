//日本語コメント可
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public static class ActorMove
{

    [Tooltip("")]
    static bool _clinbing = false;
    [Tooltip("")]
    static bool _slideWall = false;
    [Tooltip("")]
    static bool _ableJumpInput = true;
    [Tooltip("")]
    static float _timeInAir = 0f;
    [Tooltip("")]
    static Vector2 _velo = Vector2.zero;
    [Tooltip("Playerの向き")]
    static ActorVec _actorVec = ActorVec.Right;
    [Tooltip("")]
    static ActorVec _actorFallJudge = ActorVec.None;

    /// <summary>
    /// プレイヤーの回転
    /// </summary>
    /// <param name="turnSpeed"></param>
    /// <param name="rb"></param>
    /// <param name="rotVector"></param>
    /// <returns></returns>
    static public ActorVec RotateMethod(float turnSpeed, Transform targetObject, Vector2 rotVector)
    {
        //プレイヤーの方向転換
        if (rotVector.x == -1)
        {
            Quaternion rotationLeft = Quaternion.LookRotation(Vector3.left);
            targetObject.rotation = Quaternion.Slerp(targetObject.rotation, rotationLeft, Time.deltaTime * turnSpeed);
            _actorVec = ActorVec.Left;
        }
        else if (rotVector.x == 1)
        {
            Quaternion rotationRight = Quaternion.LookRotation(Vector3.right);
            targetObject.rotation = Quaternion.Slerp(targetObject.rotation, rotationRight, Time.deltaTime * turnSpeed);
            _actorVec = ActorVec.Right;
        }
        else if (rotVector.y == 1)
        {
            Quaternion rotationUp = Quaternion.LookRotation(Vector3.zero);
            targetObject.rotation = Quaternion.Slerp(targetObject.rotation, rotationUp, Time.deltaTime * turnSpeed);
            _actorVec = ActorVec.Up;
        }
        return _actorVec;
    }
    /// <summary>
    /// プレイヤーの移動
    /// </summary>
    /// <param name="h"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="normalVector">地面の法線ベクトル</param>
    /// <param name="playerState"></param>
    /// <returns></returns>
    static public Vector3 MoveMethod(float h, float moveSpeed, Rigidbody rb, Vector3 normalVector)
    {
        //プレイヤーの移動
        Vector3 velo = Vector3.zero;
        Vector3 onPlane = Vector3.ProjectOnPlane(new Vector3(h, 0f, 0f), normalVector);
        _velo.x = onPlane.x * moveSpeed;
        _velo.y = onPlane.y * moveSpeed;
        if (Mathf.Abs(_velo.y) <= 0.01f)
        {
            velo = new Vector3(_velo.x, rb.velocity.y, 0);
        }
        else if (Mathf.Abs(_velo.y) > 0.01f)
        {
            velo = new Vector3(_velo.x, _velo.y, 0);
        }
        return velo;
    }
    /// <summary>
    /// プレイヤーのダッシュ
    /// </summary>
    /// <param name="DashPower"></param>
    /// <returns></returns>
    static public Vector3 DodgeVec(Vector2 currentVec, float DashPower = 10f, float interval = 0.5f)
    {
        return new Vector3(currentVec.x * DashPower, currentVec.y, 0f);
    }
    /// <summary>
    /// プレイヤーのジャンプ
    /// </summary>
    /// <param name="jumpPower"></param>
    /// <param name="rb"></param>
    /// <param name="currentNormal"></param>
    /// <param name="stateOfPlayer"></param>
    static public void ActorJumpMethod(bool isJump,float jumpPower, Rigidbody rb, Vector2 currentNormal, (bool, StateOfPlayer) stateOfPlayer)
    {
        if (stateOfPlayer.Item1 && stateOfPlayer.Item2 == StateOfPlayer.None)
        {
            //rb.AddForce(rb.transform.up * jumpPower, ForceMode.Impulse);
            rb.velocity = new Vector3(rb.velocity.x, ActorMove.ActorBehaviourInAir(isJump, jumpPower, stateOfPlayer).y, 0f);
        }
        else if (stateOfPlayer.Item2 == StateOfPlayer.GripingWall && _ableJumpInput)
        {
            Vector3 vec = new Vector3(currentNormal.x, rb.transform.up.y, 0f).normalized * -1f;
            Vector3 kickPower;

            kickPower = vec.normalized * jumpPower;

            //RotateMethod((Vector2)_hitInfo.normal);
            rb.AddForce(kickPower, ForceMode.Impulse);
            AbleWallKick();
        }
    }
    static public Vector3 ActorBehaviourInAir(bool isJump,float jumoPawer, (bool, StateOfPlayer) stateOfPlayer, float fallSpeed = 60f, float jumpLowerLimit = 0.03f)
    {
        Vector3 ActorVertical = Vector3.zero;

        if (stateOfPlayer.Item1)
        {
            if (isJump && _ableJumpInput)
            {
                _actorFallJudge = ActorVec.Up;
                _ableJumpInput = false;
            }
            else if (_actorFallJudge == ActorVec.Down)
            {
                _actorFallJudge = ActorVec.None;
            }
            else if (!isJump)
            {
                _timeInAir = 0f;
                _ableJumpInput = true;
            }
        }
        switch (_actorFallJudge)
        {
            case ActorVec.Up:
                _timeInAir += Time.deltaTime;
                if (isJump || jumpLowerLimit > _timeInAir)
                {
                    ActorVertical.y = jumoPawer;
                    ActorVertical.y -= (fallSpeed * Mathf.Pow(_timeInAir, 2));
                }
                else
                {
                    _timeInAir += Time.deltaTime; // 落下を早める
                    ActorVertical.y = jumoPawer;
                    ActorVertical.y -= (fallSpeed * Mathf.Pow(_timeInAir, 2));
                }

                if (0f > ActorVertical.y)
                {
                    _actorFallJudge = ActorVec.Down;
                    ActorVertical.y = 0f;
                    _timeInAir = 0.1f;
                }
                break;

            case ActorVec.Down:
                _timeInAir += Time.deltaTime;

                ActorVertical.y = 0f;
                ActorVertical.y = -(fallSpeed * Mathf.Pow(_timeInAir, 2));
                break;

            default:
                break;
        }
        return ActorVertical;
    }
    /// <summary>
    /// プレイヤーの壁張り付き
    /// </summary>
    /// <param name="jump"></param>
    /// <param name="wallSlideSpeed"></param>
    /// <param name="wallJumpPower"></param>
    /// <param name="rb"></param>
    /// <param name="hitInfo"></param>
    /// <param name="stateOfPlayer"></param>
    static public void ActorBehaviourInWall(float wallSlideSpeed, Rigidbody rb, RaycastHit[] hitInfo, StateOfPlayer stateOfPlayer)
    {
        switch (stateOfPlayer)
        {
            case StateOfPlayer.GripingWall:
                if (!_slideWall)
                {
                    rb.isKinematic = true;
                    _slideWall = true;
                    rb.isKinematic = false;
                }
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
                break;
            case StateOfPlayer.GripingWallEdge:
                for (int i = 0; i < hitInfo.Length; i++)
                {
                    if (hitInfo[i].collider == null) continue;
                    if (!_clinbing && hitInfo[i].collider.TryGetComponent(out BoxCollider col))
                    {
                        _slideWall = false;
                        Vector3 wallOfTop = new Vector3(hitInfo[i].transform.position.x
                                                     , hitInfo[i].transform.position.y + col.size.y
                                                     , hitInfo[i].transform.position.z);
                        Climbing(rb, wallOfTop);
                        break;
                    }
                }
                break;
            case StateOfPlayer.HangingWallEgde:
                if (!_clinbing && hitInfo[2].collider.TryGetComponent(out BoxCollider col1))
                {
                    _slideWall = false;
                    Vector3 wallOfTop = new Vector3(hitInfo[2].transform.position.x
                                                 , hitInfo[2].transform.position.y + col1.size.y
                                                 , hitInfo[2].transform.position.z);
                    Climbing(rb, wallOfTop);
                }
                break;
            default:
                if (_slideWall)
                {
                    _slideWall = false;
                }
                break;
        }
    }
    static public Vector3 ActorBehaviourJump()
    {
        Vector3 ActorVertical = Vector3.zero;
        return ActorVertical;
    }
    /// <summary>
    /// 壁キックのインターバル
    /// </summary>
    /// <param name="interval"></param>
    static async void AbleWallKick(float interval = 0.2f)
    {
        _ableJumpInput = false;
        await UniTask.Delay(TimeSpan.FromSeconds(interval));
        _ableJumpInput = true;
    }
    /// <summary>
    /// 壁のよじ登り
    /// </summary>
    /// <param name="endPoint">よじ登る終点</param>
    /// <param name="duration">よじ登るのにかかる時間</param>
    /// <returns></returns>
    static void Climbing(Rigidbody rb, Vector3 endPoint, float duration = 0.5f)
    {
        //float time = 0;
        //Vector3 startPoint = rb.transform.position;

        //_animator.SetBool("Climbing", true);

        rb.transform.DOMove(endPoint, duration)
            .OnStart(() =>
            {
                rb.isKinematic = true;
                _clinbing = true;
                _ableJumpInput = false;
            })
            .OnComplete(() =>
            {
                rb.transform.position = endPoint;
                _clinbing = false;
                _ableJumpInput = true;
                rb.isKinematic = false;
            });
        //while (time < duration)
        //{
        //    rb.transform.position = Vector3.Lerp(startPoint, endPoint, time / duration);
        //    time += Time.deltaTime;
        //}
        //_animator.SetBool("Climbing", false);
        //_animator.SetBool("WallGrip", false);
        //rb.transform.position = endPoint;
        //_clinbing = false;
        //_ableInput = true;
        //rb.isKinematic = false;
    }
}
