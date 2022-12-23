using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponDatas", order = 1)]
public class WeaponDataBase : ScriptableObject
{
    [SerializeField,Tooltip("����̃v���n�u")]
    Weapons _weaponPrefab;

    public Weapons WeaponPrefab => _weaponPrefab;
}
