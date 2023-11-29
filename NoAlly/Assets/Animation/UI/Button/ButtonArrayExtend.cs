using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class ButtonArrayExtend : MonoBehaviour
{
    [SerializeField, Tooltip("")]
    GridLayoutGroup _gridLayout;
    [SerializeField, Tooltip("")]
    float _tweenTime = 0.2f;
    [SerializeField, Tooltip("")]
    Vecter _tweenVecter = Vecter.None;

    [Tooltip("開閉の値(要素0が閉、要素1が開)")]
    Tween _extendTween = null; 
    [Tooltip("")]
    Vector2[] _tweenValues = new Vector2[2];

    void Start()
    {
        switch (_tweenVecter)
        {
            case Vecter.Horizontal:
                _tweenValues[0] = new Vector2(-_gridLayout.cellSize.x, _gridLayout.spacing.y);
                break;
            case Vecter.Vertical:
                _tweenValues[0] = new Vector2(_gridLayout.spacing.x, -_gridLayout.cellSize.y);
                break;
            default:
                break;
        }
        _tweenValues[1] = _gridLayout.spacing;
        _gridLayout.spacing = _tweenValues[0];
    }

    public async UniTask<bool> ExtendsButton(bool isExtend)
    {
        if (isExtend)
        {
            _extendTween = DOTween.To(() => _gridLayout.spacing,
            n => _gridLayout.spacing = n,
            _tweenValues[1],
            duration: _tweenTime);
        }
        else
        {
            _extendTween = DOTween.To(() => _gridLayout.spacing,
             n => _gridLayout.spacing = n,
             _tweenValues[0],
             duration: _tweenTime);
        }
        await UniTask.Delay(System.TimeSpan.FromSeconds(_tweenTime));
        return true;
    }

    enum Vecter
    {
        None,
        Horizontal,
        Vertical
    }
}


