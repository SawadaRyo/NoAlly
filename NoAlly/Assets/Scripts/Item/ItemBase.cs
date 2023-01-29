using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(Collider))]
public class ItemBase : ObjectVisual,IObjectPool
{
    [SerializeField] protected float _plusParameter = 4;
    [SerializeField] AudioClip _getSound = null;

    bool _isActive = true;
    AudioSource _audio = null;
    HitOwner _hitOwner = HitOwner.Item;

    public bool IsActive => _isActive;
    public float PlusParameter => _plusParameter;

    private void OnEnable()
    {
        _audio = GameObject.FindObjectOfType<AudioSource>();
    }
    public virtual void Activate() { }
    public virtual void Activate(HItParameter gauge) { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HItParameter>(out HItParameter playerGauge) && _isActive)
        {
            //_audio.PlayOneShot(_getSound);
            Activate(playerGauge);
            Disactive();
        }
    }

    public void Create()
    {
        _isActive = true;
        ActiveObject(_isActive);
    }

    public void Disactive()
    {
        _isActive = false;
        ActiveObject(_isActive);
    }

    public void DisactiveForInstantiate<T>(T owner) where T : IObjectGenerator
    {
        _isActive = false;
        ActiveObject(_isActive);
    }
}