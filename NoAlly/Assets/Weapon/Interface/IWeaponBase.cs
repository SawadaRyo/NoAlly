using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IWeaponBase<T> where T : class
{
    /// <summary>
    /// このオブジェクトのオーナー
    /// </summary>
    public T Owner { get; }
    /// <summary>
    /// 武器の変形判定
    /// </summary>
    public WeaponDeformation Deformated { get; }
    /// <summary>
    /// 初期化関数
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="baseObj"></param>
    /// <param name="weaponData"></param>
    public void Initializer(T owner,WeaponController baseObj, WeaponDataEntity weaponData);
    /// <summary>
    /// 武器の変形時に実行する処理
    /// </summary>
    /// <param name="elementType"></param>
    public void WeaponModeToElement(ElementType elementType);
    /// <summary>
    /// 攻撃時の処理
    /// </summary>
    public void AttackBehaviour();
    /// <summary>
    /// 何かしらのオブジェクトがヒットした時の処理
    /// </summary>
    /// <param name="col"></param>
    public void HitObjectBehaviour(Collider col);
    /// <summary>
    /// 武器のチャージ
    /// </summary>
    /// <param name="chargeTime"></param>
    /// <returns></returns>
    public float InputCharging(float chargeTime);
    /// <summary>
    /// 武器のダメージ計算
    /// </summary>
    /// <param name="magnification"></param>
    /// <returns></returns>
    public WeaponPower CurrentPower(float magnification = 1f);
    /// <summary>
    /// 装備時関数
    /// </summary>
    public void OnEquipment();
    /// <summary>
    /// 解除時関数
    /// </summary>
    public void OnLift();
}
