using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContoller : MonoBehaviour
{
    [Header("Ground")]

    [SerializeField, Tooltip("�v���C���[�̈ړ����x")]
    float _speed = 5f;
    [Tooltip("�ω����O�̃v���C���[�̈ړ����x")]
    float _beforeSpeed = 0f;
    [SerializeField, Tooltip("�_�b�V���̔{��")]
    float _dashSpeed = 10f;
    [SerializeField, Tooltip("�v���C���[�̐U��������x")]
    float _turnSpeed = 25f;
    [Tooltip("���ړ��̃x�N�g��")]
    float _h;
    [Tooltip("�X���C�f�B���O�̔���")]
    bool _isDash = false;
    bool _dashChack = false;
    [Tooltip("�v���C���[�J����")]
    Camera _playerCamera = null;

    [Header("Jump")]
    [SerializeField, Tooltip("�v���C���[�̃W�����v��")]
    float _jumpPower = 5f;
    [SerializeField, Tooltip("�ڒn�����Ray�̎˒�")]
    float _graundDistance = 1f;
    [SerializeField, Tooltip("�ڒn�����Ray�̎ˏo�_")]
    Transform _footPos;
    [SerializeField, Tooltip("�ڒn�����SphierCast�̔��a")]
    float _isGroundRengeRadios = 1f;
    [SerializeField, Tooltip("�ڒn�����LayerMask")]
    LayerMask _groundMask = ~0;
    [Tooltip("�W�����v�̓��͔���")]
    bool _isJump = false;

    [Header("WallJump")]
    [SerializeField, Tooltip("�v���C���[�̕ǃL�b�N�̗�")]
    float _wallJump = 7f;
    [SerializeField]
    float _wallJump2 = 7f;
    [SerializeField, Tooltip("�ǂ����藎���鑬�x")]
    float _wallSlideSpeed = 0.8f;
    [SerializeField, Tooltip("�ǂ̐ڐG�����Ray�̎˒�")]
    float _walldistance = 0.1f;
    [SerializeField, Tooltip("�ǂ̐ڐG����")]
    LayerMask _wallMask = ~0;
    [SerializeField, Tooltip("�ڒn�����Ray�̎ˏo�_")]
    Transform _gripPos = null;
    [Tooltip("�ǂ̂��藎������")]
    bool _slideWall = false;

    [Header("Animation")]
    [SerializeField,Tooltip("Animation���擾����ׂ̕ϐ�")]
    Animator _animator = null;

    [Header("Audio")]
    [SerializeField, Tooltip("�W�����v�̃T�E���h")]
    AudioClip m_jumpSound;
    [Tooltip("�I�[�f�B�I���擾����ׂ̕ϐ�")]
    AudioSource _audio;

    [Tooltip("PlayerAnimationState���i�[����ϐ�")]
    PlayerAnimationState _animState;
    [Tooltip("Rigidbody�R���|�[�l���g�̎擾")]
    Rigidbody _rb;
    [Tooltip("�v���C���[�̈ړ��x�N�g�����擾")]
    Vector3 _velo = Vector3.zero;
    [Tooltip("�ڐG���Ă���I�u�W�F�N�g�̏��")]
    RaycastHit _hitInfo;
    [Tooltip("�v���C���[�̃X�e�[�^�X")]
    PlayerStatus _playerStatus = null;

    public Animator PlayerAnimator => _animator;
    public Vector3 NormalOfStickingWall { get; private set; } = Vector3.zero;
    public RaycastHit HitInfo => _hitInfo;


    void Start()
    {
        _playerCamera = GameObject.FindObjectOfType<Camera>();
        _rb = GetComponent<Rigidbody>();
        _velo = _rb.velocity;
        _animator = GetComponent<Animator>();
        _playerStatus = GetComponentInChildren<PlayerStatus>();
        _audio = gameObject.AddComponent<AudioSource>();
        _animState = PlayerAnimationState.Instance;
    }
    void Update()
    {
        if (GameManager.Instance.IsGame == GameState.InGame && _playerStatus.Living)
        {
            _h = Input.GetAxisRaw("Horizontal");
            _isDash = Input.GetButton("Dash");
            _isJump = Input.GetButtonDown("Jump");

            JumpMethod(_isJump);
        }
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.IsGame == GameState.InGame)
        {
            MoveMethod(_h, _isDash);
            WallJumpMethod(_isJump, _isDash);
        }
    }
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Vector3 isGroundCenter = m_footPos.transform.position;
    //    Ray ray = new Ray(isGroundCenter, Vector3.right);
    //    Gizmos.DrawRay(ray);
    //}
    //Player�̓����������������ꂽ�֐�-------------------------------------//
    /// <summary>�v���C���[�̈ړ�</summary>
    void MoveMethod(float h, bool dash)
    {
        //if (h != 0)
        //�v���C���[�̕����]��
        {
            if (h > 0) h = _playerCamera.transform.right.x;
            else if ((h < 0)) h = _playerCamera.transform.right.x * -1;

            if (h == -1)
            {
                Quaternion rotationLeft = Quaternion.LookRotation(Vector3.left);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationLeft, Time.deltaTime * _turnSpeed);
            }
            else if (h == 1)
            {
                Quaternion rotationRight = Quaternion.LookRotation(Vector3.right);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationRight, Time.deltaTime * _turnSpeed);
            }
        }

        float moveSpeed = 0f;
        //�v���C���[�̈ړ�
        if (h != 0 && _animState.AbleMove)
        {
            if (IsGrounded())
            {
                if (dash)
                {
                    moveSpeed = _dashSpeed;
                    _dashChack = true;
                }
                else
                {
                    moveSpeed = _speed;
                    _dashChack = false;
                }
                _beforeSpeed = moveSpeed;
            }
            else
            {
                if (_dashChack)
                {
                    moveSpeed = _dashSpeed;
                }
                else if (!_dashChack || IsWalled())
                {
                    moveSpeed = _speed;
                }
            }
        }
        Vector3 normalVector = _hitInfo.normal;
        Vector3 onPlane = Vector3.ProjectOnPlane(new Vector3(h, 0f, 0f), normalVector);
        _velo.x = onPlane.x * moveSpeed;
        _velo.y = onPlane.y * moveSpeed;
        if (Mathf.Abs(_velo.y) <= 0.01f)
        {
            _rb.velocity = new Vector3(_velo.x, _rb.velocity.y, 0);
        }
        else if (Mathf.Abs(_velo.y) > 0.01f)
        {
            _rb.velocity = new Vector3(_velo.x, _velo.y, 0);
        }
        _animator.SetFloat("MoveSpeed", Mathf.Abs(_velo.x));
    }
    /// <summary>�v���C���[�̃W�����v</summary>
    void JumpMethod(bool jump)
    {
        //�W�����v�̏���
        if (jump && IsGrounded() && _animState.AbleMove)
        {
            //m_audio.PlayOneShot(m_jumpSound);
            _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
        }
        _animator.SetBool("Jump", !IsGrounded());

    }
    /// <summary>�v���C���[�̕ǃW�����v</summary>
    void WallJumpMethod(bool jump, bool isDash)
    {
        _animator.SetBool("WallGrip", IsWalled());
        if (IsGrounded())
        {
            _slideWall = false;
            return;
        }
        //ToDo�ړ��R�}���h�ŕǃL�b�N�̗͂��ς��l�ɂ���
        else if (IsWalled())
        {
            _slideWall = true;
            if (_dashChack) _dashChack = false;
            _h = 0;
            if (jump)
            {
                _animator.SetTrigger("WallJump");
            }
        }
        else
        {
            _slideWall = false;
        }


        if (_slideWall)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -_wallSlideSpeed, float.MaxValue));
        }
    }
    /// <summary>�ڕǔ���</summary>
    public bool IsWalled()
    {
        if (IsGrounded()) return false;

        Vector3 isWallCenter = _gripPos.transform.position;
        Ray rayRight = new Ray(isWallCenter, Vector3.right);
        Ray rayLeft = new Ray(isWallCenter, Vector3.left);

        bool hitflg = Physics.Raycast(rayRight, out _hitInfo, _walldistance, _wallMask)
        || Physics.Raycast(rayLeft, out _hitInfo, _walldistance, _wallMask);

        return hitflg;
    }
    /// <summary>�ڒn����</summary>
    bool IsGrounded()
    {
        Vector3 isGroundCenter = _footPos.transform.position;
        Ray ray = new Ray(isGroundCenter, Vector3.down);
        bool hitFlg = Physics.SphereCast(ray, _isGroundRengeRadios, out _hitInfo, _graundDistance, _groundMask);
        return hitFlg;
    }
    //AnimatorEvent�ŌĂԊ֐�----------------------------------------------//
    void WallJump()
    {
        _audio.PlayOneShot(m_jumpSound);
        Vector3 vec = transform.up + _hitInfo.normal;
        if (_isDash) _rb.AddForce(vec * _wallJump2, ForceMode.Impulse);
        else _rb.AddForce(vec * _wallJump, ForceMode.Impulse);
    }
    //TODO:���ʉ���BGM��SoundManager�ŊǗ�����
    void FootSound(AudioClip footSound)
    {
        _audio.PlayOneShot(footSound);
    }
    Vector3 NomarizedMoveVecter()
    {
        Vector3 refVecter = Vector3.zero;
        return refVecter;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_footPos.position - new Vector3(0f, _graundDistance, 0f), _isGroundRengeRadios);
    }
}
