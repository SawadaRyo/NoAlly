using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquipment : SingletonBehaviour<WeaponEquipment>
{
    [SerializeField, Tooltip("メイン武器")] WeaponBase m_mainWeaponBase = default;
    [SerializeField, Tooltip("サブ武器")] WeaponBase m_subWeaponBase = default;

    [Tooltip("現在の属性")] ElementType type = default;
    [Tooltip("武器切り替え")] bool m_weaponSwitch = false;
    [Tooltip("装備中の武器")] WeaponBase m_equipmentWeapon = default;
    WeaponAction m_weaponAction = default;

    public WeaponBase EquipmentWeapon { get => m_equipmentWeapon; set => m_equipmentWeapon = value; }
    public WeaponBase MainWeapon { get => m_mainWeaponBase; set => m_mainWeaponBase = value; }
    public WeaponBase SubWeapon { get => m_subWeaponBase; set => m_subWeaponBase = value; }
    public WeaponAction EquipeWeaponAction => m_weaponAction;

    private void Awake()
    {
        m_equipmentWeapon = m_mainWeaponBase;
        m_weaponAction = m_equipmentWeapon.GetComponent<WeaponAction>();
    }

    void Start()
    {
        if (m_mainWeaponBase != null && m_subWeaponBase != null)
        {
            WeaponChangeMethod();
        }
    }
    void Update()
    {
        if (Input.GetButtonDown("WeaponChange")
            && !Input.GetButton("Attack"))
        {
            WeaponChangeMethod();
        }

        if (!MenuHander.Instance.MenuIsOpen)
        {
            m_weaponAction.WeaponAttackMethod(m_equipmentWeapon.name);
        }

        if(PlayerContoller.Instance.IsWalled())
        {
            m_equipmentWeapon.RendererActive(false);
        }
        else
        {
            m_equipmentWeapon.RendererActive(true);
        }
    }
    void WeaponChangeMethod()
    {
        //武器のメインとサブの表示を切り替える

        m_weaponSwitch = !m_weaponSwitch;
        m_mainWeaponBase.RendererActive(m_weaponSwitch);
        m_subWeaponBase.RendererActive(!m_weaponSwitch);

        //メインとサブの武器を切り替える
        if (m_weaponSwitch)
        {
            m_equipmentWeapon = m_mainWeaponBase;
        }
        else
        {
            m_equipmentWeapon = m_subWeaponBase;
        }
        m_weaponAction = m_equipmentWeapon.GetComponent<WeaponAction>();
    }
}
