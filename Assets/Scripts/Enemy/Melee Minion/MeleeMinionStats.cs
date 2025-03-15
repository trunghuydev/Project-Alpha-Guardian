using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeMinionStats : MonoBehaviour, IEnemyStats
{
    [Header("HP")]
    [SerializeField] public float hp;//Max HP
    [SerializeField] public Image hpBar; //Drag Hp image to this in editor
    public float currentHP { get; private set; }

    [Header("Defense")]
    [SerializeField] public float def;

    [Header("Attack")]
    [SerializeField] public float atk;

    [SerializeField] public float res;
    [SerializeField] public float atkSpeed;
    [SerializeField] public float movementSpeed;

    public bool isAlive = true;

    private ShowStats showStats;

    private GameController gameController;
    private Animator anim;
    private EnemySpawner spawner;

    //detect Tower
    TowerStats tower;
    private void Awake()
    {
        tower = FindObjectOfType<TowerStats>();   
        gameController = FindObjectOfType<GameController>();
        showStats = FindObjectOfType<ShowStats>();
        anim = GetComponent<Animator>();
        spawner = FindObjectOfType<EnemySpawner>();
    }
    private void OnMouseDown()
    {
        if (currentHP > 0)
        {
            gameController.SetSelectedMinion(this);
            gameController.ShowStats();
        }
    }

    //HP system
    public void SetHP()
    {
        currentHP = hp;
    }

    public void UpdateHP(float amount)
    {
        //if (!isAlive) return;

        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0f, hp); //When update hp, this wont make hp overflow the max HP, ex: when healing
        UpdateHPBar();

        //Only update stats panel if get selected
        if (gameController.selectedMinion == this && gameController.currentCharacterType == CharacterType.Minion)
        {
            showStats.UpdateMinionStats(this);
        }

        if (currentHP <= 0f)
        {
            anim.SetTrigger("death");   
            if(gameController.selectedMinion == this && gameController.currentCharacterType == CharacterType.Minion)
            {
                gameController.HideStats();
            }
            //isAlive = false;
            Invoke("DestroyMinion", 1f);
        }
    }

    public void UpdateHPBar()
    {
        float targetFillAmount = currentHP / hp;
        hpBar.fillAmount = targetFillAmount;
    }

    private void DestroyMinion()
    {
        Destroy(gameObject);
    }

    public void DamageTower()
    {
        if (!isAlive) return;
        tower.UpdateHP(-atk);
    
    }
    //Def
    
    public float ReduceDmgByDef()
    {
        float G, H; //H is dmg received, G is dmg reduced
        G = def / (def + 100);
        H = 1 - G;
        return H;
    }

    //Trong thương
    public float Vul()
    {
        float vul = 2f; //Trong thương 100%
        return vul;
    }

    //Miễn st
    public float Res()
    {
        return res;
    }

    public void DebuffDecreaseDef(float amount)
    {
        if (def > 0)
        {
            def = Mathf.Max(def - amount, 0);
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
