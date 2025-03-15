using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XayahAttack : MonoBehaviour
{
    public GameObject projectile;
    public Transform shotPoint;
    private MeleeMinionStats minion;

    private Animator anim;
    
    private int attackCount = 1;

    private XayahStats xayahStats;
    private XayahUltimate ult;
    private ChangeBorder getSelected;

    //Skill Point
    private SkillPoints skillPoint;

    //Raycast
    [SerializeField] private GameObject raycast;
    [SerializeField] private float raycastDistance;
    private bool isAttacking = false;
    public LayerMask DetectEnemy;

    

    //Cooldown : 1.2s
    private float timeCoolDown;
    public float startCooldown;

    private void Awake()
    {
        minion = FindObjectOfType<MeleeMinionStats>();
        xayahStats = FindObjectOfType<XayahStats>();
        anim = GetComponent<Animator>();
        getSelected = FindObjectOfType<ChangeBorder>();
        ult = FindObjectOfType<XayahUltimate>();
        skillPoint = FindObjectOfType<SkillPoints>();
    }
    private void Update()
    {
        
        timeCoolDown -= Time.deltaTime;

        DetectEnemyRaycast(); //Detect enemy

        UseSkill(); //Skill condition

        UseUltimate(); //Ult condition

        

    }

    public void StartCooldownAfterSpawn()
    {
        timeCoolDown = startCooldown/xayahStats.getAttackSpd();
    }

    private void DetectEnemyRaycast()
    {
        Vector2 direction = Vector2.right;
        if (transform.localScale.x < 0)
        {
            direction = Vector2.left;
        }

        RaycastHit2D hit = Physics2D.Raycast(raycast.transform.position, direction, raycastDistance, DetectEnemy);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                //Detected
                Debug.DrawRay(raycast.transform.position, direction * hit.distance, Color.red);
                if (!isAttacking && timeCoolDown <= 0)
                {
                    isAttacking = true;
                    Attack();
                }
            }
        }
        else
        {
            //Not detected
            isAttacking = false;
            Debug.DrawRay(raycast.transform.position, direction * raycastDistance, Color.green);

        }
    }

    

    //Trigger animation
    public void Attack()
    {
        if (timeCoolDown <= 0)
        {
            float animSpd = xayahStats.getAttackSpd();
            anim.SetFloat("speed", animSpd);
            anim.SetTrigger($"a{attackCount}");
            timeCoolDown = startCooldown;
            //Debug.Log($"Attack {attackCount} performed, cooldown reset to {startCooldown}s");
            //Debug.Log(countFeather);

        }
    }
    
    public void ShootFeather(float angle)
    {
        float facingDirection;
        if (transform.localScale.x > 0)
        {
            facingDirection = 1;
        }
        else
        {
            facingDirection = -1;
        }

        // Adjust the angle based on the facing direction
        float adjustedAngle = angle * facingDirection;

        GameObject feather = Instantiate(projectile, shotPoint.position, Quaternion.Euler(0, 0, adjustedAngle));
    }

    public void ShootFeatherUlt(float angle)
    {
        float facingDirection;
        if(transform.localScale.x > 0)
        {
            facingDirection = 1;
        }
        else
        {
            facingDirection = -1;
        }
        // Adjust the angle based on the facing direction
        float adjustedAngle = angle * facingDirection;

        Vector3 shotPointOffset = new Vector3(-0.2f, 0.5f, 0);
        GameObject feather = Instantiate(projectile, shotPoint.position + shotPointOffset, Quaternion.Euler(0, 0, adjustedAngle));
    }

    private void UseUltimate()
    {
        if (Input.GetKeyDown(KeyCode.Q) && getSelected.selected && ult.currentEnergy == ult.GetMaxEnergy())
        {
            anim.SetTrigger("Ult");
            ult.ResetEnergy();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && getSelected.selected && ult.currentEnergy != ult.GetMaxEnergy())
        {
            Debug.Log("Not enough energy");
        }
    }

    private void UseSkill()
    {
        if(Input.GetKeyDown(KeyCode.E) && getSelected.selected && skillPoint.currentSkillPoint > 0)
        {
            anim.ResetTrigger($"a{attackCount}");
            skillPoint.UseSkillPoint();
            anim.SetTrigger("Skill");
        }
        else if(Input.GetKeyDown(KeyCode.E) && getSelected.selected && skillPoint.currentSkillPoint <= 0)
        {
            Debug.Log("Not enough Skill Point");
        }
    }

    public void SkillAnimation()
    {
        ShootFeather(-97f);
        ShootFeather(-100);
    }

    public void NormalAttackAnimation()
    {
        ShootFeather(-98);
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

    public void ChangeAttackStateAndWithdrawFeather() //Add this for skill and ult
    {
        isAttacking = false;
        WithdrawFeathers();
    }

    //Deal damage
    public void SetDamage(MeleeMinionStats selectedMinion, float multiplier)
    {
        if (selectedMinion.currentHP > 0)
        {
            //Get attack from xayah
            float atk = xayahStats.getAttackBuff();
            float OriginalDamage = atk * multiplier; //dmg gốc
            float finalizedDmg = OriginalDamage * xayahStats.getDmgBoost() * selectedMinion.ReduceDmgByDef() * (1 - (xayahStats.getIgnoreRes() - selectedMinion.Res()));

            bool isCrit = Random.value < xayahStats.getCritRate();

            if (isCrit)
            {
                finalizedDmg *= xayahStats.getCritDmg();
            }

            selectedMinion.UpdateHP(-finalizedDmg);        
            //Debug.Log($"Dealt {finalizedDmg} damage to {selectedMinion.name}");
            DamagePopUp.Create(selectedMinion.transform.position, finalizedDmg, isCrit);
        }
    }

    public void SetDamage(RangedMinionSetUp selectedMinion, float multiplier)
    {
        if (selectedMinion.currentHP > 0)
        {
            //Get attack from xayah
            float atk = xayahStats.getAttackBuff();
            float OriginalDamage = atk * multiplier; //dmg gốc
            float finalizedDmg = OriginalDamage * xayahStats.getDmgBoost() * selectedMinion.ReduceDmgByDef() * (1 - (xayahStats.getIgnoreRes() - selectedMinion.Res()));

            bool isCrit = Random.value < xayahStats.getCritRate();

            if (isCrit)
            {
                finalizedDmg *= xayahStats.getCritDmg();
            }

            selectedMinion.UpdateHP(-finalizedDmg);
            //Debug.Log($"Dealt {finalizedDmg} damage to {selectedMinion.name}");
            DamagePopUp.Create(selectedMinion.transform.position, finalizedDmg, isCrit);
        }
    }

    public void SetDamage(SiegeMinionSetUp selectedMinion, float multiplier)
    {
        if (selectedMinion.currentHP > 0)
        {
            //Get attack from xayah
            float atk = xayahStats.getAttackBuff();
            float OriginalDamage = atk * multiplier; //dmg gốc
            float finalizedDmg = OriginalDamage * xayahStats.getDmgBoost() * selectedMinion.ReduceDmgByDef() * (1 - (xayahStats.getIgnoreRes() - selectedMinion.Res()));

            bool isCrit = Random.value < xayahStats.getCritRate();

            if (isCrit)
            {
                finalizedDmg *= xayahStats.getCritDmg();
            }

            selectedMinion.UpdateHP(-finalizedDmg);
            //Debug.Log($"Dealt {finalizedDmg} damage to {selectedMinion.name}");
            DamagePopUp.Create(selectedMinion.transform.position, finalizedDmg, isCrit);
        }
    }

    public void SetDamage(SuperMinionSetUp selectedMinion, float multiplier)
    {
        if (selectedMinion.currentHP > 0)
        {
            //Get attack from xayah
            float atk = xayahStats.getAttackBuff();
            float OriginalDamage = atk * multiplier; //dmg gốc
            float finalizedDmg = OriginalDamage * xayahStats.getDmgBoost() * selectedMinion.ReduceDmgByDef() * (1 - (xayahStats.getIgnoreRes() - selectedMinion.Res()));

            bool isCrit = Random.value < xayahStats.getCritRate();

            if (isCrit)
            {
                finalizedDmg *= xayahStats.getCritDmg();
            }

            selectedMinion.UpdateHP(-finalizedDmg);
            //Debug.Log($"Dealt {finalizedDmg} damage to {selectedMinion.name}");
            DamagePopUp.Create(selectedMinion.transform.position, finalizedDmg, isCrit);
        }
    }


    private void WithdrawFeathers()
    {
        Feather[] feathers = FindObjectsOfType<Feather>();

        foreach (Feather feather in feathers)
        {
            List<MeleeMinionStats> hitMinions = feather.GetDamagedMinions();

            foreach (MeleeMinionStats minion in hitMinions)
            {
                if (minion.currentHP > 0)
                {
                    SetDamage(minion, 0.6f);
                    //Debug.Log($"Dealt {damage} damage to {minion.name}");
                }
            }

            List<RangedMinionSetUp> hitRangedMinions = feather.GetDamagedRangedMinions();
            foreach (RangedMinionSetUp rangedMinion in hitRangedMinions)
            {
                if (rangedMinion.currentHP > 0)
                {
                    SetDamage(rangedMinion, 0.6f);
                    // Debug.Log($"Dealt {damage} damage to {rangedMinion.name}");
                }
            }

            List<SiegeMinionSetUp> hitSiegeMinions = feather.GetDamagedSiegeMinions();
            foreach (SiegeMinionSetUp siegeMinion in hitSiegeMinions)
            {
                if (siegeMinion.currentHP > 0)
                {
                    
                    SetDamage(siegeMinion, 0.6f);
                    // Debug.Log($"Dealt {damage} damage to {siegeMinion.name}");
                }
            }

            List<SuperMinionSetUp> hitSuperMinions = feather.GetDamagedSuperMinions();
            foreach (SuperMinionSetUp superMinion in hitSuperMinions)
            {
                if (superMinion.currentHP > 0)
                {

                    SetDamage(superMinion, 0.6f);
                    // Debug.Log($"Dealt {damage} damage to {siegeMinion.name}");
                }
            }

            feather.Withdraw(); 
        }
        //ApplyAreaDamage();
    }


    public void StopAttacking()
    {
        isAttacking = false;
        anim.ResetTrigger($"a{attackCount}");  // Reset the current attack trigger
                                               // You can add any additional logic to handle stopping the attack animation here.
    }

    
}
 