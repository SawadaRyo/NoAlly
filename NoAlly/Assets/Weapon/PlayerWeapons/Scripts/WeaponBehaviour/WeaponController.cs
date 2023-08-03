using UnityEngine;
using UniRx;
using DataOfWeapon;

public class WeaponController : MonoBehaviour,IWeaponController
{
    [SerializeField, Header("")]
    SetWeaponData _setWeaponData = null;
    [SerializeField, Header("WeaponScriptableObjects本体")]
    WeaponScriptableObjects _weaponScriptableObjects;
    [SerializeField,Header("武器のプレハブ本体")]
    ObjectBase _weaponPrefab;
    [SerializeField,Header("攻撃の中心点")]
    Transform _attackPos = null;
    [SerializeField]
    Transform _poolPos = null;
    [SerializeField, Header("武器の斬撃エフェクト")]
    ParticleSystem _myParticleSystem = default;
    [SerializeField, Header("判定するオブジェクトのレイヤー")]
    LayerMask hitLayer = ~0;

    [Tooltip("")]
    ReactiveProperty<WeaponBase> _equipementWeapon = new();
    [Tooltip("利用する武器")]
    WeaponBase _mainWeapon;
    [Tooltip("サブ配置")]
    WeaponBase _subWeapon;
    [Tooltip("現在の属性")]
    ReactiveProperty<ElementType> _currentElement = new();

    public Transform GetAttackPos => _attackPos;
    public Transform GetPoolPos => _poolPos;
    public ObjectBase WeaponPrefab => _weaponPrefab; 
    public ParticleSystem MyParticle => _myParticleSystem;
    public LayerMask HitLayer => hitLayer;
    public IReadOnlyReactiveProperty<WeaponBase> EquipementWeapon => _equipementWeapon;
    public IReadOnlyReactiveProperty<ElementType> CurrentElement => _currentElement;

    /// <summary>
    /// 初期化関数
    /// </summary>
    /// <param name="weaponProcessing"></param>
    /// <param name="main"></param>
    /// <param name="sub"></param>
    public void Initializer(WeaponType main = WeaponType.SWORD, WeaponType sub = WeaponType.LANCE)
    {
        _setWeaponData.WeaponBaseInstantiate(this, _weaponScriptableObjects);
        _mainWeapon = _setWeaponData.WeaponDatas[main];
        _subWeapon = _setWeaponData.WeaponDatas[sub];
        _equipementWeapon.Value = _mainWeapon;
        _myParticleSystem.Stop();
    }
    /// <summary>
    /// メイン武器・サブ武器の装備をボタンで切り替える関数
    /// </summary>
    public void SwichEquipmentWeapon(bool weaponSwitch)
    {
        if (!weaponSwitch)
        {
            _subWeapon.OnLift();
            _equipementWeapon.Value = _mainWeapon;
        }
        else
        {
            _mainWeapon.OnLift();
            _equipementWeapon.Value = _subWeapon;
        }
        _equipementWeapon.Value.OnEquipment();
    }
    /// <summary>
    /// メイン武器・サブ武器の装備を変更する関数
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="type"></param>
    public void SetEquipmentWeapon(WeaponType typeOfWeapon, CommandType type)
    {
        //_weaponPrefabs[(int)_mainWeapon.Type].ActiveObject(false);
        //_weaponPrefabs[(int)_subWeapon.Type].ActiveObject(false);
        switch (type)
        {
            case CommandType.MAIN:
                _mainWeapon.OnLift();
                _mainWeapon = _setWeaponData.WeaponDatas[typeOfWeapon];
                _mainWeapon.OnEquipment();
                break;
            case CommandType.SUB:
                _subWeapon.OnLift();
                _subWeapon = _setWeaponData.WeaponDatas[typeOfWeapon];
                _subWeapon.OnEquipment();
                break;
        }
        //playerAnimator.SetInteger("WeaponType", (int)_equipementWeapon.WeaponData.TypeOfWeapon);
    }
    /// <summary>
    /// 属性の装備
    /// </summary>
    /// <param name="elementType"></param>
    public void SetElement(ElementType elementType)
    {
        _equipementWeapon.Value.WeaponModeToElement(elementType);
        _currentElement.Value = elementType;
    }

    private void OnDisable()
    {
        _equipementWeapon.Dispose();
        _currentElement.Dispose();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // 攻撃範囲を赤い線でシーンビューに表示する
        Gizmos.color = Color.red;
        var oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(_attackPos.position, _attackPos.rotation, _attackPos.lossyScale);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 1.1f);
        Gizmos.matrix = oldMatrix;
    }
#endif
}
