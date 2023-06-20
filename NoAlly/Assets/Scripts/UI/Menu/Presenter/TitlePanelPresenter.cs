//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanelPresenter : MonoBehaviour
{
    [SerializeField]
    MenuHanderBase _menuBase = null;
    [SerializeField]
    //CommandButton[] _commands = null;

    public void Initializer()
    {
        _menuBase.Initialize();
    }
}
