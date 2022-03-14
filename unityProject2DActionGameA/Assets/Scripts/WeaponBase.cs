using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] int m_weaponPower = 5;
    public int WeaponPower { get => m_weaponPower; set => m_weaponPower = value; }
}
