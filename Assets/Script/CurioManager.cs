using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Curio
{
    public string curioName;
    public string curioEffect;
    public string imagePath;
    public int completion;

    public Curio(string name, string effect, string path, int comp)
    {
        curioName = name;
        curioEffect = effect;
        imagePath = path;
        completion = comp;
    }
}

public class CurioManager : MonoBehaviour
{
    string libPath = "Assets/Data/Library_data/Curio_lib/Curio_Stat";

    string effectPath = "Assets/Data/Library_data/Curio_lib/Curio_Effect";

    string dataPath = "Assets/Data/ingame_data/curio";

    string imagePath = "Assets/Data/curio_sprite";

    public Sprite border;
    public Sprite corner;
    public Sprite line;
    public Sprite perfectedBorder;
    public Sprite perfectedCorner;
    public Sprite perfectedLine;


    public GameObject content;

    public List<Curio> curioList = new List<Curio>();

    // Start is called before the first frame update
    void Start()
    {
        CheckInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void CheckInventory() 
    {
        string[] dataFiles = Directory.GetFiles(dataPath).Where(f => !f.EndsWith(".meta")).ToArray();
        GameObject curioSlot;

        int i = 0;
        foreach (string file in dataFiles)
        {
            i++;
            string curioid = Path.GetFileNameWithoutExtension(file);
            curioSlot = content.transform.Find("Curio" + i).gameObject;
            curioSlot.SetActive(true);
            int.TryParse(File.ReadAllText(dataPath + "/" + curioid + ".txt"), out int completion);

            string[] lines = File.ReadAllLines(libPath + "/" + curioid + ".txt");

            string curioEffectWithStat = ReplaceStatsWithCompletion(File.ReadAllText(effectPath + "/" + curioid + ".txt"), completion, ExtractNumberFromString(curioid));



            AssignInformationToCurioList(lines[0], ProcessText(curioEffectWithStat, completion), imagePath + "/" + curioid + ".png", completion);
        }
        AssignInformationToCurioSlot(content, curioList);
    }

    void AssignInformationToCurioSlot(GameObject content, List<Curio> curios)
    {
        GameObject curioSlot;
        int i = 0;
        foreach (Curio curio in curios)
        {
            i++;
            curioSlot = content.transform.Find("Curio" + i).gameObject;
            curioSlot.transform.Find("Image and Name").gameObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = curio.curioName;
            curioSlot.transform.Find("Image and Name").gameObject.transform.Find("Image").GetComponent<Image>().sprite = getImage(curio.imagePath);
            curioSlot.transform.Find("Description").gameObject.transform.Find("Scroll View").gameObject.transform.Find("Viewport").gameObject.transform.Find("Content").GetComponent<TextMeshProUGUI>().text = curio.curioEffect;
            curioSlot.transform.Find("Completion").gameObject.transform.Find("CompletionNumber").GetComponent<TextMeshProUGUI>().text = (curio.completion + "%");

            if (curio.completion < 100)
            {
                curioSlot.transform.Find("Completion").GetComponent<Image>().sprite = corner;
                curioSlot.transform.Find("Completion").gameObject.transform.Find("Image").GetComponent<Image>().sprite = line;
                curioSlot.transform.Find("Completion").gameObject.transform.Find("Image").GetComponent<Image>().fillAmount = curio.completion * 0.01f;
                curioSlot.transform.Find("border").GetComponent<Image>().sprite = border;
            }
            else
            {
                curioSlot.transform.Find("Completion").GetComponent<Image>().sprite = perfectedCorner;
                curioSlot.transform.Find("Completion").gameObject.transform.Find("Image").GetComponent<Image>().sprite = perfectedLine;
                curioSlot.transform.Find("border").GetComponent<Image>().sprite = perfectedBorder;
            }

        }
        
    }

    Sprite getImage(string imagePath)
    {
        if (File.Exists(imagePath))
        {
            // Read the image data
            byte[] imageData = File.ReadAllBytes(imagePath);

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
            Debug.LogError("File does not exist at path: " + imagePath);
        }
        return null;
    }
    void AssignInformationToCurioList(string curioName, string effectPath, string imagePath, int completion)
    {
        AddCurioToList(new Curio(curioName, effectPath, imagePath, completion));
        curioList.Sort((a,b) => b.completion.CompareTo(a.completion));
    }

    void AddCurioToList(Curio newCurio)
    {
        // Kiểm tra xem đối tượng Curio đã có trong danh sách chưa
        bool exists = curioList.Exists(curio => curio.curioName == newCurio.curioName 
                                                && curio.curioEffect == newCurio.curioEffect 
                                                && curio.imagePath == newCurio.imagePath 
                                                && curio.completion == newCurio.completion);
        if (!exists)
        {
            curioList.Add(newCurio);
        }       
    }
    string ProcessText(string text, int completion)
    {

        if (completion == 100)
        {
            text = text.Replace("<color=grey>", "<color=white>");
        }

        return text;
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

    public static int ExtractNumberFromString(string input)
    {
        string numberString = Regex.Match(input, @"\d+").Value;
        return int.Parse(numberString);
    }
}
