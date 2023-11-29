//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class ShieldWall : ObjectBase
{
    [SerializeField, Header("耐久値の上限")]
    int _maxDurableCount = 3;
    [SerializeField, Header("フェードに掛かる時間")]
    float _interval = 1f;
    [SerializeField, Header("フェード")]
    ReactiveProperty<FadeType> _type = new();

    [Tooltip("")]
    bool _inFading = false;
    [Tooltip("耐久値")]
    IntReactiveProperty _durableCount = new();

    void Start()
    {
        //IncreaseDurableCount();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_durableCount.Value > 0)
        {
            HitObject(other);
        }
    }

    void HitObject(Collider other)
    {
        //if(other.TryGetComponent(out IBullet<> bullet))
        {
            //bullet.Destroy();
        }
        _durableCount.Value--;
    }

    void IncreaseDurableCount()
    {
        Observable.EveryFixedUpdate()
            .Where(_ => _durableCount.Value < _maxDurableCount)
            .Subscribe(_ =>
            {

            }).AddTo(this);
    }

    public void TestActive(int i)
    {
        WallActive((FadeType)i);
    }

    public void WallActive(FadeType type)
    {
        if (_inFading) return;

        var seq = DOTween.Sequence();
        _inFading = true;
        foreach (var fadeTarget in _objectRenderer)
        {
            switch (type)
            {
                case FadeType.FadeOut:
                    seq.Append(fadeTarget.material.DOFade(0.5f, _interval / _objectRenderer.Length).OnStart(() => fadeTarget.enabled = true));
                    break;
                case FadeType.FadeIn:
                    seq.Append(fadeTarget.material.DOFade(0f, _interval / _objectRenderer.Length).OnComplete(() => fadeTarget.enabled = false));
                    break;
            }
        }

        switch (type)
        {
            case FadeType.FadeIn: _isActive = false; break;
            case FadeType.FadeOut: _isActive = true; break;
        }
        seq.Play().OnComplete(() => _inFading = false);
    }
    private void OnDisable()
    {
        _type.Dispose();
    }
}
