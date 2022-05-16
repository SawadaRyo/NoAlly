using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField, Tooltip("メイン武器")] GameObject m_mainWeapon = default;
    [SerializeField, Tooltip("サブ武器")] GameObject m_subWeapon = default;

    [Tooltip("武器切り替え")] bool m_weaponSwitch = true;
    [Tooltip("装備中の武器")] GameObject m_equipmentWeapon = default;
    WeaponAction m_weaponAction = default;

    public GameObject EquipmentWeapon { get => m_equipmentWeapon; set => m_equipmentWeapon = value; }
    void Start()
    {
        m_weaponAction = GetComponent<WeaponAction>();

        m_equipmentWeapon = m_mainWeapon;
        if (m_mainWeapon != null)
        {
            m_mainWeapon.SetActive(true);
            m_subWeapon.SetActive(false);
        }
        m_weaponSwitch = true;
    }
    void Update()
    {
        WeaponChangeMethod();
    }
    void WeaponChangeMethod()
    {
        //武器のメインとサブの表示を切り替える
        if (Input.GetButtonDown("WeaponChange")
            && !Input.GetButton("Attack")
            && !m_weaponAction.Attacked)
        {
            m_weaponSwitch = !m_weaponSwitch;
            m_mainWeapon.SetActive(m_weaponSwitch);
            m_subWeapon.SetActive(!m_weaponSwitch);

            //メインとサブの武器を切り替える
            if (m_weaponSwitch)
            {
                m_equipmentWeapon = m_mainWeapon;
            }
            else
            {
                m_equipmentWeapon = m_subWeapon;
            }
        }
    }
}
