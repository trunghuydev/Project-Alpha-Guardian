using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsheDetectEnemy : MonoBehaviour
{
    public float detectionRadius = 10f;  
    public LayerMask enemyLayer;
    public GameObject bulletPrefab;  
    public Transform firePoint;
    private Transform targetEnemy;
    private AsheStats ashe;

    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        ashe = FindObjectOfType<AsheStats>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        DetectEnemy();
    }

    void DetectEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        if (enemies.Length > 0)
        {
            targetEnemy = enemies.OrderBy(e => Vector2.Distance(transform.position, e.transform.position)).FirstOrDefault()?.transform;

            if (targetEnemy != null)
            {
                Attacking();
                anim.SetTrigger("attack");
            }
        }
    }
    void FireBullet()
    {
        if (targetEnemy != null)
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            AsheArrow bulletScript = bulletInstance.GetComponent<AsheArrow>();
            bulletScript.Initialize(targetEnemy);  
        }
    }

    

    public void Attacking()
    {
        anim.SetBool("back_to_idle", false);
    }
    public void BackToIdle()
    {
        anim.SetBool("back_to_idle", true);
    }
}
