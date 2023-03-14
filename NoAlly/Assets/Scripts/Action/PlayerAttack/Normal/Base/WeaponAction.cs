using System;
using UnityEngine;

public class WeaponAction : IWeaponAction
{
    [Tooltip("����̃v���n�u")]
    protected ObjectBase _weaponPrefab = null;
    [Tooltip("WeaponBase���i�[����ϐ�")]
    protected IWeaponBase _weaponBase = null;
    [Tooltip("")]
    float _time = 0;

    public virtual void WeaponChargeAttackMethod(float chrageCount, float[] chargeLevels, float[] weaponPower, ElementType elementType) { }
    protected float[] ChargePower(float[] weaponPower, ElementType top, float magnification)
    {
        if (magnification < 1)
        {
            magnification = 1;
        }
        weaponPower[(int)ElementType.RIGIT] *= magnification;
        switch (top)
        {
            case ElementType.RIGIT:
                weaponPower[(int)ElementType.RIGIT] *= magnification;
                break;
            case ElementType.FIRE:
                weaponPower[(int)ElementType.FIRE] *= magnification;
                break;
            case ElementType.ELEKE:
                weaponPower[(int)ElementType.ELEKE] *= magnification;
                break;
            case ElementType.FROZEN:
                weaponPower[(int)ElementType.FROZEN] *= magnification;
                break;
        }
        return weaponPower;
    }

    /// <summary>
    /// ����̓��͔���
    /// </summary>
    public void WeaponAttack(Animator playerAnimator,WeaponProcessing weaponProcessing)
    {
        if (!PlayerAnimationState.Instance.AbleInput || WeaponMenuHander.Instance.MenuIsOpen) return;
        ////�ʏ�U���̏���
        if (Input.GetButtonDown("Attack"))
        {
            playerAnimator.SetTrigger("AttackTrigger");
            playerAnimator.SetInteger("WeaponType", (int)weaponProcessing.TargetWeapon.Type);
        }
        else
        {
            //���ߍU���̏���(�|��̃A�j���[�V���������̏����j
            if (Input.GetButton("Attack"))
            {
                _time += Time.deltaTime;
                playerAnimator.SetBool("Charge", true);
                if (_time > weaponProcessing.TargetWeapon.Base.ChargeLevels[0])
                {
                    playerAnimator.SetTrigger("ChargeAttackTrigger");
                }
            }
            else if (Input.GetButtonUp("Attack"))
            {
                playerAnimator.SetBool("Charge", false);
                _time = 0;
            }
        }
    }
}