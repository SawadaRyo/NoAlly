using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerContoller : MonoBehaviour
{
    [Header("Ground")]
    [SerializeField, Header("�v���C���[�̈ړ����x")]
    float _speed = 5f;
    [SerializeField, Header("�_�b�V���̔{��")]
    float _dashSpeed = 10f;
    [SerializeField, Header("�v���C���[�̐U��������x")]
    float _turnSpeed = 25f;
    [Tooltip("���ړ��̃x�N�g��")]
    float _h;
    [Tooltip("�c�ړ��̃x�N�g��")]
    float _v;
    [Tooltip("�X���C�f�B���O�̔���")]
    bool _isDash = false;
    [Tooltip("Player�̌���")]
    ActorVec _playerVec;

    [Header("Camera")]
    [SerializeField,Tooltip("�v���C���[�J����")]
    Camera _playerCamera = null;

    [Header("Jump")]
    [SerializeField, Header("�v���C���[�̃W�����v��")]
    float _jumpPower = 5f;
    [SerializeField, Header("�ڒn�����Ray�̎˒�")]
    float _graundDistance = 1f;
    [SerializeField, Header("�ڒn�����SphierCast�̔��a")]
    float _isGroundRengeRadios = 1f;
    [SerializeField, Header("�ڒn�����LayerMask")]
    LayerMask _groundMask = ~0;
    [Tooltip("�W�����v�̓��͔���")]
    bool _isJump = false;

    [Header("WallJump")]
    [SerializeField, Header("�v���C���[�̕ǃL�b�N�̗�")]
    float _wallJump = 7f;
    [SerializeField]
    float _wallJump2 = 7f;
    [SerializeField, Header("�ǂ����藎���鑬�x")]
    float _wallSlideSpeed = 0.8f;
    [SerializeField, Header("�ǂ̐ڐG�����Ray�̎˒�")]
    float _walldistance = 0.1f;
    [SerializeField, Header("�ǂ̐ڐG����")]
    LayerMask[] _wallMask;
    [Tooltip("�ǂ̂��藎������")]
    bool _slideWall = false;

    [Header("Animation")]
    [SerializeField, Header("Animation���擾����ׂ̕ϐ�")]
    Animator _animator = null;

    [Tooltip("���͉\���ǂ���")]
    bool _ableInput = true;
    [Tooltip("PlayerAnimationState���i�[����ϐ�")]
    PlayerAttackStateController _animState;
    [Tooltip("Rigidbody�R���|�[�l���g�̎擾")]
    Rigidbody _rb;
    [Tooltip("�v���C���[�̃X�e�[�^�X")]
    PlayerStatus _playerStatus = null;
    [Tooltip("�v���C���[�̈ړ��x�N�g�����擾")]
    Vector3 _velo = Vector3.zero;
    [Tooltip("�v���C���[�̕ǂ̈ړ��x�N�g�����擾")]
    Vector3 _wallVec = Vector3.zero;
    [Tooltip("�ڐG���Ă���I�u�W�F�N�g�̏��")]
    RaycastHit _hitInfo;

    bool[] _isClimbWall = new bool[3];
    bool _clinbing = false;
    bool _ableJumpInput = true;
    [SerializeField, Header("�ڒn�A�ڐG�����position")]
    Transform[] _isClimbWallPos = new Transform[3];

    public bool ableJumpInput { get => _ableJumpInput; set => _ableJumpInput = value; }
    public Rigidbody Rb => _rb;
    public ActorVec Vec => _playerVec;
    public RaycastHit HitInfo => _hitInfo;


    void Start()
    {
        _playerCamera = FindObjectOfType<Camera>();
        _rb = GetComponent<Rigidbody>();
        _velo = _rb.velocity;
        _animator = GetComponent<Animator>();
        _playerStatus = GetComponentInChildren<PlayerStatus>();
        _animState = PlayerAttackStateController.Instance;
    }
    void Update()
    {
        if (GameManager.Instance.IsGame == GameState.InGame && _playerStatus.Living)
        {
            _h = Input.GetAxisRaw("Horizontal");
            _v = Input.GetAxisRaw("Vertical");
            _isDash = Input.GetButton("Dash");
            _isJump = Input.GetButtonDown("Jump");

            if (_ableJumpInput)
            {
                JumpMethod(_isJump);
                WallJumpMethod(_isJump, _isDash, IsWalled(CurrentNormal(new Vector2(_h, _v))));
            }
        }

    }
    void FixedUpdate()
    {
        if (GameManager.Instance.IsGame == GameState.InGame && _playerStatus.Living)
        {
            MoveMethod(CurrentNormal(new Vector2(_h, _v)).x, _isDash);
            RotateMethod(CurrentNormal(new Vector2(_h, _v)));
        }
    }

    Vector2 CurrentNormal(Vector2 inputVector)
    {
        float h = 0;
        float v = 0;
        if (inputVector.x > 0)
        {
            h = _playerCamera.transform.right.x;
        }
        else if ((inputVector.x < 0))
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
    /// �ڕǔ���
    /// </summary>
    public PlayerWallState IsWalled(Vector2 currentNormal)
    {
        if (IsGrounded()) return PlayerWallState.None;
        else if (Mathf.Abs(_h) < 0.01f) return PlayerWallState.None;
        int hitCount = 0;
        for (int i = 0; i < _isClimbWallPos.Length; i++)
        {
            Ray isWallOnRay = new(_isClimbWallPos[i].transform.position, new Vector3(currentNormal.normalized.x, 1f, 0f));
            _isClimbWall[i] = Physics.Raycast(isWallOnRay, out _hitInfo, _walldistance, _wallMask[(int)PlayerWallState.Griping]);
            if (_isClimbWall[i])
            {
                hitCount++;
            }
        }
        if (hitCount == _isClimbWallPos.Length)
        {
            _wallVec = new Vector3(currentNormal.x, 0f, 0f).normalized * -1f;
            return PlayerWallState.Griping;
        }
        else if (!_isClimbWall[0] && _isClimbWall[1] && _isClimbWall[2]
              || !_isClimbWall[0] && !_isClimbWall[1] && _isClimbWall[2])
        {
            return PlayerWallState.GripingEdge;
        }
        return PlayerWallState.None;
    }
    /// <summary>
    /// �ڒn����
    /// </summary>
    bool IsGrounded()
    {
        Vector3 isGroundCenter = _isClimbWallPos[2].transform.position; //Ray�̌��_
        Ray ray = new Ray(isGroundCenter, Vector3.down);�@//Ray�ˏo
        bool hitFlg = Physics.SphereCast(ray, _isGroundRengeRadios, out _hitInfo, _graundDistance, _groundMask);�@//Ray���[��SphereCast
        return hitFlg; //�ڒn�����bool�ŕԂ�
    }

    //Player�̓����������������ꂽ�֐�-------------------------------------//
    void RotateMethod(Vector2 rotVector)
    {
        if (!_ableInput) return;
        //�v���C���[�̕����]��
        if (rotVector.x == -1)
        {
            Quaternion rotationLeft = Quaternion.LookRotation(Vector3.left);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationLeft, Time.deltaTime * _turnSpeed);
            _playerVec = ActorVec.Left;
        }
        else if (rotVector.x == 1)
        {
            Quaternion rotationRight = Quaternion.LookRotation(Vector3.right);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationRight, Time.deltaTime * _turnSpeed);
            _playerVec = ActorVec.Right;
        }
        else if (rotVector.y == 1)
        {
            Quaternion rotationUp = Quaternion.LookRotation(Vector3.zero);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationUp, Time.deltaTime * _turnSpeed);
            _playerVec = ActorVec.Up;
        }
    }
    /// <summary>
    /// �v���C���[�̈ړ�
    /// </summary>
    void MoveMethod(float h, bool dash)
    {
        float moveSpeed = 0f;
        //�v���C���[�̈ړ�
        if (_animState.AbleMove)
        {
            if (IsGrounded() && IsWalled(CurrentNormal(new Vector2(_h, _v))) == PlayerWallState.None)
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
            else if (!IsGrounded() && IsWalled(CurrentNormal(new Vector2(_h, _v))) == PlayerWallState.None)
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
            else
            {
                //_animator.SetFloat("MoveSpeed", 0);
            }
        }
        _animator.SetFloat("MoveSpeed", Mathf.Abs(_velo.normalized.x * moveSpeed));
    }
    /// <summary>
    /// �v���C���[�̃W�����v
    /// </summary>
    void JumpMethod(bool jump)
    {
        //�W�����v�̏���
        if (jump && IsGrounded() && _animState.AbleMove)
        {
            //m_audio.PlayOneShot(m_jumpSound);
            _rb.AddForce(gameObject.transform.up * _jumpPower, ForceMode.Impulse);
        }
        _animator.SetBool("Jump", !IsGrounded());

    }
    /// <summary>
    /// �v���C���[�̕ǃW�����v
    /// </summary>
    void WallJumpMethod(bool jump, bool isDash, PlayerWallState climbWall)
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
    /// �悶�o��
    /// </summary>
    /// <param name="endPoint">�悶�o��I�_</param>
    /// <param name="duration">�悶�o��̂ɂ����鎞��</param>
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_isClimbWallPos[2].position - new Vector3(0f, _graundDistance, 0f), _isGroundRengeRadios);
        Gizmos.DrawRay(_isClimbWallPos[1].position, (new Vector3(_h, 0f, 0f) + Vector3.up) * _walldistance);
        Gizmos.DrawRay(_isClimbWallPos[2].position, new Vector3(_velo.x, _velo.y, 0));

        Gizmos.DrawRay(_isClimbWallPos[0].position, transform.forward * _walldistance);
        Gizmos.DrawRay(_isClimbWallPos[1].position, transform.forward * _walldistance);
        Gizmos.DrawRay(_isClimbWallPos[2].position, transform.forward * _walldistance);
    }
}

