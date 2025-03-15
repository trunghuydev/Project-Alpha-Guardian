using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiegeMinionAttack : MonoBehaviour
{
    //Get component
    SiegeMinionStats siegeMinion;
    SiegeMinionMovement siegeMinionMovement;
    private Animator anim;


    private bool isAttacking = false;
    private int attackCount = 1;
    public Transform shotPoint;
    public GameObject projectile;

    //Raycast
    public GameObject raycast;
    public LayerMask DetectHero;

    private void Awake()
    {
        siegeMinion = GetComponent<SiegeMinionStats>();
        siegeMinionMovement = GetComponent<SiegeMinionMovement>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        siegeMinionMovement.Run();
        if (DetectPlayer())
        {
            if (!isAttacking)
            {
                isAttacking = true;
                Attack();
            }
            siegeMinionMovement.Stop();
        }
        else
        {
            isAttacking = false;

            siegeMinionMovement.ContinueMoving();
        }
    }

    private bool DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycast.transform.position, Vector2.left, siegeMinion.range, DetectHero);
        if (hit.collider != null)
        {
            //Detected
            Debug.DrawRay(raycast.transform.position, Vector2.left * hit.distance, Color.red);
            if (hit.collider.CompareTag("Tower") || hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        else
        {
            //Not detected
            Debug.DrawRay(raycast.transform.position, Vector2.left * siegeMinion.range, Color.green);
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

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(projectile, shotPoint.position, Quaternion.Euler(0, 0, 0));

        //Initialize Ranged minion stats here cuz RangedMinionBullet & RangedMinionStats attach to different gameObject
        SiegeMinionBullet bulletScript = bullet.GetComponent<SiegeMinionBullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(siegeMinion);
        }
    }
}
