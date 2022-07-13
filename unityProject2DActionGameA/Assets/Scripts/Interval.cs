using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interval
{
    float m_interval = 1f;
    float m_time = 0;
    public Interval(float interval)
    {
        m_interval = interval;
    }

    public bool IsCountUp()
    {
        m_time += Time.deltaTime;
        if(m_time > m_interval)
        {
            return false;
        }
        return true;
    }

    public void ResetTimer()
    {
        m_time = 0;
    }
}
