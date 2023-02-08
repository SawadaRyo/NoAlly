using DataOfWeapon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 武器・エンチャントを変更するためのコンポーネント
/// </summary>

public class WeaponEquipment : MonoBehaviour
{
    [SerializeField, Header("武器のスクリプタブルオブジェクト")]
    WeaponScriptableObjects _weaponScriptableObjects;
    [SerializeField, Header("メイン装備を選択するボタン")]
    Button[] _mainWeapons = null;
    [SerializeField, Header("サブ装備を選択するボタン")]
    Button[] _subWeapons = null;
    [SerializeField, Header("武器の属性を選択するボタン")]
    Button[] _elements = null;
    [SerializeField, Header("プレイヤーを格納する変数")]
    PlayerContoller _playerContoller = null;

    [Tooltip("武器のデータ")]
    SetWeaponData _weaponData = new SetWeaponData();
    List<IWeapon> _weaponMethods = new();
    [Tooltip("装備中の武器")]
    ReactiveProperty<WeaponDatas> _mainWeapon = new ReactiveProperty<WeaponDatas>();
    ReactiveProperty<WeaponDatas> _subWeapon = new ReactiveProperty<WeaponDatas>();
    ElementType _elementType = default;

    public SetWeaponData Data => _weaponData;
    public ElementType Element => _elementType;
    public IReadOnlyReactiveProperty<WeaponDatas> MainWeapon => _mainWeapon;
    public IReadOnlyReactiveProperty<WeaponDatas> SubWeapon => _subWeapon;


    public void Initialize()
    {
        _weaponData.Initialize(_weaponScriptableObjects, _playerContoller.GetComponent<WeaponVisualController>(), _playerContoller);
        int weaponIndexNumber = Enum.GetNames(typeof(WeaponType)).Length - 1;
        for (int index = 0; index < weaponIndexNumber; index++)
        {
            WeaponDatas weapon = _weaponData.GetWeapon((WeaponType)index);
            _mainWeapons[index].onClick.AddListener(() => Equipment(CommandType.MAIN, weapon));
            _subWeapons[index].onClick.AddListener(() => Equipment(CommandType.SUB, weapon));
            _elements[index].onClick.AddListener(() => DisideElement((ElementType)index));
            _weaponMethods.Add(weapon.Base);
        }
        _mainWeapon.Value = _weaponData.GetWeapon(WeaponType.SWORD);
        _subWeapon.Value = _weaponData.GetWeapon(WeaponType.LANCE);
    }
    /// <summary> ここで装備武器を切り替える</summary>
    /// <param name="weaponName"></param>
    /// <param name="type"></param>
    public void Equipment(CommandType type, WeaponDatas weaponName)
    {
        WeaponDatas beforeWeapons = default;
        if (type == CommandType.MAIN)
        {
            beforeWeapons = _mainWeapon.Value;
            _mainWeapon.Value = weaponName;
            if (_subWeapon.Value.Type == _mainWeapon.Value.Type) //MainとSubの装備武器が同じだった場合それぞれの装備武器を入れ替える
            {
                _subWeapon.Value = beforeWeapons;
            }
        }
        else if (type == CommandType.SUB)
        {
            beforeWeapons = _subWeapon.Value;
            _subWeapon.Value = weaponName;
            if (_mainWeapon.Value.Type == _subWeapon.Value.Type) //SubとMainの装備武器が同じだった場合それぞれの装備武器を入れ替える
            {
                _mainWeapon.Value = beforeWeapons;
            }
        }
    }
    public void DisideElement(ElementType element)
    {
        _elementType = element;
        Debug.Log(element);
        _weaponMethods.ForEach(x => x.WeaponMode(element));
    }

    /// <summary>
    /// 使用中の武器を指定する関数
    /// </summary>
    /// <param name="weaponEnabled">現在の武器の使用状況</param>
    /// <returns></returns>
    public WeaponDatas CheckWeaponActive((bool,bool) weaponEnabled)
    {
        if (weaponEnabled.Item1)
        {
            return _mainWeapon.Value;
        }
        return _subWeapon.Value;
        
    }

    private void OnDisable()
    {
        _mainWeapon.Dispose();
        _subWeapon.Dispose();
    }
}



