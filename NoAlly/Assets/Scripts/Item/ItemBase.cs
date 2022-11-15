using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(Collider))]
public class ItemBase : MonoBehaviour,IObjectPool
{
    [SerializeField] AudioClip _getSound = null;
    [SerializeField] Renderer[] _renderer = null;

    bool _isActive = true;
    AudioSource _audio = null;

    public bool IsActive => _isActive;

    private void OnEnable()
    {
        _audio = GameObject.FindObjectOfType<AudioSource>();
    }
    public virtual void Activate(Collider other) { }
    public virtual void Activate(Collider other,PlayerStatus gauge) { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerStatus>(out PlayerStatus playerGauge) && _isActive)
        {
            _audio.PlayOneShot(_getSound);
            Activate(other,playerGauge);
            Disactive();
        }
    }

    public void Create()
    {
        _isActive = true;
        foreach (Renderer r in _renderer)
        {
            r.enabled = true;
        }
    }

    public void Disactive()
    {
        _isActive = false;
        foreach(Renderer r in _renderer)
        {
            r.enabled = false;
        }
    }

    public void DisactiveForInstantiate()
    {
        _isActive = false;
        foreach (Renderer r in _renderer)
        {
            r.enabled = false;
        }
    }
}