using UnityEngine;
using UniRx;

public class MenuHanderBase : MonoBehaviour, IMenuHander
{
    [SerializeField, Tooltip("ボタン選択のインターバル")]
    float _interval = 0.3f;

    [Tooltip("メニュー画面の開閉確認")]
    BoolReactiveProperty _isOpen = new();
    ReactiveProperty<(int, int)> _inputCross = new();
    [Tooltip("決定入力(ReactiveProperty)")]
    BoolReactiveProperty _reactiveIsDiside = new();
    [Tooltip("決定入力(ReactiveProperty)")]
    BoolReactiveProperty _reactiveIsCansel = new();
    [Tooltip("")]
    Interval _canMove = default;

    public IReadOnlyReactiveProperty<bool> IsOpen => _isOpen;
    public IReadOnlyReactiveProperty<(int, int)> InputCross => _inputCross;
    public IReadOnlyReactiveProperty<bool> IsDiside => _reactiveIsDiside;
    public IReadOnlyReactiveProperty<bool> IsCansel => _reactiveIsCansel;

    public void Initialize()
    {
        _canMove = new Interval(_interval);
    }

    public void OnUpdate()
    {
        // メニューのボタン操作
        _isOpen.Value = Input.GetButtonDown("MenuSwitch");
        _reactiveIsDiside.Value = Input.GetButtonDown("Decision"); //決定
        _reactiveIsCansel.Value = Input.GetButtonDown("CanselButton"); //戻る
        (int, int) inputCross = ((int)Input.GetAxisRaw("CrossKeyH"), (int)Input.GetAxisRaw("CrossKeyV"));//(横入力,縦入力)
        if (_canMove.IsCountUp() && (inputCross.Item1 != 0 || inputCross.Item2 != 0))
        {
            _canMove.ResetTimer();
            _inputCross.SetValueAndForceNotify(inputCross);
        }
    }

    void OnDisable()
    {
        _inputCross.Dispose();
        _reactiveIsDiside.Dispose();
    }
}
