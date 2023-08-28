//日本語コメント可
using DG.Tweening;
using UnityEngine;

public class DamageEffects : MonoBehaviour
{
    [SerializeField]
    Renderer[] targetRenderer;
    [SerializeField]
    Color _damageColor = new Color();
    [SerializeField]
    int _loopCount = 2;

    public void Damaged(float interval,ElementType type)
    {
        var seq = DOTween.Sequence();
        foreach (var renderer in targetRenderer)
        {
            Color defaultColor = renderer.material.color;
            renderer.material.DOColor(_damageColor, interval/_loopCount)
                             .OnComplete(() => renderer.material.color = defaultColor)
                             .SetEase(Ease.InCubic)
                             .SetLoops(_loopCount,LoopType.Yoyo);
        }
    }
    public void Death(float interval)
    {
        foreach (var renderer in targetRenderer)
        {
            Color defaultColor = renderer.material.color;
            renderer.material.DOFade(0f, interval * 10)
                             .OnComplete(() => renderer.enabled = false);
        }
    }
}
