using System;
using UnityEngine;

public abstract class WeaponAction : MonoBehaviour, IWeaponAction
{
    [SerializeField, Tooltip("溜め攻撃第1段階")] protected float _chargeLevel1 = 1f;
    [SerializeField, Tooltip("溜め攻撃第2段階")] protected float _chargeLevel2 = 3f;

    [Tooltip("")] 
    bool _unStored = true;
    [Tooltip("溜め攻撃の溜め時間")] 
    protected float _chrageCount = 0;
    [Tooltip("武器名")] 
    protected string _weaponName;
    [Tooltip("PlayerのAnimatorを格納する変数")]
    protected Animator _animator = default;
    [Tooltip("WeaponBaseを格納する変数")] 
    protected WeaponBase _weaponBase = default;
    [Tooltip("PlayerAnimationStateを格納する変数")]
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

        ////通常攻撃の処理
        if (Input.GetButtonDown("Attack"))
        {
            _animator.SetTrigger(weaponName);
        }

        //溜め攻撃の処理(弓矢のアニメーションもこの処理）
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
