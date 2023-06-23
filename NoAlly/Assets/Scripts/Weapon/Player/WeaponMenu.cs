using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 武器・エンチャントを変更するためのコンポーネント
/// </summary>

public class WeaponMenu : MonoBehaviour
{
    [Tooltip("装備中の武器と属性")]
    ReactiveProperty<WeaponType> _mainWeapon = new ReactiveProperty<WeaponType>();
    ReactiveProperty<WeaponType> _subWeapon = new ReactiveProperty<WeaponType>();
    ReactiveProperty<ElementType> _elementType = new ReactiveProperty<ElementType>();

    public IReadOnlyReactiveProperty<ElementType> Element => _elementType;
    public IReadOnlyReactiveProperty<WeaponType> MainWeapon => _mainWeapon;
    public IReadOnlyReactiveProperty<WeaponType> SubWeapon => _subWeapon;

   

    /// <summary> 装備武器を切り替える</summary>
    /// <param name="weaponName"></param>
    /// <param name="commandType"></param>
    public void EquipmentWeapon(int weapon)
    {
        WeaponType weaponType = (WeaponType)weapon;
        _mainWeapon.Value = weaponType;
    }
    /// <summary>
    /// 属性を切り替える
    /// </summary>
    /// <param name="element"></param>
    public void EquipmentElement(int element)
    {
        ElementType e = (ElementType)element;
        _elementType.Value = e;
    }



    private void OnDisable()
    {
        _mainWeapon.Dispose();
        _subWeapon.Dispose();
        _elementType.Dispose();
    }
}



