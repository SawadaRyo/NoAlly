using System;
using UnityEngine;

/// <summary>
/// ����̃��[�V����������Ȃǂ̕���ɂ܂�鏈�����s���N���X
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [SerializeField, Header("����̃X�N���v�^�u���I�u�W�F�N�g")]
    WeaponScriptableObjects _weaponDatas = null;
    [SerializeField, Header("�v���C���[�̕��푕���N���X")]
    WeaponEquipment _weaponEquipment = null;

    [Tooltip("�v���C���[�̓���")]
    WeaponActionType _actionType;
    void Awake()
    {
        if (SetWeaponData.Instance.WeaponDatas == null)
        {
            _weaponEquipment.Initialize();
            SetWeaponData.Instance.WeaponDatas = _weaponDatas;
            SetWeaponData.Instance.Initialize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ////�ʏ�U���̏���
        if (Input.GetButtonDown("Attack"))
        {
            _actionType = WeaponActionType.Attack;
        }
        //���ߍU���̏���(�|��̃A�j���[�V���������̏����j
        else if (Input.GetButton("Attack"))
        {
            _actionType = WeaponActionType.Charging;
        }
        else if (Input.GetButtonUp("Attack"))
        {
            _actionType = WeaponActionType.ChargeAttack;
        }
        else
        {
            _actionType = WeaponActionType.None;
        }
        _weaponEquipment.EquipeWeapon.Action.WeaponAttack(_actionType,_weaponEquipment.EquipeWeapon.Type);
    }
}





