using UnityEngine;

public class EnemyGauge : MonoBehaviour //敵の体力を管理するスクリプト
{
    [SerializeField, Tooltip("オブジェクトのHPの上限")] int m_maxHP = 20;
    [SerializeField] float m_rigitDefensePercentage = 0.8f;
    [SerializeField] float m_fireDifansePercentage = 0.8f;
    [SerializeField] float m_elekeDifansePercentage = 0.8f;
    [SerializeField] float m_frozenDifansePercentage = 0.8f;
    [SerializeField, Tooltip("プレイヤーのダメージサウンド")] AudioClip m_damageSound;
    [Tooltip("オブジェクトの現在のHP")] int m_hp;
    [Tooltip("このオブジェクトにアタッチされているEnemyBaseを取得する変数")] EnemyBase m_enemyBase;

    public void Start()
    {
        m_hp = m_maxHP;
    }

    
    //ダメージ計算
    public void DamageMethod(float weaponPower, float firePower, float elekePower, float frozenPower)
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
            m_enemyBase.Disactive();
        }
    }
}
