using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContoller : SingletonBehaviour<PlayerContoller>
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

    [Header("Jump")]
    [SerializeField, Tooltip("�v���C���[�̃W�����v��")]
    float m_jump = 5f;
    [SerializeField, Tooltip("�ڒn�����Ray�̎˒�")]
    float m_graundDistance = 1f;
    [SerializeField, Tooltip("�ڒn�����Ray�̎ˏo�_")]
    Transform m_footPos;
    [SerializeField, Tooltip("�ڒn�����SphierCast�̔��a")]
    float m_isGroundRengeRadios = 1f;
    [SerializeField, Tooltip("�ڒn�����LayerMask")]
    LayerMask m_groundMask = ~0;
    [Tooltip("�W�����v�̓��͔���")]
    bool _jump = false;

    [Header("WallJump")]
    [SerializeField, Tooltip("�v���C���[�̕ǃL�b�N�̗�")]
    float _wallJump = 7f;
    [SerializeField]
    float _wallJump2 = 7f;
    [SerializeField, Tooltip("�ǂ����藎���鑬�x")]
    float m_wallSlideSpeed = 0.8f;
    [SerializeField, Tooltip("�ǂ̐ڐG�����Ray�̎˒�")]
    float m_walldistance = 0.1f;
    [SerializeField, Tooltip("�ǂ̐ڐG����")]
    LayerMask m_wallMask = ~0;
    [SerializeField, Tooltip("�ڒn�����Ray�̎ˏo�_")]
    Transform m_gripPos = default;
    [Tooltip("�ǂ̂��藎������")]
    bool m_slideWall = false;

    [Header("Animation")]
    [Tooltip("Animation���擾����ׂ̕ϐ�")]
    Animator _animator;

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
    Vector3 _velo = default;
    [Tooltip("�ڐG���Ă���I�u�W�F�N�g�̏��")]
    RaycastHit m_hitInfo;

    public Vector3 NormalOfStickingWall { get; private set; } = Vector3.zero;
    delegate void WeaponAttacks();
    WeaponAttacks _weaponAttack;

    void Start()
    {
        //orgLocalQuaternion = this.transform.localRotation;
        _rb = GetComponent<Rigidbody>();
        _audio = gameObject.AddComponent<AudioSource>();
        _velo = _rb.velocity;
        _animator = GetComponent<Animator>();
        _animState = PlayerAnimationState.Instance;
    }
    void Update()
    {
        if (GameManager.Instance.IsGame)
        {
            _h = Input.GetAxisRaw("Horizontal");
            _isDash = Input.GetButton("Dash");
            _jump = Input.GetButtonDown("Jump");
            WallJumpMethod(_jump,_isDash);
            JumpMethod(_jump);
        }
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.IsGame)
        {
            MoveMethod(_h, _isDash);

        }
    }
    void OnCollisionEnter(Collision other)
    {
        var otherCollider = other.gameObject;
        if (otherCollider)
        {
            if (otherCollider.tag == "Pendulam" || otherCollider.tag == "Ffield")
            {
                transform.parent = other.gameObject.transform;
            }
        }
    }
    void OnCollisionExit()
    {
        transform.parent = null;
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
        if (h > 0) h = 1;
        else if ((h < 0)) h = -1;

        float moveSpeed = 0f;

        //�v���C���[�̕����]��
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
                if(_dashChack)
                {
                    moveSpeed = _dashSpeed;
                }
                else if (!_dashChack || IsWalled())
                {
                    moveSpeed = _speed;
                }
            }
        }

        _velo.x = h * moveSpeed;
        _rb.velocity = new Vector3(_velo.x, _rb.velocity.y, 0);
        _animator.SetFloat("MoveSpeed", Mathf.Abs(_velo.x));
    }
    /// <summary>�v���C���[�̃W�����v</summary>
    void JumpMethod(bool jump)
    {
        //�W�����v�̏���
        if (jump && IsGrounded())
        {
            //m_audio.PlayOneShot(m_jumpSound);
            _rb.AddForce(0f, m_jump, 0f, ForceMode.Impulse);
        }
        _animator.SetBool("Jump", !IsGrounded());

    }
    /// <summary>�v���C���[�̕ǃW�����v</summary>
    void WallJumpMethod(bool jump ,bool isDash)
    {
        _animator.SetBool("WallGrip", IsWalled());
        if (IsGrounded())
        {
            m_slideWall = false;
            return;
        }
        //ToDo�ړ��R�}���h�ŕǃL�b�N�̗͂��ς��l�ɂ���
        else if (IsWalled())
        {
            m_slideWall = true;
            _h = 0;
            if (jump)
            {
                _animator.SetTrigger("WallJump");
                Vector3 vec = transform.up + m_hitInfo.normal;
                //_rb.AddForce(vec * m_wallJump, ForceMode.Impulse);

                if (isDash) _rb.velocity = new Vector3(_rb.velocity.x, _wallJump2, 0);
                else _rb.velocity = new Vector3(_rb.velocity.x, _wallJump, 0);
            }
        }
        else
        {
            m_slideWall = false;
        }


        if (m_slideWall)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -m_wallSlideSpeed, float.MaxValue));
        }
    }
    /// <summary>�ڕǔ���</summary>
    public bool IsWalled()
    {
        if (IsGrounded()) return false;

        Vector3 isWallCenter = m_gripPos.transform.position;
        Ray rayRight = new Ray(isWallCenter, Vector3.right);
        Ray rayLeft = new Ray(isWallCenter, Vector3.left);

        bool hitflg = Physics.Raycast(rayRight, out m_hitInfo, m_walldistance, m_wallMask)
        || Physics.Raycast(rayLeft, out m_hitInfo, m_walldistance, m_wallMask);

        return hitflg;
    }
    /// <summary>�ڒn����</summary>
    bool IsGrounded()
    {
        Vector3 isGroundCenter = m_footPos.transform.position;
        Ray ray = new Ray(isGroundCenter, Vector3.down);
        bool hitFlg = Physics.SphereCast(ray, m_isGroundRengeRadios, out _, m_graundDistance, m_groundMask);
        return hitFlg;
    }
    //AnimatorEvent�ŌĂԊ֐�----------------------------------------------//
    void WallJump()
    {
        _audio.PlayOneShot(m_jumpSound);
        Vector3 vec = transform.up + m_hitInfo.normal;
        if(_isDash) _rb.AddForce(vec * _wallJump2, ForceMode.Impulse);
        else _rb.AddForce(vec * _wallJump, ForceMode.Impulse);
    }
    void FootSound(AudioClip footSound)
    {

    }
    Vector3 NomarizedMoveVecter()
    {
        Vector3 refVecter = Vector3.zero;
        return refVecter;
    }
}
