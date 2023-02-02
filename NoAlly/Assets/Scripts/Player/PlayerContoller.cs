using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContoller : MonoBehaviour
{
    [Header("Ground")]
    [SerializeField, Header("プレイヤーの移動速度")]
    float _speed = 5f;
    [SerializeField, Header("ダッシュの倍率")]
    float _dashSpeed = 10f;
    [SerializeField, Header("プレイヤーの振り向き速度")]
    float _turnSpeed = 25f;
    [Tooltip("横移動のベクトル")]
    float _h;
    [Tooltip("スライディングの判定")]
    bool _isDash = false;
    [Tooltip("ダッシュ判定")]
    bool _dashChack = false;
    [Tooltip("プレイヤーカメラ")]
    Camera _playerCamera = null;
    [Tooltip("Playerの向き")]
    PlayerVec _playerVec;

    [Header("Jump")]
    [SerializeField, Header("プレイヤーのジャンプ力")]
    float _jumpPower = 5f;
    [SerializeField, Header("接地判定のRayの射程")]
    float _graundDistance = 1f;
    [SerializeField, Header("接地判定のRayの射出点")]
    Transform _footPos;
    [SerializeField, Header("接地判定のSphierCastの半径")]
    float _isGroundRengeRadios = 1f;
    [SerializeField, Header("接地判定のLayerMask")]
    LayerMask _groundMask = ~0;
    [Tooltip("ジャンプの入力判定")]
    bool _isJump = false;

    [Header("WallJump")]
    [SerializeField, Header("プレイヤーの壁キックの力")]
    float _wallJump = 7f;
    [SerializeField]
    float _wallJump2 = 7f;
    [SerializeField, Header("壁をずり落ちる速度")]
    float _wallSlideSpeed = 0.8f;
    [SerializeField, Header("壁の接触判定のRayの射程")]
    float _walldistance = 0.1f;
    [SerializeField, Header("壁の接触判定")]
    LayerMask _wallMask = ~0;
    [SerializeField, Header("接地判定のRayの射出点")]
    Transform _gripPos = null;
    [Tooltip("壁のずり落ち判定")]
    bool _slideWall = false;

    [Header("Animation")]
    [SerializeField, Header("Animationを取得する為の変数")]
    Animator _animator = null;

    [Header("Audio")]
    [SerializeField, Header("ジャンプのサウンド")]
    AudioClip _jumpSound;
    [Tooltip("オーディオを取得する為の変数")]
    AudioSource _audio;

    [Tooltip("壁キックのインターバル")]
    bool _wallKicking = false;
    [Tooltip("PlayerAnimationStateを格納する変数")]
    PlayerAnimationState _animState;
    [Tooltip("Rigidbodyコンポーネントの取得")]
    Rigidbody _rb;
    [Tooltip("プレイヤーのステータス")]
    PlayerStatus _playerStatus = null;
    [Tooltip("プレイヤーの移動ベクトルを取得")]
    Vector3 _velo = Vector3.zero;
    [Tooltip("プレイヤーの移動ベクトルを取得")]
    Vector3 _wallVec = Vector3.zero;
    [Tooltip("接触しているオブジェクトの情報")]
    RaycastHit _hitInfo;


    public Animator PlayerAnimator => _animator;
    public Rigidbody Rb => _rb;
    public PlayerVec Vec => _playerVec;
    //public Vector3 NormalOfStickingWall { get; private set; } = Vector3.zero;
    public enum PlayerVec
    {
        RIGHT,
        LEFT,
    }

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
    //Playerの動きを処理が書かれた関数-------------------------------------//
    /// <summary>
    /// プレイヤーの移動
    /// </summary>
    void MoveMethod(float h, bool dash)
    {
        //if (h != 0)
        //プレイヤーの方向転換
        if (h > 0) h = _playerCamera.transform.right.x;
        else if ((h < 0)) h = _playerCamera.transform.right.x * -1;

        if (h == -1)
        {
            Quaternion rotationLeft = Quaternion.LookRotation(Vector3.left);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationLeft, Time.deltaTime * _turnSpeed);
            _playerVec = PlayerVec.LEFT;
        }
        else if (h == 1)
        {
            Quaternion rotationRight = Quaternion.LookRotation(Vector3.right);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationRight, Time.deltaTime * _turnSpeed);
            _playerVec = PlayerVec.RIGHT;
        }

        float moveSpeed = 0f;
        //プレイヤーの移動
        if (_animState.AbleMove)
        {
            if (IsGrounded() && IsWalled() == PlayerClimbWall.NONE)
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
                //_beforeSpeed = moveSpeed;
            }
            else if (!IsGrounded() && IsWalled() == PlayerClimbWall.NONE)
            {
                if (_dashChack)
                {
                    moveSpeed = _dashSpeed;
                }
                else if (!_dashChack || IsWalled() != PlayerClimbWall.NONE)
                {
                    moveSpeed = _speed;
                }
            }

            Vector3 normalVector = _hitInfo.normal;
            if (_wallKicking)
            {
                h = 0;
            }
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

    }
    /// <summary>
    /// プレイヤーのジャンプ
    /// </summary>
    void JumpMethod(bool jump)
    {
        //ジャンプの処理
        if (jump && IsGrounded() && _animState.AbleMove)
        {
            //m_audio.PlayOneShot(m_jumpSound);
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }
        _animator.SetBool("Jump", !IsGrounded());

    }
    /// <summary>
    /// プレイヤーの壁ジャンプ
    /// </summary>
    void WallJumpMethod(bool jump, bool isDash)
    {
        _animator.SetBool("WallGrip", IsWalled() != PlayerClimbWall.NONE);
        if (IsGrounded())
        {
            _slideWall = false;
            return;
        }
        //ToDo移動コマンドで壁キックの力が変わる様にする
        else if (IsWalled() != PlayerClimbWall.NONE)
        {
            _slideWall = true;
            if (_dashChack) _dashChack = false;
            if (jump)
            {
                StartCoroutine(WallKick());
                _audio.PlayOneShot(_jumpSound);
                Vector3 vec = transform.up + _wallVec * 2f;
                if (_isDash) _rb.AddForce(vec.normalized * _wallJump2, ForceMode.Impulse);
                else _rb.AddForce(vec.normalized * _wallJump, ForceMode.Impulse);
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
    /// <summary>
    /// 接壁判定
    /// </summary>
    public PlayerClimbWall IsWalled()
    {
        Debug.Log(_h);
        if (IsGrounded()) return PlayerClimbWall.NONE;
        else if (Mathf.Abs(_h) < 0.01f) return PlayerClimbWall.NONE;

        Vector3 isWallCenter = _gripPos.transform.position;
        Ray rayRight = new Ray(isWallCenter, Vector3.right + Vector3.up);
        Ray rayLeft = new Ray(isWallCenter, Vector3.left + Vector3.up);

        PlayerClimbWall hitflg = PlayerClimbWall.NONE;
        if (Physics.Raycast(rayRight, out _, _walldistance, _wallMask))
        {
            _wallVec = Vector3.left;
            hitflg = PlayerClimbWall.RIGHT;
        }
        else if (Physics.Raycast(rayLeft, out _, _walldistance, _wallMask))
        {
            _wallVec = Vector3.right;
            hitflg |= PlayerClimbWall.LEFT;
        }
        return hitflg;
    }
    /// <summary>
    /// 接地判定
    /// </summary>
    bool IsGrounded()
    {
        Vector3 isGroundCenter = _footPos.transform.position;
        Ray ray = new Ray(isGroundCenter, Vector3.down);
        bool hitFlg = Physics.SphereCast(ray, _isGroundRengeRadios, out _, _graundDistance, _groundMask);
        return hitFlg;
    }

    IEnumerator WallKick()
    {
        _wallKicking = true;
        yield return new WaitForSeconds(0.5f);
        _wallKicking = false;
    }
    //AnimatorEventで呼ぶ関数----------------------------------------------//
    void WallJump()
    {

    }
    //TODO:効果音やBGMはSoundManagerで管理する
    void FootSound(AudioClip footSound)
    {
        _audio.PlayOneShot(footSound);
    }
    //Vector3 NomarizedMoveVecter()
    //{
    //    Vector3 refVecter = Vector3.zero;
    //    return refVecter;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_footPos.position - new Vector3(0f, _graundDistance, 0f), _isGroundRengeRadios);
        Gizmos.DrawRay(_gripPos.position, (Vector3.right + Vector3.up) * _walldistance);
        Gizmos.DrawRay(_gripPos.position, (Vector3.left + Vector3.up) * _walldistance);
    }
}
