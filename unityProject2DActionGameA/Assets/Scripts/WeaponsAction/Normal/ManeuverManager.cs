using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManeuverManager : MonoBehaviour
{
    [SerializeField] bool[] _swodeManeuverAnimation = new bool[5];
    [SerializeField] bool[] _bowManeuverAnimation = new bool[5];
    [SerializeField] bool[] _lanceManeuverAnimation = new bool[5];
    int _swodeManeuverNumber = 0;
    int _bowManeuverNumber = 0;
    int _lanceManeuverNumber = 0;
    Animator m_animator = default;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        for(int i = 0;i < 5;i++)
        {
            _swodeManeuverAnimation[i] = false;
            _lanceManeuverAnimation[i] = false;
            _bowManeuverAnimation[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
