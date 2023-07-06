using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : UIObjectBase
{
    [SerializeField, Header("Ø‚è‘Ö‚í‚é‰æ‘œ")]
    protected Image _switchImage = null;

    public Sprite SwitchImage => _switchImage.sprite;
}


