using UnityEngine;
using UniRx;
using DataOfWeapon;


public class WeaponController : MonoBehaviour, IWeaponController
{
    [SerializeField, Header("武器のプレハブ本体")]
    ObjectBase _weaponPrefab;
    [SerializeField, Header("攻撃の中心点")]
    Transform _attackPos = null;
    [SerializeField]
    Transform _poolPos = null;
    [SerializeField, Header("武器の斬撃エフェクト")]
    ParticleSystem _myParticleSystem = default;
    [SerializeField, Header("攻撃判定するオブジェクトのレイヤー")]
    LayerMask hitLayerToAttack = ~0;
    [SerializeField, Header("防御判定するオブジェクトのレイヤー")]
    LayerMask hitLayerToBlock = ~0;


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
    public WeaponBase MainWeapon => _mainWeapon;
    public WeaponBase SubWeapon => _subWeapon;
    public ParticleSystem MyParticle => _myParticleSystem;
    public LayerMask HitLayerToAttack => hitLayerToAttack;
    public LayerMask HitLayerToBlock => hitLayerToBlock;
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
        _mainWeapon = SetWeaponData.Instance.WeaponDatas[main];
        _subWeapon = SetWeaponData.Instance.WeaponDatas[sub];
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
    /// <param name="weaponType"></param>
    /// <param name="type"></param>
    public void SetEquipmentWeapon(WeaponType weaponType, EquipmentType type)
    {
        //_weaponPrefabs[(int)_mainWeapon.Type].ActiveObject(false);
        //_weaponPrefabs[(int)_subWeapon.Type].ActiveObject(false);
        switch (type)
        {
            case EquipmentType.MAIN:
                _mainWeapon.OnLift();
                _mainWeapon = SetWeaponData.Instance.WeaponDatas[weaponType];
                _mainWeapon.OnEquipment();
                break;
            case EquipmentType.SUB:
                _subWeapon.OnLift();
                _subWeapon = SetWeaponData.Instance.WeaponDatas[weaponType];
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

    public void OnTriggerEnter(Collider other)
    {
        _equipementWeapon.Value.HitObjectBehaviour(other);
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
        if (_attackPos != null)
        {
            Gizmos.color = Color.red;
            var oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(_attackPos.position, _attackPos.rotation, _attackPos.localScale);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 1.1f);
            Gizmos.matrix = oldMatrix;
        }
    }
#endif
}
