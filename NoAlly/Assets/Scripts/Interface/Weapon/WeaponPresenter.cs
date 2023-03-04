using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DataOfWeapon;

public class WeaponPresenter : MonoBehaviour
{
    [SerializeField, Header("WeaponScriptableObjects本体")]
    WeaponScriptableObjects _weaponScriptableObjects;
    [SerializeField, Header("プレイヤー")]
    PlayerContoller _playerContoller;

    [Space(15)]
    [Header("Model")]
    [SerializeField, Header("WeaponEquipmentを格納する関数")]
    WeaponEquipment _weaponEquipment = null;

    [Space(15)]
    [Header("View")]
    [SerializeField, Header("WeaponVisualControllerを格納する関数")]
    WeaponTransController _weaponVisual = null;
    [SerializeField, Header("WeaponProcessingを格納する関数")]
    WeaponProcessing _weaponProcessing = null;
    [SerializeField, Header("WeaponEquipmentを格納する関数")]
    WeaponMenuHander _weaponMenuHander = null;

    SetWeaponData _weaponData = null;
    void Awake()
    {
        _weaponData = new SetWeaponData(_weaponScriptableObjects);
        _weaponEquipment.FirstSetWeapon(_weaponData);
        _weaponEquipment.Initialize();
        _weaponMenuHander.Initialize();
        WeaponEquipmentState();
        WeaponProcessingState();
        MenuHanderState();
    }
    void WeaponEquipmentState()
    {
        //武器の装備情報
        _weaponEquipment.MainWeapon
            .Subscribe(mainWeapon =>
            {
                if (mainWeapon == null) return;

                _weaponVisual.SetEquipment(mainWeapon, CommandType.MAIN);
                _weaponProcessing.TargetWeapon = _weaponEquipment
                                                 .CheckWeaponActive(_weaponVisual
                                                 .SwichWeapon(_weaponProcessing.IsSwichWeapon.Value));
            }).AddTo(this);
        _weaponEquipment.SubWeapon
            .Subscribe(subWeapon =>
            {
                if (subWeapon == null) return;

                _weaponVisual.SetEquipment(subWeapon, CommandType.SUB);
                _weaponProcessing.TargetWeapon = _weaponEquipment
                                                 .CheckWeaponActive(_weaponVisual
                                                 .SwichWeapon(_weaponProcessing.IsSwichWeapon.Value));
            }).AddTo(this);
    }
    void WeaponProcessingState()
    {
        _weaponProcessing.IsSwichWeapon
           .Subscribe(isSwich =>
           {
               if (!PlayerAnimationState.Instance.IsAttack)
               {
                   _weaponProcessing.TargetWeapon = _weaponEquipment.CheckWeaponActive(_weaponVisual.SwichWeapon(isSwich));
               }
           }).AddTo(this);
    }
    void MenuHanderState()
    {
        //メニューボタンの選択
        _weaponMenuHander.CrossH
            .Subscribe(crossH =>
            {
                ICommandButton b = _weaponEquipment.SelectButton(crossH, _weaponMenuHander.CrossV.Value);
                if (_weaponMenuHander.SelectButton != null)
                {
                    _weaponMenuHander.SelectButton.Selected(false);
                    _weaponMenuHander.SelectButton = b;
                    _weaponMenuHander.SelectButton.Selected(true);
                }
                else
                {
                    _weaponMenuHander.SelectButton = b;
                    _weaponMenuHander.SelectButton.Selected(true);
                }
            }).AddTo(this);
        _weaponMenuHander.CrossV
            .Subscribe(crossV =>
            {
                ICommandButton b = _weaponEquipment.SelectButton(_weaponMenuHander.CrossH.Value, crossV);
                if (_weaponMenuHander.SelectButton != null)
                {
                    _weaponMenuHander.SelectButton.Selected(false);
                    _weaponMenuHander.SelectButton = b;
                    _weaponMenuHander.SelectButton.Selected(true);
                }
                else
                {
                    _weaponMenuHander.SelectButton = b;
                    _weaponMenuHander.SelectButton.Selected(true);
                }
            }).AddTo(this);

        //武器を切り替える処理
        _weaponMenuHander.IsDiside //決定ボタンを押したときに実行
            .Subscribe(button =>
            {
                if (button)
                {
                    ICommandButton nextButton = _weaponMenuHander.SelectButton; //次に装備する武器
                    ICommandButton beforeWeapon = _weaponEquipment.SelectedButtons[(int)nextButton.TypeOfCommand]; //直前まで装備していた武器

                    switch (nextButton.TypeOfCommand)
                    {
                        case CommandType.MAIN:
                            IWeaponCommand selectedSubButton = (IWeaponCommand)_weaponEquipment.SelectedButtons[(int)CommandType.SUB];
                            IWeaponCommand nextMainweapon = (IWeaponCommand)nextButton;
                            IWeaponCommand beforeMainweapon = (IWeaponCommand)beforeWeapon;
                            if (nextMainweapon.TypeOfWeapon == selectedSubButton.TypeOfWeapon) //次に装備するメイン武器と現在装備しているサブ武器が重複している場合、
                                                                                               //サブ武器を入れ替える
                            {
                                _weaponEquipment.AllButton[(int)CommandType.SUB, (int)_weaponEquipment.SubWeapon.Value.Type].Disaide(false);
                                _weaponEquipment.AllButton[(int)CommandType.SUB, (int)beforeMainweapon.TypeOfWeapon].Disaide(true);
                                _weaponEquipment.SelectedButtons[(int)CommandType.SUB] = _weaponEquipment.AllButton[(int)CommandType.SUB, (int)beforeMainweapon.TypeOfWeapon];
                            }
                            break;
                        case CommandType.SUB:
                            IWeaponCommand selectedMainButton = (IWeaponCommand)_weaponEquipment.SelectedButtons[(int)CommandType.MAIN];
                            IWeaponCommand nextSubweapon = (IWeaponCommand)nextButton;
                            IWeaponCommand beforeSubweapon = (IWeaponCommand)beforeWeapon;
                            if (nextSubweapon.TypeOfWeapon == selectedMainButton.TypeOfWeapon) //次に装備するサブ武器と現在装備しているメイン武器が重複している場合、
                                                                                               //メイン武器を入れ替える
                            {
                                _weaponEquipment.AllButton[(int)CommandType.MAIN, (int)_weaponEquipment.MainWeapon.Value.Type].Disaide(false);
                                _weaponEquipment.AllButton[(int)CommandType.MAIN, (int)beforeSubweapon.TypeOfWeapon].Disaide(true);
                                _weaponEquipment.SelectedButtons[(int)CommandType.MAIN] = _weaponEquipment.AllButton[(int)CommandType.MAIN, (int)beforeSubweapon.TypeOfWeapon];
                            }
                            break;
                        default:
                            break;
                    }

                    //直前まで装備していた武器と次に装備する武器を切り替える
                    beforeWeapon.Disaide(false);
                    nextButton.Disaide(true);
                    _weaponEquipment.SelectedButtons[(int)nextButton.TypeOfCommand] = nextButton;
                }
            });
    }
}
