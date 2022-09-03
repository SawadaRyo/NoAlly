using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContoller : SingletonBehaviour<PlayerContoller>
{
    [Header("Ground")]

    [SerializeField, Tooltip("プレイヤーの移動速度")]
    float _speed = 5f;
    [Tooltip("変化直前のプレイヤーの移動速度")]
    float _beforeSpeed = 0f;
    [SerializeField, Tooltip("ダッシュの倍率")]
    float _dashSpeed = 10f;
    [SerializeField, Tooltip("プレイヤーの振り向き速度")]
    float _turnSpeed = 25f;
    [Tooltip("横移動のベクトル")]
    float _h;
    [Tooltip("スライディングの判定")]
    bool _isDash = false;
    bool _dashChack = false;

    [Header("Jump")]
    [SerializeField, Tooltip("プレイヤーのジャンプ力")]
    float m_jump = 5f;
    [SerializeField, Tooltip("接地判定のRayの射程")]
    float m_graundDistance = 1f;
    [SerializeField, Tooltip("接地判定のRayの射出点")]
    Transform m_footPos;
    [SerializeField, Tooltip("接地判定のSphierCastの半径")]
    float m_isGroundRengeRadios = 1f;
    [SerializeField, Tooltip("接地判定のLayerMask")]
    LayerMask m_groundMask = ~0;
    [Tooltip("ジャンプの入力判定")]
    bool _jump = false;

    [Header("WallJump")]
    [SerializeField, Tooltip("プレイヤーの壁キックの力")]
    float _wallJump = 7f;
    [SerializeField]
    float _wallJump2 = 7f;
    [SerializeField, Tooltip("壁をずり落ちる速度")]
    float m_wallSlideSpeed = 0.8f;
    [SerializeField, Tooltip("壁の接触判定のRayの射程")]
    float m_walldistance = 0.1f;
    [SerializeField, Tooltip("壁の接触判定")]
    LayerMask m_wallMask = ~0;
    [SerializeField, Tooltip("接地判定のRayの射出点")]
    Transform m_gripPos = default;
    [Tooltip("壁のずり落ち判定")]
    bool m_slideWall = false;

    [Header("Animation")]
    [Tooltip("Animationを取得する為の変数")]
    Animator _animator;

    [Header("Audio")]
    [SerializeField, Tooltip("ジャンプのサウンド")]
    AudioClip m_jumpSound;
    [Tooltip("オーディオを取得する為の変数")]
    AudioSource _audio;

    [Tooltip("PlayerAnimationStateを格納する変数")]
    PlayerAnimationState _animState;
    [Tooltip("Rigidbodyコンポーネントの取得")]
    Rigidbody _rb;
    [Tooltip("プレイヤーの移動ベクトルを取得")]
    Vector3 _velo = default;
    [Tooltip("接触しているオブジェクトの情報")]
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
    //Playerの動きを処理が書かれた関数-------------------------------------//
    /// <summary>プレイヤーの移動</summary>
    void MoveMethod(float h, bool dash)
    {
        if (h > 0) h = 1;
        else if ((h < 0)) h = -1;

        float moveSpeed = 0f;

        //プレイヤーの方向転換
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

        //プレイヤーの移動
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
    /// <summary>プレイヤーのジャンプ</summary>
    void JumpMethod(bool jump)
    {
        //ジャンプの処理
        if (jump && IsGrounded())
        {
            //m_audio.PlayOneShot(m_jumpSound);
            _rb.AddForce(0f, m_jump, 0f, ForceMode.Impulse);
        }
        _animator.SetBool("Jump", !IsGrounded());

    }
    /// <summary>プレイヤーの壁ジャンプ</summary>
    void WallJumpMethod(bool jump ,bool isDash)
    {
        _animator.SetBool("WallGrip", IsWalled());
        if (IsGrounded())
        {
            m_slideWall = false;
            return;
        }
        //ToDo移動コマンドで壁キックの力が変わる様にする
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
    /// <summary>接壁判定</summary>
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
    /// <summary>接地判定</summary>
    bool IsGrounded()
    {
        Vector3 isGroundCenter = m_footPos.transform.position;
        Ray ray = new Ray(isGroundCenter, Vector3.down);
        bool hitFlg = Physics.SphereCast(ray, m_isGroundRengeRadios, out _, m_graundDistance, m_groundMask);
        return hitFlg;
    }
    //AnimatorEventで呼ぶ関数----------------------------------------------//
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
