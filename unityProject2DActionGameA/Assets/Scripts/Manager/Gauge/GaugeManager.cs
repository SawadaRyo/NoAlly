using UnityEngine;

public class GaugeManager : MonoBehaviour //敵やプレイヤーの体力を管理するスクリプト
{
    [SerializeField, Tooltip("オブジェクトのHPの上限")] protected int m_maxHP = 20;
    [SerializeField] float m_rigitDefensePercentage = 0.8f;
    [SerializeField] float m_fireDifansePercentage = 0.8f;
    [SerializeField] float m_elekeDifansePercentage = 0.8f;
    [SerializeField] float m_frozenDifansePercentage = 0.8f;
    [SerializeField, Tooltip("プレイヤーのダメージサウンド")] AudioClip m_damageSound;
    [SerializeField, Tooltip("Animatorの格納変数")] Animator m_animator;
    [Tooltip("オブジェクトの現在のHP")] protected int m_hp;
    [Tooltip("オブジェクトの生死判定")] protected bool m_living = true;

    public void Start()
    {
        m_hp = m_maxHP;
        IsStart();
    }

    void Update()
    {
        if(m_living)
        {
            IsUpdate();
        }
        else
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            m_animator.SetBool("Death", true);
        }
    }
    public virtual void IsStart()
    {
        //Start関数で呼びたい処理はこの関数に
    }
    public virtual void IsUpdate()
    {
        //Update関数で呼びたい処理はこの関数に
    }
    //ダメージ計算
    public void DamageMethod(int weaponPower, int firePower, int elekePower,int frozenPower)
    {
        var damage = weaponPower * m_rigitDefensePercentage
            + firePower * m_fireDifansePercentage
            + elekePower * m_elekeDifansePercentage
            + frozenPower * m_frozenDifansePercentage;
        m_hp -= (int)damage;
        //Debug.Log(m_hp);
        //生死判定
        if (m_hp <= 0)
        {
            m_living = false;
            Debug.Log(m_living);
        }
        else return;
    }
    
    
    public void HPPuls(int hpPuls)
    {
        m_hp += hpPuls;
    }
}
