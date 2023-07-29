using System.Linq;
using UnityEngine;
using UniRx;
using DataOfWeapon;

public class WeaponMenuPresenter : MonoBehaviour
{
    [Space(15)]
    [Header("Model")]
    
    [SerializeField, Tooltip("")]
    MenuManagerBase _menuManager = null;
    [SerializeField, Header("")]
    WeaponController _weaponStateController = null;

    [Space(15)]
    [Header("View")]
    [SerializeField, Header("")]
    //WeaponElementColor _weaponElementColor = null;

    [Tooltip("WeaponEquipmentÇäiî[Ç∑ÇÈä÷êî")]
    Equipment[] _weaponEquipment = null;
    [Tooltip("ÉÅÉjÉÖÅ[ì‡ÇÃEquipment")]
    ReactiveCollection<Equipment> _equipment = new();
    bool _isSwitch = false;
    WeaponType _cullentWeaponType;

    public WeaponType CullentWeaponType => _cullentWeaponType;

    void Awake()
    {
        _weaponEquipment = _menuManager.GetComponentButtonList<Equipment>().OrderBy(x => x.commandType).ToArray();
        for (int i = 0; i < _weaponEquipment.Length; i++)
        {
            int index = i;
            _equipment.Add(_weaponEquipment[index]);
            WeaponEquipmentState(index);
        }
        _isSwitch = false;
        
        //_weaponInput.TargetWeapon = _weaponProcessing.SwichWeapon(_isSwitch);
    }
    void WeaponEquipmentState(int index)
    {
        //ïêäÌÇÃëïîıèÓïÒ
        _weaponEquipment[index].MainWeapon.Skip(1)
            .Subscribe(mainWeapon =>
            {
                _weaponStateController.SetEquipmentWeapon(mainWeapon, CommandType.MAIN);
                //_cullentWeaponType = mainWeapon;
                Debug.Log(mainWeapon);
            }).AddTo(this);
        _weaponEquipment[index].SubWeapon.Skip(1)
           .Subscribe(subWeapon =>
           {
               _weaponStateController.SetEquipmentWeapon(subWeapon, CommandType.SUB);
               //_cullentWeaponType = subWeapon;
               Debug.Log(subWeapon);
           }).AddTo(this);
        _weaponEquipment[index].Element.Skip(1)
            .Subscribe(element =>
            {
                _weaponStateController.SetElement(element);
                if (element != ElementType.RIGIT)
                {
                    //_weaponElementColor.IsActiveElement(_cullentWeaponType,element);
                }
            }).AddTo(this);
        _weaponEquipment[index].Equiped.Skip(1)
            .Subscribe(equiped =>
            {
                //_weaponInput.TargetWeapon = _weaponProcessing.SetEquipment(_isSwitch);
                //_weaponInput.TargetWeapon = _weaponProcessing.SwichWeapon(_isSwitch);
            });

    }
}
