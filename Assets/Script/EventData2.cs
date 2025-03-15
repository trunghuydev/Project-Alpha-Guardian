using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventData2 : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI e2_eventName;
    public Button e2_buttonTowerHealth;
    public Button e2_buttonElectricChip;
    public Button e2_buttonSystemPoint;

    public Button e2_buttonCurio2;
    public Button e2_buttonElectricChip2;
    public Button e2_buttonSystemPoint2;

    public TextMeshProUGUI currentHealth_text;
    public TextMeshProUGUI currentChip_text;
    public TextMeshProUGUI currentPoint_text;

    public TextMeshProUGUI currentChip2_text;
    public TextMeshProUGUI currentPoint2_text;

    public GameObject line1path1;
    public GameObject line2path1;

    public GameObject line1path2;

    string event2CurioPath = "Assets/Data/ingame_data/reward/curioselect.txt";
    string event2Path = "Assets/Data/Library_data/Event_lib/event2.txt";
    string event2SystemPointPath = "Assets/Data/ingame_data/system_point.txt";
    string event2ElectricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";
    string event2CurrentHealthPath = "Assets/Data/Hero_stat/ingame_stat/tower/TowerCurrentHealth.txt";

    int Successchance = 30;
    void Start()
    {
        string[] lines = File.ReadAllLines(event2Path);
        int.TryParse(lines[3], out int currentStage);
        e2_eventName.text += " " + (currentStage + 1);
        if (currentStage >= 2)
        {
            currentStage = 0;
            lines[3] = currentStage.ToString();
            line1path1.SetActive(false);
            line2path1.SetActive(false);

            line1path2.SetActive(true);
            File.WriteAllLines(event2Path, lines);
        }
        else
        {
            line1path1.SetActive(true);
            line2path1.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        AssignDataToEvent2();
        GetDataToText();
    }

    void GetDataToText()
    {
        int.TryParse(File.ReadAllText(event2CurrentHealthPath), out int currentHealth);
        int.TryParse(File.ReadAllText(event2ElectricChipPath), out int electricChip);
        int.TryParse(File.ReadAllText(event2SystemPointPath), out int systemPoint);
        currentHealth_text.text = "Máu thành luỹ hiện tại: " + currentHealth;
        currentChip_text.text = "Chip Điện tử hiện tại: " + electricChip;
        currentPoint_text.text = "Điểm Hệ thống hiện tại: " + systemPoint;
        currentChip2_text.text = "Chip Điện tử hiện tại: " + electricChip;
        currentPoint2_text.text = "Điểm Hệ thống hiện tại: " + systemPoint;
    }

    void AssignDataToEvent2()
    {
        string[] lines = File.ReadAllLines(event2Path);
        
        int.TryParse(File.ReadAllText(event2CurrentHealthPath), out int currentHealth);
        int.TryParse(File.ReadAllText(event2ElectricChipPath), out int electricChip);
        int.TryParse(File.ReadAllText(event2SystemPointPath), out int systemPoint);

        if (currentHealth <= 2000)
        {
            e2_buttonTowerHealth.interactable = false;
        }
        if (electricChip < 50)
        {
            e2_buttonElectricChip.interactable = false;
        }
        if (systemPoint < -95)
        {
            e2_buttonSystemPoint.interactable = false;
        }
    }

    public void Event2Exchange(int selection)
    {
        int.TryParse(File.ReadAllText(event2CurrentHealthPath), out int currentHealth);
        int.TryParse(File.ReadAllText(event2ElectricChipPath), out int electricChip);
        int.TryParse(File.ReadAllText(event2SystemPointPath), out int systemPoint);
        string[] lines = File.ReadAllLines(event2Path);
        

        switch (selection)
        {
            case 1:
                currentHealth += -2000;
                File.WriteAllText(event2CurrentHealthPath, currentHealth.ToString());
                Successchance += 20;
                e2_buttonTowerHealth.interactable = false;
                break;

            case 2:
                systemPoint += -5;
                File.WriteAllText(event2SystemPointPath, systemPoint.ToString());
                Successchance += 20;
                e2_buttonSystemPoint.interactable = false;
                break;

            case 3:
                electricChip += -50;
                File.WriteAllText(event2ElectricChipPath, electricChip.ToString());
                Successchance += 20;
                e2_buttonElectricChip.interactable = false;
                break;
            

            case 4:
                int.TryParse(lines[3], out int currentStage);
                int randomResult = Random.Range(1, 100);
                if(randomResult <= Successchance)
                {
                    currentStage += 1;
                }
                lines[3] = currentStage.ToString();
                File.WriteAllLines(event2Path, lines);
                LoadSceneWithDelay("Adventure", 1f);
                break;

            case 5:
                File.WriteAllText(event2CurioPath, "1");
                e2_buttonCurio2.interactable = false;
                break;


            case 6:
                systemPoint += 30;
                if(systemPoint > 100)
                {
                    systemPoint = 100;
                }
                File.WriteAllText(event2SystemPointPath, systemPoint.ToString());
                e2_buttonSystemPoint2.interactable = false;
                break;

            case 7:
                electricChip += 400;
                File.WriteAllText(event2ElectricChipPath, electricChip.ToString());
                e2_buttonElectricChip2.interactable = false;
                break;


            case 8:
                LoadSceneWithDelay("Adventure", 1f);
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
