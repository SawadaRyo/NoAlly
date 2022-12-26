using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataInitializer : MonoBehaviour
{
    [SerializeField,Tooltip("����̃X�N���v�^�u���I�u�W�F�N�g")]
    WeaponScriptableObjects _weaponDatas = null;

    private void Awake()
    {
        if(SetWeaponData.Instance.WeaponDatas == null)
        {
            SetWeaponData.Instance.WeaponDatas = _weaponDatas;
        }
        Destroy(gameObject);
    }
}
