using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RakanAura : MonoBehaviour
{
    RakanAttack rakan;
    RakanSetUp rakanSetUp;
    
    private BoxCollider2D boxCollider;

    public LayerMask detectEnemy;

    public float damageInterval = 1f;
    private float damageTimer;

    private GameObject target = null;

    private void Awake()
    {
        rakan = FindObjectOfType<RakanAttack>();
        boxCollider = GetComponent<BoxCollider2D>();
        rakanSetUp = FindObjectOfType<RakanSetUp>();
    }

    private void Update()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.size, 0f, detectEnemy);

        foreach (var enemy in enemies)
        {
            
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                DealDamage(enemy, 1.5f);
                rakanSetUp.currentShield += (rakanSetUp.getHP() + 75);
                damageTimer = 0f;  
            }
        }
        
        
    }

    private void DealDamage(Collider2D enemy, float multiplier)
    {
        IEnemyStats enemyStats = enemy.GetComponent<IEnemyStats>();  

        if (enemyStats != null)
        {
            bool isCrit = Random.value < rakanSetUp.getCritRate();
            float finalizeDmg = rakanSetUp.getAtk() * multiplier * enemyStats.ReduceDmgByDef();
            if (isCrit)
            {
                finalizeDmg *= rakanSetUp.getCritDamage();
            }

            enemyStats.UpdateHP(-finalizeDmg);
            DamagePopUp.Create(enemy.transform.position, finalizeDmg, isCrit);

        }
        else
        {
            Debug.LogWarning("Enemy does not implement IEnemyStats: " + enemy.name);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.size);
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy"))
    //    {
    //        damageTimer += Time.deltaTime;
    //        if (damageTimer >= damageInterval)
    //        {
    //            rakan.DealDamage(0.4f);
    //            rakanSetUp.currentShield += (rakanSetUp.getHP() + 25);
    //            damageTimer = 0f; 
    //        }
            
    //    }
    //    Debug.Log(damageTimer);
    //}
}
