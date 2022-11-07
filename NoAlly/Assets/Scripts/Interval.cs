using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interval
{
    float _interval = 1f;
    float _time = 0;
    public Interval(float interval)
    {
        _interval = interval;
    }

    public bool IsCountUp()
    {
        _time += Time.deltaTime;
        if(_time < _interval)
        {
            return false;
        }
        return true;
    }

    public void ResetTimer()
    {
        _time = 0;
    }
}
