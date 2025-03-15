using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class ErrorEffectManager : MonoBehaviour
{
    string pathArmor = "Assets/Data/Hero_stat/error_stat/Armor.txt";
    string pathAttack = "Assets/Data/Hero_stat/error_stat/Attack.txt";
    string pathAttackSpd = "Assets/Data/Hero_stat/error_stat/AttackSpd.txt";
    string pathCritDmg = "Assets/Data/Hero_stat/error_stat/CritDmg.txt";
    string pathCritRate = "Assets/Data/Hero_stat/error_stat/CritRate.txt";
    string pathDmgBoost = "Assets/Data/Hero_stat/error_stat/DmgBoost.txt";
    string pathHealth = "Assets/Data/Hero_stat/error_stat/Health.txt";
    string pathHealthRegen = "Assets/Data/Hero_stat/error_stat/HealthRegen.txt";
    string pathIgnoreRes = "Assets/Data/Hero_stat/error_stat/IgnoreRes.txt";
    string pathMana = "Assets/Data/Hero_stat/error_stat/Mana.txt";
    string pathManaRegen = "Assets/Data/Hero_stat/error_stat/ManaRegen.txt";
    string pathRes = "Assets/Data/Hero_stat/error_stat/Res.txt";
    string pathTowerMaxHealth = "Assets/Data/Hero_stat/error_stat/TowerMaxHealth.txt";
    string pathTowerCurrentHealth = "Assets/Data/Hero_stat/ingame_stat/tower/TowerCurrentHealth.txt";




    string errorData = "Assets/Data/ingame_data/error";

    void Start()
    {
        CheckStatFolder();
    }

    // Update is called once per frame
    void Update()
    {
        GetStatFromError();
    }

    void CheckStatFolder()
    {
        CheckFileExist(pathArmor);
        CheckFileExist(pathAttack);
        CheckFileExist(pathAttackSpd);
        CheckFileExist(pathCritDmg);
        CheckFileExist(pathCritRate);
        CheckFileExist(pathDmgBoost);
        CheckFileExist(pathHealth);
        CheckFileExist(pathHealthRegen);
        CheckFileExist(pathIgnoreRes);
        CheckFileExist(pathMana);
        CheckFileExist(pathManaRegen);
        CheckFileExist(pathRes);
        CheckFileExist(pathTowerMaxHealth);
        CheckFileExist(pathTowerCurrentHealth);
    }

    void CheckFileExist(string path)
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "0");
        }
    }

    void GetStatFromError()
    {
        string[] dataFiles = Directory.GetFiles(errorData).Where(f => !f.EndsWith(".meta")).ToArray();

        float towerMaxHealth = 0;

        float Armor = 0;
        float Attack = 0;
        float AttackSpd = 0;
        float CritDmg = 0;
        float CritRate = 0;
        float DmgBoost = 0;
        float Health = 0;
        float HealthRegen = 0;
        float IgnoreRes = 0;
        float Mana = 0;
        float ManaRegen = 0;
        float Res = 0;

        foreach (string file in dataFiles)
        {
            string errorName = Path.GetFileNameWithoutExtension(file);
            int errorid = ExtractNumberFromString(errorName);

            switch (errorid)
            {
                case 1:
                    CritRate += -0.05f;
                    CritDmg += -0.5f;
                    break;
                case 2:
                    ManaRegen += -0.5f;
                    HealthRegen += -50f;
                    break;
                case 3:
                    DmgBoost += -0.4f;
                    break;
                case 4:
                    Armor += -20f;
                    Res += -0.08f;
                    break;
                case 5:
                    AttackSpd += -0.3f;
                    break;
            }                
        }

        AssignNewStatToErrorPath(pathArmor, Armor);
        AssignNewStatToErrorPath(pathAttack, Attack);
        AssignNewStatToErrorPath(pathAttackSpd, AttackSpd);
        AssignNewStatToErrorPath(pathCritDmg, CritDmg);
        AssignNewStatToErrorPath(pathCritRate, CritRate);
        AssignNewStatToErrorPath(pathDmgBoost, DmgBoost);
        AssignNewStatToErrorPath(pathHealth, Health);
        AssignNewStatToErrorPath(pathHealthRegen, HealthRegen);
        AssignNewStatToErrorPath(pathIgnoreRes, IgnoreRes);
        AssignNewStatToErrorPath(pathMana, Mana);
        AssignNewStatToErrorPath(pathManaRegen, ManaRegen);
        AssignNewStatToErrorPath(pathRes, Res);

        AssignNewStatToErrorPath(pathTowerMaxHealth, towerMaxHealth);
    }

    public void AssignNewStatToErrorPath(string path, float newValue)
    {
        File.WriteAllText(path, newValue.ToString());
    }

    public void StatMember()
    {

    }

    public static int ExtractNumberFromString(string input)
    {
        string numberString = Regex.Match(input, @"\d+").Value;
        return int.Parse(numberString);
    }
}
