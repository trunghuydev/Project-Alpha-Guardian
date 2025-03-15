using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class PotionStatManager : MonoBehaviour
{
    // Start is called before the first frame update
    string pathArmor = "Assets/Data/Hero_stat/potion_stat/Armor.txt";
    string pathAttack = "Assets/Data/Hero_stat/potion_stat/Attack.txt";
    string pathAttackSpd = "Assets/Data/Hero_stat/potion_stat/AttackSpd.txt";
    string pathCritDmg = "Assets/Data/Hero_stat/potion_stat/CritDmg.txt";
    string pathCritRate = "Assets/Data/Hero_stat/potion_stat/CritRate.txt";
    string pathDmgBoost = "Assets/Data/Hero_stat/potion_stat/DmgBoost.txt";
    string pathHealth = "Assets/Data/Hero_stat/potion_stat/Health.txt";
    string pathHealthRegen = "Assets/Data/Hero_stat/potion_stat/HealthRegen.txt";
    string pathIgnoreRes = "Assets/Data/Hero_stat/potion_stat/IgnoreRes.txt";
    string pathMana = "Assets/Data/Hero_stat/potion_stat/Mana.txt";
    string pathManaRegen = "Assets/Data/Hero_stat/potion_stat/ManaRegen.txt";
    string pathRes = "Assets/Data/Hero_stat/potion_stat/Res.txt";




    string potionData = "Assets/Data/ingame_data/potion";

    void Start()
    {
        CheckStatFolder();
    }

    // Update is called once per frame
    void Update()
    {
        GetStatFromPotion();
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
    }

    void CheckFileExist(string path)
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "0");
        }
    }

    void GetStatFromPotion()
    {
        string[] dataFiles = Directory.GetFiles(potionData).Where(f => !f.EndsWith(".meta")).ToArray();

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
            string potionName = Path.GetFileNameWithoutExtension(file);
            int.TryParse(File.ReadAllText(file), out int count);
            int curioid = ExtractNumberFromString(potionName);

            switch (curioid)
            {
                case 1:
                    Attack += 15f * count;
                    break;
                case 2:
                    Armor += 5f * count;
                    break;
                case 3:
                    Res += 0.06f * count;
                    break;
                case 4:
                    HealthRegen += 6f * count;
                    break;
                case 5:
                    CritDmg += 0.06f * count;
                    break;
                case 6:
                    CritRate += 0.03f * count;
                    break;
                case 7:
                    Health += 150f * count;
                    break;
                case 8:
                    DmgBoost += 0.12f * count;
                    break;
                case 9:
                    ManaRegen += 0.06f * count;
                    break;
                case 10:
                    Attack += 0.03f * count;
                    break;
                case 11:
                    IgnoreRes += 0.06f * count;
                    break;
            }
        }

        AssignNewStatToPotionPath(pathArmor, Armor);
        AssignNewStatToPotionPath(pathAttack, Attack);
        AssignNewStatToPotionPath(pathAttackSpd, AttackSpd);
        AssignNewStatToPotionPath(pathCritDmg, CritDmg);
        AssignNewStatToPotionPath(pathCritRate, CritRate);
        AssignNewStatToPotionPath(pathDmgBoost, DmgBoost);
        AssignNewStatToPotionPath(pathHealth, Health);
        AssignNewStatToPotionPath(pathHealthRegen, HealthRegen);
        AssignNewStatToPotionPath(pathIgnoreRes, IgnoreRes);
        AssignNewStatToPotionPath(pathMana, Mana);
        AssignNewStatToPotionPath(pathManaRegen, ManaRegen);
        AssignNewStatToPotionPath(pathRes, Res);
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
