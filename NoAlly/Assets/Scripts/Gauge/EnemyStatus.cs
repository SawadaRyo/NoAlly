using UnityEngine;
using UniRx;

public class EnemyStatus : StatusBase //�G�̗̑͂��Ǘ�����X�N���v�g
{
    [Tooltip("���̃I�u�W�F�N�g�ɃA�^�b�`����Ă���EnemyBase���擾����ϐ�")]
    EnemyBase _enemyBase = null;

    public override void Initialize()
    {
        base.Initialize();
        _enemyBase = GetComponentInParent<EnemyBase>();
    }

    public override void DamageCalculation(WeaponBase weaponStatus, DifanseParameter difanse, ElementType type)
    {
        base.DamageCalculation(weaponStatus, difanse, type);
        if (_hp.Value <= 0)
        {
            _enemyBase.Disactive();
        }
        else return;
    }
}
