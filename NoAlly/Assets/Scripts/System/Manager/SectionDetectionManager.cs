using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

//プレイヤーの現在のステージを検知するコンポーネント
public class SectionDetectionManager : MonoBehaviour
{
    [SerializeField] GameObject _sectionPointsPairent = null;
    [SerializeField] CinemachineVirtualCamera _playerCamera = null;

    FloorDetection[] _sectionPoints = null;
    CinemachineConfiner _confiner = null;
    StageSetPos _setPos = default;

    public StageSetPos SetPos => _setPos;

    void Start()
    {
        _confiner = _playerCamera.GetComponent<CinemachineConfiner>();

        _sectionPoints = _sectionPointsPairent.GetComponentsInChildren<FloorDetection>();
        for (int i = 0; i < _sectionPoints.Length; i++)
        {
            _sectionPoints[i].Init(i, this);
        }
    }
    public void SectionDetecter(FloorDetection floor)
    {
        int activeFloorNum = floor.FloorNumber;
        for (int i = activeFloorNum - 1; i <= activeFloorNum + 1; i++)
        {

        }

    }
    public void ConfinerColliderChanger(FloorDetection floor,StageSetPos stageSetPos)
    {
        _confiner.m_BoundingShape2D = floor.CameraCollider;
        switch(stageSetPos)
        {
            case StageSetPos.Flont:
                _playerCamera.transform.rotation = Quaternion.Euler(_playerCamera.transform.rotation.x, 0f, _playerCamera.transform.rotation.z);
                break;
            case StageSetPos.Back:
                _playerCamera.transform.rotation = Quaternion.Euler(_playerCamera.transform.rotation.x, 180f, _playerCamera.transform.rotation.z);
                break;
        }
        _setPos = stageSetPos;
    }
}
