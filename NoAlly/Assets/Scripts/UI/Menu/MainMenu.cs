using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 武器・エンチャントを変更するためのコンポーネント
/// </summary>

public class MainMenu : SingletonBehaviour<MainMenu>
{
    [SerializeField] Button[] _mainWeapons = null;
    [SerializeField] Button[] _subWeapons = null;
    [SerializeField] Button[] _elements = null;

    List<IWeapon> _weaponMethods = new();
    ReactiveProperty<WeaponDateEntity> _main = new ReactiveProperty<WeaponDateEntity>();
    ReactiveProperty<WeaponDateEntity> _sub = new ReactiveProperty<WeaponDateEntity>();
    ElementType _elementType = default;


    public ElementType Element => _elementType;
    public IReadOnlyReactiveProperty<WeaponDateEntity> Main => _main;
    public IReadOnlyReactiveProperty<WeaponDateEntity> Sub => _sub;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        int weaponIndexNumber = Enum.GetNames(typeof(WeaponType)).Length;
        for (int index = 0; index < weaponIndexNumber; index++)
        {
            WeaponDateEntity weapon = SetWeaponData.Instance.GetWeapon((WeaponType)index);
            _mainWeapons[index].onClick.AddListener(() => Equipment(CommandType.MAIN, weapon));
            _subWeapons[index].onClick.AddListener(() => Equipment(CommandType.SUB, weapon));
            _elements[index].onClick.AddListener(() => DisideElement((ElementType)index));
            _weaponMethods.Add(weapon.Base);
        }
        _main.Value = SetWeaponData.Instance.GetWeapon(WeaponType.SWORD);
        _sub.Value = SetWeaponData.Instance.GetWeapon(WeaponType.LANCE);
    }
    /// <summary> ここで装備武器を切り替える（MainとSubの装備武器が同じだった場合それぞれの装備武器を入れ替える）</summary>
    /// <param name="weaponName"></param>
    /// <param name="type"></param>
    public void Equipment(CommandType type, WeaponDateEntity weaponName)
    {
        WeaponDateEntity beforeWeapons = default;
        if (type == CommandType.MAIN)
        {
            beforeWeapons = _main.Value;
            _main.Value = weaponName;
            if (_sub.Value.Type == _main.Value.Type)
            {
                _sub.Value = beforeWeapons;
            }
        }
        else if (type == CommandType.SUB)
        {
            beforeWeapons = _sub.Value;
            _sub.Value = weaponName;
            if (_main.Value.Type == _sub.Value.Type)
            {
                _main.Value = beforeWeapons;
            }
        }
    }
    public void DisideElement(ElementType element)
    {
        _elementType = element;
        _weaponMethods.ToList().ForEach(x => x.WeaponMode(element));
    }

    private void OnDisable()
    {
        _main.Dispose();
        _sub.Dispose();
    }
}



