using UniRx;

public class DoorTypeSwich : DoorBase
{
    protected override void Initialize()
    {
        base.Initialize();
        IsOpen();
    }

    void IsOpen()
    {
        _locked.Subscribe(open =>
        {
            if (!_inGame) return;
            DoorState(open);
        });
    }
}
