using DataOfWeapon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ����E�G���`�����g��ύX���邽�߂̃R���|�[�l���g
/// </summary>

public class WeaponEquipment : MonoBehaviour
{
    [SerializeField, Header("����̃X�N���v�^�u���I�u�W�F�N�g")]
    WeaponScriptableObjects _weaponScriptableObjects;
    [SerializeField, Header("���C��������I������{�^��")]
    Button[] _mainWeapons = null;
    [SerializeField, Header("�T�u������I������{�^��")]
    Button[] _subWeapons = null;
    [SerializeField, Header("����̑�����I������{�^��")]
    Button[] _elements = null;
    [SerializeField, Header("�v���C���[���i�[����ϐ�")]
    PlayerContoller _playerContoller = null;

    [Tooltip("����̃f�[�^")]
    SetWeaponData _weaponData = new SetWeaponData();
    List<IWeapon> _weaponMethods = new();
    [Tooltip("�������̕���")]
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
    /// <summary> �����ő��������؂�ւ���</summary>
    /// <param name="weaponName"></param>
    /// <param name="type"></param>
    public void Equipment(CommandType type, WeaponDatas weaponName)
    {
        WeaponDatas beforeWeapons = default;
        if (type == CommandType.MAIN)
        {
            beforeWeapons = _mainWeapon.Value;
            _mainWeapon.Value = weaponName;
            if (_subWeapon.Value.Type == _mainWeapon.Value.Type) //Main��Sub�̑������킪�����������ꍇ���ꂼ��̑�����������ւ���
            {
                _subWeapon.Value = beforeWeapons;
            }
        }
        else if (type == CommandType.SUB)
        {
            beforeWeapons = _subWeapon.Value;
            _subWeapon.Value = weaponName;
            if (_mainWeapon.Value.Type == _subWeapon.Value.Type) //Sub��Main�̑������킪�����������ꍇ���ꂼ��̑�����������ւ���
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
    /// �g�p���̕�����w�肷��֐�
    /// </summary>
    /// <param name="weaponEnabled">���݂̕���̎g�p��</param>
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



