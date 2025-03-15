using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game1Data : MonoBehaviour
{
    // Start is called before the first frame update

    string curioDataPath = "Assets/Data/ingame_data/curio";
    string pathToCurioStatLib = "Assets/Data/Library_data/Curio_lib/Curio_Stat";
    string electricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";

    public Button buttonCard1;
    public Button buttonCard2;
    public Button buttonCard3;
    public Button buttonCard4;

    public TextMeshProUGUI finalRewardText;
    public TextMeshProUGUI textCard1;
    public TextMeshProUGUI textCard2;
    public TextMeshProUGUI textCard3;
    public TextMeshProUGUI textCard4;

    public Button buttonStart;
    public Button buttonEnd;

    public GameObject buttonStartobj;
    public GameObject buttonNextobj;

    string rewardCard1 = "";
    string rewardCard2 = "";
    string rewardCard3 = "";
    string rewardCard4 = "";

    string finalReward = "";
    string finalRewardName = "";

    int completionCurio;

    private List<string> rewards = new List<string>();

    int currentStage = 1;

    void Start()
    {
        finalReward = RandomAReward();

        if(finalReward == "") 
        {
            finalRewardName = "Mỗi 1% độ hoàn chỉnh nhận 2 Chip Điện tử (nhận thêm 100 Chip Điện tử thưởng nếu đạt 100% độ hoàn chỉnh)";
        }
        else
        {
            string path = pathToCurioStatLib + "/" + finalReward + ".txt";
            string[] lines = File.ReadAllLines(path);
            finalRewardName = lines[0];
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFinalRewardText();
    }

    public void ShowNextReward()
    {
        
        switch (currentStage)
        {
            case 1:
                Round1Reward();
                break;
            case 2:
                Round2Reward();
                break;
            case 3:
                Round2Reward();
                break;
            case 4:
                Round3Reward();
                break;
            case 5:
                buttonStartobj.SetActive(true);
                buttonStart.interactable = false;
                break;
        }
        currentStage++;
        buttonStartobj.SetActive(true);
        buttonNextobj.SetActive(false);
    }

    public void Round3Reward()
    {
        rewardCard1 = "100%";
        rewardCard2 = "+10%";
        rewardCard3 = "60%";
        rewardCard4 = "x";
        textCard1.text = "100%";
        textCard2.text = "+10%";
        textCard3.text = "60%";
        textCard4.text = "x";
        rewards.Add(rewardCard1);
        rewards.Add(rewardCard2);
        rewards.Add(rewardCard3);
        rewards.Add(rewardCard4);

        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, rewards.Count);
            string selectedReward = rewards[randomIndex];
            rewards.RemoveAt(randomIndex);

            switch (i)
            {
                case 0:
                    rewardCard1 = selectedReward;
                    break;
                case 1:
                    rewardCard2 = selectedReward;
                    break;
                case 2:
                    rewardCard3 = selectedReward;
                    break;
                case 3:
                    rewardCard4 = selectedReward;
                    break;
            }
        }

    }

    public void Round1Reward()
    {
        rewardCard1 = "60%";
        rewardCard2 = "65%";
        rewardCard3 = "70%";
        rewardCard4 = "75%";
        textCard1.text = "60%";
        textCard2.text = "65%";
        textCard3.text = "70%";
        textCard4.text = "75%";
        rewards.Add(rewardCard1);
        rewards.Add(rewardCard2);
        rewards.Add(rewardCard3);
        rewards.Add(rewardCard4);
        
        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, rewards.Count);
            string selectedReward = rewards[randomIndex];
            rewards.RemoveAt(randomIndex);

            switch (i)
            {
                case 0:
                    rewardCard1 = selectedReward;
                    break;
                case 1:
                    rewardCard2 = selectedReward;
                    break;
                case 2:
                    rewardCard3 = selectedReward;
                    break;
                case 3:
                    rewardCard4 = selectedReward;
                    break;
            }
        }
    
    }

    public void Round2Reward()
    {
        rewardCard1 = "+5%";
        rewardCard2 = "+10%";
        rewardCard3 = "+15%";
        rewardCard4 = "60%";
        textCard1.text = "+5%";
        textCard2.text = "+10%";
        textCard3.text = "+15%";
        textCard4.text = "60%";
        rewards.Add(rewardCard1);
        rewards.Add(rewardCard2);
        rewards.Add(rewardCard3);
        rewards.Add(rewardCard4);

        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, rewards.Count);
            string selectedReward = rewards[randomIndex];
            rewards.RemoveAt(randomIndex);

            switch (i)
            {
                case 0:
                    rewardCard1 = selectedReward;
                    break;
                case 1:
                    rewardCard2 = selectedReward;
                    break;
                case 2:
                    rewardCard3 = selectedReward;
                    break;
                case 3:
                    rewardCard4 = selectedReward;
                    break;
            }
            Debug.Log("Reward " + i + ": " + selectedReward);
        }

    }

    public void RoundStart()
    {
        buttonStart.interactable = false;
        buttonEnd.interactable = false;

        buttonCard1.interactable = true;
        buttonCard2.interactable = true;
        buttonCard3.interactable = true;
        buttonCard4.interactable = true;
        textCard1.text = "?";
        textCard2.text = "?";
        textCard3.text = "?";
        textCard4.text = "?";
    }

    public void CardSelection(int selection)
    {
        switch (selection)
        {
            case 1:
                AssignReward(rewardCard1);
                break;
            case 2: 
                AssignReward(rewardCard2); 
                break;
            case 3: 
                AssignReward(rewardCard3);  
                break;
            case 4: 
                AssignReward(rewardCard4); 
                break;
        }

        textCard1.text = rewardCard1;
        textCard2.text = rewardCard2;
        textCard3.text = rewardCard3;
        textCard4.text = rewardCard4;

        buttonCard1.interactable = false;
        buttonCard2.interactable = false;
        buttonCard3.interactable = false;
        buttonCard4.interactable = false;
        
        buttonStart.interactable = true;
        buttonStartobj.SetActive(false);
        buttonEnd.interactable = true;

        if (currentStage <= 4)
        {
            buttonNextobj.SetActive(true);
        }
    }

    void AssignReward(string reward)
    {
        switch(reward)
        {
            case "60%":
                completionCurio = 60;
                break;
            case "65%":
                completionCurio = 65;
                break;
            case "70%":
                completionCurio = 70;
                break;
            case "75%":
                completionCurio = 75;
                break;
            case "100%":
                completionCurio = 100;
                break;
            case "+5%":
                completionCurio += 5;
                break;
            case "+10%":
                completionCurio += 10;
                break;
            case "+15%":
                completionCurio += 15;
                break;
            case "x":
                completionCurio = 0;
                break;
        }

        if(completionCurio >= 100)
        {
            completionCurio = 100;
        }
    }

    public void ExitGame()
    {
        if(completionCurio >= 60 && finalReward != "") 
        {
            string path = curioDataPath + "/" + finalReward + ".txt";
            File.WriteAllText(path, completionCurio.ToString());
        }
        else
        {
            int.TryParse(File.ReadAllText(electricChipPath), out int currentChip);
            if (completionCurio == 100)
            {
                currentChip += 300;
            }
            else
            {
                currentChip += 2 * completionCurio;
            }
            File.WriteAllText(electricChipPath, (currentChip).ToString());
        }

        LoadSceneWithDelay("Adventure", 1f);
       
    }

    string RandomAReward()
    {
        string[] existingFiles = Directory.GetFiles(curioDataPath).Where(f => !f.EndsWith(".meta")).Select(f => Path.GetFileNameWithoutExtension(f)).ToArray();

        // Tạo danh sách các tên tệp curio từ curio1 đến curio9
        List<string> allCurios = new List<string>();
        for (int i = 1; i <= 9; i++)
        {
            allCurios.Add($"curio{i}");
        }

        // Lọc ra những tệp đã tồn tại trong thư mục curio
        List<string> availableCurios = allCurios.Where(curio => !existingFiles.Contains(curio)).ToList();

        if (availableCurios.Count == 0)
        {
            // Tất cả các tệp curio đã tồn tại
            return "";
        }

        // Chọn ngẫu nhiên một tên tệp từ danh sách còn lại
        int randomIndex = Random.Range(0, availableCurios.Count);
        return availableCurios[randomIndex];
    }

    public void UpdateFinalRewardText()
    {
        finalRewardText.text = "Phần thưởng hiện tại: " + finalRewardName + "("+completionCurio + "%" + ")";
    }

    private int ExtractNumberFromString(string input)
    {
        string numberString = Regex.Match(input, @"\d+").Value;
        return int.Parse(numberString);
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
