using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedMinionSetUp : MonoBehaviour, IEnemyStats
{
    public Image currentHpImage;
    public float currentHP;
    RangedMinionStats rangedMinion;
    private GameController gameController;
    private ShowStats showStats;
    private EnemySpawner spawner;
    Animator anim;
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        showStats = FindObjectOfType<ShowStats>();
        spawner = FindObjectOfType<EnemySpawner>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        rangedMinion = GetComponent<RangedMinionStats>();
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
        currentHP = rangedMinion.hp;
    }
    public void UpdateHP(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0f, rangedMinion.hp); //When update hp, this wont make hp overflow the max HP, ex: when healing
        UpdateHPBar();

        if (gameController.selectedMinion == this && gameController.currentCharacterType == CharacterType.Ranged_Minion)
        {
            showStats.UpdateRangedMinionStats(this);
        }

        if (currentHP <= 0f)
        {
            if (gameController.selectedRangedMinion == this && gameController.currentCharacterType == CharacterType.Ranged_Minion)
            {
                gameController.HideStats();
            }
            anim.SetTrigger("death");
            Invoke("DestroyRangedMinion", 1f);
        }

    }

    private void DestroyRangedMinion()
    {
        Destroy(gameObject);
    }

    public void UpdateHPBar()
    {
        float targetFillAmount = currentHP / rangedMinion.hp;
        currentHpImage.fillAmount = targetFillAmount;
    }

    public float getHP()
    {
        return rangedMinion.getHP();
    }

    public float getDef()
    {
        return rangedMinion.getDef();
    }

    public float getAtk()
    {
        return rangedMinion.getAtk();
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
        return rangedMinion.getRes();
    }

    public void DebuffDecreaseDef(float amount)
    {
        if (rangedMinion.def > 0)  
        {
            rangedMinion.def = Mathf.Max(rangedMinion.def - amount, 0);
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
