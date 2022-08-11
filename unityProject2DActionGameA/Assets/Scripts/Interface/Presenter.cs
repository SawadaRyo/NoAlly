using UnityEngine;
using UniRx;

public class Presenter : MonoBehaviour
{
    [SerializeField] WeaponEquipment _weaponEquipment = WeaponEquipment.Instance;
    [SerializeField] MainMenu _mainMenu = MainMenu.Instance;

    void Start()
    {
        
    }
}
