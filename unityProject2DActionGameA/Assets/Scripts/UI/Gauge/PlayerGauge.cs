using UnityEngine;
using UnityEngine.UI;

public class PlayerGauge : MonoBehaviour
{
    [SerializeField, Tooltip("オブジェクトの必殺技ゲージの上限")] float m_maxSAP = 20;
    [SerializeField, Tooltip("オブジェクトのHPの上限")] float m_maxHP = 20;
    [SerializeField, Tooltip("HPゲージのslider")] Slider m_HPGague = default;
    [SerializeField, Tooltip("SAPゲージのslider")] Slider m_SAPGague = default;
    [SerializeField, Tooltip("プレイヤーのダメージサウンド")] AudioClip m_damageSound;

    [Tooltip("物理防御力")] float m_rigitDefensePercentage = 0f;
    [Tooltip("炎防御力")] float m_fireDifansePercentage = 0f;
    [Tooltip("電気防御力")] float m_elekeDifansePercentage = 0f;
    [Tooltip("氷結防御力")] float m_frozenDifansePercentage = 0f;
    [Tooltip("オブジェクトの現在の必殺技ゲージ")] float m_sap = 0f;
    [Tooltip("オブジェクトの現在のHP")] float m_hp = 0f;
    [Tooltip("オブジェクトの生死判定")] bool m_living = true;
    [Tooltip("Animatorの格納変数")] Animator m_animator;

    void Awake()
    {
        m_animator = gameObject.GetComponent<Animator>();
    }
    void Start()
    {
        if (m_HPGague != null && m_SAPGague != null)
        {
            m_hp = m_maxHP;
            m_sap = m_maxSAP;
            m_HPGague.value = m_hp;
            m_SAPGague.value = m_sap;
            m_living = true;
        }
    }

    void Update()
    {
        if (m_living)
        {
            return;
        }
        else
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            m_animator.SetBool("Death", true);
        }
    }
    //ダメージ計算
    public void DamageMethod(float weaponPower, float firePower, float elekePower, float frozenPower)
    {
        var damage = weaponPower * m_rigitDefensePercentage
            + firePower * m_fireDifansePercentage
            + elekePower * m_elekeDifansePercentage
            + frozenPower * m_frozenDifansePercentage;
        m_hp -= (int)damage;
        m_HPGague.value = m_hp;
        //生死判定
        if (m_hp <= 0)
        {
            m_living = false;
        }
        else return;
    }

    
    public void HPPuls(int hpPuls)
    {
        m_hp += hpPuls;
        if (m_hp > m_maxHP)
        {
            m_hp -= (m_hp - m_maxHP);
        }
        m_HPGague.value = m_hp/m_maxHP;
    }
    public void SAPPuls(int sapPuls)
    {
        m_sap += sapPuls;
        if (m_sap > m_maxSAP)
        {
            m_sap -= (m_sap - m_maxSAP);
        }
        m_SAPGague.value = m_sap/m_maxSAP;
    }

    public float SAP { get => m_sap; set => m_sap = value; }
}
