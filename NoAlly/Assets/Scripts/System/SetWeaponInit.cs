//日本語コメント可
using UnityEngine;
using DataOfWeapon;

public class SetWeaponInit : MonoBehaviour
{
    [SerializeField]
    WeaponScriptableObjects weaponScriptableObjects;
    [SerializeField]
    PlayerBehaviorController playerCon;
    [SerializeField]
    WeaponController weaponCon;

    void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        var instance = new SetWeaponData(weaponScriptableObjects, playerCon, weaponCon);
        SetWeaponData.Instance = instance;
        Destroy(this);
    }
}
