using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionShowStats : MonoBehaviour
{
    public Text Hp;
    public Text Atk;
    public Text Def;
    public Text Res;
    public Text Spd;
    public Text AtkSpd;
    public Image hpBar;

    public void UpdateStats(MeleeMinionStats stats)
    {
        if (stats == null || !stats.isAlive) return;

        Hp.text = $"{Mathf.FloorToInt(stats.currentHP)}/{Mathf.FloorToInt(stats.hp)}";
        Atk.text = Convert.ToString(stats.atk);
        Def.text = Convert.ToString(stats.def);
        Res.text = Convert.ToString(stats.res);
        Spd.text = Convert.ToString(stats.movementSpeed);
        AtkSpd.text = Convert.ToString(stats.atkSpeed);
        hpBar.fillAmount = stats.currentHP / stats.hp;
    }
}
