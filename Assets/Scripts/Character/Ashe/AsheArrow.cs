using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheArrow : MonoBehaviour
{
    AsheStats ashe;

    public float speed = 5f;
    private Transform target;

    private void Awake()
    {
        ashe = FindObjectOfType<AsheStats>();
    }

    public void Initialize(Transform enemyTarget)
    {
        target = enemyTarget;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.y -= 0.3f;
            direction.Normalize();
            transform.position += direction * speed * Time.deltaTime;
            RotateArrow(direction);
        }
    }

    void RotateArrow(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            IEnemyStats enemy = collider.gameObject.GetComponent<IEnemyStats>();
            if (enemy != null)
            {
                if (!enemy.HasDebuff())
                {
                    enemy.DebuffDecreaseDef(1);
                    enemy.hasApplyDebuff(true);
                }
                
                bool isCrit = Random.value < ashe.critRate;
                float finalizeDmg =  ashe.attack * 1.0f * enemy.ReduceDmgByDef() * (1 - enemy.Res());
                if (isCrit)
                {
                    finalizeDmg *= ashe.critDmg;
                }

                enemy.UpdateHP(-finalizeDmg);
                DamagePopUp.Create(collider.transform.position, finalizeDmg, isCrit);
            }
            Destroy(gameObject);
        }
        else
        {
            Invoke("DestroyArrow",1);
        }
    }
    private void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
