using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class ManeuverTable : ScriptableObject
{
	public List<ManeuverDateEntity> Maneuvers; // Replace 'EntityType' to an actual type that is serializable.
}
