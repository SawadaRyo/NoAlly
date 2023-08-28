using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class PlayerStatus : StatusBase
{
    [Tooltip("�I�u�W�F�N�g�̌��݂�HP")]
    protected FloatReactiveProperty _hpReactiveProperty;
    [Tooltip("�I�u�W�F�N�g�̌��݂̕K�E�Z�Q�[�W")]
    protected FloatReactiveProperty _sapReactiveProperty = null;
    public float MaxHP => _maxHP;
    public float MaxSAP => _maxSAP;
    public IReadOnlyReactiveProperty<float> SAP => _sapReactiveProperty;
    public IReadOnlyReactiveProperty<float> HP => _hpReactiveProperty;

    public override void Initialize()
    {
        base.Initialize();
        _hpReactiveProperty = new FloatReactiveProperty(_maxHP);
        _sapReactiveProperty = new FloatReactiveProperty(0);
    }

    public override void Damage(WeaponPower damageValue, ElementType type)
    {
        _hpReactiveProperty.Value -= base.DamageCalculation(damageValue, type);
        if (_hpReactiveProperty.Value <= 0)
        {
            CharacterDead(false);
        }
    }

    

    /// <summary>
    /// �v���C���[��HP��
    /// </summary>
    /// <param name="hpPuls"></param>
    public void HPPuls(float hpPuls)
    {
        _hpReactiveProperty.Value += hpPuls;
        if (_hpReactiveProperty.Value > _maxHP)
        {
            _hpReactiveProperty.Value = _maxHP;
        }
    }
    /// <summary>
    /// �v���C���[��SAP��
    /// </summary>
    /// <param name="sapPuls"></param>
    public void SAPPuls(float sapPuls)
    {
        _sapReactiveProperty.Value += sapPuls;
        if (_sapReactiveProperty.Value > _maxSAP)
        {
            _sapReactiveProperty.Value = _maxSAP;
        }
    }

    /// <summary>
    /// �v���C���[��SAP�g�p
    /// </summary>
    /// <param name="useSAP"></param>
    public void UseSAP(float useSAP)
    {
        _sapReactiveProperty.Value -= useSAP;
    }

    void CharacterDead(bool living)
    {
        _living = living;
        _animator.SetBool("Death", !living);
    }

    /// <summary>
    /// ReactiveProperty�̃f�X�g���N�^
    /// </summary>
    void OnDisable()
    {
        _hpReactiveProperty.Dispose();
        _sapReactiveProperty.Dispose();
    }
}



