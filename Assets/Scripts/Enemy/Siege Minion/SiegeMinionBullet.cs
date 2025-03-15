using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiegeMinionBullet : MonoBehaviour
{
    public float speed;
    public float distance;
    public LayerMask DetectHero;
    public GameObject explosionEffect;
    private GameObject instantiatedExplosionEffect;
    private GameObject target = null;

    private SiegeMinionStats siegeMinion;

    public void Initialize(SiegeMinionStats minionStats)
    {
        siegeMinion = minionStats;
    }

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, 0.4f, Vector2.left, distance, DetectHero); // Adjust the radius (0.5f) as necessary
        if (hitInfo.collider != null)
        {
            target = hitInfo.collider.gameObject;
            if (target.CompareTag("Tower"))
            {
                TowerStats tower = target.GetComponent<TowerStats>();
                if (tower != null)
                {
                    tower.UpdateHP(-siegeMinion.atk);
                }
            }
            if (target.CompareTag("Player"))
            {
                IPlayerStats playerStats = target.GetComponent<IPlayerStats>();
                if (playerStats != null)
                {
                    playerStats.UpdateHP(-siegeMinion.atk * playerStats.ReduceDmgByDef());
                }
                else
                {
                    Debug.LogError("IPlayerStats component not found on target.");
                }
            }
            DestroyProjectile();
        }
        else
        {
            target = null;
        }
    }
    private void InstantiateExplosionEffect()
    {
        if (explosionEffect != null)
        {
            instantiatedExplosionEffect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(instantiatedExplosionEffect, 0.3f);
        }

    }


    private void DestroyProjectile()
    {
        InstantiateExplosionEffect();
        Destroy(gameObject);
    }

}
