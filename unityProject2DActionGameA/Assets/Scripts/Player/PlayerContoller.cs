using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーの移動速度")] float m_speed = 5f;
    [SerializeField, Tooltip("プレイヤーのクライム速度")] float m_crimbSpeed = 4f;
    [SerializeField, Tooltip("ダッシュの倍率")] float m_dashSpeed = 10f;
    //[SerializeField, Tooltip("空中での移動速度")] float m_grindFroth = 5f;
    [SerializeField, Tooltip("プレイヤーのジャンプ力")] float m_jump = 5f;
    [SerializeField, Tooltip("接地判定のRayの射程")] float m_graundDistance = 1f;
    [SerializeField, Tooltip("壁の接触判定のRayの射程")] float m_walldistance = 0.5f;
    [SerializeField, Tooltip("SphierCastの半径")] float m_isGroundRengeRadios = 1f;
    [SerializeField, Tooltip("壁ジャンの力")] float m_wallJumpPower = 7f;
    [SerializeField, Tooltip("ジャンプのサウンド")] AudioClip m_jumpSound;

    [SerializeField, Tooltip("Rayの射出点")] Transform m_footPos;
    [SerializeField, Tooltip("接地判定のLayerMask")] LayerMask m_groundMask = ~0;
    [SerializeField, Tooltip("壁の接触判定")] LayerMask m_wallMask = ~0;

    [SerializeField, Tooltip("アニメーションを取得する為の変数")] Animator m_Animator;
    [SerializeField, Tooltip("GamManagerを格納する変数")] GameManager m_gameManager;
    [SerializeField, Tooltip("WeaponChangerを格納する変数")] WeaponChanger m_weaponChanger;

    
    //[Tooltip("移動力指定")] float moveSpeed = 0f;
    [Tooltip("Playerの振り向き速度")] float m_turnSpeed = 25f;
    [Tooltip("横移動のベクトル")] float m_h;
    [Tooltip("スライディングの判定")] bool m_isDash = false;
    [Tooltip("")] bool m_ableMove = true;
    [Tooltip("Rigidbodyコンポーネントの取得")]Rigidbody m_rb;
    [Tooltip("プレイヤーの移動ベクトルを取得")] Vector3 m_velo = default;
    [Tooltip("オーディオを取得する為の変数")] AudioSource m_Audio;
    //[Tooltip("Playerの向いている方向")] Quaternion orgLocalQuaternion;
    RaycastHit m_hitInfo;
    public Vector3 NormalOfStickingWall { get; private set; } = Vector3.zero;
    delegate void PlayerMethod();
    PlayerMethod m_playerMethod = default;
    

    void Start()
    {
        //orgLocalQuaternion = this.transform.localRotation;
        m_rb = GetComponent<Rigidbody>();
        m_Audio = gameObject.AddComponent<AudioSource>();
        m_velo = m_rb.velocity;

    }
    void OnEnable()
    {
        m_playerMethod += WallJumpMethod;
        m_playerMethod += MoveMethod;
        m_playerMethod += JumpMethod;
    }
    void OnDisable()
    {
        m_playerMethod -= WallJumpMethod;
        m_playerMethod -= MoveMethod;
        m_playerMethod -= JumpMethod;
    }
    void Update()
    {
        if (!m_gameManager.IsGame) return;
        else
        {
            m_playerMethod();
        }
    }

    //Playerの動きを処理が書かれた関数
    //----------------------------------------------
    void MoveMethod()
    {
        m_h = Input.GetAxisRaw("Horizontal");
        m_isDash = Input.GetButton("Dash");
        var moveSpeed = 0f;

        //プレイヤーの方向転換
        if (m_h == -1)
        {
            Quaternion rotationLeft = Quaternion.LookRotation(Vector3.left);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationLeft, Time.deltaTime * m_turnSpeed);
        }
        else if (m_h == 1)
        {
            Quaternion rotationRight = Quaternion.LookRotation(Vector3.right);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationRight, Time.deltaTime * m_turnSpeed);
        }

        //プレイヤーの移動
        if (m_h != 0 && m_ableMove)
        { 
            moveSpeed = m_speed;
            //ダッシュコマンド
            if (m_isDash)
            {
                if (!IsGrounded() && m_isDash) return;
                moveSpeed = m_dashSpeed;
            }
        }

        m_velo.x = m_h * moveSpeed;
        m_rb.velocity = new Vector3(m_velo.x, m_rb.velocity.y, 0);
        m_Animator.SetFloat("MoveSpeed", Mathf.Abs(m_velo.x));
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Pendulam" || other.gameObject.tag == "Ffield")
        {
            transform.parent = other.gameObject.transform;
        }
    }

    void OnCollisionExit()
    {
        transform.parent = null;
    }

    bool IsGrounded()
    {
        Vector3 isGroundCenter = m_footPos.transform.position;
        Ray ray = new Ray(isGroundCenter, Vector3.down);
        bool hitFlg = Physics.SphereCast(ray, m_isGroundRengeRadios, out _, m_graundDistance, m_groundMask);
        return hitFlg;
    }
    bool IsWalled()
    {
        Vector3 isWallCenter = m_footPos.transform.position;
        Ray rayRight = new Ray(isWallCenter, Vector3.right);
        Ray rayLeft = new Ray(isWallCenter, Vector3.left);
        bool hitFlg = Physics.Raycast(rayRight, out m_hitInfo, m_walldistance, m_wallMask)
                   || Physics.Raycast(rayLeft, out m_hitInfo, m_walldistance, m_wallMask);
        return hitFlg;
    }
    void JumpMethod()
    {
        //ジャンプの処理
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            m_Audio.PlayOneShot(m_jumpSound);
            m_rb.AddForce(0f, m_jump, 0f, ForceMode.Impulse);
        }
        m_Animator.SetBool("Jump", !IsGrounded());
        
    }
    void WallJumpMethod()
    {
        float m_v = Input.GetAxisRaw("Vertical");
        m_Animator.SetBool("WallGrip", IsWalled() && m_h != 0);
        m_weaponChanger.EquipmentWeapon.SetActive(!IsWalled());
        if (IsGrounded())
        {
            m_rb.useGravity = true;
            return;
        }
        //ToDo移動コマンドで壁キックの力が変わる様にする
        else if (IsWalled())
        {
            m_rb.useGravity = false;
            if (m_v != 0)
            {
                m_velo.y = m_v * m_crimbSpeed;
                m_rb.velocity = new Vector3(m_velo.x, m_velo.y, 0);
                m_Animator.SetFloat("CrimbSpeed", m_velo.y);
            }
        }
        else
        {
            m_rb.useGravity = true;
        }
    }
    //AnimatorEventで呼ぶ関数
    //----------------------------------------------
    void MoveJud()
    {
        m_ableMove = !m_ableMove;
    }

    void WallJump()
    {
        m_Audio.PlayOneShot(m_jumpSound);
        Vector3 vec = transform.up + m_hitInfo.normal;
        m_rb.AddForce(vec * m_wallJumpPower, ForceMode.Impulse);
    }
}
