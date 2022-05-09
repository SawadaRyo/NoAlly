using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Image m_manuPanel = default;
    [SerializeField] Button m_fastButton = default;
    bool m_manuSwitch = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ManuHandler()
    {
        if(Input.GetButtonDown("ManuSwitch"))
        {
            //ToDo メニューの開閉にアニメーションを加える
            m_manuSwitch = !m_manuSwitch;
            m_manuPanel.enabled = m_manuSwitch;
            m_fastButton.Select();
        }

        if(m_manuSwitch)
        {

        }
    }
}
