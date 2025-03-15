using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ShowStatOfMember : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject StatPanel;

    public GameObject hero1Button;
    public GameObject hero2Button;
    void Start()
    {
        string heroid = "Assets/Data/hero_select/hero1.txt";
        string heroid2 = "Assets/Data/hero_select/hero2.txt";

        if (File.Exists(heroid) && File.ReadAllText(heroid) != "")
        {
            hero1Button.SetActive(true);
        }
        else
        {
            hero1Button.SetActive(false);
        }
        if (File.Exists(heroid2) && File.ReadAllText(heroid2) != "")
        {
            hero2Button.SetActive(true);
        }
        else
        {
            hero2Button.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowStatOfMemberToPanel(string heroIndex)
    {
        string heroname;
        string heroid = File.ReadAllText("Assets/Data/hero_select/" + heroIndex + ".txt");
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

        UpdateToPanel("Armor", heroname);
        UpdateToPanel("Attack", heroname);
        UpdateToPanel("AttackSpd", heroname);
        UpdateToPanel("Health", heroname);
        UpdateToPanel("HealthRegen", heroname);
        UpdateToPanel("Mana", heroname);
        UpdateToPanel("CritDmg", heroname);
        UpdateToPanel("CritRate", heroname);
        UpdateToPanel("DmgBoost", heroname);
        UpdateToPanel("ManaRegen", heroname);
        UpdateToPanel("IgnoreRes", heroname);
        UpdateToPanel("Res", heroname);
    }

    void UpdateToPanel(string stat, string heroname)
    {
        switch (stat)
        {
            
            case "Attack":
            case "Health":
            
            case "Mana":
                float stringText = float.Parse(File.ReadAllText("Assets/Data/Hero_stat/ingame_stat/" + heroname + "/" + stat + ".txt"));
                StatPanel.transform.Find(stat).transform.Find("BG").transform.Find("Stat").GetComponent<TextMeshProUGUI>().text = stringText.ToString("0");
                break;
            case "AttackSpd":
                stringText = float.Parse(File.ReadAllText("Assets/Data/Hero_stat/ingame_stat/" + heroname + "/" + stat + ".txt"));
                StatPanel.transform.Find(stat).transform.Find("BG").transform.Find("Stat").GetComponent<TextMeshProUGUI>().text = stringText.ToString("0.000");
                break;
            case "Armor":
            case "HealthRegen":
                stringText = float.Parse(File.ReadAllText("Assets/Data/Hero_stat/ingame_stat/" + heroname + "/" + stat + ".txt"));
                StatPanel.transform.Find(stat).transform.Find("BG").transform.Find("Stat").GetComponent<TextMeshProUGUI>().text = stringText.ToString("0.0");
                break;

            case "CritDmg":
            case "CritRate":
            case "DmgBoost":
            case "ManaRegen":
            case "IgnoreRes":
            case "Res":
                string data = File.ReadAllText("Assets/Data/Hero_stat/ingame_stat/" + heroname + "/" + stat + ".txt");
                float.TryParse(data, out float statNum);
                StatPanel.transform.Find(stat).transform.Find("BG").transform.Find("Stat").GetComponent<TextMeshProUGUI>().text = (statNum * 100).ToString("0.0") + "%";
                break;
        }
        
    }
}
