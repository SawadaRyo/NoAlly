using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

 public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected int m_weaponPower = 5;
    [SerializeField] protected int m_elekePower = 0;
    [SerializeField] protected int m_firePower = 0;
    [SerializeField] protected int m_frozenPower = 0;
    [SerializeField] protected LayerMask enemyLayer = ~0;
    public int WeaponPower { get => m_weaponPower; set => m_weaponPower = value; }

    public virtual void IsStart()
    {
        //Start関数で呼びたい処理はこの関数に
    }
    public virtual void IsUpdate()
    {
        //Update関数で呼びたい処理はこの関数に
    }
    void Start()
    {
        IsStart();
    }
    void Update()
    {
        IsUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TargetObject")
        {
            var enemyHP = other.gameObject.GetComponent<GaugeManager>();
            if (enemyHP)
            {
                enemyHP.DamageMethod(m_weaponPower, m_firePower, m_elekePower, m_frozenPower);
            }
            else return;
        }
    }
    public void IsAttack(CapsuleCollider isAttack)
    {
        //ここで受け取ったコライダーからOverlapCapsuleの引数を計算
        var direction = new Vector3 { [isAttack.direction] = 1 };
        var offset = isAttack.height / 2 - isAttack.radius;
        var localPoint0 = isAttack.center - direction * offset;
        var localPoint1 = isAttack.center + direction * offset;

        //計算した値はローカル座標なのでワールド座標に変換
        var point0 = transform.TransformPoint(localPoint0);
        var point1 = transform.TransformPoint(localPoint1);
        var r = transform.TransformVector(isAttack.radius, isAttack.radius, isAttack.radius);
        var radius = Enumerable.Range(0, 3).Select(xyz => xyz == isAttack.direction ? 0 : r[xyz]).Select(Mathf.Abs).Max();
        var enemiesCol = Physics.OverlapCapsule(point0, point1, isAttack.radius, enemyLayer);
        foreach (var c in enemiesCol)
        {
            GaugeManager enemiesHP = c.gameObject.GetComponent<GaugeManager>();
            Debug.Log(enemiesHP);
            if (enemiesHP)
            {
                enemiesHP.DamageMethod(WeaponPower, m_firePower, m_elekePower, m_frozenPower);
            }
        }
    }
}
