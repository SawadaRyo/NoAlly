//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanelPresenter : MonoBehaviour
{
    [SerializeField]
    MenuHanderBase _menuBase = null;
    [SerializeField]
    MenuManagerBase _menuManagerBase = null;

    public void Initializer()
    {
        _menuBase.Initialize();
        _menuManagerBase.Initialize();
    }
}
