using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMove", menuName = "ScriptableObjects/PlayerState/PlayerMove", order = 2)]
public class PlayerState : ScriptableObject
{
    [Header("Ground")]
    [SerializeField, Header("�v���C���[�̈ړ����x")]
    float _speed = 5f;
    [SerializeField, Header("�_�b�V���̔{��")]
    float _dashSpeed = 10f;
    [SerializeField, Header("�v���C���[�̐U��������x")]
    float _turnSpeed = 25f;
    PlayerVec _playerVec;

    [Header("WallJump")]
    [SerializeField, Header("�v���C���[�̕ǃL�b�N�̗�")]
    float[] _wallJumpPower = new float[2] { 15f, 22f };
    [SerializeField, Header("�ǂ����藎���鑬�x")]
    float _wallSlideSpeed = 0.8f;
    [SerializeField, Header("�ǂ̐ڐG�����Ray�̎˒�")]
    float _wallDistance = 0.1f;
    [SerializeField, Header("�ǂ̐ڐG����")]
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

    //public void RotateMethod(PlayerContoller player,Vector2 rotVector,bool IsGrounded)
    //{
    //    //if (h != 0)
    //    //�v���C���[�̕����]��
    //    if (rotVector.x == -1)
    //    {
    //        Quaternion rotationLeft = Quaternion.LookRotation(Vector3.left);
    //        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotationLeft, Time.deltaTime * _turnSpeed);
    //        _playerVec = PlayerVec.LEFT;
    //    }
    //    else if (rotVector.x == 1)
    //    {
    //        Quaternion rotationRight = Quaternion.LookRotation(Vector3.right);
    //        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotationRight, Time.deltaTime * _turnSpeed);
    //        _playerVec = PlayerVec.RIGHT;
    //    }
    //    else if (rotVector.y == 1)
    //    {
    //        Quaternion rotationUp = Quaternion.LookRotation(Vector3.zero);
    //        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotationUp, Time.deltaTime * _turnSpeed);
    //        _playerVec = PlayerVec.UP;
    //    }
    //}
    ///// <summary>
    ///// �v���C���[�̈ړ�
    ///// </summary>
    //public void MoveMethod(float h, bool dash)
    //{
    //    float moveSpeed = 0f;
    //    //�v���C���[�̈ړ�
    //    if (_animState.AbleMove)
    //    {
    //        if (IsGrounded && IsWalled(CurrentNormal(new Vector2(_h, _v))) == PlayerClimbWall.NONE)
    //        {
    //            if (dash)
    //            {
    //                moveSpeed = _dashSpeed;
    //            }
    //            else
    //            {
    //                moveSpeed = _speed;
    //            }
    //            //_beforeSpeed = moveSpeed;
    //        }
    //        else if (!IsGrounded() && IsWalled(CurrentNormal(new Vector2(_h, _v))) == PlayerClimbWall.NONE)
    //        {
    //            if (dash)
    //            {
    //                moveSpeed = _dashSpeed;
    //            }
    //            else if (!dash || IsWalled(CurrentNormal(new Vector2(_h, _v))) != PlayerClimbWall.NONE)
    //            {
    //                moveSpeed = _speed;
    //            }
    //        }

    //        Vector3 normalVector = _hitInfo.normal;
    //        if (_isKickWall)
    //        {
    //            h = 0;
    //        }
    //        Vector3 onPlane = Vector3.ProjectOnPlane(new Vector3(h, 0f, 0f), normalVector);
    //        _velo.x = onPlane.x * moveSpeed;
    //        _velo.y = onPlane.y * moveSpeed;
    //        if (Mathf.Abs(_velo.y) <= 0.01f)
    //        {
    //            _rb.velocity = new Vector3(_velo.x, _rb.velocity.y, 0);
    //        }
    //        else if (Mathf.Abs(_velo.y) > 0.01f)
    //        {
    //            _rb.velocity = new Vector3(_velo.x, _velo.y, 0);
    //        }
    //        _animator.SetFloat("MoveSpeed", Mathf.Abs(_velo.x));
    //    }

    //}
    ///// <summary>
    ///// �v���C���[�̃W�����v
    ///// </summary>
    //void JumpMethod(bool jump)
    //{
    //    //�W�����v�̏���
    //    if (jump && IsGrounded() && _animState.AbleMove)
    //    {
    //        //m_audio.PlayOneShot(m_jumpSound);
    //        _rb.AddForce(gameObject.transform.up * _jumpPower, ForceMode.Impulse);
    //    }
    //    _animator.SetBool("Jump", !IsGrounded());

    //}
    ///// <summary>
    ///// �v���C���[�̕ǃW�����v
    ///// </summary>
    //void WallJumpMethod(bool jump, bool isDash)
    //{
    //    _animator.SetBool("WallGrip", IsWalled(CurrentNormal(new Vector2(_h, _v))) != PlayerClimbWall.NONE);
    //    if (IsGrounded())
    //    {
    //        _slideWall = false;
    //        return;
    //    }
    //    //ToDo�ړ��R�}���h�ŕǃL�b�N�̗͂��ς��l�ɂ���
    //    else if (IsWalled(CurrentNormal(new Vector2(_h, _v))) != PlayerClimbWall.NONE)
    //    {
    //        if (!_slideWall)
    //        {
    //            _rb.isKinematic = true;
    //            _slideWall = true;
    //            _rb.isKinematic = false;
    //        }
    //        if (jump)
    //        {
    //            StartCoroutine(AbleWallKick());
    //            Vector3 vec = transform.up + _wallVec * 2f;
    //            if (isDash) _rb.AddForce(vec.normalized * _wallJump2, ForceMode.Impulse);
    //            else _rb.AddForce(vec.normalized * _wallJump, ForceMode.Impulse);
    //            //_animator.SetTrigger("WallJump");
    //        }
    //    }
    //    else
    //    {
    //        _slideWall = false;
    //    }


    //    if (_slideWall)
    //    {
    //        _rb.velocity = new Vector3(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -_wallSlideSpeed, float.MaxValue));
    //    }
    //}
    //IEnumerator AbleWallKick()
    //{
    //    _isKickWall = true;
    //    yield return new WaitForSeconds(0.2f);
    //    _isKickWall = false;
    //}
}
