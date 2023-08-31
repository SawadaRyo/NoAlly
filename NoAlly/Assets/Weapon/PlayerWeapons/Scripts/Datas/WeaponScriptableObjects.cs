using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatas", menuName = "ScriptableObjects/WeaponDatasObject", order = 1)]
public class WeaponScriptableObjects : ScriptableObject
{
    [SerializeField, Tooltip("�e����̃f�[�^")]
    WeaponDataEntity[] _weaponDatas;

    public WeaponDataEntity[] WeaponDatas => _weaponDatas;
}

[System.Serializable]
public class WeaponDataEntity
{
    [SerializeField, Header("�g�p����WeaponBase�̖��O")]
    WeaponClassName _weaponBaseName = WeaponClassName.None;
    [SerializeField, Header("����̃^�C�v")]
    WeaponType _type = WeaponType.NONE;
    [SerializeField, Header("����̕����U����")]
    float[] _rigitPower = new float[2];
    [SerializeField, Header("����̉��U����")]
    float[] _firePower = new float[2];
    [SerializeField, Header("����̗��U����")]
    float[] _elekePower = new float[2];
    [SerializeField, Header("����̕X���U����")]
    float[] _frozenPower = new float[2];
    [SerializeField, Header("���ߍU���̂��ߎ���")]
    float[] _chargeLevels = new float[2] { 1f, 3f };
    [SerializeField, Header("���ߍU���̍U���{��")]
    float[] _chargePowerLevels = new float[2] { 1.2f, 1.5f };

    [SerializeField, Range(0.1f, 1f), Header("�`���[�W���̈ړ����x�̔{��")]
    float _magnificationInCharge = 1f;

    public WeaponClassName WeaponBaseName => _weaponBaseName;
    public WeaponType TypeOfWeapon => _type;
    public float[] RigitPower => _rigitPower;
    public float[] FirePower => _firePower;
    public float[] ElekePower => _elekePower;
    public float[] FrozenPower => _frozenPower;
    public float[] ChargeLevels => _chargeLevels;
    public float[] ChargePowerLevels => _chargePowerLevels;
    public float SpeedInCharge => _magnificationInCharge;
}

public enum WeaponClassName
{
    None,
    WeaponCombat,
    WeaponArrow,
    WeaponShield
}

[System.Serializable]
public struct WeaponPower
{
    public float defaultPower;
    public float elementPower;

    static readonly WeaponPower zeroPower = new(0, 0);

    public static WeaponPower zero => zeroPower;

    public WeaponPower(float defaultPower, float elementPower)
    {
        this.defaultPower = defaultPower;
        this.elementPower = elementPower;
    }
}
