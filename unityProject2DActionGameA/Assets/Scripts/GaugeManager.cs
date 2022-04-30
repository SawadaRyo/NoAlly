using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeManager : MonoBehaviour //HPやSAPゲージなどを統括するスクリプト
{
    [SerializeField, Tooltip("オブジェクトのHPの上限")] int m_maxHP = 20;
    [SerializeField, Tooltip("オブジェクトの現在のHP")] protected int m_HP = 20;
    [SerializeField, Tooltip("HPゲージのslider")] Slider m_HPGague;
    [SerializeField, Tooltip("SAPゲージのslider")] Slider m_SAPGague;
    [SerializeField] float m_rigitDefensePercentage = 0.8f;
    [SerializeField] float m_fireDifansePercentage = 0.8f;
    [SerializeField] float m_elekeDifansePercentage = 0.8f;
    [SerializeField] float m_frozenDifansePercentage = 0.8f;
    [SerializeField, Tooltip("プレイヤーのダメージサウンド")] AudioClip m_damageSound;
    [Tooltip("オブジェクトの必殺技ゲージ")] int m_SAP = 0;
    [Tooltip("必殺技ゲージの上限")] int m_maxSAP = 20;
    [Tooltip("オブジェクトの生死判定")] protected bool m_living = false;

    public int HP { get => m_HP; set => m_HP = value; }
    public int SAP { get => m_SAP; set => m_SAP = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (m_HPGague != null)
        {
            //m_HP = m_maxHP;
            m_HPGague.value = m_HP;
            m_living = true;
        }

        if (m_SAPGague != null && this.gameObject.tag == "Player")
        {
            m_SAPGague.value = m_SAP;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "Player")
        {
            m_HPGague.value = HP;
            m_SAPGague.value = SAP;
        }
        MaxGagueController();
    }
    void MaxGagueController()
    {
        if (HP > m_maxHP)
        {
            HP -= (HP - m_maxHP);
        }

        if (m_SAP > m_maxSAP)
        {
            m_SAP -= (m_SAP - m_maxSAP);
        }
    }
    //ダメージ計算
    public void DamageMethod(int weaponPower, int firePower, int elekePower,int frozenPower)
    {
        var damage = weaponPower * m_rigitDefensePercentage
            + firePower * m_fireDifansePercentage
            + elekePower * m_elekeDifansePercentage
            + frozenPower * m_frozenDifansePercentage;
        HP -= (int)damage;
        //生死判定
        if (HP == 0)
        {
            m_living = false;
        }
        else return;
    }
}
