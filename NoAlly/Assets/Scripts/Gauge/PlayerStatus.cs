using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class PlayerStatus : StatusBase
{
    [SerializeField, Tooltip("�I�u�W�F�N�g�̕K�E�Z�Q�[�W�̏��")]
    float _maxSAP = 20;

    [Tooltip("�I�u�W�F�N�g�̌��݂̕K�E�Z�Q�[�W")]
    FloatReactiveProperty _sap = null;

    public float MaxHP => _maxHP;
    public float MaxSAP => _maxSAP;
    public FloatReactiveProperty SAP => _sap;

    public override void Init()
    {
        base.Init();
        _animator = gameObject.GetComponent<Animator>();
        _sap = new FloatReactiveProperty(0);
    }

    public override void DamageCalculation(WeaponBase weaponStatus, DifanseParameter difanse, ElementType type)
    {
        base.DamageCalculation(weaponStatus, difanse, type);
        if (_hp.Value <= 0)
        {
            CharacterDead(false);
        }
        else return;
    }

    /// <summary>
    /// �v���C���[��HP��
    /// </summary>
    /// <param name="hpPuls"></param>
    public void HPPuls(int hpPuls)
    {
        _hp.Value += hpPuls;
        if (_hp.Value > _maxHP)
        {
            _hp.Value = _maxHP;
        }
    }
    /// <summary>
    /// �v���C���[��SAP��
    /// </summary>
    /// <param name="sapPuls"></param>
    public void SAPPuls(int sapPuls)
    {
        _sap.Value += sapPuls;
        if (_sap.Value > _maxSAP)
        {
            _sap.Value = _maxSAP;
        }
    }

    /// <summary>
    /// �v���C���[��SAP�g�p
    /// </summary>
    /// <param name="useSAP"></param>
    public void UseSAP(float useSAP)
    {
        _sap.Value -= useSAP;
    }

    void CharacterDead(bool living)
    {
        _living = living;
        gameObject.GetComponent<CapsuleCollider>().enabled = living;
        _animator.SetBool("Death", !living);
    }

    /// <summary>
    /// ReactiveProperty�̃f�X�g���N�^
    /// </summary>
    void OnDisable()
    {
        _hp.Dispose();
        _sap.Dispose();
    }
}



