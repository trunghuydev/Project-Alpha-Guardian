using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class PanelStatManager : MonoBehaviour
{
    string hero1Path = "Assets/Data/hero_select/hero1.txt";
    string hero2Path = "Assets/Data/hero_select/hero2.txt";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(File.ReadAllText(hero1Path) != "")
        {
            UpdateStatInGame(File.ReadAllText(hero1Path));
        }

        if (File.ReadAllText(hero2Path) != "")
        {
            UpdateStatInGame(File.ReadAllText(hero2Path));
        }
        UpdateStatsAshe();
    }

    void UpdateStatInGame(string heroid)
    {
        string heroname;
        switch(heroid)
        {
            case "00001":
                heroname = "xayah"; 
                break;

            case "00002":
                heroname = "rakan";
                break;

            default:
                heroname = "";
                break;
        }
        UpdateBaseOnStatName("Armor", heroname);
        UpdateBaseOnStatName("Attack", heroname);
        UpdateBaseOnStatName("AttackSpd", heroname);
        UpdateBaseOnStatName("CritDmg", heroname);
        UpdateBaseOnStatName("CritRate", heroname);
        UpdateBaseOnStatName("DmgBoost", heroname);
        UpdateBaseOnStatName("Health", heroname);
        UpdateBaseOnStatName("HealthRegen", heroname);
        UpdateBaseOnStatName("IgnoreRes", heroname);
        UpdateBaseOnStatName("Mana", heroname);
        UpdateBaseOnStatName("ManaRegen", heroname);
        UpdateBaseOnStatName("Res", heroname);
        UpdateBaseOnStatName("TowerMaxHealth", "tower");

    }

    private void UpdateStatsAshe()
    {
        string heroname = "tower";
        UpdateBaseOnStatName("Attack", heroname);
        UpdateBaseOnStatName("AttackSpd", heroname);
        UpdateBaseOnStatName("CritDmg", heroname);
        UpdateBaseOnStatName("CritRate", heroname);
        UpdateBaseOnStatName("DmgBoost", heroname);
        UpdateBaseOnStatName("IgnoreRes", heroname);
    }

    void UpdateBaseOnStatName(string stat, string heroname)
    {
        string statFromCurio = "Assets/Data/Hero_stat/curio_stat/" + stat + ".txt";
        string statFromBase = "Assets/Data/Hero_stat/base_stat/" + heroname + "/" + stat + ".txt";
        string statFromError = "Assets/Data/Hero_stat/error_stat/" + stat + ".txt";
        string statFromPotion = "Assets/Data/Hero_stat/potion_stat/" + stat + ".txt";
        string statFromConverted = "Assets/Data/Hero_stat/converted_stat/" + heroname + "/" + stat + ".txt";
        string statInGame = "Assets/Data/Hero_stat/ingame_stat/" + heroname + "/" + stat + ".txt";


        // Read the content of the files
        string statBaseContent = File.ReadAllText(statFromBase);
        string statCurioContent = File.ReadAllText(statFromCurio);
        string statErrorContent = File.ReadAllText(statFromError);
        string statPotionContent = File.ReadAllText(statFromPotion);
        string statConvertedContent = File.ReadAllText(statFromConverted);

        // Parse the content to float
        float.TryParse(statBaseContent, out float statBase);
        float.TryParse(statCurioContent, out float statCurio);
        float.TryParse(statErrorContent, out float statError);
        float.TryParse(statPotionContent, out float statPotion);
        float.TryParse(statConvertedContent, out float statConverted);


        float statUpdate = statBase + statCurio + statError + statPotion + statConverted;


        // Write the updated stat to the file
        File.WriteAllText(statInGame, statUpdate.ToString());

    }

    
}
