using UnityEngine;

public class EnemyStatus : MonoBehaviour //�G�̗̑͂��Ǘ�����X�N���v�g
{
    [SerializeField, Tooltip("�G��HP�̏��")]
    float _maxHP = 5;
    [SerializeField, Tooltip("�G�̕����ϐ�")]
    float _rigitDefensePercentage = 1f;
    [SerializeField, Tooltip("�G�̉��ϐ�")]
    float _fireDifansePercentage = 1f;
    [SerializeField, Tooltip("�G�̗��ϐ�")]
    float _elekeDifansePercentage = 1f;
    [SerializeField, Tooltip("�G�̕X���ϐ�")]
    float _frozenDifansePercentage = 1f;
    [SerializeField, Tooltip("�_���[�W�T�E���h")]
    AudioClip _damageSound;
    [Tooltip("�I�u�W�F�N�g�̌��݂�HP")]
    float _hp;
    [Tooltip("���̃I�u�W�F�N�g�ɃA�^�b�`����Ă���EnemyBase���擾����ϐ�")]
    EnemyBase _enemyBase = null;
    [Tooltip("���G����")]
    Interval _invincibleTime = new Interval(0.4f);
    [Tooltip("AudioSource���i�[����ϐ�")]
    AudioSource _audioSource = null;


    public bool IsInvincible => _invincibleTime.IsCountUp();
    public void Start()
    {
        _audioSource = GameObject.FindObjectOfType<AudioSource>();
        _hp = _maxHP;
        _enemyBase = GetComponentInParent<EnemyBase>();
    }

    //�_���[�W�v�Z
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
        //��������
        if (_hp <= 0)
        {
            _enemyBase.Disactive();
        }
    }
}
