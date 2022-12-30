using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataObject", menuName = "ScriptableObject/WeaponDatas", order = 1)]
public class WeaponScriptableObjects : ScriptableObject
{
    [SerializeField,Tooltip("各武器のデータ")]
    Weapons _weaponDatas;

    public Weapons WeaponDatas => _weaponDatas;
}
