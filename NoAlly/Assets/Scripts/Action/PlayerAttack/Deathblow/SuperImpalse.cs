using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperImpalse : DeathblowBase
{
    int m_deathBlowRange = 0;

    public override void DeathblowAbility()
    {
        StartCoroutine(SuperImpalseAction());
    }

    

    IEnumerator SuperImpalseAction()
    {
        for(int i = 1; i <= 5;i++)
        {
            var enemyCollider = Physics.OverlapSphere(this.transform.position, m_deathBlowRange, m_targetLayer);
            foreach (var ec in enemyCollider)
            {
                var enemyHp = ec.gameObject.GetComponent<EnemyGauge>();
                if (enemyHp)
                {
                    enemyHp.DamageMethod(m_power, 0, 0, 0);
                }
            }
            m_deathBlowRange += i;
            yield return new WaitForSeconds(0.5f);
        }
        m_deathBlowRange = 0;
        yield break;
    }
}
