using System;
using UnityEngine;

public class WeaponAction : MonoBehaviour, IWeaponAction
{
    [SerializeField, Tooltip("���ߍU����1�i�K")] protected float _chargeLevel1 = 1f;
    [SerializeField, Tooltip("���ߍU����2�i�K")] protected float _chargeLevel2 = 3f;

    [Tooltip("����R���|�[�l���g�̏�����")]
    bool _unStored = true;
    [Tooltip("���ߍU���̗��ߎ���")]
    protected float _chrageCount = 0;
    [Tooltip("���햼")]
    protected string _weaponName;
    [Tooltip("Player��Animator���i�[����ϐ�")]
    protected Animator _animator = null;
    [Tooltip("WeaponBase���i�[����ϐ�")]
    protected WeaponBase _weaponBase = null;
    [Tooltip("PlayerAnimationState���i�[����ϐ�")]
    PlayerAnimationState _state = null;
    [Tooltip("WeaponEquipment���i�[����ϐ�")]
    WeaponEquipment _weaponEquipment = null;

    public WeaponBase Base => _weaponBase;

    public virtual void WeaponChargeAttackMethod() { }
    public virtual void Enable() { }
    public virtual void ResetValue()
    {
        _chrageCount = 0;
    }

    void OnEnable()
    {
        if (_unStored)
        {
            Enable();
            _weaponName = this.gameObject.name;
            _weaponEquipment = GetComponentInParent<WeaponEquipment>();
            _state = PlayerAnimationState.Instance;
            _animator = GetComponentInParent<PlayerContoller>().GetComponent<Animator>();
            _weaponBase = GetComponent<WeaponBase>();
            _unStored = false;
        }
    }

    public void WeaponAttack()
    {
        if (_state.AbleInput && _weaponEquipment.Available)
        {
            ////�ʏ�U���̏���
            if (Input.GetButtonDown("Attack"))
            {
                _animator.SetTrigger(_weaponName);
            }
            //���ߍU���̏���(�|��̃A�j���[�V���������̏����j
            if (Input.GetButton("Attack") && _chrageCount < _chargeLevel2)
            {
                _chrageCount += Time.deltaTime;
            }
            else if (Input.GetButtonUp("Attack"))
            {
                //if(_chrageCount > 0f)
                //{
                //    WeaponChargeAttackMethod(_chrageCount);
                //}
            }

            _animator.SetBool("Charge", Input.GetButton("Attack"));
        }
    }
}
