using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField, Tooltip("νprefabπi[·ιΟ")] GameObject[] m_weaponPrefab = new GameObject[3];
    [SerializeField, Tooltip("Cν")] GameObject m_mainWeapon = default;
    [SerializeField, Tooltip("Tuν")] GameObject m_subWeapon = default;

    [Tooltip("νΨθΦ¦")] bool m_weaponSwitch = true;
    [Tooltip("υΜν")] GameObject m_equipmentWeapon = default;
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
        //νΜCΖTuΜ\¦πΨθΦ¦ι
        if (Input.GetButtonDown("WeaponChange")
            && !Input.GetButton("Attack")
            && !m_weaponAction.Attacked)
        {
            m_weaponSwitch = !m_weaponSwitch;
            m_mainWeapon.SetActive(m_weaponSwitch);
            m_subWeapon.SetActive(!m_weaponSwitch);

            //CΖTuΜνπΨθΦ¦ι
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
