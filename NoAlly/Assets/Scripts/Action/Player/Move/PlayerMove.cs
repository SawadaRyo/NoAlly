//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Header("")]
    GameObject _targetObject = null;

    [Tooltip("Playerの向き")]
    PlayerVec _playerVec = PlayerVec.Right;

    public PlayerVec RotateMethod(Vector2 rotVector)
    {
        //プレイヤーの方向転換
        if (rotVector.x == -1)
        {
            Quaternion rotationLeft = Quaternion.LookRotation(Vector3.left);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationLeft, Time.deltaTime * _turnSpeed);
            _playerVec = PlayerVec.Left;
        }
        else if (rotVector.x == 1)
        {
            Quaternion rotationRight = Quaternion.LookRotation(Vector3.right);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationRight, Time.deltaTime * _turnSpeed);
            _playerVec = PlayerVec.Right;
        }
        else if (rotVector.y == 1)
        {
            Quaternion rotationUp = Quaternion.LookRotation(Vector3.zero);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationUp, Time.deltaTime * _turnSpeed);
            _playerVec = PlayerVec.Up;
        }
        return _playerVec;
    }
    /// <summary>
    /// プレイヤーの移動
    /// </summary>
    public void MoveMethod(float h, bool dash,bool isGrounded)
    {
        float moveSpeed = 0f;
        //プレイヤーの移動
        if (isGrounded && IsWalled(CurrentNormal(new Vector2(_h, _v))) == PlayerWallState.None)
        {
            if (dash)
            {
                moveSpeed = _dashSpeed;
            }
            else
            {
                moveSpeed = _speed;
            }
            //_beforeSpeed = moveSpeed;
        }
        else if (!isGrounded && IsWalled(CurrentNormal(new Vector2(_h, _v))) == PlayerWallState.None)
        {
            if (dash)
            {
                moveSpeed = _dashSpeed;
            }
            else if (!dash || IsWalled(CurrentNormal(new Vector2(_h, _v))) != PlayerWallState.None)
            {
                moveSpeed = _speed;
            }
        }

        Vector3 normalVector = _hitInfo.normal;
        if (_ableInput)
        {
            Vector3 onPlane = Vector3.ProjectOnPlane(new Vector3(h, 0f, 0f), normalVector);
            _velo.x = onPlane.x * moveSpeed;
            _velo.y = onPlane.y * moveSpeed;
            if (Mathf.Abs(_velo.y) <= 0.01f)
            {
                _rb.velocity = new Vector3(_velo.x, _rb.velocity.y, 0);
            }
            else if (Mathf.Abs(_velo.y) > 0.01f && !_isJump)
            {
                _rb.velocity = new Vector3(_velo.x, _velo.y, 0);
            }
        }

        _animator.SetFloat("MoveSpeed", Mathf.Abs(_velo.normalized.x * moveSpeed));
    }
    /// <summary>
    /// プレイヤーのジャンプ
    /// </summary>
    public void JumpMethod(bool jump)
    {
        //ジャンプの処理
        if (jump && IsGrounded() && _animState.AbleMove)
        {
            //m_audio.PlayOneShot(m_jumpSound);
            _rb.AddForce(gameObject.transform.up * _jumpPower, ForceMode.Impulse);
        }
    }
    /// <summary>
    /// プレイヤーの壁ジャンプ
    /// </summary>
    public void WallJumpMethod(bool jump, bool isDash, PlayerWallState climbWall)
    {
        switch (climbWall)
        {
            case PlayerWallState.None:
                _animator.SetBool("WallGrip", false);
                _slideWall = false;
                return;
            case PlayerWallState.Griping:
                _animator.SetBool("WallGrip", true);
                if (!_slideWall)
                {
                    _rb.isKinematic = true;
                    _slideWall = true;
                    _rb.isKinematic = false;
                }
                if (jump)
                {
                    Vector3 vec = transform.up + _wallVec;
                    Debug.Log(_wallVec);
                    Vector3 kickPower;
                    if (isDash)
                    {
                        kickPower = vec.normalized * _wallJump2;
                    }
                    else
                    {
                        kickPower = vec.normalized * _wallJump;
                    }
                    RotateMethod((Vector2)_hitInfo.normal);
                    _rb.AddForce(kickPower, ForceMode.Impulse);
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
            _rb.velocity = new Vector3(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -_wallSlideSpeed, float.MaxValue));
        }
    }
    IEnumerator AbleWallKick()
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
    IEnumerator Climbing(Vector3 endPoint, float duration)
    {
        float time = 0;
        Vector3 startPoint = transform.position;
        _rb.isKinematic = true;
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
        _rb.isKinematic = false;
    }
}
