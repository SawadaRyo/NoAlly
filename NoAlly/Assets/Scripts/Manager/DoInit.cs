using System;
using UnityEngine;

public class DoInit : MonoBehaviour
{
    [SerializeField]
    InitializerBase[] _initializerBases = null;

    void Awake()
    {
        Array.ForEach(_initializerBases, x => x.Init());
    }
}
