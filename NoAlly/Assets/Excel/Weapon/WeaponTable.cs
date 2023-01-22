using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class WeaponTable : ScriptableObject
{
	public List<WeaponDataEntity> WeaponData;
	public List<WeaponDataEntity> ElementWeaponData;
}
