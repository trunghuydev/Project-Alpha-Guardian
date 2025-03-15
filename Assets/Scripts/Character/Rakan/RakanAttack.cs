using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RakanAttack : MonoBehaviour
{
    //Get component
    RakanSetUp rakan;    
    private Animator anim;
    private AimLine aimLine;
    private ChangeBorderRakan getSelected;
    private RakanUltimate ult;
    SpawnRakan spawnRakan;
    private BoxCollider2D boxCollider;

    private bool isAttacking = false;
    private int attackCount = 1;

    //Set target to deal dmg to detected one
    private GameObject target = null;

    //Raycast
    public GameObject raycast;
    public LayerMask detectEnemy;

    //Cooldown : 1.2s
    private float timeCoolDown;
    public float startCooldown;

    // Aim line variables
    private bool isAiming = false;  // Track whether we're aiming
    private Vector3 aimEndPosition;

    //Skill Point
    private SkillPoints skillPoint;

    //Ult aura
    public Image aura;
    
    private void Awake()
    {
        
    }

    private void Start()
    {
        rakan = GetComponent<RakanSetUp>();
        anim = GetComponent<Animator>();
        aimLine = GetComponent<AimLine>();
        skillPoint = FindObjectOfType<SkillPoints>();
        getSelected = FindObjectOfType<ChangeBorderRakan>();
        ult = FindObjectOfType<RakanUltimate>();
        spawnRakan = FindObjectOfType<SpawnRakan>();
        Reset();
    }

    private void Update()
    {
        if (DetectEnemy())
        {
            if (!isAttacking)
            {
                isAttacking = true;
                Attack();
            }
        }
        else
        {
            isAttacking = false;
        }

        UseSkill();
        UseUltimate();
        ActivateAura();
        
        
    }

    private IEnumerator MoveToEndPosition()
    {
        anim.SetTrigger("skill");

        float duration = 1f;  
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;
        
        float currentY = transform.position.y;
        float moveDistance = 5f; 
        if (transform.localScale.x < 0)  
        {
            moveDistance = -5f;  
        }

        float leftBoundary = -6f; 
        float rightBoundary = 20f; 

        while (elapsedTime < duration)
        {
            float newXPosition = Mathf.Lerp(startingPosition.x, startingPosition.x + moveDistance, elapsedTime / duration);
            newXPosition = Mathf.Clamp(newXPosition, leftBoundary, rightBoundary);
            transform.position = new Vector3(
                newXPosition,
                currentY,
                transform.position.z 
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftBoundary, rightBoundary), currentY, transform.position.z);
        isAiming = false;
    }

    private bool DetectEnemy()
    {
        if (raycast == null)
        {
            Debug.LogError("raycast is null!");
            return false;
        }

        Vector2 direction = Vector2.right;
        if (transform.localScale.x < 0)
        {
            direction = Vector2.left;
        }

        RaycastHit2D hit = Physics2D.Raycast(raycast.transform.position, direction, rakan.getRange(), detectEnemy);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            //Detected
            Debug.DrawRay(raycast.transform.position, direction * hit.distance, Color.red);
            target = hit.collider.gameObject;
            return true;
        }
        else
        {
            //Not detected
            target = null;
            Debug.DrawRay(raycast.transform.position, direction * rakan.getRange(), Color.green);
        }
        return false;
    }

    private void Attack()
    {
        if (isAttacking)
        {
            //Debug.Log(attackCount);
            float animSpeed = rakan.getAttackSpd();
            anim.SetFloat("speed", animSpeed);
            anim.SetTrigger($"a{attackCount}");
                
        }
    }

    
    public void StartCooldownAfterSpawn()
    {
        timeCoolDown = startCooldown;
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

    public void ResetAttackState()
    {
        isAttacking = false;
    }

    public void ResetSkillAiming()
    {
        isAiming = false;
    }

    
    public void DealDamage(float multiplier)
    {
        if (target == null)
        {
            return;
        }

        if (target.CompareTag("Enemy"))
        {
            IEnemyStats enemy = target.GetComponent<IEnemyStats>();
            //XayahStats xayah = target.GetComponent<XayahStats>();
            if (enemy != null)
            {
                bool isCrit = Random.value < rakan.getCritRate();
                float finalizeDmg = rakan.getAtk() * multiplier * enemy.ReduceDmgByDef() * rakan.getDamageBoost() * (1 - (rakan.getIgnoreRes() - enemy.Res()));
                if (isCrit)
                {
                    finalizeDmg *= rakan.getCritDamage();
                }
                
                enemy.UpdateHP(-finalizeDmg);
                DamagePopUp.Create(target.transform.position, finalizeDmg, isCrit);
            }
            else
            {
                Debug.LogError("No enemy found");
            }

        }
        
        target = null;
    }
    

    private void UseUltimate()
    {
        if (Input.GetKeyDown(KeyCode.Q) && getSelected.selected && ult.currentEnergy == ult.GetMaxEnergy() && !spawnRakan.isUltActive)
        {
            anim.SetTrigger("ult");
            anim.SetBool("isUlt", true);
            ult.ResetEnergy();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && getSelected.selected && ult.currentEnergy != ult.GetMaxEnergy() && !spawnRakan.isUltActive)
        {
            Debug.Log("Not enough energy");
        }
    }



    private void UseSkill()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isAiming)
            {
                isAiming = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && getSelected.selected && skillPoint.currentSkillPoint > 0)
        {
            skillPoint.UseSkillPoint();
            ult.GainEnergySkill();
            aimEndPosition = aimLine.GetAimEndPosition();
            StartCoroutine(MoveToEndPosition());
        }
        else if (Input.GetKeyUp(KeyCode.E) && getSelected.selected && skillPoint.currentSkillPoint <= 0)
        {
            Debug.Log("Not enough Skill Point");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Enemy") && isAiming)
        {
            DealDamage(1.6f);
            ult.GainEnergySkillEachEnemy();
            rakan.currentShield += (rakan.getHP() * 0.06f + 75);
        }
    }
    public void ActivateUltState()
    {
        spawnRakan.isUltActive = true;
    }
    public void ResetUltState()
    {
        spawnRakan.isUltActive = false;
    }

    public void ActivateAura()
    {
        if (spawnRakan.isUltActive)
        {
            aura.gameObject.SetActive(true);
        }
        else
        {
            aura.gameObject.SetActive(false);
        }
    }
    private void Reset()
    {
        isAttacking = false;
        attackCount = 1;
        target = null;
        timeCoolDown = startCooldown;
        isAiming = false;
         
    }

}
