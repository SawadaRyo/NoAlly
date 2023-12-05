using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class DoInit : MonoBehaviour
{
    [SerializeField]
    InitializerBase[] _initializerBases = null;
    [SerializeField]
    PanelFade _panelFade = null;

    void Awake()
    {
        _panelFade.ImageFade(FadeType.FadeIn);
        //await UniTask.Delay(TimeSpan.FromSeconds(_panelFade.Interval));
        Array.ForEach(_initializerBases, x => x.Init());
    }
}
