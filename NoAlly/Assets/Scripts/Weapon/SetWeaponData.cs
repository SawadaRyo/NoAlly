using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeaponData : SingletonBehaviour<SetWeaponData>
{
    string _filePath = "";
    WeaponTable _weaponsData;
    WeaponBase[] _weapons = null;

    private void Start()
    {
        _weapons = Resources.LoadAll<WeaponBase>(_filePath);
    }
}
