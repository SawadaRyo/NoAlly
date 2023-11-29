using UnityEngine;
using UniRx;

public class MenuHanderBase : MonoBehaviour, IMenuHander
{
    [SerializeField, Tooltip("�{�^���I���̃C���^�[�o��")]
    float _interval = 0.3f;

    [Tooltip("���j���[��ʂ̊J�m�F")]
    BoolReactiveProperty _isOpen = new();
    ReactiveProperty<(int, int)> _inputCross = new();
    [Tooltip("�������(ReactiveProperty)")]
    BoolReactiveProperty _reactiveIsDiside = new();
    [Tooltip("�������(ReactiveProperty)")]
    BoolReactiveProperty _reactiveIsCansel = new();
    [Tooltip("")]
    Interval _canMove = default;
    [Tooltip("")]
    IntReactiveProperty _inputTrigger = new();

    public IReadOnlyReactiveProperty<bool> IsOpen => _isOpen;
    public IReadOnlyReactiveProperty<(int, int)> InputCross => _inputCross;
    public IReadOnlyReactiveProperty<bool> IsDiside => _reactiveIsDiside;
    public IReadOnlyReactiveProperty<bool> IsCansel => _reactiveIsCansel;
    public IReadOnlyReactiveProperty<int> CurrentTrigger => _inputTrigger;

    public void Initialize()
    {
        _canMove = new Interval(_interval);
    }

    public void OnUpdate()
    {
        // ���j���[�̃{�^������
        if (Input.GetButtonDown("MenuSwitch"))
        {
            _isOpen.Value = !_isOpen.Value;
        }
        _reactiveIsDiside.Value = Input.GetButtonDown("Decision"); //����
        _reactiveIsCansel.Value = Input.GetButtonDown("CanselButton"); //�߂�
        _inputTrigger.Value = InputTrigger();
        (int, int) inputCross = ((int)Input.GetAxisRaw("CrossKeyH"), (int)Input.GetAxisRaw("CrossKeyV"));//(������,�c����)
        if (_canMove.IsCountUp() && (inputCross.Item1 != 0 || inputCross.Item2 != 0))
        {
            _canMove.ResetTimer();
            _inputCross.SetValueAndForceNotify(inputCross);
        }
    }

    public void OnUpdateMouse()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            string objectName = hit.transform.name;
            Debug.Log(objectName);
        }
    }
    public int InputTrigger()
    {

        if (Input.GetButtonDown("RightTrigger") && Input.GetButtonDown("LeftTrigger"))
        {
            return 0;
        }
        else if(Input.GetButtonDown("RightTrigger"))
        {
            return 1;
        }
        else if(Input.GetButtonDown("LeftTrigger"))
        {
            return -1;
        }
        return 0;
    }

    void OnDisable()
    {
        _inputCross.Dispose();
        _reactiveIsDiside.Dispose();
    }
}