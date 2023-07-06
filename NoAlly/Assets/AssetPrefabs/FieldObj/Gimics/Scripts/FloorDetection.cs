using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetection : MonoBehaviour
{
    [SerializeField] Collider2D _cameraCollider = null;
    [SerializeField] StageSetPos _stage = StageSetPos.Flont;

    bool _active;
    int _floorNumber = 0;
    Renderer[] _floorObjectRenderers = null;
    SectionDetectionManager _sectionDetectionManager = null;

    public bool Active => _active;
    public int FloorNumber => _floorNumber;
    public Collider2D CameraCollider => _cameraCollider;

    public void Init(int number,SectionDetectionManager sectionDetection)
    {
        _floorNumber = number;
        _floorObjectRenderers = GetComponentsInChildren<Renderer>();
        _sectionDetectionManager = sectionDetection;
    }

    public void FloorActive(bool isActive)
    {
        foreach (Renderer r in _floorObjectRenderers)
        {
            r.enabled = isActive;
        }
        _cameraCollider.enabled = isActive;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerContoller>() != null)
        {
            _active = true;
            _sectionDetectionManager.ConfinerColliderChanger(this, _stage);
            _sectionDetectionManager.SectionDetecter(this);
        }
    }
}
public enum StageSetPos
{
    Flont,
    Back
}

