using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINormalized : MonoBehaviour
{
    [SerializeField] Transform _targetTfm;

    RectTransform myRectTfm;
    Vector3 offset = new Vector3(0, 1.5f, 0);

    void Start()
    {
        myRectTfm = GetComponent<RectTransform>();
    }

    void Update()
    {
        myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _targetTfm.position + offset);
    }
}
