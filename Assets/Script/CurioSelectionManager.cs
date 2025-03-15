using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class CurioSelectionManager : MonoBehaviour
{
    // Start is called before the first frame update
    string curioRewardPath = "Assets/Data/ingame_data/reward/curioselect.txt";

    public GameObject button_anim;
    public Animator animator;
    public GameObject button;

    public GameObject selection1;
    public GameObject selection2;
    public GameObject selection3;

    public Sprite perfectedBorder;
    public Sprite perfectedProgressLine;
    public Sprite perfectedProgressCorner;

    public Sprite Border;
    public Sprite ProgressLine;
    public Sprite ProgressCorner;

    int curioid1 = 0;
    int curioid2 = 0;
    int curioid3 = 0;

    int curioCompletion1 = 0;
    int curioCompletion2 = 0;
    int curioCompletion3 = 0;

    public GameObject thisCanvas;

    public GameObject anotherOption;
    public GameObject rewardappear;
    public Image item_image;
    public TextMeshProUGUI itemquantity;
    public Sprite electric_chip;


    int selection = 0;

    public int selectionQuantity = 3;

    string libPath = "Assets/Data/Library_data/Curio_lib/Curio_Stat";

    string effectPath = "Assets/Data/Library_data/Curio_lib/Curio_Effect";

    string dataPath = "Assets/Data/ingame_data/curio";

    string imagePath = "Assets/Data/curio_sprite";
    void Start()
    {
        GetListOfCurio();
        Debug.Log("Curio 1: " + curioid1 + "Completion: " + curioCompletion1 + "Curio 2: " + curioid2 + "Completion: " + curioCompletion2 + "Curio 3: " + curioid3 + "Completion: " + curioCompletion3);
        if(curioid1 != 0)
            AssignCurioToSelection(curioid1, selection1, curioCompletion1);
        if (curioid2 != 0)
            AssignCurioToSelection(curioid2, selection2, curioCompletion2);
        if (curioid3 != 0)
            AssignCurioToSelection(curioid3, selection3, curioCompletion3);


    }

    // Update is called once per frame
    void Update()
    {
        if (curioid1 != 0)
        {
            selection1.GetComponent<Button>().interactable = true;
        }
        if (curioid2 != 0)
        {
            selection2.GetComponent<Button>().interactable = true;
        }
        if (curioid3 != 0)
        {
            selection3.GetComponent<Button>().interactable = true;
        }

        if(curioid1 == 0 && curioid2 == 0 && curioid3 == 0)
        {
            anotherOption.SetActive(true);
        }
    }


    void AssignCurioToSelection(int index, GameObject selection, int curioCompletion)
    {
        string image_path = imagePath + "/curio" + index + ".png";

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
                    selection.gameObject.transform.Find("Name and Image").gameObject.transform.Find("Image").gameObject.transform.Find("Curio image").GetComponent<Image>().sprite = newSprite;
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

        string[] lines = File.ReadAllLines(libPath + "/curio" + index + ".txt");

        string curioName = lines[0];
        selection.gameObject.transform.Find("Name and Image").gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = curioName;

        selection.gameObject.transform.Find("Completion").gameObject.transform.Find("CompletionNumber").GetComponent<TextMeshProUGUI>().text = (curioCompletion.ToString()+"%");


        string curioEffect = File.ReadAllText(effectPath + "/curio" + index + ".txt");



        if (curioCompletion != 100)
        {
            selection.gameObject.GetComponent<Image>().sprite = Border;
            selection.gameObject.transform.Find("Completion").GetComponent<Image>().sprite = ProgressCorner;
            selection.gameObject.transform.Find("Completion").gameObject.transform.Find("CompletionLine").GetComponent<Image>().sprite = ProgressLine;
            selection.gameObject.transform.Find("Completion").gameObject.transform.Find("CompletionLine").GetComponent<Image>().fillAmount = curioCompletion * 0.01f;
        }
        else
        {
            selection.gameObject.GetComponent<Image>().sprite = perfectedBorder;
            selection.gameObject.transform.Find("Completion").GetComponent<Image>().sprite = perfectedProgressCorner;
            selection.gameObject.transform.Find("Completion").gameObject.transform.Find("CompletionLine").GetComponent<Image>().fillAmount = curioCompletion * 0.01f;
        }

        string curioEffectWithStat = ReplaceStatsWithCompletion(curioEffect, curioCompletion, index);

        GameObject selectionContent = selection.gameObject.transform.Find("Description").gameObject.transform.Find("Scroll View").gameObject.transform.Find("Viewport").gameObject.transform.Find("Content").gameObject;

        GameObject scrollView = selection.gameObject.transform.Find("Description").gameObject.transform.Find("Scroll View").gameObject;

        selectionContent.GetComponent<TextMeshProUGUI>().text = curioEffectWithStat;

        StartCoroutine(UpdateScrollViewContentSize(selectionContent.GetComponent<TextMeshProUGUI>(), scrollView.GetComponent<ScrollRect>()));
    }

    IEnumerator UpdateScrollViewContentSize(TextMeshProUGUI textMeshProUGUI, ScrollRect scrollRect)
    {
        yield return null;

        textMeshProUGUI.ForceMeshUpdate();
        RectTransform textRectTransform = textMeshProUGUI.GetComponent<RectTransform>();
        RectTransform contentRectTransform = scrollRect.content;

                // Cập nhật kích thước của Content trong ScrollView
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, textRectTransform.sizeDelta.y);

        // Điều chỉnh lại vị trí của nội dung
        contentRectTransform.anchoredPosition = new Vector2(contentRectTransform.anchoredPosition.x, 0);
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
        switch(curioid)
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
    
    void GetListOfCurio()
    {
        for (int i = 0; i < selectionQuantity; i++)
        {
            GetCurio();
        }
    }
    bool isMythicFishAvailable()
    {
        string curio8Path = dataPath + "/curio8.txt";
        string curio9Path = dataPath + "/curio9.txt";

        if (File.Exists(curio8Path) && File.Exists(curio9Path))
        {
            int.TryParse(File.ReadAllText(curio8Path), out int curio8Completion);
            int.TryParse(File.ReadAllText(curio9Path), out int curio9Completion);

            if (curio8Completion == 100 && curio9Completion == 100)
            {
                return true;
            }
        }

        return false;
    }

    void GetCurio()
    {
        int totalWeight = 0;
        string[] libFiles = Directory.GetFiles(libPath).Where(f => !f.EndsWith(".meta")).ToArray();

        for (int i = 1; i <= libFiles.Length; i++)
        {
            if (!CheckInventory(i) && i != curioid1 && i != curioid2 && i != curioid3)
            {
                totalWeight += GetWeight(libFiles[i - 1]);
            }
        }

        int maxAttempts = 100; // Giới hạn số lần thử để tránh vòng lặp vô hạn
        int totalAttempts = 0; // Tổng số lần thử

        int[] selectedCurios = new int[3];
        int[] curioCompletions = new int[3];

        for (int selectionIndex = 0; selectionIndex < 3; selectionIndex++)
        {
            bool curioSelected = false;

            while (!curioSelected && totalAttempts < maxAttempts)
            {
                totalAttempts++;
                int randomValue = Random.Range(0, totalWeight);
                int cumulativeWeight = 0;

                for (int i = 1; i <= libFiles.Length; i++)
                {
                    if (!CheckInventory(i) && !selectedCurios.Contains(i))
                    {
                        cumulativeWeight += GetWeight(libFiles[i - 1]);

                        if (randomValue <= cumulativeWeight)
                        {
                            if (i == 10 && !isMythicFishAvailable())
                            {
                                // MythicFish không khả dụng, bỏ qua và chọn curio khác
                                break;
                            }

                            selectedCurios[selectionIndex] = i;
                            curioCompletions[selectionIndex] = (i == 10) ? 100 : Random.Range(60, 101);
                            curioSelected = true;
                            break;
                        }
                    }
                }
            }

            // Kiểm tra nếu không chọn được curio hợp lệ sau số lần thử tối đa
            if (!curioSelected)
            {
                Debug.Log("Không thể chọn curio hợp lệ sau " + maxAttempts + " lần thử.");
                break; // Thoát khỏi vòng lặp nếu không chọn được curio
            }
        }

        // Cập nhật các biến curioid và curioCompletion với các giá trị đã chọn
        curioid1 = selectedCurios[0];
        curioCompletion1 = curioCompletions[0];
        curioid2 = selectedCurios[1];
        curioCompletion2 = curioCompletions[1];
        curioid3 = selectedCurios[2];
        curioCompletion3 = curioCompletions[2];
    }





    bool CheckInventory(int curioid)
    {
        return File.Exists(dataPath + "/curio" + curioid + ".txt");
    }

    int GetWeight(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        int.TryParse(lines[2], out int result);
        return result;
    }

    public void Selection(int selectIndex)
    {
        if (selectIndex != 0)
        {
            selection = selectIndex;
            button_anim.SetActive(true);
            animator.SetTrigger("trigger");
            button.GetComponent<Button>().interactable = true;
        }
        else
        {
            button_anim.SetActive(false);
        }
    }

    public void ConfirmSelection()
    {
        string path = dataPath + "/curio";
        switch (selection)
        {
            case 1:
                File.WriteAllText(path + curioid1 + ".txt", curioCompletion1.ToString());
                break;


            case 2:
                File.WriteAllText(path + curioid2 + ".txt", curioCompletion2.ToString());
                break;

            case 3:
                File.WriteAllText(path + curioid3 + ".txt", curioCompletion3.ToString());
                break;
        }

        int.TryParse(File.ReadAllText(curioRewardPath), out int remaining);
        remaining--;
        if(remaining <= 0)
        {
            File.Delete(curioRewardPath);
        }
        else
        {
            File.WriteAllText(curioRewardPath, remaining.ToString());
        }
        thisCanvas.SetActive(false);
    }

    public void AnotherOption()
    {
        RewardScreen("electric_chip", 150);
        int.TryParse(File.ReadAllText(curioRewardPath), out int remaining);
        remaining--;
        if (remaining <= 0)
        {
            File.Delete(curioRewardPath);
        }
        else
        {
            File.WriteAllText(curioRewardPath, remaining.ToString());
        }
        thisCanvas.SetActive(false);
    }

    void RewardScreen(string itemname, int quantity)
    {
        rewardappear.SetActive(true);
        switch (itemname)
        {
            case "electric_chip":
                item_image.sprite = electric_chip;
                itemquantity.text = quantity.ToString();
                int.TryParse(File.ReadAllText("Assets/Data/ingame_data/electric_chip_amount.txt"), out int chipquantity);
                File.WriteAllText("Assets/Data/ingame_data/electric_chip_amount.txt", (chipquantity + 150).ToString());
                File.WriteAllText("Assets/Data/receive/itemget.txt", "90000");
                break;
        }
    }
}
