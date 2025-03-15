using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class ConvertedStatManager : MonoBehaviour
{

    string hero1Path = "Assets/Data/hero_select/hero1.txt";
    string hero2Path = "Assets/Data/hero_select/hero2.txt";


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (File.ReadAllText(hero1Path) != "")
        {
            UpdateStatInGame(File.ReadAllText(hero1Path));
        }

        if (File.ReadAllText(hero2Path) != "")
        {
            UpdateStatInGame(File.ReadAllText(hero2Path));
        }
    }

    void UpdateStatInGame(string heroid)
    {
        string heroname;
        switch (heroid)
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
    }

    void UpdateBaseOnStatName(string stat, string heroname)
    {
        string statFromConverted = "Assets/Data/Hero_stat/converted_stat/" + heroname + "/" + stat + ".txt";

        string newValue = CalculatedStat(stat, heroname);
        
        File.WriteAllText(statFromConverted, newValue);
    }

    string CalculatedStat(string stat, string heroname)
    {
        string statFromCurio = "Assets/Data/Hero_stat/curio_stat/" + stat + ".txt";
        string statFromBase = "Assets/Data/Hero_stat/base_stat/" + heroname + "/" + stat + ".txt";
        string statFromError = "Assets/Data/Hero_stat/error_stat/" + stat + ".txt";
        string statFromPotion = "Assets/Data/Hero_stat/potion_stat/" + stat + ".txt";

        // Read the content of the files
        string statBaseContent = File.ReadAllText(statFromBase);
        string statCurioContent = File.ReadAllText(statFromCurio);
        string statErrorContent = File.ReadAllText(statFromError);
        string statPotionContent = File.ReadAllText(statFromPotion);

        // Parse the content to float
        float.TryParse(statBaseContent, out float statBase);
        float.TryParse(statCurioContent, out float statCurio);
        float.TryParse(statErrorContent, out float statError);
        float.TryParse(statPotionContent, out float statPotion);

        float statSum = statBase + statCurio + statError + statPotion;

        string statConverted = "0";

        switch (stat)
        {
            case "CritRate":
                if (statSum > 1)
                {
                    float overflowCritRate = statSum - 1;
                    statConverted = (-overflowCritRate).ToString();
                }
                break;

            case "CritDmg":
                float overCritRate = float.Parse(File.ReadAllText("Assets/Data/Hero_stat/converted_stat/" + heroname + "/CritRate.txt"));
                statConverted = (-overCritRate * 2).ToString();
                break;

            case "AtkSpd":
                if (statSum > 5)
                {
                    float overflowAtkSpd = statSum - 5;
                    statConverted = (-overflowAtkSpd).ToString();
                }
                break;

            case "Res":
                if (statSum > 0.5)
                {
                    float overflowRes = statSum - 0.5f;
                    statConverted = (-overflowRes).ToString();
                }
                break;

            case "Armor":
                float overRes = float.Parse(File.ReadAllText("Assets/Data/Hero_stat/converted_stat/" + heroname + "/Res.txt"));
                statConverted = (-overRes * 200).ToString();
                break;

            default:
                break;
        }

        return statConverted;
    }

    public void AssignNewStatToPotionPath(string path, float newValue)
    {
        File.WriteAllText(path, newValue.ToString());
    }

    public static int ExtractNumberFromString(string input)
    {
        string numberString = Regex.Match(input, @"\d+").Value;
        return int.Parse(numberString);
    }
}