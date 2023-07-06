//日本語コメント可
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelFade : MonoBehaviour
{
    [SerializeField] Image _fadeTarget;
    [SerializeField] float _interval = 0.5f;

    public float Interval => _interval;

    public void ImageFade(FadeType type)
    {
        switch (type)
        {
            case FadeType.FadeIn:
                DOTween.To(() => _fadeTarget.color,
                 n => _fadeTarget.color = n,
                 new Color(_fadeTarget.color.r, _fadeTarget.color.g, _fadeTarget.color.b, 0f),
                 duration: _interval);
                break;
            case FadeType.FadeOut:
                DOTween.To(() => _fadeTarget.color,
                 n => _fadeTarget.color = n,
                 new Color(_fadeTarget.color.r, _fadeTarget.color.g, _fadeTarget.color.b, 1f),
                 duration: _interval);
                break;
        }
    }
    public enum FadeType
    {
        FadeIn,
        FadeOut
    }
}
