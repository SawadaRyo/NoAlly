using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuHander : SingletonBehaviour<MenuHander>
{
    [SerializeField] float m_interval = 1f;

    Image[] m_gameUIGauges = default;
    GameObject[] m_gamePanels = new GameObject[2];

    bool m_menuIsOpen = false;
    bool m_safeZorn = false;
    float m_crossH = 0;
    float m_crossV = 0;
    GameObject m_canvas = default;
    Button m_targetButton = default;
    Button[,] m_allButtons = default;
    Interval m_canMove = default;
    public bool SafeZorn { get => m_safeZorn; set => m_safeZorn = value; }
    public bool MenuIsOpen => m_menuIsOpen;

    void Start()
    {
        m_canMove = new Interval(m_interval);

        //Canvas上の全てのButtonを取得する。
        m_canvas = GameObject.FindGameObjectWithTag("Canvas");
        m_allButtons = new Button[Enum.GetNames(typeof(EquipmentType)).Length, Enum.GetNames(typeof(ElementType)).Length];
        int length = 0;
        Button[] buttonArray = GetComponentsInChildren<Button>(true);

        for (int y = 0; y < m_allButtons.GetLength(0); y++)
        {
            for (int x = 0; x < m_allButtons.GetLength(1); x++)
            {
                m_allButtons[y, x] = buttonArray[length];
                length++;
            }
        }

        //UIのゲーム起動時の初期設定
        //ToDo マジックナンバーを直す。
        for (int i = 0; i < 2; i++)
        {
            m_gamePanels[i] = m_canvas.transform.GetChild(i).gameObject;
            m_gamePanels[i].SetActive(false);
        }

        m_gameUIGauges = m_canvas.transform.GetChild(2).GetComponentsInChildren<Image>();
        foreach (Image image in m_gameUIGauges)
        {
            image.enabled = true;
        }
    }


    void Update()
    {
        if (Input.GetButtonDown("MenuSwitch"))
        {
            m_menuIsOpen = !m_menuIsOpen;
            IsManuExpand(m_menuIsOpen);
        }

        if (m_menuIsOpen)
        {
            float h = Input.GetAxisRaw("CrossKeyH");
            float v = Input.GetAxisRaw("CrossKeyV");

            Debug.Log(m_canMove.IsCountUp());
            if (m_canMove.IsCountUp())
            {
                m_targetButton = SelectButton(h, v);
                if(m_targetButton != null)
                {
                    m_targetButton.Select();
                }
            }
            Debug.Log($"{m_crossH},{m_crossV}");
            if (Input.GetButtonDown("Decision"))
            {
                m_targetButton.onClick.Invoke();
            }
        }
    }
    /// <summary>メニュー画面の展開</summary>
    /// <param name="isOpen"></param>
    void IsManuExpand(bool isOpen)
    {
        //ToDo メニューの開閉にアニメーションを加える
        m_gamePanels[0].SetActive(isOpen);
        if (m_safeZorn)
        {
            m_gamePanels[1].SetActive(isOpen);
        }
        foreach (Image image in m_gameUIGauges)
        {
            image.enabled = !isOpen;
        }

        if (isOpen)
        {
            MenuOpen();
        }
        else
        {
            MenuClose();
        }
    }

    Button SelectButton(float h, float v)
    {
        m_canMove.ResetTimer();
        if (h > 0)
        {
            m_crossH++;
            if (m_crossH > m_allButtons.GetLength(1)) m_crossH = 0;
        }
        if(v > 0)
        {
            m_crossV--;
            if (m_crossV < 0) m_crossV = m_allButtons.GetLength(0) - 1;
        }
        if(h < 0)
        {
            m_crossH--;
            if (m_crossH < 0) m_crossH = m_allButtons.GetLength(1) - 1;
        }
        if(v < 0)
        {
            m_crossV++;
            if (m_crossV > m_allButtons.GetLength(0)) m_crossV = 0;
        }

        return m_allButtons[(int)m_crossV, (int)m_crossH];
        
    }

    /// <summary>メニュー画面展開時に呼ぶ関数 </summary>
    void MenuOpen()
    {
        m_allButtons[0, 0].Select();
    }

    /// <summary>メニュー画面縮小時に呼ぶ関数</summary>
    void MenuClose()
    {

    }
}
