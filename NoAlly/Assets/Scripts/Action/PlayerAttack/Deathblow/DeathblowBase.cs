using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeathblowBase : MonoBehaviour
{
    [SerializeField] protected PlayerGauge _playerHP = default;
    [SerializeField] protected float _power = 20;
    [SerializeField] protected LayerMask _targetLayer = ~0;
    [SerializeField] float _needSAP = 20;
    protected Animator _playerAnimator = default;
    public abstract void DeathblowAbility();

    void Start()
    {
        _playerAnimator = gameObject.GetComponentInParent<Animator>();
    }

    void Update()
    {
        if(Input.GetButtonDown("SpecialAction"))
        {
            Debug.Log("a");
            if(_playerHP.SAP.Value >= _needSAP)
            {
                _playerAnimator.SetTrigger("DeathBlow");
                _playerHP.UseSAP(_needSAP);
            }
        }
    }
}
