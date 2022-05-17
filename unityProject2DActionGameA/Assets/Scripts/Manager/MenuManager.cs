using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] m_menuPanels = new GameObject[2];
    [SerializeField] GameObject[] m_GaugeUI = new GameObject[2];
    [SerializeField] Button m_fastButton = default;
    bool m_menuSwitch = false;

    

    // Update is called once per frame
    void Update()
    {
        ManuHandler();
    }
    void ManuHandler()
    {
        if(Input.GetButtonDown("MenuSwitch"))
        {
            //ToDo メニューの開閉にアニメーションを加える
            m_menuSwitch = !m_menuSwitch;
            m_menuPanels[0].SetActive(m_menuSwitch);
            m_menuPanels[1].SetActive(m_menuSwitch);
            m_GaugeUI[0].SetActive(!m_menuSwitch);
            m_GaugeUI[1].SetActive(!m_menuSwitch);
            if(m_menuSwitch)
            {
                m_fastButton.Select();
            }
        }

    }
}
