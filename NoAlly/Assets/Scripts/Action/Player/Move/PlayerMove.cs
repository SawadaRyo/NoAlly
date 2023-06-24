//日本語コメント可
using System;
using System.Collections;
using UnityEngine;

public static class ActorMove
{

    [Tooltip("")]
    static bool _clinbing = false;
    [Tooltip("")]
    static bool _slideWall = false;
    [Tooltip("")]
    static Vector2 _velo = Vector2.zero;
    [Tooltip("プレイヤーの壁の移動ベクトルを取得")]
    static Vector3 _wallVec = Vector3.zero;
    [Tooltip("Playerの向き")]
    static PlayerVec _playerVec = PlayerVec.Right;


    static public PlayerVec RotateMethod(float turnSpeed, Rigidbody rb, Vector2 rotVector)
    {
        //プレイヤーの方向転換
        if (rotVector.x == -1)
        {
            Quaternion rotationLeft = Quaternion.LookRotation(Vector3.left);
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, rotationLeft, Time.deltaTime * turnSpeed);
            _playerVec = PlayerVec.Left;
        }
        else if (rotVector.x == 1)
        {
            Quaternion rotationRight = Quaternion.LookRotation(Vector3.right);
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, rotationRight, Time.deltaTime * turnSpeed);
            _playerVec = PlayerVec.Right;
        }
        else if (rotVector.y == 1)
        {
            Quaternion rotationUp = Quaternion.LookRotation(Vector3.zero);
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, rotationUp, Time.deltaTime * turnSpeed);
            _playerVec = PlayerVec.Up;
        }
        return _playerVec;
    }
    /// <summary>
    /// プレイヤーの移動
    /// </summary>
    /// <param name="h"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="normalVector">地面の法線ベクトル</param>
    /// <param name="playerState"></param>
    /// <returns></returns>
    static public Vector3 MoveMethod(float h, float moveSpeed, Rigidbody rb, Vector3 normalVector, PlayerWallState playerState)
    {
        //プレイヤーの移動
        if (playerState == PlayerWallState.None)
        {
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
        return Vector3.zero;
    }
    static public void Dodge()
    {

    }
    /// <summary>
    /// プレイヤーのジャンプ
    /// </summary>
    static public void ActorJumpMethod(float jumpPower,Rigidbody rb)
    {
        //m_audio.PlayOneShot(m_jumpSound);
        rb.AddForce(rb.transform.up * jumpPower, ForceMode.Impulse);
    }
    /// <summary>
    /// プレイヤーの壁ジャンプ
    /// </summary>
    static public void WallJumpMethod(bool isDash,float wallSlideSpeed, Rigidbody rb,PlayerWallState climbWall)
    {
        switch (climbWall)
        {
            case PlayerWallState.None:
                _slideWall = false;
                return;
            case PlayerWallState.Griping:
                if (!_slideWall)
                {
                    rb.isKinematic = true;
                    _slideWall = true;
                    rb.isKinematic = false;
                }
                if (jump)
                {
                    Vector3 vec = rb.transform.up + _wallVec;
                    Vector3 kickPower;
                    if (isDash)
                    {
                        kickPower = vec.normalized * _wallJump2;
                    }
                    else
                    {
                        kickPower = vec.normalized * _wallJump;
                    }
                    //RotateMethod((Vector2)_hitInfo.normal);
                    rb.AddForce(kickPower, ForceMode.Impulse);
                    StartCoroutine(AbleWallKick());
                }
                break;
            case PlayerWallState.GripingEdge:
                if (!_clinbing && _hitInfo.collider.TryGetComponent(out BoxCollider col))
                {
                    _slideWall = false;
                    Vector3 wallOfTop = new Vector3(_hitInfo.transform.position.x
                                                 , _hitInfo.transform.position.y + col.size.y
                                                 , _hitInfo.transform.position.z);
                    StartCoroutine(Climbing(wallOfTop, 0.5f));
                }
                return;
        }


        if (_slideWall)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }
    static void WallNormal(PlayerWallState isHits, Vector2 currentNormal, out Vector3 wallVec, out RaycastHit hitObjInfo)
    {
        hitObjInfo = new();
        wallVec = Vector3.zero;
        switch (isHits)
        {
            case PlayerWallState.Griping:
                wallVec = new Vector3(currentNormal.x, 0f, 0f).normalized * -1f;
                break;
            case PlayerWallState.GripingEdge:
                break;
            case PlayerWallState.HangingEgde:
                break;
            default:
                break;

        }
    }
    static IEnumerator AbleWallKick()
    {
        _ableInput = false;
        yield return new WaitForSeconds(0.2f);
        _ableInput = true;
    }
    /// <summary>
    /// 壁のよじ登り
    /// </summary>
    /// <param name="endPoint">よじ登る終点</param>
    /// <param name="duration">よじ登るのにかかる時間</param>
    /// <returns></returns>
    static IEnumerator Climbing(float duration, Rigidbody rb, Vector3 endPoint)
    {
        float time = 0;
        Vector3 startPoint = rb.transform.position;
        rb.isKinematic = true;
        _clinbing = true;
        _animator.SetBool("Climbing", true);
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPoint, endPoint, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _animator.SetBool("Climbing", false);
        _animator.SetBool("WallGrip", false);
        transform.position = endPoint;
        _clinbing = false;
        rb.isKinematic = false;
    }
}
