using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPrefab:MonoBehaviour
{
    [SerializeField,Tooltip("����̎��")]
    WeaponType _type;

    public WeaponDateEntity dateEntity => SetWeaponData.Instance.GetWeapon(_type); 
}
