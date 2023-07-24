using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DataOfWeapon;

public class WeaponController : MonoBehaviour
{
    [SerializeField, Header("")]
    SetWeaponData _setWeaponData = null;
    [SerializeField, Header("WeaponScriptableObjects{Ì")]
    WeaponScriptableObjects _weaponScriptableObjects;

    [SerializeField]
    Transform _attackPos = null;
    [SerializeField]
    Transform _poolPos = null;
    [SerializeField, Header("íÌaGtFNg")]
    ParticleSystem _myParticleSystem = default;

    [Tooltip("")]
    ReactiveProperty<WeaponBase> _equipementWeapon = new();
    [Tooltip("p·éí")]
    WeaponBase _mainWeapon;
    [Tooltip("Tuzu")]
    WeaponBase _subWeapon;
    [Tooltip("")]
    ReactiveProperty<Collider> _targetCol = new();
    [Tooltip("»ÝÌ®«")]
    ReactiveProperty<ElementType> _currentElement = new();

    public Transform GetAttackPos => _attackPos;
    public Transform GetPoolPos => _poolPos;
    public IReadOnlyReactiveProperty<Collider> TargetCol => _targetCol;
    public IReadOnlyReactiveProperty<WeaponBase> EquipementWeapon => _equipementWeapon;
    public IReadOnlyReactiveProperty<ElementType> CurrentElement => _currentElement;
    void OnTriggerEnter(Collider other) => _targetCol.Value = other;

    /// <summary>
    /// ú»Ö
    /// </summary>
    /// <param name="weaponProcessing"></param>
    /// <param name="main"></param>
    /// <param name="sub"></param>
    public void Initializer(WeaponType main = WeaponType.SWORD, WeaponType sub = WeaponType.LANCE)
    {
        _setWeaponData.WeaponBaseInstantiate(this, _weaponScriptableObjects);
        _mainWeapon = _setWeaponData.WeaponDatas[main];
        _subWeapon = _setWeaponData.WeaponDatas[sub];
        _equipementWeapon.Value = _mainWeapon;
    }
    /// <summary>
    /// CíETuíÌõð{^ÅØèÖ¦éÖ
    /// </summary>
    public void SwichEquipmentWeapon(bool weaponSwitch)
    {
        if (!weaponSwitch)
        {
            _subWeapon.OnLift();
            _equipementWeapon.Value = _mainWeapon;
        }
        else
        {
            _mainWeapon.OnLift();
            _equipementWeapon.Value = _subWeapon;
        }
        _equipementWeapon.Value.OnEquipment();
    }
    /// <summary>
    /// CíETuíÌõðÏX·éÖ
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="type"></param>
    public void SetEquipmentWeapon(WeaponType typeOfWeapon, CommandType type)
    {
        //_weaponPrefabs[(int)_mainWeapon.Type].ActiveObject(false);
        //_weaponPrefabs[(int)_subWeapon.Type].ActiveObject(false);
        switch (type)
        {
            case CommandType.MAIN:
                _mainWeapon.OnLift();
                _mainWeapon = _setWeaponData.WeaponDatas[typeOfWeapon];
                _mainWeapon.OnEquipment();
                break;
            case CommandType.SUB:
                _subWeapon.OnLift();
                _subWeapon = _setWeaponData.WeaponDatas[typeOfWeapon];
                _subWeapon.OnEquipment();
                break;
        }
        //playerAnimator.SetInteger("WeaponType", (int)_equipementWeapon.WeaponData.TypeOfWeapon);
    }
    /// <summary>
    /// ®«Ìõ
    /// </summary>
    /// <param name="elementType"></param>
    public void SetElement(ElementType elementType)
    {
        _equipementWeapon.Value.WeaponModeToElement(elementType);
        _currentElement.Value = elementType;
    }
}
