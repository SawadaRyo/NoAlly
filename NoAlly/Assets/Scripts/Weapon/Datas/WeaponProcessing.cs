using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����̃��[�V����������Ȃǂ̕���ɂ܂�鏈�����s���N���X
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [SerializeField, Header("�v���C���[�̕��푕���N���X")]
    WeaponEquipment _weaponEquipment = null;

    [Tooltip("�v���C���[�̓���")]
    WeaponActionType _actionType;
    // Start is called before the first frame update
    void Start()
    {
        _weaponEquipment.Initialize();
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
        if (Input.GetButton("Attack"))
        {
            _actionType = WeaponActionType.Charging;
        }
        else if (Input.GetButtonUp("Attack"))
        {
            _actionType = WeaponActionType.ChargeAttack;
        }
        _weaponEquipment.EquipeWeapon.Action.WeaponAttack(_actionType,_weaponEquipment.EquipeWeapon.Type);
        _actionType = WeaponActionType.None;
    }
}




