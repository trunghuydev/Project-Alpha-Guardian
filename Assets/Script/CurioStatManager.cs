using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    // Start is called before the first frame update

    string pathArmor = "Assets/Data/Hero_stat/curio_stat/Armor.txt";
    string pathAttack = "Assets/Data/Hero_stat/curio_stat/Attack.txt";
    string pathAttackSpd = "Assets/Data/Hero_stat/curio_stat/AttackSpd.txt";
    string pathCritDmg = "Assets/Data/Hero_stat/curio_stat/CritDmg.txt";
    string pathCritRate = "Assets/Data/Hero_stat/curio_stat/CritRate.txt";
    string pathDmgBoost = "Assets/Data/Hero_stat/curio_stat/DmgBoost.txt";
    string pathHealth = "Assets/Data/Hero_stat/curio_stat/Health.txt";
    string pathHealthRegen = "Assets/Data/Hero_stat/curio_stat/HealthRegen.txt";
    string pathIgnoreRes = "Assets/Data/Hero_stat/curio_stat/IgnoreRes.txt";
    string pathMana = "Assets/Data/Hero_stat/curio_stat/Mana.txt";
    string pathManaRegen = "Assets/Data/Hero_stat/curio_stat/ManaRegen.txt";
    string pathRes = "Assets/Data/Hero_stat/curio_stat/Res.txt";
    string pathTowerMaxHealth = "Assets/Data/Hero_stat/curio_stat/TowerMaxHealth.txt";
    string pathTowerCurrentHealth = "Assets/Data/Hero_stat/ingame_stat/tower/TowerMaxHealth.txt";



    string curioData = "Assets/Data/ingame_data/curio";

    void Start()
    {
        CheckStatFolder();
    }

    // Update is called once per frame
    void Update()
    {
        GetStatFromCurio();
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

    void GetStatFromCurio()
    {
        string[] dataFiles = Directory.GetFiles(curioData).Where(f => !f.EndsWith(".meta")).ToArray();

        float towerMaxHealth = 0;
        float towerCurrentHealth = 0;
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
            string curioName = Path.GetFileNameWithoutExtension(file);
            int.TryParse(File.ReadAllText(file), out int completion);
            int curioid = ExtractNumberFromString(curioName);

            switch (curioid)
            {
                case 1:
                    HealthRegen += 10f + 0.2f * completion;
                    if (completion == 100)
                    {
                        Res += 0.12f;
                    }
                    break;
                case 3:
                    int perfectedCurioQuantity = 0;
                    foreach(string completionfilecheck in dataFiles)
                    {
                        int.TryParse(File.ReadAllText(completionfilecheck), out int completionCheck);
                        if (completionCheck == 100)
                        {
                            perfectedCurioQuantity++;
                        }
                    }
                    float DmgBoostfromCurio = ((dataFiles.Length + perfectedCurioQuantity * 4f) * completion) / 10000f;
                    DmgBoost += DmgBoostfromCurio;
                    break;

                case 4:
                    AttackSpd += 0.002f * completion;
                    if (completion == 100)
                    {
                        ManaRegen += 0.3f;
                    }
                    break;
                case 5:
                    DmgBoost += ((completion-100f)/100f);
                    if (completion == 100)
                    {
                        IgnoreRes += 0.3f;
                    }
                    break;
                case 6:
                    towerMaxHealth += 3000 + completion * 20f ; 
                    if (completion == 100)
                    {
                        Health += 1000f;
                    }
                    break;

                case 7:
                    Armor += 0.2f * completion;
                    if (completion == 100)
                    {
                        Res += 0.08f;
                    }
                    break;
                case 8:
                    DmgBoost += completion * 0.003f;
                    if (completion == 100)
                    {
                        IgnoreRes += 0.1f;
                    }
                    break;
                case 9:
                    CritRate += 0.003f * completion;
                    if (completion == 100)
                    {
                        CritRate += 0.15f;
                    }
                    break;
                case 10:
                    CritRate += 0.77f;
                    CritDmg += 0.77f;
                    IgnoreRes += 0.77f;
                    break;
            }
        }

        AssignNewStatToCurioPath(pathArmor, Armor);
        AssignNewStatToCurioPath(pathAttack, Attack);
        AssignNewStatToCurioPath(pathAttackSpd, AttackSpd);
        AssignNewStatToCurioPath(pathCritDmg, CritDmg);
        AssignNewStatToCurioPath(pathCritRate, CritRate);
        AssignNewStatToCurioPath(pathDmgBoost, DmgBoost);
        AssignNewStatToCurioPath(pathHealth, Health);
        AssignNewStatToCurioPath(pathHealthRegen, HealthRegen);
        AssignNewStatToCurioPath(pathIgnoreRes, IgnoreRes);
        AssignNewStatToCurioPath(pathMana, Mana);
        AssignNewStatToCurioPath(pathManaRegen, ManaRegen);
        AssignNewStatToCurioPath(pathRes, Res);

        AssignNewStatToCurioPath(pathTowerMaxHealth, towerMaxHealth);
        
    }

    public void AssignNewStatToCurioPath(string path, float newValue)
    {
        File.WriteAllText(path, newValue.ToString());
    }

    public static int ExtractNumberFromString(string input)
    {
        string numberString = Regex.Match(input, @"\d+").Value;
        return int.Parse(numberString);
    }
}
