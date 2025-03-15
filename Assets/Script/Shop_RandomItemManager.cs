using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Shop_RandomItemManager : MonoBehaviour
{
    public GameObject sellItemPanel;

    string pathToPotionLib = "Assets/Data/Library_data/Potion_lib";
    string pathToCurioStatLib = "Assets/Data/Library_data/Curio_lib/Curio_Stat";
    string pathToCurioEffectLib = "Assets/Data/Library_data/Curio_lib/Curio_Effect";
    string pathToPotionData = "Assets/Data/ingame_data/potion";
    string pathToCurioData = "Assets/Data/ingame_data/curio";
    string pathToPurchasedPotionCount = "Assets/Data/ingame_data/purchased_potion_count.txt";

    string pathToCurrentEChip = "Assets/Data/ingame_data/electric_chip_amount.txt";

    public Sprite bgForCurio;
    public Sprite bgForPerfectedCurio;
    public Sprite bgForPotion;
    public Sprite bgForLimitedPotion;

    public TextMeshProUGUI nameSlot;
    public TextMeshProUGUI descriptionSlot;
    public TextMeshProUGUI priceSlot;
    public GameObject imageSlot;

    string selectedCurio1;
    string selectedCurio2;
    string selectedCurio3;

    int[] completionCurios;

    int[] priceSlots;

    private string[] slots;

    int refreshShopRemaining = 1;

    public GameObject purchaseButton;
    public TextMeshProUGUI costButtonText;

    public GameObject refreshButton;
    public TextMeshProUGUI refreshQuantity;

    public TextMeshProUGUI itemPurchasedQuantityText;

    public TextMeshProUGUI bonusSaleFromCurio;


    int currentindex = 0;

    int currentElectricChip = 0;

    bool[] isPurchased;

    bool isSaleActivated = false;

    void Start()
    {
        isPurchased = new bool[6];
        for(int i = 0; i < 6; i++)
        {
            isPurchased[i] = false;
        }
        slots = new string[6];
        priceSlots = new int[6];
        completionCurios = new int[3];

        SelectCurioToSell();
        FillToShopSlot();
    }

    void Update()
    {
        ShowShopSlot();
        CheckForRefreshButton();
        UpdateChipQuantity();
        CheckForPurchased();
        UpdateItemPurchasedQuantity();
        CheckForSale();
    }

    void CheckForSale()
    {
        float sale = 0;
        string path = pathToCurioData + "/curio2.txt";
        if (File.Exists(path))
        {
            int completion = int.Parse(File.ReadAllText(path));
            sale = completion * 0.0025f;
            bonusSaleFromCurio.text = "Chào mừng thành viên Câu lạc bộ. Bạn nhận được ưu đãi <color=yellow>" + (sale * 100) + "</color>%.";
        }
        if(File.Exists(path) && isSaleActivated == false)
        {
            for (int i = 0; i < 6; i++)
            {
                priceSlots[i] = (int)Math.Round(priceSlots[i] * (1-sale));
            }
            isSaleActivated = true;
        }      
    }

    void UpdateItemPurchasedQuantity()
    {
        int count = int.Parse(File.ReadAllText(pathToPurchasedPotionCount));
        count = 5-count;
        if(count > 0) 
        {
            itemPurchasedQuantityText.text = "Mua thêm <color=yellow>" + count + "</color> vật phẩm để chắc chắn xuất hiện Dược phẩm giới hạn trong lần làm mới cửa hàng tiếp theo.";
        }
        else
        {
            count = 0;
            itemPurchasedQuantityText.text = "Lần làm mới cửa hàng tiếp theo <color=yellow>chắc chắn</color> xuất hiện Dược phẩm giới hạn.";
        }     
    }
    void CheckForPurchased()
    {
        if(currentindex != 0)
        {
            if (isPurchased[currentindex-1] == true)
            {
                costButtonText.text = "Đã mua";
                purchaseButton.GetComponent<Button>().interactable = false;
            }
        }
    }

    void UpdateChipQuantity()
    {
        currentElectricChip = int.Parse(File.ReadAllText(pathToCurrentEChip));
    }
    public void PurchaseItem()
    {
        sellItemPanel.transform.Find("Vật phẩm " + currentindex).transform.Find("Giá").transform.Find("Chưa mua").gameObject.SetActive(false);
        sellItemPanel.transform.Find("Vật phẩm " + currentindex).transform.Find("Giá").transform.Find("Đã mua").gameObject.SetActive(true);
        isPurchased[currentindex - 1] = true;

        int count = int.Parse(File.ReadAllText(pathToPurchasedPotionCount));
        File.WriteAllText(pathToPurchasedPotionCount, (count + 1).ToString());

        if (slots[currentindex - 1].Contains("potion"))
        {
            PurchasePotion();
        }
        if (slots[currentindex - 1].Contains("curio"))
        {
            PurchaseCurio();
        }
    }

    void PurchaseCurio()
    {
        string path = pathToCurioData + "/" + slots[currentindex - 1] + ".txt";
        File.WriteAllText(path, completionCurios[currentindex-1].ToString());
        Debug.Log("Đã mua: " + slots[currentindex - 1] + "Độ hoàn chỉnh: " + completionCurios[currentindex-1]);
        File.WriteAllText(pathToCurrentEChip, (currentElectricChip - priceSlots[currentindex-1]).ToString());
        
    }
    void PurchasePotion()
    {
        int current = 0;
        string path = pathToPotionData + "/" + slots[currentindex-1] + ".txt";
        if (File.Exists(path))
        {
            current = int.Parse(File.ReadAllText(path));
            File.WriteAllText(path, (current+1).ToString());
            Debug.Log("Đã mua: " + slots[currentindex - 1] + "Số lượng hiện tại: " + (current + 1));
        }
        else
        {
            File.WriteAllText(path, "1");
            Debug.Log("Đã mua: " + slots[currentindex - 1] + "Số lượng hiện tại: " + (current + 1));
        }
        File.WriteAllText(pathToCurrentEChip, (currentElectricChip - priceSlots[currentindex - 1]).ToString());

        if (priceSlots[currentindex-1] == 100)
        {
            int count = int.Parse(File.ReadAllText(pathToPurchasedPotionCount));
            count += -5;
            if(count < 0)
            {
                count = 0;
            }
            File.WriteAllText(pathToPurchasedPotionCount, count.ToString());
        }
    }

    void CheckForRefreshButton()
    {
        refreshQuantity.text = "Còn lại: " + refreshShopRemaining.ToString();
        if (refreshShopRemaining > 0)
        {
            refreshButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            refreshButton.GetComponent<Button>().interactable = false;
        }
    }
    public void ShowInformationToPanel(int index)
    {
        currentindex = index;
        if(currentElectricChip >= priceSlots[currentindex - 1])
        {
            purchaseButton.GetComponent<Button>().interactable = true;
            costButtonText.color = Color.white;
        }
        else
        {
            purchaseButton.GetComponent<Button>().interactable = false;
            costButtonText.color = Color.red;
        }
        nameSlot.text = GetItemName(slots[index - 1]);
        GetItemImage(slots[index - 1]);
        priceSlot.text = priceSlots[index - 1].ToString();
        GetItemDescription(index);
    }

    void GetItemDescription(int index)
    {
        if (slots[index-1].Contains("potion"))
        {
            GetPotionDescription(slots[index-1]);
        }
        if (slots[index-1].Contains("curio"))
        {
            GetCurioDescription(slots[index-1], completionCurios[index-1], ExtractNumberFromString(slots[index-1]));
        }
    }


    void GetPotionDescription(string filename)
    {
        string pathToPotion = pathToPotionLib + "/" + filename + ".txt";

        string[] lines = File.ReadAllLines(pathToPotion);

        descriptionSlot.text = lines[3];
    }

    void GetCurioDescription(string filename, int completion, int curioid)
    {
        string path = pathToCurioEffectLib + "/" + filename + ".txt";

        string effect = File.ReadAllText(path);

        effect = ReplaceStatsWithCompletion(effect, completion, curioid);

        effect = ProcessText(effect, completion);

        descriptionSlot.text = effect;
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

    string GetItemName(string filename)
    {
        string path = "";
        if (filename.Contains("potion"))
        {
            path = "Assets/Data/Library_data/Potion_lib/" + filename + ".txt";
        }
        if (filename.Contains("curio"))
        {
            path = "Assets/Data/Library_data/Curio_lib/Curio_Stat/" + filename + ".txt";
        }
        if(path != "")
        {
            string[] lines = File.ReadAllLines(path);
            return lines[0];
        }
        return null;
    }

    void GetItemImage(string filename)
    {
        if (filename.Contains("potion"))
        {
            GetPotionImage(filename);
        }
        if (filename.Contains("curio"))
        {
            GetCurioImage(filename);
        }
    }

    void GetCurioImage(string filename)
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
                    imageSlot.GetComponent<Image>().sprite = newSprite;
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
    }
    void GetPotionImage(string filename)
    {
        string pathToPotion = pathToPotionLib + "/" + filename + ".txt";

        string[] lines = File.ReadAllLines(pathToPotion);

        string image_path = "Assets/Sprite/Dược phẩm/" + lines[0] + ".png";

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
                    imageSlot.GetComponent<Image>().sprite = newSprite;
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
    }



    void ShowShopSlot()
    {
        int curioQuantity = 0;
        for(int i = 0; i<6; i++)
        {
            if (slots[i].Contains("curio"))
            {
                curioQuantity += 1;
            }
        }
        if(curioQuantity > 0)
        {
            DisplayCurioSlot(1, slots[0], completionCurios[0], priceSlots[0]);
        }
        else
        {
            DisplayPotionSlot(1, slots[0], priceSlots[0]);
        }
        
        if (curioQuantity > 1)
        {
            DisplayCurioSlot(2, slots[1], completionCurios[1], priceSlots[1]);
            if (curioQuantity > 2)
            {
                DisplayCurioSlot(3, slots[2], completionCurios[2], priceSlots[2]);
            }
            else
            {
                DisplayPotionSlot(3, slots[2], priceSlots[2]);
            }
        }
        else
        {
            DisplayPotionSlot(2, slots[1], priceSlots[1]);
            DisplayPotionSlot(3, slots[2], priceSlots[2]);
        }
        DisplayPotionSlot(4, slots[3], priceSlots[3]);
        DisplayPotionSlot(5, slots[4], priceSlots[4]);
        DisplayPotionSlot(6, slots[5], priceSlots[5]);

    }


    void DisplayPotionSlot(int index, string slot, int slotPrice)
    {
        string pathToPotion = pathToPotionLib + "/" + slot + ".txt";

        string[] lines = File.ReadAllLines(pathToPotion); 

        string image_path = "Assets/Sprite/Dược phẩm/" + lines[0] + ".png";

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
                    sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Vật thể lạ").transform.Find("Hình vật thể lạ").GetComponent<Image>().sprite = newSprite;
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

        sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Vật thể lạ").transform.Find("Độ hoàn chỉnh").gameObject.GetComponent<TextMeshProUGUI>().text = "";

        if(isPurchased[index - 1] == true)
        {
            sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Giá").transform.Find("Đã mua").gameObject.SetActive(true);

            sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Giá").transform.Find("Chưa mua").gameObject.SetActive(false);
        }
        else
        {
            sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Giá").transform.Find("Đã mua").gameObject.SetActive(false);

            sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Giá").transform.Find("Chưa mua").gameObject.SetActive(true);

            sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Giá").transform.Find("Chưa mua").transform.Find("Giá").gameObject.GetComponent<TextMeshProUGUI>().text = slotPrice.ToString();
        }

        

        if (ExtractNumberFromString(lines[1]) <=1 )
        {
            sellItemPanel.transform.Find("Vật phẩm " + index).gameObject.GetComponent<Image>().sprite = bgForLimitedPotion;
        }
        else
        {
            sellItemPanel.transform.Find("Vật phẩm " + index).gameObject.GetComponent<Image>().sprite = bgForPotion;
        }
    }

    void DisplayCurioSlot(int index, string slot, int completionSlot, int slotPrice)
    {
        string pathToCurioStat = pathToCurioStatLib + "/" + slot + ".txt";
        string pathToCurioEffect = pathToCurioEffectLib + "/" + slot + ".txt";
        string image_path = "Assets/Data/curio_sprite/" + slot + ".png";


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
                    sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Vật thể lạ").transform.Find("Hình vật thể lạ").GetComponent<Image>().sprite = newSprite;
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

        if (isPurchased[index - 1] == true)
        {
            sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Giá").transform.Find("Đã mua").gameObject.SetActive(true);

            sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Giá").transform.Find("Chưa mua").gameObject.SetActive(false);
        }
        else
        {
            sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Giá").transform.Find("Đã mua").gameObject.SetActive(false);

            sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Giá").transform.Find("Chưa mua").gameObject.SetActive(true);

            sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Giá").transform.Find("Chưa mua").transform.Find("Giá").gameObject.GetComponent<TextMeshProUGUI>().text = slotPrice.ToString();
        }

        sellItemPanel.transform.Find("Vật phẩm " + index).transform.Find("Vật thể lạ").transform.Find("Độ hoàn chỉnh").gameObject.GetComponent<TextMeshProUGUI>().text = completionSlot.ToString() + "%";

        if (completionSlot < 100)
        {
            sellItemPanel.transform.Find("Vật phẩm " + index).gameObject.GetComponent<Image>().sprite = bgForCurio;
        }
        else
        {
            sellItemPanel.transform.Find("Vật phẩm " + index).gameObject.GetComponent<Image>().sprite = bgForPerfectedCurio;
        }


    }

    void FillToShopSlot()
    {
        CheckCurioSelected(selectedCurio1, 0, priceSlots[0]);
        CheckCurioSelected(selectedCurio2, 1, priceSlots[1]);
        CheckCurioSelected(selectedCurio3, 2, priceSlots[2]);
        FillRemainingSlotWithPotions();
        DisplayAllSlot();
    }

    public void RefreshShop()
    {
        slots = new string[6];
        isPurchased = new bool[6];
        for (int i = 0; i < 6; i++)
        {
            isPurchased[i] = false;
        }
        SelectCurioToSell();
        FillToShopSlot();
        refreshShopRemaining--;
        isSaleActivated = false;
        Debug.Log("Shop đã được làm mới.");
    }

    void DisplayAllSlot()
    {
        Debug.Log("Vật phẩm bày bán hiện tại: ");
        for (int i = 0; i < slots.Length; i++)
        {
            Debug.Log($"Slot {i + 1}: {slots[i]}");
            Debug.Log($"Priceslots {i + 1}: {priceSlots[i]}");
        }
        Debug.Log($"Số lượng potion đã mua: {ReadPurchasedPotionCount()}");
    }

    void CheckCurioSelected(string selectedCurio, int index, int price)
    {

        Debug.Log("PriceSlot:" + price);
        if (!string.IsNullOrEmpty(selectedCurio))
        {
            Debug.Log("PriceSlot:" + price);
            FillInSlot(Path.GetFileNameWithoutExtension(selectedCurio), index, price);
        }
    }

    void FillRemainingSlotWithPotions()
    {
        // Đọc số lượng potion đã mua từ file
        int purchasedPotionCount = ReadPurchasedPotionCount();

        // Lấy tất cả các file potion
        string[] allPotionFiles = Directory.GetFiles(pathToPotionLib).Where(f => f.EndsWith(".txt")).ToArray();

        // Tạo danh sách để lưu các potion
        List<string> potionList = new List<string>();
        List<string> limitedPotionList = new List<string>(); // Danh sách các potion giới hạn
        List<string> finalPotionList = new List<string>(); // Danh sách cuối cùng gồm 6 potion

        // Đọc thông tin từ các file potion và thêm vào danh sách dựa trên tỉ lệ xuất hiện
        foreach (string file in allPotionFiles)
        {
            string[] lines = File.ReadAllLines(file);
            if (lines.Length >= 4)
            {
                string name = lines[0];
                int rate = ExtractNumberFromString(lines[1]);
                int price = ExtractNumberFromString(lines[2]); // Lấy giá từ dòng thứ 3 trong file
                string effect = lines[3];

                if (rate == 1)
                {
                    limitedPotionList.Add(file); // Thêm potion giới hạn vào danh sách riêng
                }
                else
                {
                    for (int i = 0; i < rate; i++)
                    {
                        potionList.Add(file); // Thêm potion vào danh sách nhiều lần dựa trên tỉ lệ xuất hiện
                    }
                }
            }
        }

        // Khai báo System.Random
        System.Random rand = new System.Random();

        // Thêm log để kiểm tra danh sách potion giới hạn
        Debug.Log("Limited Potion List: " + string.Join(", ", limitedPotionList.Select(p => Path.GetFileNameWithoutExtension(p))));

        // Tạo tỷ lệ xuất hiện cho potion giới hạn
        potionList.AddRange(limitedPotionList); // Thêm potion giới hạn vào danh sách chính để có xác suất xuất hiện

        // Kiểm tra tỷ lệ xuất hiện của potion giới hạn
        if (purchasedPotionCount >= 5 && limitedPotionList.Any())
        {
            string guaranteedLimitedPotion = limitedPotionList.OrderBy(x => rand.Next()).First();
            finalPotionList.Add(guaranteedLimitedPotion);
        }

        // Điền phần còn lại của danh sách với các potion ngẫu nhiên
        var randomPotions = potionList.OrderBy(x => rand.Next()).Take(6 - finalPotionList.Count).ToList();
        finalPotionList.AddRange(randomPotions);

        // Điền các potion vào các slot trống và lưu giá vào priceSlots
        for (int i = 0; i < finalPotionList.Count; i++)
        {
            int slotIndex = FindEmptySlot();
            if (slotIndex == -1) break; // Nếu không còn slot trống, thoát khỏi vòng lặp

            string[] lines = File.ReadAllLines(finalPotionList[i]);
            if (lines.Length >= 4)
            {
                string name = lines[0];
                int price = ExtractNumberFromString(lines[2]); // Lấy giá từ dòng thứ 3 trong file
                FillInSlot(Path.GetFileNameWithoutExtension(finalPotionList[i]), slotIndex, price);
            }
        }

        // Thêm log để kiểm tra danh sách potion cuối cùng
        Debug.Log("Final Potion List: " + string.Join(", ", finalPotionList.Select(p => Path.GetFileNameWithoutExtension(p))));
    }




    void FillInSlot(string name, int slotIndex, int price)
    {
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            slots[slotIndex] = name;
            priceSlots[slotIndex] = price;
            Debug.Log($"Slot {slotIndex + 1}: {slots[slotIndex]}, Price: {priceSlots[slotIndex]}");
        }
    }

    int FindEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (string.IsNullOrEmpty(slots[i]))
            {
                return i;
            }
        }
        return -1; // Không có slot trống
    }





    void SelectCurioToSell()
    {
        completionCurios = new int[3];
        slots = new string[6];
        priceSlots = new int[6];
        selectedCurio1 = null;
        selectedCurio2 = null;
        selectedCurio3 = null;

        // Lấy tất cả các file curio
        string[] allCurioFiles = Directory.GetFiles(pathToCurioStatLib).Where(f => f.EndsWith(".txt")).ToArray();

        // Lấy tất cả các file curio đã sở hữu
        string[] ownedCurioFiles = Directory.GetFiles(pathToCurioData).Where(f => f.EndsWith(".txt")).ToArray();

        // Lấy tên các curio đã sở hữu
        HashSet<string> ownedCurios = new HashSet<string>(ownedCurioFiles.Select(f => Path.GetFileNameWithoutExtension(f)));

        List<string> availableCurios = new List<string>();
        Dictionary<string, int> curioWeights = new Dictionary<string, int>();

        foreach (string file in allCurioFiles)
        {
            string curioName = Path.GetFileNameWithoutExtension(file);

            // Nếu người chơi chưa sở hữu curio này
            if (!ownedCurios.Contains(curioName))
            {
                availableCurios.Add(file);

                string[] lines = File.ReadAllLines(file);
                if (lines.Length >= 3 && int.TryParse(lines[2], out int weight))
                {
                    curioWeights[curioName] = weight;
                }
                else
                {
                    curioWeights[curioName] = 1; // Trọng số mặc định là 1 nếu không tìm thấy hoặc không đọc được
                }
            }
        }

        // Tạo danh sách các curio dựa trên trọng số (weight)
        List<string> weightedCurios = new List<string>();
        foreach (var curio in availableCurios)
        {
            string curioName = Path.GetFileNameWithoutExtension(curio);
            int weight = curioWeights[curioName];

            for (int i = 0; i < weight; i++)
            {
                weightedCurios.Add(curio);
            }
        }

        // Sử dụng System.Random để chọn ngẫu nhiên tối đa 3 curio chưa sở hữu để hiển thị
        System.Random rand = new System.Random();
        var randomCurios = weightedCurios.OrderBy(x => rand.Next()).Distinct().Take(3).ToList();

        // Lưu tên file của các curio được chọn vào các biến string
        if (randomCurios.Count > 0)
        {
            selectedCurio1 = Path.GetFileNameWithoutExtension(randomCurios[0]);
            if (Path.GetFileNameWithoutExtension(randomCurios[0]) != "curio10")
            {
                completionCurios[0] = UnityEngine.Random.Range(60, 101);
                if (completionCurios[0] != 100)
                {
                    priceSlots[0] = completionCurios[0] * 2;
                }
                else
                {
                    priceSlots[0] = completionCurios[0] * 2 + 100;
                }

            }
            else
            {
                completionCurios[0] = 100;
                priceSlots[0] = completionCurios[0] * 7 + 77;
            }
        }
        if (randomCurios.Count > 1)
        {
            selectedCurio2 = Path.GetFileNameWithoutExtension(randomCurios[1]);
            if (Path.GetFileNameWithoutExtension(randomCurios[1]) != "curio10")
            {
                completionCurios[1] = UnityEngine.Random.Range(60, 101);
                if (completionCurios[1] != 100)
                {
                    priceSlots[1] = completionCurios[1] * 2;
                }
                else
                {
                    priceSlots[1] = completionCurios[1] * 2 + 100;
                }

            }
            else
            {
                completionCurios[1] = 100;
                priceSlots[1] = completionCurios[1] * 7 + 77;
            }
        }
        if (randomCurios.Count > 2)
        {
            selectedCurio3 = Path.GetFileNameWithoutExtension(randomCurios[2]);
            if (Path.GetFileNameWithoutExtension(randomCurios[2]) != "curio10")
            {
                completionCurios[2] = UnityEngine.Random.Range(60, 101);
                if (completionCurios[2] != 100)
                {
                    priceSlots[2] = completionCurios[2] * 2;
                }
                else
                {
                    priceSlots[2] = completionCurios[2] * 2 + 100;
                }

            }
            else
            {
                completionCurios[2] = 100;
                priceSlots[2] = completionCurios[2] * 7 + 77;
            }
        }
    }

    private int ExtractNumberFromString(string input)
    {
        string numberString = Regex.Match(input, @"\d+").Value;
        return int.Parse(numberString);
    }

    private int ReadPurchasedPotionCount()
    {
        if (File.Exists(pathToPurchasedPotionCount))
        {
            string content = File.ReadAllText(pathToPurchasedPotionCount);
            if (int.TryParse(content, out int count))
            {
                return count;
            }
        }
        return 0;
    }

    private void WritePurchasedPotionCount(int count)
    {
        File.WriteAllText(pathToPurchasedPotionCount, count.ToString());
    }
}
