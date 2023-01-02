using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataInitializer : MonoBehaviour
{
    [SerializeField,Tooltip("武器のスクリプタブルオブジェクト")]
    WeaponScriptableObjects _weaponDatas = null;

    void Awake()
    {
        if(SetWeaponData.Instance.WeaponDatas == null)
        {
            SetWeaponData.Instance.WeaponDatas = _weaponDatas;
            SetWeaponData.Instance.Initialize();
        }
        Destroy(gameObject);
    }
}
