using UnityEngine;
using UniRx;

public class EnemyStatus : StatusBase //�G�̗̑͂��Ǘ�����X�N���v�g
{
    [Tooltip("���̃I�u�W�F�N�g�ɃA�^�b�`����Ă���EnemyBase���擾����ϐ�")]
    EnemyBase _enemyBase = null;

    public override void Init()
    {
        base.Init();
        _enemyBase = GetComponentInParent<EnemyBase>();
    }

    //�_���[�W�v�Z
    public float DamageMethod(float weaponPower, float firePower, float elekePower, float frozenPower, ElementType type)
    {
        float baseDamage = weaponPower * _rigitDefensePercentage;
        float elemantDamage = 0;
        switch (type)
        {
            case ElementType.FIRE:
                elemantDamage = firePower* _fireDifansePercentage;
                break;
            case ElementType.ELEKE:
                elemantDamage = elekePower* _elekeDifansePercentage;
                break;
            case ElementType.FROZEN:
                elemantDamage = frozenPower* _frozenDifansePercentage;
                break;
        }
        _hp.Value -= (baseDamage + elemantDamage);
        _audioSource.PlayOneShot(_damageSound);
        //Debug.Log(m_hp);
        //��������
        if (_hp.Value <= 0)
        {
            _enemyBase.Disactive();
        }
        return _hp.Value;
    }
}
