using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShowStats : MonoBehaviour
{
    public Text Hp;
    public Text Atk;
    public Text Def;
    public Text Energy;
    public Text CritRate;
    public Text CritDmg;
    public Text AtkSpd;
    public Text MoveSpeed;
    public Text Res;
    public Text Name;

    public Image HpBar;
    public Image TotalHPBar;

    public Image EnergyBar;
    public Image TotalEnergyBar;

    private GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void UpdateMinionStats(MeleeMinionStats minionStats)
    {
        if (gameController.currentCharacterType == CharacterType.Minion)
        {
            Name.text = "Melee Minion " + "lv."+Convert.ToString(minionStats.Res()*100);
            Hp.text = Mathf.FloorToInt(minionStats.currentHP) + "/" + Mathf.FloorToInt(minionStats.hp);
            Energy.text = "";
            Atk.text = Convert.ToString(minionStats.atk);
            Def.text = Convert.ToString(minionStats.def);
            CritRate.text = "0%";
            CritDmg.text = "0%";
            AtkSpd.text = "1";
            MoveSpeed.text = "1";
            Res.text = Convert.ToString(minionStats.Res() * 100) + "%";

            HpBar.fillAmount = minionStats.currentHP / minionStats.hp;

            EnergyBar.gameObject.SetActive(false);
            TotalEnergyBar.gameObject.SetActive(false);
        }
    }

    public void UpdateRangedMinionStats(RangedMinionSetUp rangedMinion)
    {
        if (gameController.currentCharacterType == CharacterType.Ranged_Minion)
        {
            Name.text = "Ranged Minion " + "lv." + Convert.ToString(rangedMinion.Res() * 100);
            Hp.text = Mathf.FloorToInt(rangedMinion.currentHP) + "/" + Mathf.FloorToInt(rangedMinion.getHP());
            Energy.text = "";
            Atk.text = Convert.ToString(rangedMinion.getAtk());
            Def.text = Convert.ToString(rangedMinion.getDef());
            CritRate.text = "0%";
            CritDmg.text = "0%";
            AtkSpd.text = "1";
            MoveSpeed.text = "1";
            Res.text = Convert.ToString(rangedMinion.Res() * 100) + "%";
            HpBar.fillAmount = rangedMinion.currentHP / rangedMinion.getHP();

            EnergyBar.gameObject.SetActive(false);
            TotalEnergyBar.gameObject.SetActive(false);
        }
    }

    public void UpdateSiegeMinionStats(SiegeMinionSetUp siegeMinion)
    {
        if (gameController.currentCharacterType == CharacterType.Siege_Minion)
        {
            Name.text = "Siege Minion" + "lv." + Convert.ToString(siegeMinion.Res() * 100);
            Hp.text = Mathf.FloorToInt(siegeMinion.currentHP) + "/" + Mathf.FloorToInt(siegeMinion.getHP());
            Energy.text = "";
            Atk.text = Convert.ToString(siegeMinion.getAtk());
            Def.text = Convert.ToString(siegeMinion.getDef());
            CritRate.text = "0%";
            CritDmg.text = "0%";
            AtkSpd.text = "1";
            MoveSpeed.text = "1";
            Res.text = Convert.ToString(siegeMinion.Res() * 100) + "%";
            
            HpBar.fillAmount = siegeMinion.currentHP / siegeMinion.getHP();

            EnergyBar.gameObject.SetActive(false);
            TotalEnergyBar.gameObject.SetActive(false);
        }
    }

    public void UpdateSuperMinionStats(SuperMinionSetUp superMinion)
    {
        if (gameController.currentCharacterType == CharacterType.Siege_Minion)
        {
            Name.text = "Super Minion" + "lv." + Convert.ToString(superMinion.Res() * 100);
            Hp.text = Mathf.FloorToInt(superMinion.currentHP) + "/" + Mathf.FloorToInt(superMinion.getHP());
            Energy.text = "";
            Atk.text = Convert.ToString(superMinion.getAtk());
            Def.text = Convert.ToString(superMinion.getDef());
            CritRate.text = "0%";
            CritDmg.text = "0%";
            AtkSpd.text = "1";
            MoveSpeed.text = "1";
            Res.text = Convert.ToString(superMinion.Res() * 100) + "%";
            HpBar.fillAmount = superMinion.currentHP / superMinion.getHP();

            EnergyBar.gameObject.SetActive(false);
            TotalEnergyBar.gameObject.SetActive(false);
        }
    }

    public void UpdateXayahStats(XayahStats xayahStats)
    {
        if (gameController.currentCharacterType == CharacterType.Xayah)
        {
            Name.text = "Xayah";
            Hp.text = Mathf.FloorToInt(xayahStats.currentHP) + "/" + Mathf.FloorToInt(xayahStats.getHealth());
            Energy.text = xayahStats.GetCurrentEnergy().ToString("F1") + "/" + xayahStats.GetMaxEnergy().ToString("F1");

            Atk.text = Convert.ToString(xayahStats.getAttackBuff());
            Def.text = Convert.ToString(xayahStats.getArmor());
            CritRate.text = Convert.ToString(xayahStats.getCritRate()*100) +"%";
            CritDmg.text = Convert.ToString(xayahStats.getCritDmg()*100)+"%";
            AtkSpd.text = Convert.ToString(xayahStats.getAttackSpd());
            MoveSpeed.text = Convert.ToString(xayahStats.movementSpeed);
            Res.text = Convert.ToString(xayahStats.getRes()*100)+"%";
            HpBar.fillAmount = xayahStats.currentHP / xayahStats.getHealth();
            EnergyBar.fillAmount = xayahStats.GetCurrentEnergy() / xayahStats.GetMaxEnergy();

            EnergyBar.gameObject.SetActive(true);
            TotalEnergyBar.gameObject.SetActive(true);
        }
    }

    public void UpdateRakanStats(RakanSetUp rakan)
    {
        if (gameController.currentCharacterType == CharacterType.Rakan)
        {
            
            Name.text = "Rakan";

            RakanUltimate rakanUltimate = FindObjectOfType<RakanUltimate>();

            Hp.text = Mathf.FloorToInt(rakan.currentHP) + "/" + Mathf.FloorToInt(rakan.getHP());
            Energy.text = rakanUltimate.GetCurrentEnergy().ToString("F1") + "/" + rakanUltimate.GetMaxEnergy().ToString("F1");
            Atk.text = Convert.ToString(rakan.getAtk());
            Def.text = Convert.ToString(rakan.getDef());
            CritRate.text = Convert.ToString(rakan.getCritRate() * 100) + "%";
            CritDmg.text = Convert.ToString(rakan.getCritDamage() * 100) + "%";
            AtkSpd.text = Convert.ToString(rakan.getAttackSpd());
            MoveSpeed.text = Convert.ToString(rakan.getMovementSpeed());
            Res.text = Convert.ToString(rakan.getRes() * 100) + "%";
            HpBar.fillAmount = rakan.currentHP / rakan.getHP();
            EnergyBar.fillAmount = rakanUltimate.GetCurrentEnergy() / rakanUltimate.GetMaxEnergy();

            EnergyBar.gameObject.SetActive(true);
            TotalEnergyBar.gameObject.SetActive(true);
            
        }
    }

}
