using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.IO;
using System.Globalization;

public class XayahStats : MonoBehaviour, IPlayerStats
{
    #region Base Stats
    //Character base stats
    
    public Image hpBar;
    public float currentHP;
    public float atkSpeed;
    public float movementSpeed;
    private bool isHealing = false;
    public float shield;
    public Image currentShieldImage;
    public float currentShield;
    #endregion

    #region Buffed Stats
    public float armor;
    public float attack;
    public float attackSpd;
    public float critDmgBuff;
    public float critRateBuff;
    public float dmgBoost;
    public float health;
    public float healthRegen;
    public float ignoreRes;
    public float mana;
    public float manaRegen;
    public float resBuff;
    #endregion

    //Character base stats + Chrono card
    public float baseAtk;
    public float baseDef;
    public float baseHp;

    //invicible after spawn
    private bool isInvincible = false;

    public TheThoiKhong theThoiKhong;
    private string chronoCardPath = "Assets/Data/Hero Equipments/Xayah/chronoCard.txt"; // File path for chrono card state
    private string chronoCardID;

    

    private Animator anim;
    private GameController gameController;
    private ShowStats showStats;
    private XayahUltimate xayahUltimate;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
        showStats = FindObjectOfType<ShowStats>();
        xayahUltimate = GetComponent<XayahUltimate>();
    }

    private void Start()
    {
        GetChronoCard();
        ApplyLightConeEffects();
        SetHP();
        StartHealing();
        StartCoroutine(InvincibilityCoroutine());

    }

    private void OnMouseDown()
    {
        if(currentHP > 0)
        {
            gameController.SetSelectedXayah(this);
            gameController.ShowStats();
        }
    }

    public void SetHP()
    {
        currentHP = getHealth();
    }

    public void SetShield()
    {
        
    }

    public void UpdateHP(float amount) 
    {
        if (isInvincible)
        {
            return;
        }

        //float shieldDamage = Mathf.Min(amount * -1, currentShield);
        //currentShield -= shieldDamage;
        //amount += shieldDamage;


        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0f, getHealth()); //When update hp, this wont make hp overflow the max HP, ex: when healing
        currentShield = Mathf.Clamp(currentShield, 0f, shield);
        UpdateHPBar();

        if (currentHP <= 0f)
        {
            if (gameController.selectedXayah == this && gameController.currentCharacterType == CharacterType.Xayah)
            {
                gameController.HideStats();
            }
            anim.SetTrigger("death");
            Invoke("Death", 1f);

        }

        //Only update stats panel if get selected
        if (gameController.selectedXayah == this && gameController.currentCharacterType == CharacterType.Xayah)
        {
            showStats.UpdateXayahStats(this);
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
            UpdateHP(getHealthRegen()); 
            yield return new WaitForSeconds(1f); 
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        //Debug.Log("Xayah is invincible for " + 3 + " seconds.");

        yield return new WaitForSeconds(3f); 

        isInvincible = false;
        //Debug.Log("Xayah is no longer invincible.");
    }

    public void UpdateHPBar()
    {
        if (currentShield > 0)
        {
            float total = currentShield + currentHP;
            float shieldFillAmount = currentShield / total;
            float hpFillAmount = currentHP / total;

            currentShieldImage.fillAmount = shieldFillAmount;
            hpBar.fillAmount = hpFillAmount;
        }
        else
        {
            float targetFillAmount = currentHP / getHealth();
            hpBar.fillAmount = targetFillAmount;
            currentShieldImage.fillAmount = 0;
        }

    }

    public void Death()
    {
        Destroy(gameObject);
        SpawnCharacter spawnCharacterScript = FindObjectOfType<SpawnCharacter>();
        if (spawnCharacterScript != null)
        {
            spawnCharacterScript.currentCharacter = null;  // Reset the reference in the SpawnCharacter script
            spawnCharacterScript.StartCooldown();
        }
    }

    public void ApplyLightConeEffects()
    {
        if(theThoiKhong != null)
        {
            health += theThoiKhong.baseHp;
            attack += theThoiKhong.baseAtk;
            armor += theThoiKhong.baseDef;
            //Debug.Log($"Equipped Light Cone: {theThoiKhong.lightConeName}");
            //Debug.Log($"Attack: {baseAtk}, Defense: {baseDef}, Hp: {baseHp}");
        }
        else
        {
            health += 0;
            attack += 0;
            armor += 0;
            //Debug.Log($"Attack: {baseAtk}, Defense: {baseDef}, Hp: {baseHp}");
        }
    }

    private void GetChronoCard()
    {
        string libraryPath = "Assets/Data/Library/ChronoCard";
        string xayahPath = "Assets/Data/Hero Equipments/Xayah";
        //string chronoCard = "";

        if (Directory.Exists(xayahPath))
        {
            string path = xayahPath + "/chronoCard.txt";
            //chronoCard = File.ReadAllText(path).Trim(); 
            chronoCardID = File.ReadAllText(path).Trim();//Get id of card
            Debug.Log("Loaded ChronoCard ID: " + chronoCardID);
            if (chronoCardID != "0")
            {
                TheThoiKhong lightCone = Resources.Load<TheThoiKhong>("ChronoCard/" + chronoCardID);
                if (lightCone != null)
                {
                    theThoiKhong = lightCone;
                    ApplyLightConeEffects();  // Apply the effects after loading
                }
            }
            else
            {
                theThoiKhong = null;
                ApplyLightConeEffects();
            }
        }
        else
        {
            Debug.Log("does not find chrono card path");
        }
    }

    public void EquipLightCone(TheThoiKhong newLightCone)
    {
        theThoiKhong = newLightCone;
        File.WriteAllText(chronoCardPath, newLightCone.ID);  
        ApplyLightConeEffects();
        SetHP();
        Debug.Log($"Equipped {newLightCone.lightConeName} (ID: {newLightCone.ID})");
    }

    public void UnequipLightCone()
    {
        theThoiKhong = null;
        File.WriteAllText(chronoCardPath, "0"); 
        ApplyLightConeEffects();
        SetHP();
        Debug.Log("Unequipped Light Cone.");
    }

    public float GetCurrentEnergy()
    {
        return xayahUltimate.GetCurrentEnergy();
    }

    public float GetMaxEnergy()
    {
        return xayahUltimate.GetMaxEnergy();
    }

    public float getAttackBuff()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/xayah/Attack.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if(float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out attack))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
            
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return attack;
    }

    public float getArmor()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/xayah/Armor.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out armor))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
            
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return armor;
    }

    public float getAttackSpd()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/xayah/AttackSpd.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out attackSpd))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }

        }
        else
        {
            Debug.LogError("Path not found");
        }
        return attackSpd;
    }

    public float getCritDmg()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/xayah/CritDmg.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out critDmgBuff))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
            
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return critDmgBuff+1;
    }

    public float getCritRate()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/xayah/CritRate.txt";
        
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path).Trim();
            fileContent = fileContent.Replace(',', '.');
            if(float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out critRateBuff))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }

        }
        else
        {
            Debug.LogError("Path not found");
        }
        return critRateBuff;
    }

    public float getDmgBoost()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/xayah/DmgBoost.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out dmgBoost))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return dmgBoost+1;
    }

    public float getHealth()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/xayah/Health.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out health))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
            
        }
        else
        {
            Debug.LogError("Path not found");
        }

        return health;
    }

    public float getHealthRegen()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/xayah/HealthRegen.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out healthRegen))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return healthRegen;
    }

    public float getIgnoreRes()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/xayah/IgnoreRes.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out ignoreRes))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return ignoreRes;
    }

    public float getMana()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/xayah/Mana.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out mana))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return mana;
    }

    public float getManaRegen()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/xayah/ManaRegen.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out manaRegen))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
            
        }
        else
        {
            Debug.LogError("Path not found");
        }

        return manaRegen;
    }

    public float getRes()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/xayah/Res.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out resBuff))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return resBuff;
    }
    public float ReduceDmgByDef()
    {
        float G, H; //H is dmg received, G is dmg reduced
        G = getArmor() / (getArmor() + 100);
        H = 1 - G;
        return H;
    }

    
}
