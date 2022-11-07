using System;
using UnityEngine;

public abstract class WeaponAction : MonoBehaviour, IWeaponAction
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
    protected Animator _animator = default;
    [Tooltip("WeaponBaseを格納する変数")]
    protected WeaponBase _weaponBase = default;
    [Tooltip("PlayerAnimationStateを格納する変数")]
    PlayerAnimationState _state;
    public float ChrageCount => _chrageCount;
    public WeaponBase Base => _weaponBase;

    public abstract void WeaponChargeAttackMethod(float chrageCount);
    public virtual void Enable() { }

    void OnEnable()
    {
        if (_unStored)
        {
            Enable();
            _state = PlayerAnimationState.Instance;
            _animator = GetComponentInParent<PlayerContoller>().GetComponent<Animator>();
            _weaponBase = GetComponent<WeaponBase>();
            _unStored = false;
        }
    }

    public void WeaponAttack()
    {
        if (_state.AbleInput && WeaponEquipment.Instance.Available)
        {
            ////通常攻撃の処理
            if (Input.GetButtonDown("Attack"))
            {
                _animator.SetTrigger(this.gameObject.name);
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
                _chrageCount = 0f;
            }

            _animator.SetBool("Charge", Input.GetButton("Attack"));
        }
    }
}
