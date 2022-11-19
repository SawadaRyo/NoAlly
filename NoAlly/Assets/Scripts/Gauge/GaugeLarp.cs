using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GaugeLarp : MonoBehaviour
{
    public void SetSliderValue(Slider slider,float value)
    {
        DOTween.To(() => slider.value,
                 n => slider.value = n,
                 value,
                 duration: 1.0f);
    }
}


