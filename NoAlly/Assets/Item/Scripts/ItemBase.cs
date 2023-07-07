using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
//[RequireComponent(typeof(Collider))]
public class ItemBase : ObjectBase,IObjectPool<IObjectGenerator>
{
    [SerializeField] protected float _plusParameter = 4;
    [SerializeField] AudioClip _getSound = null;

    AudioSource _audio = null;

    public float PlusParameter => _plusParameter;

    public IObjectGenerator Owner => throw new System.NotImplementedException();

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

    public virtual void Create()
    {
        _isActive = true;
        ActiveObject(_isActive);
    }

    public virtual void Disactive()
    {
        _isActive = false;
        ActiveObject(_isActive);
    }
    public virtual async void Disactive(float interval)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(interval));
        _isActive = false;
        ActiveObject(_isActive);
    }

    public void DisactiveForInstantiate(IObjectGenerator Owner)
    {
        throw new System.NotImplementedException();
    }
}