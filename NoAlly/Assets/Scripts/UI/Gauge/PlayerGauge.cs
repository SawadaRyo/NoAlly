using UnityEngine;
using UnityEngine.UI;

public class PlayerGauge : MonoBehaviour
{
    [SerializeField, Tooltip("オブジェクトの必殺技ゲージの上限")] float _maxSAP = 20;
    [SerializeField, Tooltip("オブジェクトのHPの上限")] float _maxHP = 20;
    [SerializeField, Tooltip("HPゲージのslider")] Slider _HPGague = default;
    [SerializeField, Tooltip("SAPゲージのslider")] Slider _SAPGague = default;
    [SerializeField, Tooltip("プレイヤーのダメージサウンド")] AudioClip _damageSound;

    [Tooltip("物理防御力")] float _rigitDefensePercentage = 0f;
    [Tooltip("炎防御力")] float _fireDifansePercentage = 0f;
    [Tooltip("電気防御力")] float _elekeDifansePercentage = 0f;
    [Tooltip("氷結防御力")] float _frozenDifansePercentage = 0f;
    [Tooltip("オブジェクトの現在の必殺技ゲージ")] float _sap = 0f;
    [Tooltip("オブジェクトの現在のHP")] float _hp = 0f;
    [Tooltip("オブジェクトの生死判定")] bool _living = true;
    [Tooltip("Animatorの格納変数")] Animator _animator;

    void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }
    void Start()
    {
        if (_HPGague != null && _SAPGague != null)
        {
            _hp = _maxHP;
            _sap = _maxSAP;
            _HPGague.value = _hp;
            _SAPGague.value = _sap;
            _living = true;
        }
    }

    void Update()
    {
        if (_living)
        {
            return;
        }
        else
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            _animator.SetBool("Death", true);
        }
    }
    //ダメージ計算
    public void DamageMethod(float weaponPower, float firePower, float elekePower, float frozenPower)
    {
        var damage = weaponPower * _rigitDefensePercentage
            + firePower * _fireDifansePercentage
            + elekePower * _elekeDifansePercentage
            + frozenPower * _frozenDifansePercentage;
        _hp -= (int)damage;
        _HPGague.value = _hp;
        //生死判定
        if (_hp <= 0)
        {
            _living = false;
        }
        else return;
    }

    
    public void HPPuls(int hpPuls)
    {
        _hp += hpPuls;
        if (_hp > _maxHP)
        {
            _hp -= (_hp - _maxHP);
        }
        _HPGague.value = _hp/_maxHP;
    }
    public void SAPPuls(int sapPuls)
    {
        _sap += sapPuls;
        if (_sap > _maxSAP)
        {
            _sap -= (_sap - _maxSAP);
        }
        _SAPGague.value = _sap/_maxSAP;
    }

    public float SAP { get => _sap; set => _sap = value; }
}
