using UnityEngine;
using UniRx;

public class EnemyStatus: StatusBase //�G�̗̑͂��Ǘ�����X�N���v�g
{
    [Tooltip("�G��HP")]
    float _hp = 0;
    [Tooltip("���̃I�u�W�F�N�g�ɃA�^�b�`����Ă���EnemyBase���擾����ϐ�")]
    EnemyBase _enemyBase = null;

    public void Initialize(EnemyBase enemyBase)
    {
        base.Initialize();
        _enemyBase = enemyBase;
        _animator = _enemyBase.EnemyAnimator;
        _hp = _maxHP;
    }

    public override void Damage(WeaponBase weaponStatus, HitParameter difanse, ElementType type)
    {
        _hp -= base.DamageCalculation(weaponStatus, difanse, type);
        if (_hp <= 0)
        {
            Debug.Log("Death");
            _enemyBase.EnemyStateMachine.Dispatch((int)StateOfEnemy.Death);
        }
    }
}
