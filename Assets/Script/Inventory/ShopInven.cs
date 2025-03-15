using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using System;

public class ShopInven : MonoBehaviour
{
    public InventoryController invencontrol;
    public GameObject shopPanel;
    public GameObject detailPanel;

    public TextMeshProUGUI clickCountText1, clickCountText2, clickCountText3;
    private int clickCount1 = 0, clickCount2 = 0, clickCount3 = 0;

    string folderPathQuan = "Assets/Data/InventoryData"; // Đường dẫn file số lượng
    private Dictionary<string, int> itemClicks = new Dictionary<string, int>();
    public TextMeshProUGUI moneyText; // Text hiển thị số tiền
    private string moneyFilePath = "Assets/Data/InventoryData/item00001.txt"; // File lưu số tiền
    private int money = 0; // Số tiền hiện tại

    private void Start()
    {
        // Khởi tạo số lần click cho các item
        itemClicks["item10000"] = 0;
        itemClicks["item10001"] = 0;
        itemClicks["item10002"] = 0;
        LoadMoney();
    }

    private Dictionary<string, int> itemPrices = new Dictionary<string, int>()
{
    {"item10000", 200},
    {"item10001", 2000},
    {"item10002", 20000}
};
    // Đọc số tiền từ file
    private void LoadMoney()
    {
        if (File.Exists(moneyFilePath))
        {
            string moneyStr = File.ReadAllText(moneyFilePath);
            if (int.TryParse(moneyStr, out money))
            {
                UpdateMoneyText();
            }
            else
            {
                Debug.LogError($"Không thể đọc số tiền từ file: {moneyFilePath}");
            }
        }
        else
        {
            Debug.LogError($"File số tiền không tồn tại: {moneyFilePath}");
        }
    }
    // Lưu tiền vào file
    private void SaveMoney()
    {
        File.WriteAllText(moneyFilePath, money.ToString());
    }

    // Cập nhật tiền lên UI
    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = " " + money;
        }
    }
    // Ẩn/hiện panel shop
    public void ToggleShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
        }

        if (invencontrol != null)
        {
            if (invencontrol.InventoryBoardContent != null)
            {
                invencontrol.InventoryBoardContent.SetActive(!shopPanel.activeSelf);
            }
        }

        if (detailPanel != null)
        {
            detailPanel.SetActive(!shopPanel.activeSelf);
        }

        
        ReloadUI();
    }

    public void ReloadUI()
    {
        
        invencontrol.ClearAllItems();

    
        var Items = Directory.GetFiles(invencontrol.folderPathLib).Where(file => !file.EndsWith(".meta")).ToArray();
        invencontrol.InitItems(Items);  

        
        invencontrol.LoadView(invencontrol.itemInventories.OrderByDescending(f => f.rank).ToList());
    }

    // Xử lý khi bấm nút item 10006
    public void OnButtonClick10006()
    {
        itemClicks["item10000"]++;
        UpdateClickCountText(clickCountText1, itemClicks["item10000"]);
    }

    // Xử lý khi bấm nút item 10007
    public void OnButtonClick10007()
    {
        itemClicks["item10001"]++;
        UpdateClickCountText(clickCountText2, itemClicks["item10001"]);
    }

    // Xử lý khi bấm nút item 10008
    public void OnButtonClick10008()
    {
        itemClicks["item10002"]++;
        UpdateClickCountText(clickCountText3, itemClicks["item10002"]);
    }

    // Cập nhật hiển thị text số lần bấm
    private void UpdateClickCountText(TextMeshProUGUI clickCountText, int clickCount)
    {
        if (clickCountText != null)
        {
            clickCountText.text = "Số lượng: " + clickCount;
        }
    }

    // Xử lý khi ấn nút mua
    public void BuyItems()
    {
        var itemKeys = new List<string>(itemClicks.Keys);

        foreach (var itemId in itemKeys)
        {
            int quantityToAdd = itemClicks[itemId];
            if (quantityToAdd > 0)
            {
                if (itemPrices.TryGetValue(itemId, out int itemCost))
                {
                    int totalCost = itemCost * quantityToAdd;

                    if (money >= totalCost)
                    {
                        money -= totalCost; 
                        UpdateQuantityFile(itemId + ".txt", quantityToAdd);
                       
                        // Cập nhật số lượng trong inventory
                        InventoryController.instance.UpdateItemQuantity(itemId, quantityToAdd);
                        itemClicks[itemId] = 0;  // Đặt lại số lần bấm của item về 0
                        UpdateClickCountText(clickCountText1, itemClicks["item10000"]);
                        UpdateClickCountText(clickCountText2, itemClicks["item10001"]);
                        UpdateClickCountText(clickCountText3, itemClicks["item10002"]);
                    }
                    else
                    {
                        Debug.LogWarning($"Không đủ tiền để mua item: {itemId}");
                    }
                }
                else
                {
                    Debug.LogError($"Không tìm thấy giá cho item: {itemId}");
                }
            }
        }

        // Cập nhật lại UI tiền và ghi file
        UpdateMoneyText();
        SaveMoney();
    }



    // Hàm cập nhật file số lượng
    private void UpdateQuantityFile(string fileName, int quantityToAdd)
    {
        string filePath = Path.Combine(folderPathQuan, fileName);

        if (File.Exists(filePath))
        {
            int currentQuantity = int.Parse(File.ReadAllText(filePath));
            currentQuantity += quantityToAdd;
            File.WriteAllText(filePath, currentQuantity.ToString());
            Debug.Log($"Đã cập nhật số lượng trong {fileName}: {currentQuantity}");
        }
        else
        {
            Debug.LogError($"Không tìm thấy file: {filePath}");
        }
    }
}
