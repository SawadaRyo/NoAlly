using System;
using UnityEngine;

public class DoInit : MonoBehaviour
{
    [SerializeField]
    InitializerBase[] _initializerBases = null;
    [SerializeField]
    PanelFade _panelFade = null;

    void Awake()
    {
        _panelFade.ImageFade(FadeType.FadeIn);
        Array.ForEach(_initializerBases, x => x.Init());
    }
}
