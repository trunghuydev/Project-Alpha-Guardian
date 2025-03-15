using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using System;

public class InventoryController : MonoBehaviour
{
    public static InventoryController instance;
    public GameObject itemPrefab;
    public Sprite five_stars_bg;
    public Sprite four_stars_bg;
    public Sprite three_stars_bg;
    public Sprite two_stars_bg;
    public GameObject InventoryBoardContent;
    public List<ItemInventory> itemInventories;
    public List<GameObject> itemObjects;
    public List<ItemInventory> inventoryFilterLists;
    public string idItemSelected;
    public string folderPathLib = "Assets/Data/LibraryData";
    string folderPathQuan = "Assets/Data/InventoryData";
 


    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // Đếm số lượng file trong thư mục InventoryData
        var Items = Directory.GetFiles(folderPathLib).Where(file => !file.EndsWith(".meta")).ToArray();
        int num = Items.Length;
        // Hiển thị số lượng file ra console
        Debug.Log($"Số lượng file trong thư mục {folderPathQuan}: {num}");
        InitItems(Items);
    }
    public void InitItems(String[] itemFiles)
    {
        itemInventories = new List<ItemInventory>();
        if (itemInventories.Count > 0)
        {
            itemInventories.Clear();
        }
        // Hiển thị tên các file ra console
        for (int i = 0; i < itemFiles.Length; i++)
        {
            InitItemsAndQuans(itemFiles[i]);
        }
        //Load View
        LoadView(itemInventories.OrderByDescending(f => f.rank).ToList());
        inventoryFilterLists = itemInventories;
    }
    private void InitItemsAndQuans(string itemFileName)
    {
        //Lấy controller của prefab

        Debug.Log(itemFileName);
        // Đọc và đặt hình ảnh của vật phẩm từ file ảnh

        var quans = int.TryParse(File.ReadAllText(folderPathQuan + "/" + itemFileName.Split("/").Last().Substring(12)), out int Quantity);
        string[] lines = File.ReadAllLines(folderPathLib + "/" + itemFileName.Split("/").Last().Substring(12));
        var id = itemFileName.Split("/").Last().Substring(16, 5);
        var name = lines[0];
        var des = lines[1] + "\n" + lines[2];
        var rank = int.Parse(lines[3]);
        var quanity = quans ? int.Parse(File.ReadAllText(folderPathQuan + "/" + itemFileName.Split("/").Last().Substring(12))) : 0;
        var newInventoryItem = new ItemInventory(id, itemFileName, name, des, quanity, rank);
        itemInventories.Add(newInventoryItem);
    }
    public void LoadView(List<ItemInventory> items)
    {
        ClearAllItems();
        foreach (var newInventoryItem in items)
        {
            string image_path = "Assets/Resources/item/item" + newInventoryItem.id + ".png";
            if (File.Exists(image_path))
            {
                byte[] imageData = File.ReadAllBytes(image_path);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                if (newInventoryItem.quanity < 1) continue;
                var itemSlotController = itemPrefab.GetComponent<ItemSlot>();
                itemSlotController.Initialize(newSprite, newInventoryItem.id, newInventoryItem.quanity, GetRankBG(newInventoryItem.rank.ToString()), newInventoryItem.name, newInventoryItem.description);
                var item = Instantiate(itemPrefab, InventoryBoardContent.transform);
                itemObjects.Add(item);
                Toggle toggle = item.GetComponent<Toggle>();
                toggle.group = item.transform.parent.GetComponent<ToggleGroup>();
            }
            else
            {
                Debug.LogError($"Không tìm thấy ảnh tại: {image_path}");
            }
        }
    }

    
    public Sprite GetRankBG(string line)
    {
        switch (line)
        {
            case "5":
                return five_stars_bg;
            case "4":
                return four_stars_bg;
            case "3":
                return three_stars_bg;
            case "2":
                return two_stars_bg;
            default: return null;
        }
    }

    public void OnItemSelected(string itemId)
    {
        idItemSelected = itemId;
        Debug.Log($"Item được chọn: {idItemSelected}");
    }

    public void BreakSelectedItem()
    {
        if (!string.IsNullOrEmpty(idItemSelected))
        {
            BreakItem(idItemSelected);
        }
        else
        {
            Debug.LogWarning("Không có item nào được chọn để xóa.");
        }
    }

    public void BreakItem(string itemId)
    {
        Debug.Log($"BreakItem: Xóa item với ID {itemId}");

        // Tìm item trong danh sách itemObjects
        GameObject itemToRemove = null;
        foreach (var item in itemObjects)
        {
            var itemController = item.GetComponent<ItemSlot>();
            if (itemController.id == itemId)
            {
                itemToRemove = item; 
                break;
            }
        }

        if (itemToRemove == null)
        {
            Debug.LogWarning($"Không tìm thấy item trong UI với ID: {itemId}");
            return;
        }

        Destroy(itemToRemove);
        itemObjects.Remove(itemToRemove);

        var itemInventory = itemInventories.FirstOrDefault(item => item.id == itemId);
        if (itemInventory != null)
        {
            itemInventory.quanity = 0;

            string filePath = Path.Combine(folderPathQuan,"item"+ itemId + ".txt");
            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, "0");
                Debug.Log($"Số lượng của item {itemId} đã được cập nhật về 0 tại {filePath}");
            }
            else
            {
                Debug.LogError($"Không tìm thấy file số lượng tại: {filePath}");
            }

            // Cập nhật danh sách
            itemInventories = itemInventories.Where(item => item.id != itemId).ToList();
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy item trong danh sách itemInventories với ID: {itemId}");
        }
    }



    private void RewriteFile(ItemSlot item)
    {
        foreach (ItemInventory i in itemInventories)
        {
            if (i.id == item.id)
            {
                i.quanity = 0;
                File.WriteAllText(folderPathQuan + "/item" + i.id + ".txt", "0");
                break;
            }
        }
        var Items = Directory.GetFiles(folderPathLib).Where(file => !file.EndsWith(".meta")).ToArray();
        InitItems(Items);
    }

    internal void ClearAllItems()
    {
        itemObjects.Clear();
        foreach (Transform t in InventoryBoardContent.transform)
        {
            Destroy(t.gameObject);
        }
    }
    void OnEnable()
    {
        var Items = Directory.GetFiles(folderPathLib).Where(file => !file.EndsWith(".meta")).ToArray();
        InitItems(Items);
    }


    //ghép đồ test script 
    public void UpdateItemQuantity(string itemId, int newQuantity)
    {
        // Cập nhật số lượng trong danh sách itemInventories
        var itemToUpdate = itemInventories.FirstOrDefault(item => item.id == itemId);
        if (itemToUpdate != null)
        {
            itemToUpdate.quanity = newQuantity;

           
            foreach (var itemObject in itemObjects)
            {
                var itemController = itemObject.GetComponent<ItemSlot>();
                if (itemController.id == itemId)
                {
                    itemController.UpdateQuantity(newQuantity); 
                    break;
                }
            }

 
        }
    }


    // test load panel mỗi frame 
    public ItemInventory GetItemById(string itemId)
    {
        
        return itemInventories.FirstOrDefault(item => item.id == itemId);
    }



}

