using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChangeHander : MonoBehaviour
{
    [SerializeField] GameObject[] m_weaponPrefab = new GameObject[3];
    [SerializeField] WeaponChanger m_weaponChanger = default;
    [SerializeField] TargetWeaponChanger m_targetWeaponChanger = default;
     enum TargetWeaponChanger { Main,Sub }

    public void ChangeWeapon(int WeaponNumber)
    {
        if(m_targetWeaponChanger == TargetWeaponChanger.Main)
        {

        }
        else if(m_targetWeaponChanger == TargetWeaponChanger.Sub)
        {

        }
    }
}
