using UnityEngine;
using UniRx;

public class MenuPresenter : MonoBehaviour
{
    [SerializeField, Tooltip("")]
    MenuHanderBase _menuHander;
    [SerializeField, Tooltip("")]
    MenuManagerBase _menuManager;
    [SerializeField]
    UIObjectBase _manuEnabledInvertedUI;
    void Awake()
    {
        _menuHander.Initialize();
        _menuManager.Initialize();
        ObjectSelect();
        UpdateMenu();
    }

    void UpdateMenu()
    {
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                _menuHander.OnUpdate();
                //_menuHander.OnUpdateMouse();
            }).AddTo(this);
    }

    void ObjectSelect()
    {
        _menuHander.IsOpen.Skip(1)
            .Subscribe(isMenuOpen =>
            {
                _menuManager.IsMenuOpen();
                _manuEnabledInvertedUI.ActiveUIObject(!_menuManager.IsActive);
            }).AddTo(this);
        _menuHander.InputCross.Skip(1)
            .Subscribe(inputCross =>
            {
                _menuManager.SelectTargetButton(inputCross.Item1, inputCross.Item2);
            }).AddTo(this);
        _menuHander.CurrentTrigger.Skip(1)
            .Subscribe(currentTrigger =>
            {
                _menuManager.SwitchPanal(currentTrigger);
            }).AddTo(this);
        _menuHander.IsDiside.Skip(1)
            .Subscribe(isDiside =>
            {
                if (!isDiside) return;
                _menuManager.OnDisaide();
            }).AddTo(this);
        _menuHander.IsCansel.Skip(1)
            .Subscribe(isCansel =>
            {
                if (!isCansel) return;
                _menuManager.OnCansel();
            }).AddTo(this);
    }
}
