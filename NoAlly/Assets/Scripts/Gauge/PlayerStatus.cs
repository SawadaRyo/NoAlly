using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class PlayerStatus : StatusBase
{
    

    public float MaxHP => _maxHP;
    public float MaxSAP => _maxSAP;
    public FloatReactiveProperty SAP => _sap;

    public override void Initialize()
    {
        base.Initialize();
        _animator = gameObject.GetComponent<Animator>();
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
    /// プレイヤーのHP回復
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
    /// プレイヤーのSAP回復
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
    /// プレイヤーのSAP使用
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
    /// ReactivePropertyのデストラクタ
    /// </summary>
    void OnDisable()
    {
        _hp.Dispose();
        _sap.Dispose();
    }
}



