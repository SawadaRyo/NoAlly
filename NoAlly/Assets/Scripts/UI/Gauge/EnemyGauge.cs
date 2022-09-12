using UnityEngine;

public class EnemyGauge : MonoBehaviour //�G�̗̑͂��Ǘ�����X�N���v�g
{
    [SerializeField, Tooltip("�I�u�W�F�N�g��HP�̏��")] int m_maxHP = 20;
    [SerializeField] float m_rigitDefensePercentage = 0.8f;
    [SerializeField] float m_fireDifansePercentage = 0.8f;
    [SerializeField] float m_elekeDifansePercentage = 0.8f;
    [SerializeField] float m_frozenDifansePercentage = 0.8f;
    [SerializeField, Tooltip("�v���C���[�̃_���[�W�T�E���h")] AudioClip m_damageSound;
    [Tooltip("�I�u�W�F�N�g�̌��݂�HP")] int m_hp;
    [Tooltip("���̃I�u�W�F�N�g�ɃA�^�b�`����Ă���EnemyBase���擾����ϐ�")] EnemyBase m_enemyBase;

    public void Start()
    {
        m_hp = m_maxHP;
    }

    
    //�_���[�W�v�Z
    public void DamageMethod(float weaponPower, float firePower, float elekePower, float frozenPower)
    {
        var damage = weaponPower * m_rigitDefensePercentage
            + firePower * m_fireDifansePercentage
            + elekePower * m_elekeDifansePercentage
            + frozenPower * m_frozenDifansePercentage;
        m_hp -= (int)damage;
        //Debug.Log(m_hp);
        //��������
        if (m_hp <= 0)
        {
            m_enemyBase.Disactive();
        }
    }
}
