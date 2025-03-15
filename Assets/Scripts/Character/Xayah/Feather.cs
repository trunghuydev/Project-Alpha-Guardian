using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float distance;
    public LayerMask DetectEnemy;
    public LayerMask GroundLayer;
    public int angle;
    public GameObject explosionEffect;
    private GameObject instantiatedExplosionEffect;

    private bool isMoving = true;
    private bool firstHit = true; //xem minion nào ăn dmg trước

    //List để lưu những minion đã ăn dmg
    private List<MeleeMinionStats> damagedMinions = new List<MeleeMinionStats>();
    private List<RangedMinionSetUp> damagedRangedMinions = new List<RangedMinionSetUp>();
    private List<SiegeMinionSetUp> damagedSiegeMinions = new List<SiegeMinionSetUp>();
    private List<SuperMinionSetUp> damagedSuperMinions = new List<SuperMinionSetUp>();


    XayahAttack xayah;

    private void Awake()    
    {
        xayah = FindObjectOfType<XayahAttack>();

    }
    private void Start()
    {
        //i will fix this
        //transform.rotation = Quaternion.Euler(0, 0, angle);
        Vector3 scale = transform.localScale;
        scale.y = -Mathf.Abs(scale.y); // Ensure the y-scale is negative
        transform.localScale = scale;
        Invoke("DestroyProjectile", lifeTime);
        

    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            //RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, DetectEnemy);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 0.6f, DetectEnemy); // Adjust the radius (0.5f) as necessary
            foreach (var hitCollider in hitEnemies)
            {
                if (hitCollider != null && hitCollider.CompareTag("Enemy"))
                {
                    MeleeMinionStats meleeMinion = hitCollider.GetComponent<MeleeMinionStats>();
                    if (meleeMinion != null && !damagedMinions.Contains(meleeMinion))
                    {
                        ApplyDamageToMinion(meleeMinion, firstHit);
                        damagedMinions.Add(meleeMinion);
                        InstantiateExplosionEffect();
                    }

                    RangedMinionSetUp rangedMinion = hitCollider.GetComponent<RangedMinionSetUp>();
                    if (rangedMinion != null && !damagedRangedMinions.Contains(rangedMinion))
                    {
                        ApplyDamageToRangedMinion(rangedMinion, firstHit);
                        damagedRangedMinions.Add(rangedMinion);
                        InstantiateExplosionEffect();
                    }

                    SiegeMinionSetUp siegeMinion = hitCollider.GetComponent<SiegeMinionSetUp>();
                    if (siegeMinion != null && !damagedSiegeMinions.Contains(siegeMinion))
                    {
                        ApplyDamageToSiegeMinion(siegeMinion, firstHit);
                        damagedSiegeMinions.Add(siegeMinion);
                        InstantiateExplosionEffect();
                    }

                    SuperMinionSetUp superMinion = hitCollider.GetComponent<SuperMinionSetUp>();
                    if (superMinion != null && !damagedSuperMinions.Contains(superMinion))
                    {
                        ApplyDamageToSuperMinion(superMinion, firstHit);
                        damagedSuperMinions.Add(superMinion);
                        InstantiateExplosionEffect();
                    }
                }
            }
            //check if hit ground
            RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, distance, GroundLayer);
            if (groundHit.collider != null)
            {
                isMoving = false;
                
            }
        }
    }

    private void InstantiateExplosionEffect()
    {
        if (explosionEffect != null)
        {
            instantiatedExplosionEffect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(instantiatedExplosionEffect, 0.4f); 
        }
    }

    public void Withdraw()
    {
        if (xayah != null)
        {
            Vector3 targetPosition = xayah.transform.position;
            targetPosition.y -= 0.5f; // Subtract 1 from the y position
            //Debug.Log($"Feather is withdrawing to Xayah's position: {targetPosition}");
            StartCoroutine(MoveToTarget(targetPosition, 0.2f)); 
        }
    }

    private IEnumerator MoveToTarget(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(start, target, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); // Destroy the feather after moving
    }

    private void ApplyDamageToMinion(MeleeMinionStats minion, bool isFirstHit)
    {
        if (isFirstHit)
        {
            xayah.SetDamage(minion, 0.9f);
            firstHit = false;
        }
        else
        {
            xayah.SetDamage(minion, 0.45f);
        }
    }

    private void ApplyDamageToRangedMinion(RangedMinionSetUp rangedMinion, bool isFirstHit)
    {
        if (isFirstHit)
        {
            xayah.SetDamage(rangedMinion, 0.9f);
            firstHit = false;
        }
        else
        {
            xayah.SetDamage(rangedMinion, 0.45f);
        }
    }

    private void ApplyDamageToSiegeMinion(SiegeMinionSetUp siegeMinion, bool isFirstHit)
    {
        if (isFirstHit)
        {
            xayah.SetDamage(siegeMinion, 0.9f);
            firstHit = false;
        }
        else
        {
            xayah.SetDamage(siegeMinion, 0.45f);
        }
    }

    private void ApplyDamageToSuperMinion(SuperMinionSetUp superMinion, bool isFirstHit)
    {
        if (isFirstHit)
        {
            xayah.SetDamage(superMinion, 0.9f);
            firstHit = false;
        }
        else
        {
            xayah.SetDamage(superMinion, 0.45f);
        }
    }


    public List<MeleeMinionStats> GetDamagedMinions()
    {
        return damagedMinions;
    }

    public List<RangedMinionSetUp> GetDamagedRangedMinions()
    {
        return damagedRangedMinions;
    }

    public List<SiegeMinionSetUp> GetDamagedSiegeMinions()
    {
        return damagedSiegeMinions;
    }

    public List<SuperMinionSetUp> GetDamagedSuperMinions()
    {
        return damagedSuperMinions;
    }

    //Use later
    private void DestroyProjectile()
    {     
        Destroy(gameObject);
    }
    
}         