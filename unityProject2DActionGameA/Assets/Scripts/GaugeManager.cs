using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeManager : MonoBehaviour //HPやSAPゲージなどを統括するスクリプト
{
    [SerializeField, Tooltip("HPの上限")] int m_maxHP = 20;
    [SerializeField, Tooltip("オブジェクトのHP")] int m_HP = 20;
    [SerializeField, Tooltip("HPゲージのslider")] Slider m_HPGague;
    [SerializeField, Tooltip("SAPゲージのslider")] Slider m_SAPGague;
    [SerializeField] Image m_HPGagieHandle;
    [SerializeField] Image m_SAPGagieHandle;
    [SerializeField] float m_rigitDefensePercentage = 0.2f;
    [SerializeField] float m_fireDifansePercentage = 0.2f;
    [SerializeField] float m_elekeDifansePercentage = 0.2f;
    [SerializeField] float m_frozenDifansePercentage = 0.2f;
    [Tooltip("オブジェクトの必殺技ゲージ")] int m_SAP = 0;
    [Tooltip("必殺技ゲージの上限")] int m_maxSAP = 20;
    [Tooltip("オブジェクトの生死判定")] bool m_living = false;

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
        m_HPGague.value = HP;
        m_SAPGague.value = SAP;
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
        var damage = weaponPower * (1 - m_rigitDefensePercentage)
            + firePower * (1 - m_fireDifansePercentage)
             * elekePower * (1 - m_elekeDifansePercentage)
             * frozenPower * (1 - m_frozenDifansePercentage);
        HP -= (int)damage;
        //生死判定
        if (HP == 0)
        {
            m_living = false;
        }
        else return;
    }
}
