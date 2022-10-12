using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class WeaponTable : ScriptableObject
{
	public List<WeaponDateEntity> WeaponData;// Replace 'EntityType' to an actual type that is serializable.
}
