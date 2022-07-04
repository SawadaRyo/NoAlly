using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject[] m_locks = default;
    Material m_doorLight = default;
    Animator m_animator = default;
    int m_remainLock = 0;
    bool m_unLock = false;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_doorLight = transform.GetChild(0).GetComponent<Renderer>().materials[1];
        m_remainLock = m_locks.Length;
        UnLock();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerContoller p))
        {
            DoorState(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerContoller p))
        {
            DoorState(false);
        }
    }

    void DoorState(bool doorLock)
    {
        if (!m_unLock) return;
        m_animator.SetBool("DoorLock", doorLock);
    }

    public void UnLock()
    {
        if(m_remainLock > 0)
        {
            m_remainLock--;
        }

        if(m_remainLock == 0)
        {
            m_unLock = true;
            m_doorLight.SetColor("Emission", Color.green);
        }
    }
}
