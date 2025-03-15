using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SiegeMinionSetUp : MonoBehaviour, IEnemyStats
{
    public Image currentHpImage;
    public float currentHP;
    SiegeMinionStats siegeMinion;
    private GameController gameController;
    private ShowStats showStats;
    private Animator anim;
    private EnemySpawner spawner;
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        showStats = FindObjectOfType<ShowStats>();
        anim = GetComponent<Animator>();
        spawner = FindObjectOfType<EnemySpawner>();
    }
    private void Start()
    {
        siegeMinion = GetComponent<SiegeMinionStats>();
        SetHP();
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
        currentHP = siegeMinion.hp;
    }
    public void UpdateHP(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0f, siegeMinion.hp); //When update hp, this wont make hp overflow the max HP, ex: when healing
        UpdateHPBar();

        if (gameController.selectedMinion == this && gameController.currentCharacterType == CharacterType.Siege_Minion)
        {
            showStats.UpdateSiegeMinionStats(this);
        }

        if (currentHP <= 0f)
        {
            if (gameController.selectedSiegeMinion == this && gameController.currentCharacterType == CharacterType.Siege_Minion)
            {
                gameController.HideStats();
            }
            anim.SetTrigger("death");
            Invoke("DestroySiegeMinion",1f);
            
        }
    }

    private void DestroySiegeMinion()
    {
        Destroy(gameObject);
    }

    public void UpdateHPBar()
    {
        float targetFillAmount = currentHP / siegeMinion.hp;
        currentHpImage.fillAmount = targetFillAmount;
    }

    public float getHP()
    {
        return siegeMinion.getHP();
    }

    public float getDef()
    {
        return siegeMinion.getDef();
    }

    public float getAtk()
    {
        return siegeMinion.getAtk();
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
        return siegeMinion.getRes();
    }

    public void DebuffDecreaseDef(float amount)
    {
        if (siegeMinion.def > 0)
        {
            siegeMinion.def = Mathf.Max(siegeMinion.def - amount, 0);
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
