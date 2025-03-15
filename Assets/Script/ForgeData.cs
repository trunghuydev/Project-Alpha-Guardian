using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ForgeData : MonoBehaviour
{
    // Start is called before the first frame update

    string curioDataPath = "Assets/Data/ingame_data/curio";
    string pathToCurioStatLib = "Assets/Data/Library_data/Curio_lib/Curio_Stat";
    string pathToCurioEffectLib = "Assets/Data/Library_data/Curio_lib/Curio_Effect";
    string electricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";

    public TextMeshProUGUI currentEChipText;
    public GameObject content;

    public GameObject forgePanel;

    public TextMeshProUGUI forgePrice;

    public GameObject curio3Notification;

    public GameObject animationForge;

    public Button forgeButton;

    public Sprite perfectedCurio;
    public Sprite normalCurio;
    public Sprite perfectedLine;
    public Sprite normalLine;

    int forgingPrice = 25;

    int CurioForgingBonus = 0;

    int currentIndex = 0;

    public class Curio
    {
        public string name;
        public int completion;

        public Curio(string name, int comp)
        {
            this.name = name;
            this.completion = comp;
        }
    }

    public List<Curio> curioList = new List<Curio>();

    void Start()
    {
        // Bạn có thể gọi UpdateCurioData() tại đây nếu muốn cập nhật dữ liệu ngay khi khởi động
        UpdateCurioData();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEChipAmount();
        UpdateCurioData();
        CheckCurio3Perfected();
        UpdateForgingPrice();
    }

    void UpdateForgingPrice()
    {
        forgePrice.text = forgingPrice.ToString();
        int currentEChip = int.Parse(File.ReadAllText(electricChipPath));
        if (forgingPrice > currentEChip)
        {
            forgeButton.interactable = false;
            forgePrice.color = Color.red;
        }
    }

    void CheckCurio3Perfected()
    {
        string path = curioDataPath + "/curio3.txt";
        if (File.Exists(path))
        {
            int.TryParse(File.ReadAllText(path), out int completion);
            if (completion == 100)
            {
                CurioForgingBonus = 5;
                curio3Notification.SetActive(true);
            }
        }
    }

    public void GetInformationToForgePanel(int index)
    {
        if(curioList[index - 1].completion >= 100)
        {
            forgeButton.interactable = false;
        }
        else
        {
            forgeButton.interactable = true;
        }

        forgePanel.SetActive(true);
        currentIndex = index;
        forgePanel.transform.Find("Tên và Hình").transform.Find("Tên").gameObject.GetComponent<TextMeshProUGUI>().text = GetCurioName(curioList[index - 1].name);
        forgePanel.transform.Find("Tên và Hình").transform.Find("Hình vật thể lạ").gameObject.GetComponent<Image>().sprite = GetCurioImage(curioList[index - 1].name);
        forgePanel.transform.Find("Độ hoàn chỉnh _ BG").transform.Find("Độ hoàn chỉnh _ Text").gameObject.GetComponent<TextMeshProUGUI>().text = curioList[index-1].completion.ToString() + "%";
        if (curioList[index - 1].completion == 100)
        {
            forgePanel.transform.Find("Độ hoàn chỉnh _ BG").transform.Find("Độ hoàn chỉnh _ Hiện tại").gameObject.GetComponent<Image>().sprite = perfectedLine;
        }
        else
        {
            forgePanel.transform.Find("Độ hoàn chỉnh _ BG").transform.Find("Độ hoàn chỉnh _ Hiện tại").gameObject.GetComponent<Image>().sprite = normalLine;
        }
        forgePanel.transform.Find("Độ hoàn chỉnh _ BG").transform.Find("Độ hoàn chỉnh _ Hiện tại").gameObject.GetComponent<Image>().fillAmount = (float) curioList[index-1].completion / 100;
        forgePanel.transform.Find("Mô tả").transform.Find("Scroll View").transform.Find("Viewport").transform.Find("Content").gameObject.GetComponent<TextMeshProUGUI>().text = GetCurioDescription(curioList[index - 1].name, curioList[index-1].completion);
    }

    public void ForgeCurio()
    {
    
        int currentEChip = int.Parse(File.ReadAllText(electricChipPath));
        File.WriteAllText(electricChipPath, (currentEChip - forgingPrice).ToString());

        forgingPrice += 15;

        animationForge.SetActive(true);

        string path = curioDataPath + "/" + curioList[currentIndex -1].name + ".txt";

        int completion = int.Parse(File.ReadAllText(path));

        int newCompletion = Random.Range(completion,101);

        if(newCompletion < completion + CurioForgingBonus)
        {
            newCompletion = completion + CurioForgingBonus;
        }
        if(newCompletion > 100) 
        {
            newCompletion = 100;
        }
        File.WriteAllText(path, newCompletion.ToString());
        curioList[currentIndex - 1].completion = newCompletion;
        GetInformationToForgePanel(currentIndex);
    }

    string GetCurioDescription(string curioid, int completion)
    {
        string path = pathToCurioEffectLib + "/" + curioid + ".txt";
        string effect = File.ReadAllText(path);

        effect = ReplaceStatsWithCompletion(effect, completion, ExtractNumberFromString(curioid));

        effect = ProcessText(effect, completion);

        return effect;
    }

    string ProcessText(string text, int completion)
    {

        if (completion == 100)
        {
            text = text.Replace("<color=grey>", "<color=white>");
        }

        return text;
    }

    private int ExtractNumberFromString(string input)
    {
        string numberString = Regex.Match(input, @"\d+").Value;
        return int.Parse(numberString);
    }

    string ReplaceStatsWithCompletion(string curioEffect, int completion, int curioid)
    {
        string pattern = @"\{([^{}]+)\}";

        string newContent = Regex.Replace(curioEffect, pattern, GetReplaceContent(curioid, completion));

        return newContent;
    }

    string GetReplaceContent(int curioid, int completion)
    {
        string statText = "";
        float statNum = 0f;
        switch (curioid)
        {
            case 1:
                statNum = 10 + 0.2f * completion;
                statText = statNum.ToString();
                break;
            case 2:
                statNum = 0.25f * completion;
                statText = statNum.ToString() + "%";
                break;
            case 3:
                statNum = 0.01f * completion;
                statText = statNum.ToString() + "%";
                break;
            case 4:
                statNum = 0.2f * completion;
                statText = statNum.ToString() + "%";
                break;
            case 5:
                statNum = 100 - completion;
                statText = statNum.ToString() + "%";
                break;
            case 6:
                statNum = 3000 + 20 * completion;
                statText = statNum.ToString();
                break;
            case 7:
                statNum = 0.2f * completion;
                statText = statNum.ToString();
                break;
            case 8:
                statNum = 0.3f * completion;
                statText = statNum.ToString() + "%";
                break;
            case 9:
                statNum = 0.3f * completion;
                statText = statNum.ToString() + "%";
                break;
            case 10:
                statText = "hoàn hảo";
                break;
        }

        return statText;
    }




    string GetCurioName(string curioid)
    {
        string path = pathToCurioStatLib + "/" + curioid + ".txt";
        string[] lines = File.ReadAllLines(path);
        return lines[0];
    }

    void UpdateCurioData()
    {
        string[] curioFiles = Directory.GetFiles(curioDataPath).Where(f => !f.EndsWith(".meta")).ToArray();

        // Lưu tên các tệp và số liệu vào danh sách curioList nếu chưa tồn tại
        foreach (string file in curioFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            int completion = int.Parse(File.ReadAllText(file));

            if (!curioList.Any(c => c.name == fileName))
            {
                curioList.Add(new Curio(fileName, completion));
                curioList.Sort((b, a) => a.completion.CompareTo(b.completion));
            }
        }

        for (int i = 0; i < curioList.Count; i++)
        {
            Transform curioTransform = content.transform.Find("Curio" + (i + 1));
            if (curioTransform != null)
            {
                curioTransform.gameObject.SetActive(true);             
            }
            else
            {
                Debug.Log("Curio" + (i + 1) + " not found in content");
            }
        }

        int index = 0;

        foreach (Curio curio in curioList)
        {
            index++;
            Transform curioTransform = content.transform.Find("Curio" + (index));

            if (curioTransform != null)
            {
                curioTransform.gameObject.SetActive(true);
                curioTransform.Find("CurioImage").gameObject.GetComponent<Image>().sprite = GetCurioImage(curio.name);
                curioTransform.Find("Completion").gameObject.GetComponent<TextMeshProUGUI>().text = curio.completion.ToString() + "%";

                if(curio.completion == 100)
                {
                    curioTransform.gameObject.GetComponent<Image>().sprite = perfectedCurio;
                }
            }
            else
            {
                Debug.Log("Curio" + (index + 1) + " not found in content");
            }

        }
    }

    Sprite GetCurioImage(string filename)
    {
        string image_path = "Assets/Data/curio_sprite/" + filename + ".png";


        if (File.Exists(image_path))
        {
            // Read the image data
            byte[] imageData = File.ReadAllBytes(image_path);

            // Check if the image data is valid
            if (imageData.Length > 0)
            {
                Texture2D texture = new Texture2D(2, 2);

                // Try loading the image into the texture
                if (texture.LoadImage(imageData))
                {

                    Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    return newSprite;
                }
                else
                {
                    Debug.LogError("Failed to load texture from image data.");
                }
            }
            else
            {
                Debug.LogError("Image data is empty.");
            }
        }
        else
        {
            Debug.LogError("File does not exist at path: " + image_path);
        }
        return null;
    }

    void UpdateEChipAmount()
    {
        currentEChipText.text = int.Parse(File.ReadAllText(electricChipPath)).ToString();
    }

    public void ExitForge()
    {
        LoadSceneWithDelay("Adventure", 1f);
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
