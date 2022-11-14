using UnityEngine;

public class EnemyStatus : MonoBehaviour //敵の体力を管理するスクリプト
{
    [SerializeField, Tooltip("敵のHPの上限")]
    float _maxHP = 5;
    [SerializeField, Tooltip("敵の物理耐性")]
    float _rigitDefensePercentage = 1f;
    [SerializeField, Tooltip("敵の炎耐性")]
    float _fireDifansePercentage = 1f;
    [SerializeField, Tooltip("敵の雷耐性")]
    float _elekeDifansePercentage = 1f;
    [SerializeField, Tooltip("敵の氷結耐性")]
    float _frozenDifansePercentage = 1f;
    [SerializeField, Tooltip("ダメージサウンド")]
    AudioClip _damageSound;
    [Tooltip("オブジェクトの現在のHP")]
    float _hp;
    [Tooltip("このオブジェクトにアタッチされているEnemyBaseを取得する変数")]
    EnemyBase _enemyBase = null;
    [Tooltip("無敵時間")]
    Interval _invincibleTime = new Interval(0.4f);
    [Tooltip("AudioSourceを格納する変数")]
    AudioSource _audioSource = null;


    public bool IsInvincible => _invincibleTime.IsCountUp();
    public void Start()
    {
        _audioSource = GameObject.FindObjectOfType<AudioSource>();
        _hp = _maxHP;
        _enemyBase = GetComponentInParent<EnemyBase>();
    }

    //ダメージ計算
    public void DamageMethod(float weaponPower, float firePower, float elekePower, float frozenPower, ElementType type)
    {
        float baseDamage = weaponPower * _rigitDefensePercentage;
        float elemantDamage = 0;
        switch (type)
        {
            case ElementType.FIRE:
                elemantDamage = firePower* _fireDifansePercentage;
                break;
            case ElementType.ELEKE:
                elemantDamage = elekePower* _elekeDifansePercentage;
                break;
            case ElementType.FROZEN:
                elemantDamage = frozenPower* _frozenDifansePercentage;
                break;
        }
        _hp -= (baseDamage + elemantDamage);
        _audioSource.PlayOneShot(_damageSound);
        //Debug.Log(m_hp);
        //生死判定
        if (_hp <= 0)
        {
            _enemyBase.Disactive();
        }
    }
}
