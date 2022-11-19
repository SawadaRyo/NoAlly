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

    void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }
    public override void Init()
    {
        base.Init();
        _sap = new FloatReactiveProperty(0);
    }

    void Update()
    {
        if (_living) return;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        _animator.SetBool("Death", true);
    }
    //�_���[�W�v�Z
    public void DamageMethod(float weaponPower, float firePower, float elekePower, float frozenPower)
    {
        var damage = weaponPower * _rigitDefensePercentage
            + firePower * _fireDifansePercentage
            + elekePower * _elekeDifansePercentage
            + frozenPower * _frozenDifansePercentage;
        _hp.Value -= damage;

        //��������
        if (_hp.Value <= 0)
        {
            _living = false;
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

    public void UseSAP(float useSAP)
    {
        _sap.Value -= useSAP;
    }

    private void OnDisable()
    {
        _hp.Dispose();
        _sap.Dispose();
    }
}



