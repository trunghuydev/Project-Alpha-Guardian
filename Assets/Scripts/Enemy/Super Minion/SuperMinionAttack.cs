using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMinionAttack : MonoBehaviour
{
    //Get component
    SuperMinionStats superMinion;
    SuperMinionMovement superMinionMovement;
    private Animator anim;


    private bool isAttacking = false;
    private int attackCount = 1;

    //Set target to deal dmg to detected one
    private GameObject target = null;

    //Raycast
    public GameObject raycast;
    public LayerMask DetectHero;

    private void Awake()
    {
        superMinion = GetComponent<SuperMinionStats>();
        superMinionMovement = GetComponent<SuperMinionMovement>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        superMinionMovement.Run();
        if (DetectPlayer())
        {
            if (!isAttacking)
            {
                isAttacking = true;
                
                Attack();
            }
            superMinionMovement.Stop();
        }
        else
        {
            isAttacking = false;

            superMinionMovement.ContinueMoving();
        }
    }

    private bool DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycast.transform.position, Vector2.left, superMinion.range, DetectHero);
        if (hit.collider != null)
        {
            //Detected
            Debug.DrawRay(raycast.transform.position, Vector2.left * hit.distance, Color.red);
            if (hit.collider.CompareTag("Tower") || hit.collider.CompareTag("Player"))
            {
                target = hit.collider.gameObject;
                return true;
            }
        }
        else
        {
            //Not detected
            target = null;
            Debug.DrawRay(raycast.transform.position, Vector2.left * superMinion.range, Color.green);
        }
        return false;
    }
    private void Attack()
    {
        if (isAttacking)
        {
            //Debug.Log(attackCount);
            anim.SetTrigger($"a{attackCount}");
        }
    }
    public void AttackAnimationEnd()
    {
        isAttacking = false;
        if (attackCount >= 2)
        {
            attackCount = 0;
        }
        attackCount++;
    }

    public void DealDamage()
    {
        if (target == null)
        {
            return;
        }

        if (target.CompareTag("Player"))
        {
            IPlayerStats playerStats = target.GetComponent<IPlayerStats>();
            //XayahStats xayah = target.GetComponent<XayahStats>();
            if (playerStats != null)
            {
                playerStats.UpdateHP(-superMinion.atk * playerStats.ReduceDmgByDef());
            }
            else
            {
                Debug.LogError("IPlayerStats component not found on target.");
            }

        }
        if (target.CompareTag("Tower"))
        {
            TowerStats tower = target.GetComponent<TowerStats>();
            if (tower != null)
            {
                tower.UpdateHP(-superMinion.atk);
            }
            else
            {
                Debug.LogError("XayahStats component not found on target.");
            }
        }
        target = null;
    }
}
