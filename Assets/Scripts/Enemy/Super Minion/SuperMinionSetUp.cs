using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperMinionSetUp : MonoBehaviour, IEnemyStats
{
    public Image currentHpImage;
    public float currentHP;
    public Image currentShieldImage;
    public float currentShield;

    SuperMinionStats superMinion;
    private GameController gameController;
    private ShowStats showStats;
    private EnemySpawner spawner;

    public Image shieldInitiante;
    public Image shieldBreak;

    private bool isShieldBreak = false;
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        showStats = FindObjectOfType<ShowStats>();
        spawner = FindObjectOfType<EnemySpawner>();
    }
    private void Start()
    {
        superMinion = GetComponent<SuperMinionStats>();
        ShieldInitiante();
        SetHP();
        SetShield();
        UpdateHPBar();
    }
    float timer = 0;
    private void Update()
    {
        if (isShieldBreak)
        {
            
            timer += Time.deltaTime;
            if(timer >= 10)
            {
                timer = 0;
                SetShield();
                UpdateHPBar();
                ShieldInitiante();
            }
            Debug.Log(timer);
        }
        
    }



    private void OnMouseDown()
    {
        if (currentHP > 0)
        {
            gameController.SetSelectedMinion(this);
            gameController.ShowStats();
        }
    }

    private void SetHP()
    {
        currentHP = superMinion.hp;
    }

    private void SetShield()
    {
        currentShield = superMinion.shield;
    }

    public void UpdateHP(float amount)
    {       
        float shieldDamage = Mathf.Min(amount * -1, currentShield); 
        currentShield -= shieldDamage;
        amount += shieldDamage;    

        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0f, superMinion.hp); //When update hp, this wont make hp overflow the max HP, ex: when healing
        currentShield = Mathf.Clamp(currentShield, 0f, superMinion.shield);
        UpdateHPBar();

        if (gameController.selectedSuperMinion == this && gameController.currentCharacterType == CharacterType.Super_Minion)
        {
            showStats.UpdateSuperMinionStats(this);
        }

        if(currentShield <= 0)
        {
            ShieldBreak();
        }

        if (currentHP <= 0f)
        {
            if (gameController.selectedMinion == this && gameController.currentCharacterType == CharacterType.Super_Minion)
            {
                gameController.HideStats();
            }
            Destroy(gameObject);
        }

    }

    public void UpdateHPBar()
    {
        if(currentShield > 0)
        {
            float total = currentShield + currentHP;
            float shieldFillAmount = currentShield / total;
            float hpFillAmount = currentHP / total;

            currentShieldImage.fillAmount = shieldFillAmount;
            currentHpImage.fillAmount = hpFillAmount;
        }
        else
        {
            float targetFillAmount = currentHP / superMinion.hp;
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
        return superMinion.getHP();
    }

    public float getDef()
    {
        return superMinion.getDef();
    }

    public float getAtk()
    {
        return superMinion.getAtk();
    }

    public float ReduceDmgByDef()
    {
        float G, H; //H is dmg received, G is dmg reduced
        G = getDef() / (getDef() + 100);
        H = 1 - G;
        return H;
    }

    public float Res()
    {
        return superMinion.getRes();
    }

    public void DebuffDecreaseDef(float amount)
    {
        if (superMinion.def > 0)
        {
            superMinion.def = Mathf.Max(superMinion.def - amount, 0);
        }
    }
    private bool isDebuff;
    public bool HasDebuff()
    {
        return isDebuff;
    }
    public void hasApplyDebuff(bool state)
    {
        isDebuff = state;
    }
    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.RemoveEnemyFromList(this.gameObject);
        }
    }
}
