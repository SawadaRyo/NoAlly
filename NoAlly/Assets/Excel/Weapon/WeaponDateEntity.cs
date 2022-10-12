using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class WeaponDateEntity
{
    [Tooltip("���햼")]
    public WeaponName Name;
    [Tooltip("����̕����U����")]
    [SerializeField] public float RigitPower;
    [Tooltip("����̗��U����")]
    [SerializeField] public float ElekePower;
    [Tooltip("����̉��U����")]
    [SerializeField] public float FirePower;
    [Tooltip("����̕X���U����")]
    [SerializeField] public float FrozenPower;
}
