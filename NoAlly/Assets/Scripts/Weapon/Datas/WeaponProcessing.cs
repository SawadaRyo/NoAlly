using System;
using UnityEngine;
using UniRx;

/// <summary>
/// 武器のモーションや可視化などの武器にまつわる処理を行うクラス
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [SerializeField,Tooltip("")]
    ObjectBase _weaponPrefab = null;
    [SerializeField, Header("プレイヤーのアニメーター")]
    Animator _playerAnimator = null;
    [SerializeField, Header("武器の斬撃エフェクト")]
    ParticleSystem _myParticleSystem = default;

    [Tooltip("メイン武器とサブ武器")]
    WeaponData[] _mainAndSub = new WeaponData[2];
    [Tooltip("装備している武器")]
    WeaponData _targetWeapon;

    BoolReactiveProperty _isSwichWeapon = new BoolReactiveProperty();

    public WeaponData TargetWeapon { get => _targetWeapon; set => _targetWeapon = value; }
    public IReadOnlyReactiveProperty<bool> IsSwichWeapon => _isSwichWeapon;

    private void Start()
    {
        _myParticleSystem.Stop();
    }
    void Update()
    {
        if (!WeaponMenuHander.Instance.MenuIsOpen && !PlayerAnimationState.Instance.IsAttack)
        {
            SwichWeapon(Input.GetButton("SubWeaponSwitch"));
        }
        _targetWeapon.Action.WeaponAttack(_playerAnimator,this);
    }
    private void OnTriggerEnter(Collider other)
    {
        _targetWeapon.Base.HitMovement(other);
    }
    
    
    /// <summary>
    /// メイン武器・サブ武器の装備をボタンで切り替える関数
    /// </summary>
    public void SwichWeapon(bool weaponSwitch)
    {
        _targetWeapon.WeaponEnabled = false;
        if (!weaponSwitch)
        {
            _targetWeapon = _mainAndSub[(int)CommandType.MAIN];
        }
        else
        {
            _targetWeapon = _mainAndSub[(int)CommandType.SUB];
        }
        _targetWeapon.WeaponEnabled = true;
        _weaponPrefab.ObjectAnimator.SetInteger("WeaponType", (int)_targetWeapon.Type);
    }
    /// <summary>
    /// 武器の装備
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="type"></param>
    public void SetEquipment(WeaponData weaponType, CommandType type)
    {
        _mainAndSub[(int)type] = weaponType;
        _weaponPrefab.ObjectAnimator.SetInteger("WeaponType", (int)weaponType.Type);
        if (_targetWeapon == null)
        {
            _targetWeapon = _mainAndSub[(int)type];
        }
    }
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
}





