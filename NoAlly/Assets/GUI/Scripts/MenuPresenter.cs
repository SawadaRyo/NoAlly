using UnityEngine;
using UniRx;

public class MenuPresenter : MonoBehaviour
{
    [SerializeField, Tooltip("")]
    MenuHanderBase _menuHander;

    [SerializeField, Tooltip("")]
    MenuManagerBase _menuManager;


    void Start()
    {
        _menuHander.Initialize();
        _menuManager.Initialize();
        ObjectSelect();
        UpdateMenu();
    }

    void UpdateMenu()
    {
        Observable.EveryUpdate()
            .Subscribe(_ => _menuHander.OnUpdate()).AddTo(this);
    }

    void ObjectSelect()
    {
        _menuHander.IsOpen.Skip(1)
            .Where(isMenuOpen => isMenuOpen)
            .Subscribe(isMenuOpen =>
            {
                _menuManager.IsMenuOpen();
            }).AddTo(this);
        _menuHander.InputCross.Skip(1)
            .Subscribe(inputCross =>
            {
                _menuManager.SelectTargetButton(inputCross.Item1, inputCross.Item2);
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
