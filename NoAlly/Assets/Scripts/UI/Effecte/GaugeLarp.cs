using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GaugeLarp : MonoBehaviour
{
    [SerializeField]
    Slider _slider = null;

    public void SetSliderValue(float value)
    {
        DOTween.To(() => _slider.value,
                 n => _slider.value = n,
                 value,
                 duration: 1.0f);
    }
}


