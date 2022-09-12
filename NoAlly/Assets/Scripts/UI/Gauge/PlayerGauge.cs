using UnityEngine;
using UnityEngine.UI;

public class PlayerGauge : MonoBehaviour
{
    [SerializeField, Tooltip("�I�u�W�F�N�g�̕K�E�Z�Q�[�W�̏��")] float _maxSAP = 20;
    [SerializeField, Tooltip("�I�u�W�F�N�g��HP�̏��")] float _maxHP = 20;
    [SerializeField, Tooltip("HP�Q�[�W��slider")] Slider _HPGague = default;
    [SerializeField, Tooltip("SAP�Q�[�W��slider")] Slider _SAPGague = default;
    [SerializeField, Tooltip("�v���C���[�̃_���[�W�T�E���h")] AudioClip _damageSound;

    [Tooltip("�����h���")] float _rigitDefensePercentage = 0f;
    [Tooltip("���h���")] float _fireDifansePercentage = 0f;
    [Tooltip("�d�C�h���")] float _elekeDifansePercentage = 0f;
    [Tooltip("�X���h���")] float _frozenDifansePercentage = 0f;
    [Tooltip("�I�u�W�F�N�g�̌��݂̕K�E�Z�Q�[�W")] float _sap = 0f;
    [Tooltip("�I�u�W�F�N�g�̌��݂�HP")] float _hp = 0f;
    [Tooltip("�I�u�W�F�N�g�̐�������")] bool _living = true;
    [Tooltip("Animator�̊i�[�ϐ�")] Animator _animator;

    void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }
    void Start()
    {
        if (_HPGague != null && _SAPGague != null)
        {
            _hp = _maxHP;
            _sap = _maxSAP;
            _HPGague.value = _hp;
            _SAPGague.value = _sap;
            _living = true;
        }
    }

    void Update()
    {
        if (_living)
        {
            return;
        }
        else
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            _animator.SetBool("Death", true);
        }
    }
    //�_���[�W�v�Z
    public void DamageMethod(float weaponPower, float firePower, float elekePower, float frozenPower)
    {
        var damage = weaponPower * _rigitDefensePercentage
            + firePower * _fireDifansePercentage
            + elekePower * _elekeDifansePercentage
            + frozenPower * _frozenDifansePercentage;
        _hp -= (int)damage;
        _HPGague.value = _hp;
        //��������
        if (_hp <= 0)
        {
            _living = false;
        }
        else return;
    }

    
    public void HPPuls(int hpPuls)
    {
        _hp += hpPuls;
        if (_hp > _maxHP)
        {
            _hp -= (_hp - _maxHP);
        }
        _HPGague.value = _hp/_maxHP;
    }
    public void SAPPuls(int sapPuls)
    {
        _sap += sapPuls;
        if (_sap > _maxSAP)
        {
            _sap -= (_sap - _maxSAP);
        }
        _SAPGague.value = _sap/_maxSAP;
    }

    public float SAP { get => _sap; set => _sap = value; }
}
