//日本語コメント可
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class InputToWeapon : MonoBehaviour
{
    [SerializeField, Tooltip("武器のプレハブ")]
    ObjectBase _weaponPrefab = null;
    [SerializeField]
    Animator _playerAnimator = null;

    [Tooltip("武器が変形中かどうか")]
    bool _inDeformation = false;
    [Tooltip("武器切り替えフラグ")]
    BoolReactiveProperty _isSwtchWeapon = new();
    [Tooltip("武器のアニメーションの状態")]
    ObservableStateMachineTrigger _trigger = null;
    [Tooltip("装備している武器")]
    WeaponData _targetWeapon;

    public ObjectBase WeaponPrefab => _weaponPrefab;
    public WeaponData TargetWeapon { get => _targetWeapon; set => _targetWeapon = value; }
    public IReadOnlyReactiveProperty<bool> IsSwichWeapon => _isSwtchWeapon;

    // Start is called before the first frame update
    private void Start()
    {
        _trigger = _weaponPrefab.ObjectAnimator.GetBehaviour<ObservableStateMachineTrigger>();
        WeaponState();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!WeaponMenuHander.Instance.MenuIsOpen)
        {
            if (!PlayerAnimationState.Instance.IsAttack)
            {
                _isSwtchWeapon.Value = Input.GetButton("SubWeaponSwitch");
            }
            WeaponAttack(_playerAnimator);
        }
    }
    /// <summary>
    /// 武器の入力判定
    /// </summary>
    void WeaponState()
    {
        IDisposable weaponState = _trigger
        .OnStateEnterAsObservable()　　//Animationの遷移開始を検知
        .Subscribe(onStateInfo =>
        {
            if (onStateInfo.StateInfo.IsTag("InDeformation"))
            {
                _inDeformation = true;
            }
            else
            {
                _inDeformation = false;
            }
        }).AddTo(this);
    }
    public void WeaponAttack(Animator playerAnimator)
    {
        //if (!PlayerAnimationState.Instance.AbleInput || WeaponMenuHander.Instance.MenuIsOpen) return;
        if (!PlayerAnimationState.Instance.AbleInput) return;
        if (_inDeformation) return;
        ////通常攻撃の処理
        if (Input.GetButtonDown("Attack") && _targetWeapon != null)
        {
            playerAnimator.SetTrigger("AttackTrigger");
            playerAnimator.SetInteger("WeaponType", (int)_targetWeapon.Type);
        }
        else
        {
            //溜め攻撃の処理(弓矢のアニメーションもこの処理）
            if (Input.GetButton("Attack") && _targetWeapon != null)
            {
                _targetWeapon.Action.ChargeCount += Time.deltaTime;
                playerAnimator.SetBool("Charge", true);
                if (_targetWeapon.Action.ChargeCount > _targetWeapon.Base.ChargeLevels[0])
                {
                    playerAnimator.SetTrigger("ChargeAttackTrigger");
                }
            }
            else if (Input.GetButtonUp("Attack"))
            {
                playerAnimator.SetBool("Charge", false);
            }
        }
    }
}
