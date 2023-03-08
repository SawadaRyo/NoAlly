using System;
using UnityEngine;
using UniRx;

/// <summary>
/// 武器のモーションや可視化などの武器にまつわる処理を行うクラス
/// </summary>
public class WeaponProcessing : ObjectBase
{
    [SerializeField, Header("プレイヤーのアニメーター")]
    Animator _playerAnimator = null;
    [SerializeField, Header("武器の斬撃エフェクト")]
    ParticleSystem _myParticleSystem = default;

    [Tooltip("メイン武器とサブ武器")]
    WeaponData[] _mainAndSub = new WeaponData[2];
    [Tooltip("装備している武器")]
    WeaponData _targetWeapon;
    float time = 0;

    BoolReactiveProperty _isSwichWeapon = new BoolReactiveProperty();

    public WeaponData TargetWeapon { get => _targetWeapon; set => _targetWeapon = value; }
    public IReadOnlyReactiveProperty<bool> IsSwichWeapon => _isSwichWeapon;

    void Update()
    {
        if (!WeaponMenuHander.Instance.MenuIsOpen && !Input.GetButton("Attack"))
        {
            _isSwichWeapon.Value = Input.GetButton("SubWeaponSwitch");
        }
        WeaponAttack();
    }
    private void OnTriggerEnter(Collider other)
    {
        _targetWeapon.Action.HitMovement(other,_targetWeapon.Base);
    }
    /// <summary>
    /// 武器の入力判定
    /// </summary>
    void WeaponAttack()
    {
        ////通常攻撃の処理
        if (Input.GetButtonDown("Attack"))
        {
            _playerAnimator.SetTrigger("AttackTrigger");
            _playerAnimator.SetInteger("WeaponType", (int)_targetWeapon.Type);
        }
        //溜め攻撃の処理(弓矢のアニメーションもこの処理）
        else if (Input.GetButton("Attack"))
        {
            time += Time.deltaTime;
            if (time > _targetWeapon.Action.ChargeLevel1 / 20)
            {
                _playerAnimator.SetBool("Charge", true);
            }
        }
        else if (Input.GetButtonUp("Attack"))
        {
            if (time > _targetWeapon.Action.ChargeLevel1 / 20)
            {
                _playerAnimator.SetTrigger("ChargeAttackTrigger");
            }
            _playerAnimator.SetBool("Charge", false);
            time = 0;
        }
    }
    public void HitJud(BoolAttack isAttack)
    {
        switch (isAttack)
        {
            case BoolAttack.ATTACKING:
                _myParticleSystem.Play();
                ActiveCollider(true);
                break;
            default:
                _myParticleSystem.Stop();
                ActiveCollider(false);
                break;
        }
    }
    /// <summary>
    /// メイン武器・サブ武器の装備をボタンで切り替える関数
    /// </summary>
    public void SwichWeapon(bool weaponSwitch)
    {
        if (!weaponSwitch)
        {
            _targetWeapon = _mainAndSub[(int)CommandType.MAIN];
        }
        else
        {
            _targetWeapon = _mainAndSub[(int)CommandType.SUB];
        }
    }
    /// <summary>
    /// 武器の装備
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="type"></param>
    public void SetEquipment(WeaponData weaponType, CommandType type)
    {
        _mainAndSub[(int)type] = weaponType;
        _objectAnimator.SetInteger("WeaponType", (int)weaponType.Type);
    }
    public void SetElement(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.RIGIT:
                _objectAnimator.SetBool("IsOpen", false);
                break;
            default:
                _objectAnimator.SetBool("IsOpen", true);
                break;
        }
        _targetWeapon.Base.WeaponModeToElement(elementType);
    }
}





