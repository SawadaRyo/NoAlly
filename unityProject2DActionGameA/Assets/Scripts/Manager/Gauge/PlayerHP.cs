using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : GaugeManager
{
    [SerializeField, Tooltip("HPゲージのslider")] Slider m_HPGague = default;
    [SerializeField, Tooltip("SAPゲージのslider")] Slider m_SAPGague = default;
    [Tooltip("オブジェクトの現在の必殺技ゲージ")]  int m_sap = 0;
    [SerializeField,Tooltip("オブジェクトの必殺技ゲージの上限")]  int m_maxSAP = 20;

    public int SAP { get => m_sap;set=>m_sap = value; }
    public override void IsStart()
    {
        if (m_HPGague != null && m_SAPGague != null)
        {
            m_sap = m_maxSAP;
            m_HPGague.value = m_hp;
            m_SAPGague.value = m_sap;
            m_living = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_HPGague.value = m_hp;
        MaxGagueController();
    }
    void MaxGagueController()
    {
        if (m_hp > m_maxHP)
        {
            m_hp -= (m_hp - m_maxHP);
        }

        if (m_sap > m_maxSAP)
        {
            m_sap -= (m_sap - m_maxSAP);
        }
    }
    public void SAPPuls(int sapPuls)
    {
        m_sap += sapPuls;
        m_SAPGague.value = m_sap;
    }
}
