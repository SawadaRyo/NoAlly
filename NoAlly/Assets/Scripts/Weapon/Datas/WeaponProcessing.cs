using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 武器のモーションや可視化などの武器にまつわる処理を行うクラス
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [SerializeField, Tooltip("武器のプレハブ")]
    ObjectBase _weaponPrefab = null;
    [SerializeField, Header("プレイヤーのアニメーター")]
    Animator _playerAnimator = null;
    [SerializeField, Header("武器の斬撃エフェクト")]
    ParticleSystem _myParticleSystem = default;

    [Tooltip("武器が変形中かどうか")]
    bool _inDeformation = false;
    [Tooltip("武器切り替えの")]
    BoolReactiveProperty _isSwtchWeapon = new BoolReactiveProperty();
    [Tooltip("武器のアニメーションの状態")]
    ObservableStateMachineTrigger _trigger = null;
    [Tooltip("メイン武器とサブ武器")]
    WeaponData[] _mainAndSub = new WeaponData[2];
    [Tooltip("装備している武器")]
    WeaponData _targetWeapon;

    public ObjectBase WeaponPrefab => _weaponPrefab;
    public ParticleSystem MyParticleSystem => _myParticleSystem;
    public WeaponData TargetWeapon { get => _targetWeapon; set => _targetWeapon = value; }

    private void Start()
    {
        _trigger = _weaponPrefab.ObjectAnimator.GetBehaviour<ObservableStateMachineTrigger>();
        _myParticleSystem.Stop();
        SwichWeapon(_isSwtchWeapon);
        WeaponState();
    }
    void Update()
    {
        if (!WeaponMenuHander.Instance.MenuIsOpen)
        {
            if (!PlayerAnimationState.Instance.IsAttack)
            {
                _isSwtchWeapon.Value = Input.GetButton("SubWeaponSwitch");
            }
            WeaponAttack(_playerAnimator);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        TargetWeapon.Base.AttackMovement(other, TargetWeapon.Action);
    }
    /// <summary>
    /// 武器の入力判定
    /// </summary>
    public void WeaponAttack(Animator playerAnimator)
    {
        if (!PlayerAnimationState.Instance.AbleInput || WeaponMenuHander.Instance.MenuIsOpen) return;
        if (_inDeformation) return;
        ////通常攻撃の処理
        if (Input.GetButtonDown("Attack"))
        {
            playerAnimator.SetTrigger("AttackTrigger");
            playerAnimator.SetInteger("WeaponType", (int)TargetWeapon.Type);
        }
        else
        {
            //溜め攻撃の処理(弓矢のアニメーションもこの処理）
            if (Input.GetButton("Attack"))
            {
                TargetWeapon.Action.ChargeCount += Time.deltaTime;
                playerAnimator.SetBool("Charge", true);
                if (TargetWeapon.Action.ChargeCount > TargetWeapon.Base.ChargeLevels[0])
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
    /// <summary>
    /// メイン武器・サブ武器の装備をボタンで切り替える関数
    /// </summary>
    public void SwichWeapon(BoolReactiveProperty weaponSwitch)
    {
        weaponSwitch
            .Subscribe(onWeaponSwitch =>
            {
                _targetWeapon.WeaponEnabled = false;
                if (!onWeaponSwitch)
                {
                    _targetWeapon = _mainAndSub[(int)CommandType.MAIN];
                }
                else
                {
                    _targetWeapon = _mainAndSub[(int)CommandType.SUB];
                }
                _targetWeapon.WeaponEnabled = true;
                _weaponPrefab.ObjectAnimator.SetInteger("WeaponType", (int)_targetWeapon.Type);
                _playerAnimator.SetInteger("WeaponType", (int)_targetWeapon.Type);
            }).AddTo(this);
    }
    /// <summary>
    /// 武器の装備
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="type"></param>
    public void SetEquipment(WeaponData weaponType, CommandType type)
    {
        _mainAndSub[(int)type] = weaponType;
        _targetWeapon = _mainAndSub[(int)type];
        if (_targetWeapon.Type == _mainAndSub[(int)type].Type)
        {
            _weaponPrefab.ObjectAnimator.SetInteger("WeaponType", (int)weaponType.Type);
        }
    }
    /// <summary>
    /// 属性の装備
    /// </summary>
    /// <param name="elementType"></param>
    public void SetElement(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.RIGIT:
                _weaponPrefab.ObjectAnimator.SetBool("IsOpen", false);
                break;
            default:
                _weaponPrefab.ObjectAnimator.SetBool("IsOpen", true);
                break;
        }
        _targetWeapon.Base.WeaponModeToElement(elementType);
    }

    private void OnDisable()
    {
        _isSwtchWeapon.Dispose();
    }
}





