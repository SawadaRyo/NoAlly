//日本語コメント可
using UniRx;
using UnityEngine;

public interface IWeaponController
{
    public IReadOnlyReactiveProperty<WeaponBase> EquipementWeapon { get; }
    public IReadOnlyReactiveProperty<ElementType> CurrentElement { get; }
    /// <summary>
    /// 初期化関数
    /// </summary>
    /// <param name="weaponProcessing"></param>
    /// <param name="main"></param>
    /// <param name="sub"></param>
    public void Initializer(WeaponType main = WeaponType.SWORD, WeaponType sub = WeaponType.LANCE);
    /// <summary>
    /// メイン武器・サブ武器の装備をボタンで切り替える関数
    /// </summary>
    public void SwichEquipmentWeapon(bool weaponSwitch);
    /// <summary>
    /// メイン武器・サブ武器の装備を変更する関数
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="type"></param>
    public void SetEquipmentWeapon(WeaponType typeOfWeapon, CommandType type);
    /// <summary>
    /// 属性の装備
    /// </summary>
    /// <param name="elementType"></param>
    public void SetElement(ElementType elementType);
}
