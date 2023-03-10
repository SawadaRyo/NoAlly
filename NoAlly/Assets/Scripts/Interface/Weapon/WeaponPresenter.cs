using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DataOfWeapon;

public class WeaponPresenter : MonoBehaviour
{
    [SerializeField, Header("�v���C���[")]
    PlayerContoller _playerContoller;

    [Space(15)]
    [Header("Model")]
    [SerializeField, Header("WeaponEquipment���i�[����֐�")]
    WeaponMenu _weaponEquipment = null;

    [Space(15)]
    [Header("View")]
    [SerializeField, Header("WeaponProcessing���i�[����֐�")]
    WeaponProcessing _weaponProcessing = null;
    [SerializeField, Header("WeaponEquipment���i�[����֐�")]
    WeaponMenuHander _weaponMenuHander = null;
    void Awake()
    {
        _weaponEquipment.Initialize();
        _weaponMenuHander.Initialize();
        WeaponEquipmentState();
        //WeaponProcessingState();
        MenuHanderState();
    }
    void WeaponEquipmentState()
    {
        //����̑������
        _weaponEquipment.MainWeapon
            .Subscribe(mainWeapon =>
            {
                _weaponProcessing.SetEquipment(mainWeapon, CommandType.MAIN);
                if (_weaponProcessing.TargetWeapon.Base == mainWeapon)
                {
                    _weaponProcessing.TargetWeapon.Base.WeaponModeToElement(_weaponEquipment.Element.Value);
                }
            }).AddTo(this);
        _weaponEquipment.SubWeapon
            .Subscribe(subWeapon =>
            {
                _weaponProcessing.SetEquipment(subWeapon, CommandType.SUB);
                if (_weaponProcessing.TargetWeapon.Base == subWeapon)
                {
                    _weaponProcessing.TargetWeapon.Base.WeaponModeToElement(_weaponEquipment.Element.Value);
                }
            }).AddTo(this);
        _weaponEquipment.Element
            .Subscribe(element =>
            {
                _weaponProcessing.SetElement(element);
            }).AddTo(this);
    }
    //void WeaponProcessingState()
    //{
    //    _weaponProcessing.IsSwichWeapon
    //       .Subscribe(isSwich =>
    //       {
    //           if (!PlayerAnimationState.Instance.IsAttack)
    //           {
    //               _weaponProcessing.TargetWeapon = _weaponEquipment.CheckWeaponActive(_weaponVisual.SwichWeapon(isSwich));
    //           }
    //       }).AddTo(this);
    //}
    void MenuHanderState()
    {
        //���j���[�{�^���̑I��
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

        //�����؂�ւ��鏈��
        _weaponMenuHander.IsDiside //����{�^�����������Ƃ��Ɏ��s
            .Subscribe(button =>
            {
                if (button)
                {
                    ICommandButton nextButton = _weaponMenuHander.SelectButton; //���ɑ������镐��
                    ICommandButton beforeWeapon = _weaponEquipment.SelectedButtons[(int)nextButton.TypeOfCommand]; //���O�܂ő������Ă�������

                    switch (nextButton.TypeOfCommand)
                    {
                        case CommandType.MAIN:
                            IWeaponCommand selectedSubButton = (IWeaponCommand)_weaponEquipment.SelectedButtons[(int)CommandType.SUB];
                            IWeaponCommand nextMainweapon = (IWeaponCommand)nextButton;
                            IWeaponCommand beforeMainweapon = (IWeaponCommand)beforeWeapon;
                            if (nextMainweapon.TypeOfWeapon == selectedSubButton.TypeOfWeapon) //���ɑ������郁�C������ƌ��ݑ������Ă���T�u���킪�d�����Ă���ꍇ�A
                                                                                               //�T�u��������ւ���
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
                            if (nextSubweapon.TypeOfWeapon == selectedMainButton.TypeOfWeapon) //���ɑ�������T�u����ƌ��ݑ������Ă��郁�C�����킪�d�����Ă���ꍇ�A
                                                                                               //���C����������ւ���
                            {
                                _weaponEquipment.AllButton[(int)CommandType.MAIN, (int)_weaponEquipment.MainWeapon.Value.Type].Disaide(false);
                                _weaponEquipment.AllButton[(int)CommandType.MAIN, (int)beforeSubweapon.TypeOfWeapon].Disaide(true);
                                _weaponEquipment.SelectedButtons[(int)CommandType.MAIN] = _weaponEquipment.AllButton[(int)CommandType.MAIN, (int)beforeSubweapon.TypeOfWeapon];
                            }
                            break;
                        default:
                            break;
                    }

                    //���O�܂ő������Ă�������Ǝ��ɑ������镐���؂�ւ���
                    beforeWeapon.Disaide(false);
                    nextButton.Disaide(true);
                    _weaponEquipment.SelectedButtons[(int)nextButton.TypeOfCommand] = nextButton;
                }
            });
    }
}
