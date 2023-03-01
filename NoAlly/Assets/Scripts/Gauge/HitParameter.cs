using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����蔻��ƃ_���[�W�v�Z�̗v�f��n���֐�
/// </summary>

[RequireComponent(typeof(Collider))]
public class HitParameter : MonoBehaviour, IHitBehavorOfAttack
{
    [SerializeField, Tooltip("���̃N���X�̃I�[�i�[")]
    ObjectOwner _owner;
    [Header("�U�����ꂽ���̔{���B�l�������قǎ󂯂�_���[�W���オ��")]
    [Range(0f, 2f), SerializeField, Header("�����U���̔{��")]
    float _rigitDefensePercentage = 1f;
    [Range(0f, 2f), SerializeField, Header("���U���̔{��")]
    float _fireDifansePercentage = 1f;
    [Range(0f, 2f), SerializeField, Header("�d�C�U���̔{��")]
    float _elekeDifansePercentage = 1f;
    [Range(0f, 2f), SerializeField, Header("�X�U���̔{��")]
    float _frozenDifansePercentage = 1f;

    [Tooltip("�I�u�W�F�N�g��StatusBase")]
    StatusBase _status = null;

    /// <summary>
    /// �����U���̔{���̃v���p�e�B(�ǂݎ���p)
    /// </summary>
    public float RigitDefensePercentage => _rigitDefensePercentage;
    /// <summary>
    /// ���U���̔{���̃v���p�e�B(�ǂݎ���p)
    /// </summary>
    public float FireDifansePercentage => _fireDifansePercentage;
    /// <summary>
    /// �d�C�U���̔{���̃v���p�e�B(�ǂݎ���p)
    /// </summary>
    public float ElekeDifansePercentage => _elekeDifansePercentage;
    /// <summary>
    /// �X�U���̔{���̃v���p�e�B(�ǂݎ���p)
    /// </summary>
    public float FrozenDifansePercentage => _frozenDifansePercentage;
    /// <summary>
    /// �I�u�W�F�N�g��StatusBase�̃v���p�e�B(�ǂݎ���p)
    /// </summary>
    StatusBase Status
    {
        get
        {
            if (_status == null)
            {
                _status = this.GetComponentInParent<StatusBase>();
            }
            return _status;
        }
    }
    public ObjectOwner Owner => _owner;

    public void BehaviorOfHIt(float[] damageValue, ElementType type)
    {
        Status.Damage(damageValue, this, type);
    }

    public void BehaviorOfHit<TPlus>(TPlus pulsItem) where TPlus : ItemBase
    {

        if (Status is PlayerStatus)
        {
            PlayerStatus playerStatus = (PlayerStatus)Status;
            if (pulsItem is HPPlus)
            {
                playerStatus.HPPuls(pulsItem.PlusParameter);
            }
            else if (pulsItem is SAPPlus)
            {
                playerStatus.SAPPuls(pulsItem.PlusParameter);
            }
        }
    }

}
