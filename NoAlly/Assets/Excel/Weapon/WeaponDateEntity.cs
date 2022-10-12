using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class WeaponDateEntity
{
    [Tooltip("•Ší–¼")]
    public WeaponName Name;
    [Tooltip("•Ší‚Ì•¨—UŒ‚—Í")]
    [SerializeField] public float RigitPower;
    [Tooltip("•Ší‚Ì—‹UŒ‚—Í")]
    [SerializeField] public float ElekePower;
    [Tooltip("•Ší‚Ì‰ŠUŒ‚—Í")]
    [SerializeField] public float FirePower;
    [Tooltip("•Ší‚Ì•XŒ‹UŒ‚—Í")]
    [SerializeField] public float FrozenPower;
}
