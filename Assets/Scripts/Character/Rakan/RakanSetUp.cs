using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RakanSetUp : MonoBehaviour, IPlayerStats
{
    public Image currentHpImage;
    public float currentHP;
    public Image currentShieldImage;
    public float currentShield;

    RakanStats rakan;
    private GameController gameController;
    private ShowStats showStats;
    private Animator anim;
    private RakanUltimate rakanUlt;
    private bool isShieldBreak = false;

    public Image shieldInitiante;
    public Image shieldBreak;
    private bool isInvincible = false;
    private bool isHealing = false;

    //Character base stats + Chrono card
    public float baseAtk;
    public float baseDef;
    public float baseHp;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        showStats = FindObjectOfType<ShowStats>();
        anim = GetComponent<Animator>();
        rakanUlt = GetComponent<RakanUltimate>();
    }
    private void Start()
    {
        rakan = GetComponent<RakanStats>();
        StartCoroutine(InvincibilityCoroutine());
        ShieldInitiante();
        SetHP();
        currentShield += SetShield();
        UpdateHPBar();
        StartHealing();
    }

    float timer = 0;
    private void Update()
    {
        if (isShieldBreak)
        {

            timer += Time.deltaTime;
            if (timer >= 40)
            {
                timer = 0;
                currentShield += SetShield();
                UpdateHPBar();
                ShieldInitiante();
            }
        }

    }

    private void OnMouseDown()
    {
        if (currentHP > 0)
        {
            gameController.SetSelectedRakan(this);
            gameController.ShowStats();
        }
    }

    private void SetHP()
    {
        currentHP = rakan.getHealth();
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        //Debug.Log("Xayah is invincible for " + 3 + " seconds.");

        yield return new WaitForSeconds(3f);

        isInvincible = false;
        //Debug.Log("Xayah is no longer invincible.");
    }
    private float SetShield()
    {
        return rakan.getHealth() * 0.18f;
    }
    public void UpdateHP(float amount)
    {
        if (isInvincible)
        {
            return;
        }

        if (currentShield > 0)
        {
            
            float shieldDamage = Mathf.Min(amount * -1, currentShield);
            currentShield -= shieldDamage;
            amount += shieldDamage;  
        }

        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0f, rakan.getHealth()); //When update hp, this wont make hp overflow the max HP, ex: when healing
        currentShield = Mathf.Clamp(currentShield, 0f, rakan.shield);
        UpdateHPBar();

        if (gameController.selectedRakan == this && gameController.currentCharacterType == CharacterType.Rakan)
        {
            showStats.UpdateRakanStats(this);
        }

        if (currentShield <= 0)
        {
            ShieldBreak();
        }

        if (currentHP <= 0f)
        {
            if (gameController.selectedRakan == this && gameController.currentCharacterType == CharacterType.Rakan)
            {
                gameController.HideStats();
            }
            anim.SetTrigger("death");
            Invoke("Death", 1f);

        }
    }

    private void StartHealing()
    {
        if (!isHealing)
        {
            isHealing = true;
            StartCoroutine(HealOverTimeCoroutine());
        }
    }

    private IEnumerator HealOverTimeCoroutine()
    {
        while (currentHP > 0)
        {
            UpdateHP(GetHealthRegen());
            yield return new WaitForSeconds(1f);
        }
    }
    public void Death()
    {
        Destroy(gameObject);
        SpawnRakan spawnCharacterScript = FindObjectOfType<SpawnRakan>();
        if (spawnCharacterScript != null)
        {
            spawnCharacterScript.currentCharacter = null;  // Reset the reference in the SpawnCharacter script
            spawnCharacterScript.StartCooldown();
        }
    }

    

    public void UpdateHPBar()
    {
        if (currentShield > 0)
        {
            float total = currentShield + currentHP;
            float shieldFillAmount = currentShield / total;
            float hpFillAmount = currentHP / total;

            currentShieldImage.fillAmount = shieldFillAmount;
            currentHpImage.fillAmount = hpFillAmount;
        }
        else
        {
            float targetFillAmount = currentHP / rakan.getHealth();
            currentHpImage.fillAmount = targetFillAmount;
            currentShieldImage.fillAmount = 0;
        }

    }

    private void ShieldInitiante()
    {
        shieldInitiante.gameObject.SetActive(true);
        shieldBreak.gameObject.SetActive(false);
        isShieldBreak = false;
    }

    private void ShieldBreak()
    {
        shieldBreak.gameObject.SetActive(true);
        shieldInitiante.gameObject.SetActive(false);
        isShieldBreak = true;
        
        
    }
    
    public float getHP()
    {
        return rakan.getHealth();
    }

    public float getDef()
    {
        return rakan.getArmor();
    }

    public float getAtk()
    {
        return rakan.getAttackBuff();
    }

    public float getRange()
    {
        return rakan.getRange();
    }

    public float getCritRate()
    {
        return rakan.getCritRate();
    }

    public float getCritDamage()
    {
        return rakan.getCritDmg();
    }

    public float getDamageBoost()
    {
        return rakan.getDmgBoost();
    }
    public float getIgnoreRes()
    {
        return rakan.getIgnoreRes();
    }
    public float getRes()
    {
        return rakan.getRes();
    }
    public float getMovementSpeed()
    {
        return rakan.getMovementSpeed();
    }

    public float getAttackSpd()
    {
        return rakan.getAttackSpd();
    }

    public float GetCurrentEnergy()
    {
        return rakanUlt.GetCurrentEnergy();
    }

    public float GetMaxEnergy()
    {
        return rakanUlt.GetMaxEnergy();
    }

    public float GetHealthRegen()
    {
        return rakan.getHealthRegen();
    }
    public float ReduceDmgByDef()
    {
        float G, H; //H is dmg received, G is dmg reduced
        G = getDef() / (getDef() + 100);
        H = 1 - G;
        return H;
    }


}
