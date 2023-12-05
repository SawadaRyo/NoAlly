using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IWeaponBase<T> where T : class
{
    /// <summary>
    /// ���̃I�u�W�F�N�g�̃I�[�i�[
    /// </summary>
    public T Owner { get; }
    /// <summary>
    /// ����̕ό`����
    /// </summary>
    public WeaponDeformation Deformated { get; }
    /// <summary>
    /// �������֐�
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="baseObj"></param>
    /// <param name="weaponData"></param>
    public void Initializer(T owner,WeaponController baseObj, WeaponDataEntity weaponData);
    /// <summary>
    /// ����̕ό`���Ɏ��s���鏈��
    /// </summary>
    /// <param name="elementType"></param>
    public void WeaponModeToElement(ElementType elementType);
    /// <summary>
    /// �U�����̏���
    /// </summary>
    public void AttackBehaviour();
    /// <summary>
    /// ��������̃I�u�W�F�N�g���q�b�g�������̏���
    /// </summary>
    /// <param name="col"></param>
    public void HitObjectBehaviour(Collider col);
    /// <summary>
    /// ����̃`���[�W
    /// </summary>
    /// <param name="chargeTime"></param>
    /// <returns></returns>
    public float InputCharging(float chargeTime);
    /// <summary>
    /// ����̃_���[�W�v�Z
    /// </summary>
    /// <param name="magnification"></param>
    /// <returns></returns>
    public WeaponPower CurrentPower(float magnification = 1f);
    /// <summary>
    /// �������֐�
    /// </summary>
    public void OnEquipment();
    /// <summary>
    /// �������֐�
    /// </summary>
    public void OnLift();
}
