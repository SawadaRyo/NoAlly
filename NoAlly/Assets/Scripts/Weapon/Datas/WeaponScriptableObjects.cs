using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponDatas", order = 1)]
public class WeaponScriptableObjects : ScriptableObject
{
    [SerializeField,Tooltip("•Ší‚ÌƒvƒŒƒnƒu")]
    Weapons _weaponPrefab;

    public Weapons WeaponPrefab => _weaponPrefab;
}
