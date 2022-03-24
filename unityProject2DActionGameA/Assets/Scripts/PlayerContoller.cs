using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーの移動速度")] float speed = 5f;
    [SerializeField, Tooltip("ダッシュの倍率")] float dashPowor = 10f;
    [SerializeField, Tooltip("空中での移動速度")] float grindFroth = 5f;
    [SerializeField, Tooltip("プレイヤーのジャンプ力")] float jump = 3f;
    [SerializeField, Tooltip("接地判定のRayの射程")] float distance = 0.1f;
    [SerializeField, Tooltip("壁の接触判定のRayの射程")] float walldistance = 0.1f;
    [SerializeField, Tooltip("SphierCastの半径")] float isGroundRengeRadios = 1f;
    [SerializeField, Tooltip("壁ジャンの力")] float wallJumpPower = 10f;
    [Tooltip("プレイヤーに掛かる重力値")] float m_gravity = -0.981f;
    [SerializeField, Tooltip("ジャンプのサウンド")] AudioClip jumpSound;
    [SerializeField, Tooltip("プレイヤーのダメージサウンド")] AudioClip damageSound;

    [SerializeField, Tooltip("Rayの射出点")] Transform footPos;
    [SerializeField, Tooltip("接地判定のLayerMask")] LayerMask groundMask = ~0;
    [SerializeField, Tooltip("壁の接触判定")] LayerMask wallMask = ~0;

    [SerializeField, Tooltip("アニメーションを取得する為の変数")] Animator m_Animator;
    [SerializeField, Tooltip("GamManagerを格納する変数")] GameManager m_gameManager;

    [SerializeField, Tooltip("武器prefabを格納する変数")] GameObject[] m_weaponPrefab = new GameObject[4];

    float m_turnSpeed = 15f;
    float m_h;
    bool m_weaponSwitch = true;
    Rigidbody m_rb;
    //List<ItemBase> m_ItemList = new List<ItemBase>();
    [Tooltip("オーディオを取得する為の変数")] AudioSource m_Audio;
    [Tooltip("Playerの向いている方向")] Quaternion orgLocalQuaternion;
    [Tooltip("メイン武器")] GameObject m_mainWeapon = default;
    [Tooltip("サブ武器")] GameObject m_subWeapon = default;
    [Tooltip("装備中の武器")] GameObject m_equipmentWeapon = default;
    Collider isAttack;
    RaycastHit m_hitInfo;
    public Vector3 NormalOfStickingWall { get; private set; } = Vector3.zero;

    void Start()
    {
        orgLocalQuaternion = this.transform.localRotation;
        m_rb = GetComponent<Rigidbody>();
        m_Audio = gameObject.AddComponent<AudioSource>();

        m_weaponSwitch = true;
        m_mainWeapon = m_weaponPrefab[0];
        m_subWeapon = m_weaponPrefab[1];
        m_mainWeapon.SetActive(true);
        m_subWeapon.SetActive(false);
        m_equipmentWeapon = m_mainWeapon;
        isAttack = m_equipmentWeapon.GetComponent<Collider>();
        //m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (!m_gameManager.IsGame) return;
        else
        {
            JumpMethod();
            wallJumpMethod();
            MoveMethod();
            WeaponActionMethod();
        }
    }

    void MoveMethod()
    {
        m_h = Input.GetAxisRaw("Horizontal");
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
        if (m_h != 0)
        {
            if (Input.GetButton("Dash") && IsGrounded())//ダッシュコマンド
            {
                velocity.x = m_h * dashPowor;
            }
            else
            {
                velocity.x = m_h * speed;
            }
        }
        m_rb.velocity = new Vector3(velocity.x, m_rb.velocity.y, 0);
        m_Animator.SetFloat("MoveSpeed", Mathf.Abs(velocity.x));
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Damage")
        {
            m_Audio.PlayOneShot(damageSound);
        }

        if (other.gameObject.tag == "Pendulam" || other.gameObject.tag == "Ffield")
        {
            transform.parent = other.gameObject.transform;
        }

        if (other.gameObject.tag == "Piston")
        {
            StartCoroutine("WaitKeyInput");
        }
    }

    //public void GetItem(ItemBase item)
    //{
    //    m_ItemList.Add(item);
    //}

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
    bool IsRightWalled()
    {
        Vector3 isWallCenter = footPos.transform.position;
        Ray ray = new Ray(isWallCenter, Vector3.right);
        bool hitFlg = Physics.Raycast(ray, out m_hitInfo, walldistance, wallMask);
        return hitFlg;
    }

    bool IsLeftWalled()
    {
        Vector3 isWallCenter = footPos.transform.position;
        Ray ray = new Ray(isWallCenter, Vector3.left);
        bool hitFlg = Physics.Raycast(ray, out m_hitInfo, walldistance, wallMask);
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

        if (!IsGrounded())
        {
            m_Animator.SetBool("Jump", true);
        }
        else
        {
            m_Animator.SetBool("Jump", false);
        }
    }
    void wallJumpMethod()
    {
        if (IsGrounded())
        {
            m_Animator.SetBool("WallGrip", false);
            m_rb.useGravity = true;
            return;
        }
        //ToDo移動コマンドで壁キックの力が変わる様にする
        else if (IsLeftWalled() || IsRightWalled())
        {
            m_Animator.SetBool("WallGrip", true);
            m_rb.useGravity = false;
            if (Input.GetButtonDown("Jump"))
            {
                m_Animator.SetBool("WallGrip", false);
                m_Animator.SetTrigger("WallJump");
                //StartCoroutine("WallJumpTime");
            }
        }
        else
        {
            m_Animator.SetBool("WallGrip", false);
            m_rb.useGravity = true;
            return;
        }
    }
    IEnumerable WallJumpTime()
    {
        m_h = 0;
        yield return new WaitForSeconds(0.5f);
    }

    void WalljumpPower()
    {
        m_Audio.PlayOneShot(jumpSound);
        Vector3 vec = transform.up + m_hitInfo.normal;
        m_rb.AddForce(vec * wallJumpPower, ForceMode.Impulse);
        StartCoroutine("WallJumpTime");
    }

    void NormalAttack(int weapnNumber)
    {
        var attackRange = m_weaponPrefab[weapnNumber].GetComponent<CapsuleCollider>();
        attackRange.enabled = !attackRange.enabled;
    }

    void WeaponActionMethod()
    {
        //攻撃
        if(Input.GetButtonDown("Attack"))
        {
            //m_Animator.SetTrigger("Attack");
            m_Animator.SetTrigger(m_equipmentWeapon.name + "Attack");
        }
        
        //メインとサブの表示を切り替える
        if (Input.GetButtonDown("WeaponChange"))
        {
            m_weaponSwitch = !m_weaponSwitch;
            m_mainWeapon.SetActive(m_weaponSwitch);
            m_subWeapon.SetActive(!m_weaponSwitch);
        }

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

    void IsAttack()
    {
        isAttack.enabled = !isAttack.enabled;
    }
}
