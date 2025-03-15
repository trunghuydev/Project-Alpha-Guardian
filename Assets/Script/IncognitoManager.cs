using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IncognitoManager : MonoBehaviour
{
    // Start is called before the first frame update

    int Option1Cost;
    int Option1Max;
    int Option1Min;
    int Option2Cost;
    int Option2Chance;
    int Option3Cost;
    int Option3Min;

    int currentBonusSystemPoint;

    string electricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";
    string systemPointPath = "Assets/Data/ingame_data/system_point.txt";
    string curioRewardPath = "Assets/Data/ingame_data/reward/curioselect.txt";
    string potionDataPath = "Assets/Data/ingame_data/potion";

    public TextMeshProUGUI currentBonusSystemPointText;
    public TextMeshProUGUI button1Description;
    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public TextMeshProUGUI button3Text;
    public TextMeshProUGUI button4Text;
    public TextMeshProUGUI button5Text;

    public TextMeshProUGUI result1Text;
    public TextMeshProUGUI result2Text;
    public TextMeshProUGUI result3Text;

    public GameObject Line2;

    bool isOption1Active = false;
    bool isOption2Active = false;
    bool isOption3Active = false;

    int currentSystemPoint;

    void Start()
    {
        currentBonusSystemPoint = 20;
        GenerateOptions();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEChipQuantity();
        UpdateBonusSystemPoint();
    }


    private void UpdateEChipQuantity()
    {
        int currentchip = int.Parse(File.ReadAllText(electricChipPath));
        button1Description.text = "Chip điện tử hiện tại: " + currentchip.ToString();
    }

    private void UpdateBonusSystemPoint()
    {
        currentSystemPoint = int.Parse(File.ReadAllText(systemPointPath));
        currentBonusSystemPointText.text = "<color=orange> \"Bác sĩ công nghệ\": </color>\r\nĐiểm hệ thống thưởng hiện tại: <color=yellow>"+ currentBonusSystemPoint.ToString() + "</color>.\r\nĐiểm hệ thống hiện tại: <color=yellow>" + currentSystemPoint.ToString() + "</color>.";
        if(currentBonusSystemPoint+currentSystemPoint-Option1Cost < -100)
        {
            Line2.transform.Find("Option1").GetComponent<Button>().interactable = false;
        }
        if (currentBonusSystemPoint + currentSystemPoint - Option2Cost < -100)
        {
            Line2.transform.Find("Option2").GetComponent<Button>().interactable = false;
        }
        if (currentBonusSystemPoint + currentSystemPoint - Option3Cost < -100)
        {
            Line2.transform.Find("Option3").GetComponent<Button>().interactable = false;
        }
    }

    public void SelectOption(int selected)
    {
        switch (selected)
        {
            case 1:
                isOption1Active = true;
                button4Text.text = "Xem kết quả";
                currentBonusSystemPoint += -Option1Cost;
                Line2.transform.Find("Option1").GetComponent<Button>().interactable = false;
                Line2.transform.Find("Option5").gameObject.SetActive(false);
                Line2.transform.Find("Option4").gameObject.SetActive(true);
                break;
            case 2:
                isOption2Active = true;
                button4Text.text = "Xem kết quả";
                currentBonusSystemPoint += -Option2Cost;
                Line2.transform.Find("Option2").GetComponent<Button>().interactable = false;
                Line2.transform.Find("Option5").gameObject.SetActive(false);
                Line2.transform.Find("Option4").gameObject.SetActive(true);
                break;
            case 3:
                isOption3Active = true;
                button4Text.text = "Xem kết quả";
                currentBonusSystemPoint += -Option3Cost;
                Line2.transform.Find("Option3").GetComponent<Button>().interactable = false;
                Line2.transform.Find("Option5").gameObject.SetActive(false);
                Line2.transform.Find("Option4").gameObject.SetActive(true);
                break;
            case 4:
                Line2.transform.Find("Option1").GetComponent<Button>().interactable = false;
                Line2.transform.Find("Option2").GetComponent<Button>().interactable = false;
                Line2.transform.Find("Option3").GetComponent<Button>().interactable = false;
                if (isOption1Active)
                {
                    int Echipquantity = Random.Range(Option1Min, Option1Max + 1);
                    result1Text.text = "+" + Echipquantity.ToString() + " Chip Điện tử";
                    int currentchip = int.Parse(File.ReadAllText(electricChipPath));
                    File.WriteAllText(electricChipPath, (currentchip + Echipquantity).ToString());
                }
                if (isOption2Active)
                {
                    int randomResult = Random.Range(0, 100);
                    int curio = 0;
                    if (randomResult < Option2Chance) 
                    {
                        curio = 1;
                        File.WriteAllText(curioRewardPath, "1");
                    }
                    result2Text.text = "+" + curio.ToString() + " Vật thể lạ";                
                }
                if (isOption3Active)
                {
                    int randomResult = Random.Range(0, 2);
                    int potionBonus = 0;
                    if (randomResult < 1)
                    {
                        potionBonus = 1;
                    }
                    result3Text.text = "+" + (Option3Min + potionBonus).ToString() + " Dược phẩm";

                    List<int> weightedList = CreateWeightedList();
        
                    List<int> randomNumbers = GetRandomNumbers(weightedList, Option3Min + potionBonus);
                    
                    foreach (var number in randomNumbers)
                    {
                        Debug.Log("Potion: " + number);

                        string path = potionDataPath + "/potion" + number + ".txt";

                        if(File.Exists(path))
                        {
                            int.TryParse(File.ReadAllText(path), out int current);
                            File.WriteAllText(path, (current+1).ToString());
                        }
                        else
                        {
                            File.WriteAllText(path, "1");
                        }
                    }
                }
                Line2.transform.Find("Option4").gameObject.SetActive(false);
                Line2.transform.Find("Option5").gameObject.SetActive(true);
                break;
            case 5:
                currentSystemPoint = int.Parse(File.ReadAllText(systemPointPath));
                int updateSystemPoint = currentSystemPoint + currentBonusSystemPoint;

                if(updateSystemPoint > 100) 
                {
                    updateSystemPoint = 100;
                }

                File.WriteAllText (systemPointPath, updateSystemPoint.ToString());

                LoadSceneWithDelay("Adventure", 1f);
                break;
        }
    }
    
    public static List<int> GetRandomNumbers(List<int> weightedList, int count)
    {
        List<int> randomNumbers = new List<int>();
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, weightedList.Count);
            randomNumbers.Add(weightedList[randomIndex]);
        }
        return randomNumbers;
    }
    public static List<int> CreateWeightedList()
    {
        // Tạo danh sách trọng số
        Dictionary<int, int> weightedNumbers = new Dictionary<int, int>
            {
                { 1, 5 },
                { 2, 5 },
                { 3, 1 },
                { 4, 5 },
                { 5, 5 },
                { 6, 5 },
                { 7, 5 },
                { 8, 1 },
                { 9, 5 },
                { 10, 5 },
                { 11, 1 }
            };

        // Tạo danh sách với trọng số
        List<int> weightedList = new List<int>();
        foreach (var kvp in weightedNumbers)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                weightedList.Add(kvp.Key);
            }
        }

        return weightedList;
    }

    void GenerateOptions()
    {
        Option1Cost = Random.Range(1, 3);
        Option2Cost = Random.Range(1, 3);
        Option3Cost = Random.Range(1, 3);

        if (Option1Cost > 1)
        {
            Option1Cost = 5;
        }
        else
        {
            Option1Cost = 10;
        }

        if (Option2Cost > 1)
        {
            Option2Cost = 5;
        }
        else
        {
            Option2Cost = 10;
        }

        if (Option3Cost > 1)
        {
            Option3Cost = 5;
        }
        else
        {
            Option3Cost = 10;
        }

        if (Option1Cost == 5)
        {
            Option1Min = Random.Range(1,31);
            Option1Max = Random.Range(60,100);
            button1Text.text = "Đổi 5 điểm hệ thống lấy ngẫu nhiên từ " + Option1Min + " đến " + Option1Max + " Chip Điện tử.";
        }
        else
        {
            Option1Min = Random.Range(30, 61);
            Option1Max = Random.Range(100, 151);
            button1Text.text = "Đổi 10 điểm hệ thống lấy ngẫu nhiên từ " + Option1Min + " đến " + Option1Max + " Chip Điện tử.";
        }

        if (Option2Cost == 5)
        {
            Option2Chance = Random.Range(30, 51);
            button2Text.text = "Đổi 5 điểm hệ thống để có tỉ lệ " + Option2Chance + "% nhận 1 Vật thể lạ.";
        }
        else
        {
            Option2Chance = Random.Range(50, 71);
            button2Text.text = "Đổi 10 điểm hệ thống để có tỉ lệ " + Option2Chance + "% nhận 1 Vật thể lạ.";
        }

        if (Option3Cost == 5)
        {
            Option3Min = 1;
            button3Text.text = "Đổi 5 điểm hệ thống để có nhận ngẫu nhiên " + Option3Min + " hoặc " + (Option3Min + 1) + " Dược phẩm ngẫu nhiên.";
        }
        else
        {
            Option3Min = 2;
            button3Text.text = "Đổi 10 điểm hệ thống để có nhận ngẫu nhiên " + Option3Min + " hoặc " + (Option3Min + 1) + " Dược phẩm ngẫu nhiên.";
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
