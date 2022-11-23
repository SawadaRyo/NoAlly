using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StatusBase : MonoBehaviour
{
    [Tooltip("ƒIƒuƒWƒFƒNƒg‚Ì¶Ž€”»’è")]
    protected bool _living = true;
    [SerializeField, Tooltip("“G‚ÌHP‚ÌãŒÀ")]
    protected float _maxHP = 5;
    [SerializeField, Tooltip("“G‚Ì•¨—‘Ï«")]
    protected float _rigitDefensePercentage = 1f;
    [SerializeField, Tooltip("“G‚Ì‰Š‘Ï«")]
    protected float _fireDifansePercentage = 1f;
    [SerializeField, Tooltip("“G‚Ì—‹‘Ï«")]
    protected float _elekeDifansePercentage = 1f;
    [SerializeField, Tooltip("“G‚Ì•XŒ‹‘Ï«")]
    protected float _frozenDifansePercentage = 1f;

    [Tooltip("ƒIƒuƒWƒFƒNƒg‚ÌŒ»Ý‚ÌHP")]
    protected FloatReactiveProperty _hp;
    [Tooltip("–³“GŽžŠÔ")]
    protected Interval _invincibleTime = new Interval(0.4f);
    [Tooltip("AudioSource‚ðŠi”[‚·‚é•Ï”")]
    protected AudioSource _audioSource = null;
    [SerializeField, Tooltip("ƒ_ƒ[ƒWƒTƒEƒ“ƒh")]
    protected AudioClip _damageSound;
    [Tooltip("Animator‚ÌŠi”[•Ï”")]
    protected Animator _animator;

    public bool Living => _living;
    public bool IsInvincible => _invincibleTime.IsCountUp();
    public FloatReactiveProperty HP => _hp;

    private void Awake()
    {
        Init();
    }
    public virtual void Init()
    {
        _hp = new FloatReactiveProperty(_maxHP);
        _living = true;
        _animator = GetComponentInParent<Animator>();
    }

}
