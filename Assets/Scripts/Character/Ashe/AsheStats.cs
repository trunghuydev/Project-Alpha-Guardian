using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class AsheStats : MonoBehaviour
{
    public float attack;
    public float critRate;
    public float critDmg;
    public float dmgBoost;
    public float resPen;
    public float attackSpd;

    private string basePath = "Assets/Data/Hero_stat/Ingame_stat/tower/";


    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        LoadStats();
        IncreaseAttackSpd();
        LogStats();
    }

    // Method to load all stats from files
    public void LoadStats()
    {
        attack = GetStatFromFile("Attack.txt") + 1;
        critRate = GetStatFromFile("CritRate.txt");
        critDmg = GetStatFromFile("CritDmg.txt")+1;
        dmgBoost = GetStatFromFile("DmgBoost.txt") + 1;
        resPen = GetStatFromFile("IgnoreRes.txt");
        attackSpd = GetStatFromFile("AttackSpd.txt");
    }

    private void IncreaseAttackSpd()
    {
        anim.SetFloat("speed", attackSpd);
    }

    private float GetStatFromFile(string fileName)
    {
        string path = Path.Combine(basePath, fileName);

        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
            {
                return result;
            }
            else
            {
                Debug.LogError($"Failed to parse the value in {fileName}");
                return 0f; 
            }
        }
        else
        {
            Debug.LogError($"File {fileName} not found at {path}");
            return 0f; 
        }
    }

 
    public void LogStats()
    {
        Debug.Log($"Attack: {attack}, Crit Rate: {critRate}, Crit Damage: {critDmg}, Damage Boost: {dmgBoost}, Res Pen: {resPen}, Attack Speed: {attackSpd}");
    }


}
