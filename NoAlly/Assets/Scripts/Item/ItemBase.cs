using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(Collider))]
public class ItemBase : ObjectBase
{
    [SerializeField] protected float _plusParameter = 4;
    [SerializeField] AudioClip _getSound = null;

    bool _isActive = true;
    AudioSource _audio = null;

    public float PlusParameter => _plusParameter;

    private void OnEnable()
    {
        _audio = GameObject.FindObjectOfType<AudioSource>();
    }
    public virtual void Activate() { }
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