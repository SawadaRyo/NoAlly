using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamaraController : MonoBehaviour
{
    [SerializeField] GameObject _sectionPointsPairent = null;
    [SerializeField] CinemachineConfiner _confiner = null;
    [SerializeField] SectionDetectionManager _sectionDetectionManager = null;

    FloorDetection[] _sectionPoints = null;

    void Start()
    {
        _sectionPoints = _sectionPointsPairent.GetComponentsInChildren<FloorDetection>();
        for (int i = 0; i < _sectionPoints.Length; i++)
        {
            _sectionPoints[i].Init(i, _sectionDetectionManager);
        }
    }
    public void ConfinerColliderChanger(FloorDetection floor)
    {
        _confiner.m_BoundingShape2D = floor.CameraCollider;
    }
}
