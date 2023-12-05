using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeathblowBase : MonoBehaviour
{
    [SerializeField] protected PlayerStatus _playerStatus = default;
    [SerializeField] protected float _power = 20;
    [SerializeField] protected LayerMask _targetLayer = ~0;
    [SerializeField] float _needSAP = 20;
    protected Animator _playerAnimator = default;
    public virtual void DeathblowAbility() { }
    public virtual void DeathblowMotion() { }

    void Start()
    {
        _playerAnimator = gameObject.GetComponentInParent<Animator>();
    }

    void Update()
    {
        if(Input.GetButtonDown("SpecialAction"))
        {
            Debug.Log("a");
            if(_playerStatus.SAP.Value >= _needSAP)
            {
                _playerAnimator.SetTrigger("DeathBlow");
                _playerStatus.UseSAP(_needSAP);
            }
        }
    }
}
