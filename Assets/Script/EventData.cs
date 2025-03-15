using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventData : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI e1_text;
    public Button e1_buttonGift;
    public Button e1_buttonPoint;
    public Button e1_buttonTowerHealth;
    public Button e1_buttonElectricChip;
    public Button e1_buttonSystemPoint;

    public Button e1_buttonCurio2;
    public Button e1_buttonElectricChip2;
    public Button e1_buttonSystemPoint2;

    public TextMeshProUGUI currentHealth_text;
    public TextMeshProUGUI currentChip_text;
    public TextMeshProUGUI currentPoint_text;

    public TextMeshProUGUI currentChip_text2;
    public TextMeshProUGUI currentPoint_text2;

    string event1Path = "Assets/Data/Library_data/Event_lib/event1.txt";
    string event1SystemPointPath = "Assets/Data/ingame_data/system_point.txt";
    string event1ElectricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";
    string event1CurrentHealthPath = "Assets/Data/Hero_stat/ingame_stat/tower/TowerCurrentHealth.txt";
    string event1CurioPath = "Assets/Data/ingame_data/reward/curioselect.txt";

    int e1currentPoint = 0;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AssignDataToEvent1();
        GetDataToText();
        
    }

    void GetDataToText()
    {
        int.TryParse(File.ReadAllText(event1CurrentHealthPath), out int currentHealth);
        int.TryParse(File.ReadAllText(event1ElectricChipPath), out int electricChip);
        int.TryParse(File.ReadAllText(event1SystemPointPath), out int systemPoint);
        currentHealth_text.text = "Máu thành luỹ hiện tại: " + currentHealth;
        currentChip_text.text = "Chip Điện tử hiện tại: " + electricChip;
        currentPoint_text.text = "Điểm Hệ thống hiện tại: " + systemPoint;

        currentChip_text2.text = "Chip Điện tử hiện tại: " + electricChip;
        currentPoint_text2.text = "Điểm Hệ thống hiện tại: " + systemPoint;
    }

    void AssignDataToEvent1()
    {
        string[] lines = File.ReadAllLines(event1Path);
        int.TryParse(lines[3], out e1currentPoint);
        string e1_oldText = lines[4] + "\n" + lines[5];
        string e1_newText = ReplaceOldTextWithNewText(e1_oldText, e1currentPoint.ToString());
        e1_text.text = e1_newText;
        if(e1currentPoint == 0)
        {
            e1_buttonGift.interactable = false;
        }
        int.TryParse(File.ReadAllText(event1CurrentHealthPath) , out int currentHealth);
        int.TryParse(File.ReadAllText(event1ElectricChipPath) , out int electricChip);
        int.TryParse(File.ReadAllText(event1SystemPointPath) , out int systemPoint);

        if (currentHealth <= 4000)
        {
            e1_buttonTowerHealth.interactable = false;
        }
        if (electricChip < 100)
        {
            e1_buttonElectricChip.interactable = false;
        }
        if (systemPoint < -90)
        {
            e1_buttonSystemPoint.interactable = false;
        }


        if (e1currentPoint < 2)
        {
            e1_buttonCurio2.interactable = false;
        }
        if (e1currentPoint < 1)
        {
            e1_buttonElectricChip2.interactable = false;
        }
        if (systemPoint >= 100 || e1currentPoint < 1)
        {
            e1_buttonSystemPoint2.interactable = false;
        }
    }

    string ReplaceOldTextWithNewText(string oldText, string newText)
    {
        string pattern = @"\{([^{}]+)\}";

        string newContent = Regex.Replace(oldText, pattern, newText);

        return newContent;
    }

    public void Event1Exchange(int selection)
    {
        int.TryParse(File.ReadAllText(event1CurrentHealthPath), out int currentHealth);
        int.TryParse(File.ReadAllText(event1ElectricChipPath), out int electricChip);
        int.TryParse(File.ReadAllText(event1SystemPointPath), out int systemPoint);
        string[] lines = File.ReadAllLines(event1Path);

        switch (selection)
        {
            case 1:
                currentHealth += -4000;
                File.WriteAllText(event1CurrentHealthPath, currentHealth.ToString());
                int.TryParse(lines[3], out e1currentPoint);
                e1currentPoint += 1;
                lines[3] = e1currentPoint.ToString();
                File.WriteAllLines(event1Path,lines);
                e1_buttonTowerHealth.interactable = false; 
                break;

            case 2:
                electricChip += -100;
                File.WriteAllText(event1ElectricChipPath, electricChip.ToString());
                int.TryParse(lines[3], out e1currentPoint);
                e1currentPoint += 1;
                lines[3] = e1currentPoint.ToString();
                File.WriteAllLines(event1Path, lines);
                e1_buttonElectricChip.interactable = false;
                break;

            case 3:
                systemPoint += -10;
                File.WriteAllText(event1SystemPointPath, systemPoint.ToString());
                int.TryParse(lines[3], out e1currentPoint);
                e1currentPoint += 1;
                lines[3] = e1currentPoint.ToString();
                File.WriteAllLines(event1Path, lines);
                e1_buttonSystemPoint.interactable = false;
                break;

            case 4:
                LoadSceneWithDelay("Adventure", 1f);
                break;

            case 5:
                systemPoint += 20;
                if(systemPoint > 100)
                {
                    systemPoint = 100;
                }
                File.WriteAllText(event1SystemPointPath, systemPoint.ToString());
                int.TryParse(lines[3], out e1currentPoint);
                e1currentPoint += -1;
                lines[3] = e1currentPoint.ToString();
                File.WriteAllLines(event1Path, lines);
                e1_buttonSystemPoint2.interactable = false;
                break;

            case 6:
                electricChip += 200;
                File.WriteAllText(event1ElectricChipPath, electricChip.ToString());
                int.TryParse(lines[3], out e1currentPoint);
                e1currentPoint += -1;
                lines[3] = e1currentPoint.ToString();
                File.WriteAllLines(event1Path, lines);
                e1_buttonElectricChip2.interactable = false;
                break;

            case 7:
                File.WriteAllText(event1CurioPath, "1");
                int.TryParse(lines[3], out e1currentPoint);
                e1currentPoint += -2;
                lines[3] = e1currentPoint.ToString();
                File.WriteAllLines(event1Path, lines);
                e1_buttonCurio2.interactable = false;
                break;
        }
    }

    public void LoadSceneWithDelay(string sceneName, float delay)
    {
        StartCoroutine(LoadSceneAfterDelay(sceneName, delay));
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
