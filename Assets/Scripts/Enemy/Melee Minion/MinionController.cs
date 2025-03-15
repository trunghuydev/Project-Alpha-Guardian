using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    MeleeMinionMovement meleeMinionMovement;
    MeleeMinionStats meleeMinionStats;

    private Animator anim;
    private int attackCount = 1;

    //Set target to deal dmg to detected one
    private GameObject target = null;

    //Raycast
    [SerializeField] private GameObject raycast;
    [SerializeField] private float raycastDistance;
    private bool isAttacking = false;

    public LayerMask DetectHero;
    private void Awake()
    {
        meleeMinionMovement = GetComponent<MeleeMinionMovement>();
        meleeMinionStats = GetComponent<MeleeMinionStats>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        meleeMinionStats.SetHP();
    }

    private void Update()
    {
        meleeMinionMovement.Run();
        
        RaycastHit2D hit = Physics2D.Raycast(raycast.transform.position, Vector2.left, raycastDistance, DetectHero);
        if (hit.collider != null)
        {
            //Detected
            Debug.DrawRay(raycast.transform.position, Vector2.left * hit.distance, Color.red);
            if (hit.collider.CompareTag("Tower") || hit.collider.CompareTag("Player"))
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    target = hit.collider.gameObject;
                    
                    Attack();
                }
                meleeMinionMovement.Stop();
            }
            
        }
        else
        {
            //Not detected
            isAttacking = false;
            target = null;
            Debug.DrawRay(raycast.transform.position, Vector2.left * raycastDistance, Color.green);
            meleeMinionMovement.ContinueMoving();
        }

    }

    private void Attack()
    {
        if (isAttacking)
        {
            anim.SetTrigger($"attack_{attackCount}");
        }   
    }

    public void AttackAnimationEnd()
    {
        isAttacking = false;
        if (attackCount >= 3)
        {
            attackCount = 0;
        }
        attackCount++;
    }

    public void DealDamage()
    {
        //if tag is player: deal dmg to player
        //if tag is tower: deal dmg to tower
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
                playerStats.UpdateHP(-meleeMinionStats.atk * playerStats.ReduceDmgByDef());
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
                tower.UpdateHP(-meleeMinionStats.atk);
            }
            else
            {
                Debug.LogError("XayahStats component not found on target.");
            }
        }
        target = null;
    }

    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * 1);
    }
    */
}
