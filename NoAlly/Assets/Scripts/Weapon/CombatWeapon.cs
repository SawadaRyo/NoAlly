using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CombatWeapon : WeaponBase
{
    [SerializeField] Transform _center = default;
    [SerializeField] protected Vector3 _harfExtents = new Vector3(0.25f, 1.2f, 0.1f);

    bool _completedAttack = false;
    bool _attack = false;
    float _fadeInColor = 0f;
    float _fadeOutColor = 0.5f;
    float _fadeTime = 0.5f;
    ObservableStateMachineTrigger _trigger = default;

    [Tooltip("武器が変形中かどうか")]
    protected bool _isDeformated = false;
    [Tooltip("変形前の武器の当たり判定")]
    protected Vector3 _normalHarfExtents = new Vector3(0.25f, 1.2f, 0.1f);
    [Tooltip("変形後の武器の当たり判定")]
    protected Vector3 _pawerUpHarfExtents = new Vector3(0.4f, 1.7f, 0.1f);
    [Tooltip("武器のアニメーション")]
    protected Animator _weaponAnimator = default;
    [Tooltip("")]
    protected Renderer[] _bladeRenderer = default;

    public override void Awake()
    {
        _weaponAnimator = GetComponent<Animator>(); 
        _trigger = _weaponAnimator.GetBehaviour<ObservableStateMachineTrigger>();
        if (_weaponAnimator != null)
        {
            _bladeRenderer = transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
            foreach (Renderer bR in _bladeRenderer)
            {
                BladeFadeIn(bR.material.color);
            }
        }
        base.Awake();
        _myParticleSystem.Stop();
    }
    public void AttackOfCloseRenge(bool isAttack)
    {
        if (isAttack)
        {
            Collider[] enemiesInRenge = Physics.OverlapBox(_center.position, _harfExtents, Quaternion.identity, _enemyLayer);
            Debug.Log(enemiesInRenge.Length);
            if (enemiesInRenge.Length > 0)
            {
                foreach (Collider enemy in enemiesInRenge)
                {
                    if (enemy.TryGetComponent<EnemyGauge>(out EnemyGauge enemyGauge))
                    {
                        enemyGauge.DamageMethod(_rigitPower, _firePower, _elekePower, _frozenPower);
                    }
                    else if (enemy.TryGetComponent<EnemyBullet>(out EnemyBullet enemyBullet))
                    {
                        enemyBullet.Disactive();
                    }
                }
                _completedAttack = true;
            }
        }
    }
    public IEnumerator LoopJud(bool isAttack)
    {
        _attack = isAttack;
        _myParticleSystem.Play();
        while (_attack)
        {
            if (_completedAttack)
            {
                _completedAttack = false;
                _myParticleSystem.Stop();
                break;
            }
            else
            {
                AttackOfCloseRenge(true);
            }
            yield return null;
        }
        _completedAttack = false;
        _myParticleSystem.Stop();
    }
    public override void RendererActive(bool stats)
    {
        base.RendererActive(stats);
        if (_isDeformated)
        {
            foreach (Renderer bRs in _bladeRenderer)
            {
                bRs.enabled = stats;
            }
        }
    }
    //protected IEnumerator BladeFadeIn(Renderer bR)
    //{
    //    _time = _fadeTime;
    //    while (true)
    //    {
    //        _time -= Time.deltaTime;
    //        Color c = bR.material.color;
    //        c.a = _time / _fadeTime;
    //        bR.material.color = c;
    //        if (_time <= 0)
    //        {
    //            bR.enabled = false;
    //            _weaponAnimator.SetBool("IsOpen", false);
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //}

    //public IEnumerator BladeFadeOut(Renderer bR)
    //{
    //    _time = 0f;
    //    while (true)
    //    {
    //        _time += Time.deltaTime;
    //        Color c = bR.material.color;
    //        c.a = _time / _fadeTime;
    //        bR.material.color = c;
    //        if (_time / _fadeTime >= 0.5f)
    //        {
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //}
    protected void BladeFadeIn(Color bRc)
    {
        DOTween.To(() => _fadeInColor,
            x =>bRc.a = x,
            _fadeOutColor,_fadeTime)
            .OnComplete(() => _weaponAnimator.SetBool("IsOpen",false));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_center.position, _harfExtents);
    }
}
