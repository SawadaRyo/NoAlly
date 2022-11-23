using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class PlayerStatus : StatusBase
{
    [SerializeField, Tooltip("オブジェクトの必殺技ゲージの上限")]
    float _maxSAP = 20;


    [Tooltip("オブジェクトの現在の必殺技ゲージ")]
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

    void Update()
    {

    }
    void DamageMethod(float weaponPower,ElementType type)
    {
        _hp.Value -= weaponPower;
    }
    //ダメージ計算
    public void DamageMethod(float weaponPower, float firePower, float elekePower, float frozenPower)
    {
        var damage = weaponPower * _rigitDefensePercentage
            + firePower * _fireDifansePercentage
            + elekePower * _elekeDifansePercentage
            + frozenPower * _frozenDifansePercentage;
        _hp.Value -= damage;

        //生死判定
        if (_hp.Value <= 0)
        {
            _living = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            _animator.SetBool("Death", true);
        }
        else return;
    }


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

    public void UseSAP(float useSAP)
    {
        _sap.Value -= useSAP;
    }

    void OnDisable()
    {
        _hp.Dispose();
        _sap.Dispose();
    }

    void IsLiving(bool live)
    {
        _living = live;
        gameObject.GetComponent<CapsuleCollider>().enabled = live;
        _animator.SetBool("Death", !live);
    }
}



