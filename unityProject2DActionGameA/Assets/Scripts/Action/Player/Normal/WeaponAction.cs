using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class WeaponAction : MonoBehaviour
{
    [SerializeField, Tooltip("溜め攻撃の溜めカウンター")] int m_chrageAttackCounter = 1800;
    [SerializeField, Tooltip("WeaponChangerを格納する変数")] WeaponChanger m_weaponChanger = default;
    [SerializeField, Tooltip("Animatorを格納する変数")] Animator m_animator = default;

    [Tooltip("溜め攻撃の溜め時間")] protected int m_chrageAttackCount = 0;
    [Tooltip("")] bool m_attacked = false;

    public bool Attacked { get => m_attacked; set => m_attacked = value; }


    public virtual void IsStart()
    {
        //Start関数で行いたい処理はここに書く
        //this.UpdateAsObservable()
        //    .TakeUntilDestroy(this)
        //    .Where(_ => Input.GetButtonDown("Attack"))
        //    .ThrottleFirst(TimeSpan.FromMilliseconds(100))
        //    .Subscribe(_ => 
        //    {
        //        print("");
        //        //通常攻撃の処理
        //        m_animator.SetTrigger(m_weaponChanger.EquipmentWeapon.name + "Attack");
        //    });
        //print("Start");
    }

    public virtual void WeaponChargeAttackMethod()
    {
       //武器ごとの溜め攻撃の処理をここに書く
    }
    void WeaponAttackMethod()
    {
        ////通常攻撃の処理
        if (Input.GetButtonDown("Attack"))
        {
            m_animator.SetTrigger(m_weaponChanger.EquipmentWeapon.name + "Attack");
        }

        //溜め攻撃の処理(弓矢のアニメーションもこの処理）
        if (Input.GetButton("Attack") && m_chrageAttackCount < m_chrageAttackCounter)
        {
            m_chrageAttackCount++;
        }
        if (Input.GetButtonUp("Attack"))
        {
            //m_bow.BulletInstance(m_chrageAttackCount);
            if (m_chrageAttackCount > 0)
            {
                m_chrageAttackCount = 0;
            }
        }
        m_animator.SetBool("Charge", Input.GetButton("Attack"));
    }
    void Start()
    {
        IsStart();
    }

    void Update()
    {
        WeaponAttackMethod();
        WeaponChargeAttackMethod();
    }

    void AttackJud()
    {
        m_attacked = !m_attacked;
    }
}
