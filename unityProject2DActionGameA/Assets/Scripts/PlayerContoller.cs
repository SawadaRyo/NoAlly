using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField, Tooltip("溜め攻撃の溜めカウンター")] int m_chrageAttackCounter = 120;
    [SerializeField, Tooltip("プレイヤーの移動速度")] float speed = 5f;
    [SerializeField, Tooltip("ダッシュの倍率")] float dashPowor = 10f;
    [SerializeField, Tooltip("空中での移動速度")] float grindFroth = 5f;
    [SerializeField, Tooltip("プレイヤーのジャンプ力")] float jump = 3f;
    [SerializeField, Tooltip("接地判定のRayの射程")] float distance = 0.1f;
    [SerializeField, Tooltip("壁の接触判定のRayの射程")] float walldistance = 0.1f;
    [SerializeField, Tooltip("SphierCastの半径")] float isGroundRengeRadios = 1f;
    [SerializeField, Tooltip("壁ジャンの力")] float wallJumpPower = 10f;
    [SerializeField, Tooltip("ジャンプのサウンド")] AudioClip jumpSound;
    [SerializeField, Tooltip("プレイヤーのダメージサウンド")] AudioClip damageSound;

    [SerializeField, Tooltip("Rayの射出点")] Transform footPos;
    [SerializeField, Tooltip("接地判定のLayerMask")] LayerMask groundMask = ~0;
    [SerializeField, Tooltip("壁の接触判定")] LayerMask wallMask = ~0;

    [SerializeField, Tooltip("アニメーションを取得する為の変数")] Animator m_Animator;
    [SerializeField, Tooltip("GamManagerを格納する変数")] GameManager m_gameManager;

    [SerializeField, Tooltip("武器prefabを格納する変数")] GameObject[] m_weaponPrefab = new GameObject[4];
    [SerializeField, Tooltip("メイン武器")] GameObject m_mainWeapon = default;
    [SerializeField, Tooltip("サブ武器")] GameObject m_subWeapon = default;


    [Tooltip("溜め攻撃の溜め時間")] int m_chrageAttackCount = 0;
    [Tooltip("Playerの振り向き速度")]float m_turnSpeed = 25f;
    [Tooltip("横移動のベクトル")]float m_h;
    [Tooltip("武器切り替え")] bool m_weaponSwitch = true;
    [Tooltip("スライディングの判定")] bool m_isDash = false;
    bool m_attacked = false;
    bool m_ableMove = false;
    [Tooltip("Rigidbodyコンポーネントの取得")]Rigidbody m_rb;
    //List<ItemBase> m_ItemList = new List<ItemBase>();
    [Tooltip("オーディオを取得する為の変数")] AudioSource m_Audio;
    //[Tooltip("Playerの向いている方向")] Quaternion orgLocalQuaternion;
    [Tooltip("装備中の武器")] GameObject m_equipmentWeapon = default;
    RaycastHit m_hitInfo;
    public Vector3 NormalOfStickingWall { get; private set; } = Vector3.zero;
    public bool Charge { get; private set; } = false;
    public int ChrageAttackCount { get => m_chrageAttackCount; set => m_chrageAttackCount = value; }
    public bool Attacked { get => m_attacked; set => m_attacked = value; }
    delegate void PlayerMethod();
    PlayerMethod m_playerMethod = default;
    

    void Start()
    {
        //orgLocalQuaternion = this.transform.localRotation;
        m_rb = GetComponent<Rigidbody>();
        m_Audio = gameObject.AddComponent<AudioSource>();

        m_weaponSwitch = true;
        //m_mainWeapon = m_weaponPrefab[0];
        //m_subWeapon = m_weaponPrefab[1];
        m_mainWeapon.SetActive(true);
        m_subWeapon.SetActive(false);
        m_equipmentWeapon = m_mainWeapon;
        //m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void OnEnable()
    {
        m_playerMethod += JumpMethod;
        m_playerMethod += WeaponActionMethod;
        m_playerMethod += WallJumpMethod;
        m_playerMethod += MoveMethod;
        Charge = Input.GetButton("Attack");
    }
    void OnDisable()
    {
        m_playerMethod -= JumpMethod;
        m_playerMethod -= WeaponActionMethod;
        m_playerMethod -= WallJumpMethod;
        m_playerMethod -= MoveMethod;
    }
    void Update()
    {
        if (!m_gameManager.IsGame) return;
        else
        {
            m_playerMethod();
            //Debug.Log(m_chrageAttackCount);
            Debug.Log(Attacked);
        }
    }

    //Playerの動きを処理が書かれた関数
    //----------------------------------------------
    void MoveMethod()
    {
        m_h = Input.GetAxisRaw("Horizontal");
        m_isDash = Input.GetButtonDown("Dash");
        var moveSpeed = 0f;
        Vector3 velocity = m_rb.velocity;
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
        if (m_h != 0 && !m_ableMove)
        { 
            moveSpeed = speed;
        }

        //ダッシュコマンド
        if (m_isDash && IsGrounded())
        {
            StartCoroutine("Dash", m_isDash);
            m_Animator.SetBool("Dash",m_isDash);
        }
        velocity.x = m_h * moveSpeed;
        m_rb.velocity = new Vector3(velocity.x, m_rb.velocity.y, 0);
        m_Animator.SetFloat("MoveSpeed", Mathf.Abs(velocity.x));
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Damage")
        {
            m_Audio.PlayOneShot(damageSound);
        }

        else if (other.gameObject.tag == "Pendulam" || other.gameObject.tag == "Ffield")
        {
            transform.parent = other.gameObject.transform;
        }
    }

    void OnCollisionExit()
    {
        transform.parent = null;
        //this.transform.localRotation = orgLocalQuaternion;
    }

    bool IsGrounded()
    {
        Vector3 isGroundCenter = footPos.transform.position;
        Ray ray = new Ray(isGroundCenter, Vector3.down);
        bool hitFlg = Physics.SphereCast(ray, isGroundRengeRadios, out _, distance, groundMask);
        return hitFlg;
    }
    bool IsWalled()
    {
        Vector3 isWallCenter = footPos.transform.position;
        Ray rayRight = new Ray(isWallCenter, Vector3.right);
        Ray rayLeft = new Ray(isWallCenter, Vector3.left);
        bool hitFlg = Physics.Raycast(rayRight, out m_hitInfo, walldistance, wallMask)
                   || Physics.Raycast(rayLeft, out m_hitInfo, walldistance, wallMask);
        return hitFlg;
    }
    void JumpMethod()
    {
        //ジャンプの処理
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            //Debug.Log("a");
            m_Audio.PlayOneShot(jumpSound);
            m_rb.AddForce(0f, jump, 0f, ForceMode.Impulse);
        }
        m_Animator.SetBool("Jump", !IsGrounded());
        
    }
    void WallJumpMethod()
    {
        m_Animator.SetBool("WallGrip",IsWalled());
        m_equipmentWeapon.SetActive(!IsWalled());
        if (IsGrounded())
        {
            m_rb.useGravity = true;
            return;
        }
        //ToDo移動コマンドで壁キックの力が変わる様にする
        else if (IsWalled() && m_h != 0)
        {
            m_rb.useGravity = false;
            if (Input.GetButtonDown("Jump"))
            {
                m_Animator.SetTrigger("WallJump");
                //StartCoroutine("WallJumpTime");
            }
        }
        else
        {
            m_rb.useGravity = true;
            return;
        }
    }
    IEnumerable Dash(bool isDash)
    {
        if(m_isDash)
        {
            for (float i = 0; i <= 0.5f; i += 0.1f)
            {

            }
        }
        else if(!m_isDash)
        {
            yield break;
        }
    }
    //AnimationEventで呼ぶ関数
    //---------------------------------------------------------
    void WalljumpPower()
    {
        m_Audio.PlayOneShot(jumpSound);
        //Vector3 vec = transform.up + m_hitInfo.normal;
        m_rb.AddForce(transform.up * wallJumpPower, ForceMode.Impulse);
    }
    void AttackJud()
    {
        Attacked = !Attacked;
    }
    void MoveJud()
    {
        m_ableMove = !m_ableMove;
        Debug.Log(m_ableMove);
    }

    void WeaponActionMethod()
    {
        //通常攻撃の処理
        if(Input.GetButtonDown("Attack"))
        {
            m_Animator.SetTrigger(m_equipmentWeapon.name + "Attack");
        }

        //溜め攻撃の処理(弓矢のアニメーションもこの処理）
        if (Charge && ChrageAttackCount < 1800 )
        {
            ChrageAttackCount++;
        }
        if (Input.GetButtonUp("Attack"))
        {
            if (ChrageAttackCount > 0)
            {
                ChrageAttackCount = 0;
            }
        }
        m_Animator.SetBool("Charge", Input.GetButton("Attack"));
        //メインとサブの表示を切り替える
        if (Input.GetButtonDown("WeaponChange") && !Charge && !Attacked)
        {
            m_weaponSwitch = !m_weaponSwitch;
            m_mainWeapon.SetActive(m_weaponSwitch);
            m_subWeapon.SetActive(!m_weaponSwitch);

            //メインとサブの武器を切り替える
            if (m_weaponSwitch)
            {
                m_equipmentWeapon = m_mainWeapon;
            }
            else
            {
                m_equipmentWeapon = m_subWeapon;
            }
        }
    }

#if UNITY_EDITOR
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Vector3 isGroundCenter = footPos.transform.position;
    //    Gizmos.DrawWireSphere(footPos.transform.position + Vector3.down * distance, isGroundRengeRadios);

    //    Vector3 isWallCenter = footPos.transform.position;
    //    Ray ray = new Ray(isWallCenter, Vector3.right);
    //    bool hitFlg = Physics.Raycast(ray, out m_hitInfo, distance, wallMask);

    //    Vector3 isWallCenter2 = footPos.transform.position;
    //    Ray ray2 = new Ray(isWallCenter, Vector3.right);
    //    bool hitFlg2 = Physics.Raycast(ray, out m_hitInfo, distance, wallMask);
    //}
#endif
}
