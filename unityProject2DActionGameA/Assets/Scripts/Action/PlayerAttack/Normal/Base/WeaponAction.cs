using System;
using UnityEngine;

public abstract class WeaponAction : MonoBehaviour, IWeaponAction
{
    [SerializeField, Tooltip("���ߍU����1�i�K")] protected float _chargeLevel1 = 1f;
    [SerializeField, Tooltip("���ߍU����2�i�K")] protected float _chargeLevel2 = 3f;

    [Tooltip("")] 
    bool _unStored = true;
    [Tooltip("���ߍU���̗��ߎ���")] 
    protected float _chrageCount = 0;
    [Tooltip("���햼")] 
    protected string _weaponName;
    [Tooltip("Player��Animator���i�[����ϐ�")]
    protected Animator _animator = default;
    [Tooltip("WeaponBase���i�[����ϐ�")] 
    protected WeaponBase _weaponBase = default;
    [Tooltip("PlayerAnimationState���i�[����ϐ�")]
    PlayerAnimationState _state;

    public abstract void WeaponChargeAttackMethod(float chrageCount);
    public virtual void Enable() { }

    void OnEnable()
    {
        if(_unStored)
        {
            Enable();
            _state = PlayerAnimationState.Instance;
            _animator = PlayerContoller.Instance.GetComponent<Animator>();
            _weaponBase = GetComponent<WeaponBase>();
            _unStored = false;
        }
    }
   
    public void WeaponAttack(string weaponName)
    {
        if (!_state.AbleInput) return;

        ////�ʏ�U���̏���
        if (Input.GetButtonDown("Attack"))
        {
            _animator.SetTrigger(weaponName);
        }

        //���ߍU���̏���(�|��̃A�j���[�V���������̏����j
        if (Input.GetButton("Attack") && _chrageCount < _chargeLevel2)
        {
            _chrageCount += Time.deltaTime;
        }
        else if (Input.GetButtonUp("Attack"))
        {
            if(_chrageCount > 0f)
            {
                WeaponChargeAttackMethod(_chrageCount);
            }
            _chrageCount = 0f;
        }

        _animator.SetBool("Charge", Input.GetButton("Attack"));
    }

    
}
