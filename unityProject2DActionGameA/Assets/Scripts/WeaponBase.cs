using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] int m_weaponPower = 5;
    Collider isAttack;
    public int WeaponPower { get => m_weaponPower; set => m_weaponPower = value; }

    private void Start()
    {
        isAttack = this.GetComponent<Collider>();
    }
    public void IsAttack()
    {
        isAttack.enabled = !isAttack.enabled;
    }
}
