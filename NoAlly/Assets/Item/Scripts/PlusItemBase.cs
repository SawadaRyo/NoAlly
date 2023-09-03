//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusItemBase : ItemBase, IObjectPool<IObjectGenerator>
{
    [SerializeField] protected float _plusParameter = 4;

    public float PlusParameter => _plusParameter;

    public virtual void Activate(HitParameter gauge) { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HitParameter>(out HitParameter playerGauge) && _isActive)
        {
            //_audio.PlayOneShot(_getSound);
            Activate(playerGauge);
            Disactive();
        }
    }
}
