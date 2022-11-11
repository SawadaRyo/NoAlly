using System;
using UnityEngine;

public class WeaponAction : MonoBehaviour, IWeaponAction
{
    [SerializeField, Tooltip("溜め攻撃第1段階")] protected float _chargeLevel1 = 1f;
    [SerializeField, Tooltip("溜め攻撃第2段階")] protected float _chargeLevel2 = 3f;

    [Tooltip("武器コンポーネントの初期化")]
    bool _unStored = true;
    [Tooltip("溜め攻撃の溜め時間")]
    protected float _chrageCount = 0;
    [Tooltip("武器名")]
    protected string _weaponName;
    [Tooltip("PlayerのAnimatorを格納する変数")]
    protected Animator _animator = null;
    [Tooltip("WeaponBaseを格納する変数")]
    protected WeaponBase _weaponBase = null;
    [Tooltip("PlayerAnimationStateを格納する変数")]
    PlayerAnimationState _state = null;
    [Tooltip("WeaponEquipmentを格納する変数")]
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
            ////通常攻撃の処理
            if (Input.GetButtonDown("Attack"))
            {
                _animator.SetTrigger(_weaponName);
            }
            //溜め攻撃の処理(弓矢のアニメーションもこの処理）
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
