using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;

public class ItemMerge : MonoBehaviour
{
    
    public Button showButton;        
    public Button closeButton;        
    public Button mergeButton;        // Nút bấm để ghép đồ
    public GameObject panel;          // Panel  hiển thị/ẩn
    public Image itemImage;           // Hình ảnh của item
    public TextMeshProUGUI itemQuantityText; // Số lượng của item 1
    public TextMeshProUGUI itemNameText;     // Tên item 1
    public TextMeshProUGUI resultMessageText; 

    public Button backToMenuButton;

    // Thông tin item ghép được
    public TextMeshProUGUI secondItemNameText;
    public Image secondItemImage;
    public TextMeshProUGUI secondItemQuantityText;

    
    private string folderPathQuan = "Assets/Data/InventoryData"; 
    private string folderPathLib = "Assets/Data/LibraryData";    

    [SerializeField] private InventoryController instance;

    void Start()
    {
       
        
        panel.SetActive(false);

       
        showButton.onClick.AddListener(ShowPanel);
        closeButton.onClick.AddListener(HidePanel);
        mergeButton.onClick.AddListener(MergeSelectedItem);
        backToMenuButton.onClick.AddListener(SwitchToMenuScene);
    }

    public  void SwitchToMenuScene()
    {
        SceneManager.LoadScene("StartMenu");
    }

    //  đọc dữ liệu từ file
    private bool TryReadItemFile(string filePath, out string[] lines)
    {
        lines = null;
        if (File.Exists(filePath))
        {
            lines = File.ReadAllLines(filePath);
            return true;
        }
        Debug.LogError($"Không tìm thấy file: {filePath}");
        return false;
    }

    // Lấy ID từ dòng 5 của file thông tin
private (string, int) GetItemIdFromInfoFile(string itemInfoFilePath)
{
    if (TryReadItemFile(itemInfoFilePath, out string[] lines) && lines.Length >= 6)
    {
        string itemId = lines[4]; 
        int quantityToSubtract = int.TryParse(lines[5], out int result) ? result : 0; 
        return (itemId, quantityToSubtract);
    }
    return (null, 0);
}


    // Hiển thị số lượng của item chính (item 1)
    private void UpdatePrimaryItemQuantityDisplay(string itemId)
    {
        string quantityFilePath = Path.Combine(folderPathQuan, $"item{itemId}.txt");

        if (TryReadItemFile(quantityFilePath, out string[] lines) && lines.Length > 0 && int.TryParse(lines[0], out int quantity))
        {
            itemQuantityText.text = $"Số lượng: {quantity}";
        }
        else
        {
            itemQuantityText.text = "Số lượng: 0";
        }
    }

    // Hiển thị số lượng của item ghép (item 2)
    private void UpdateSecondaryItemQuantityDisplay(string itemId)
    {
        string quantityFilePath = Path.Combine(folderPathQuan, $"{itemId}.txt");

        if (TryReadItemFile(quantityFilePath, out string[] lines) && lines.Length > 0 && int.TryParse(lines[0], out int quantity))
        {
            secondItemQuantityText.text = $"Số lượng: {quantity}";
        }
        else
        {
            secondItemQuantityText.text = "Số lượng: 0";
        }
    }

    // Hiển thị thông tin của item
    public void ShowPanel()
    {
        resultMessageText.text = "";
        if (InventoryController.instance.idItemSelected != null)
        {
            var selectedItem = InventoryController.instance.itemInventories
                .FirstOrDefault(item => item.id == InventoryController.instance.idItemSelected);

            if (selectedItem != null)
            {
                itemNameText.text = selectedItem.name;
                string imagePath = $"item/item{selectedItem.id}";

                Sprite itemSprite = Resources.Load<Sprite>(imagePath);
                if (itemSprite != null)
                {
                    itemImage.sprite = itemSprite;
                }
                else
                {
                    Debug.LogError($"Không tìm thấy hình ảnh cho item {selectedItem.id}");
                }

                string selectedItemInfoFilePath = Path.Combine(folderPathLib, $"item{selectedItem.id}.txt");

                // Hiển thị số lượng của item chính (item 1)
                UpdatePrimaryItemQuantityDisplay(selectedItem.id.ToString());

                // Lấy ID của item thứ 2 và số lượng bị trừ từ file
                var (secondItemId, _) = GetItemIdFromInfoFile(selectedItemInfoFilePath);

                if (string.IsNullOrEmpty(secondItemId))
                {
                    secondItemNameText.text = "";
                    secondItemImage.sprite = null;
                    secondItemImage.gameObject.SetActive(false);
                    secondItemQuantityText.text = "";
                    resultMessageText.text = "!Không ghép được.";  
                }
                else
                {
                    secondItemImage.gameObject.SetActive(true);
                    string secondItemInfoFilePath = Path.Combine(folderPathLib, $"{secondItemId}.txt");

                    if (TryReadItemFile(secondItemInfoFilePath, out string[] secondItemLines) && secondItemLines.Length > 0)
                    {
                        secondItemNameText.text = secondItemLines[0];

                        string secondItemImagePath = $"item/{secondItemId}";
                        Sprite secondItemSprite = Resources.Load<Sprite>(secondItemImagePath);
                        if (secondItemSprite != null)
                        {
                            secondItemImage.sprite = secondItemSprite;
                        }
                        else
                        {
                            Debug.LogError($"Không tìm thấy hình ảnh cho item {secondItemId}");
                        }

                        // Hiển thị số lượng của item thứ 2
                        UpdateSecondaryItemQuantityDisplay(secondItemId);
                    }
                }

                panel.SetActive(true);
            }
            else
            {
                Debug.LogError("Không tìm thấy item đã chọn.");
            }
        }
        else
        {
            Debug.LogError("Không có item nào được chọn.");
        }
    }



    // Ẩn panel
    public void HidePanel()
    {
        panel.SetActive(false);
    }

    // Ghép đồ
   public void MergeSelectedItem()
{
    if (InventoryController.instance.idItemSelected != null)
    {
        var selectedItem = InventoryController.instance.itemInventories
            .FirstOrDefault(item => item.id == InventoryController.instance.idItemSelected);

        if (selectedItem != null)
        {
            string itemInfoFilePath = Path.Combine(folderPathLib, $"item{selectedItem.id}.txt");

            // Lấy ID của item và số lượng bị trừ từ file
            var (secondItemId, quantityToSubtract) = GetItemIdFromInfoFile(itemInfoFilePath);

            if (!string.IsNullOrEmpty(secondItemId))
            {
                string quantityFilePath = Path.Combine(folderPathQuan, $"item{selectedItem.id}.txt");

                if (TryReadItemFile(quantityFilePath, out string[] quantityLines) && quantityLines.Length > 0 && int.TryParse(quantityLines[0], out int currentQuantity))
                {
                    if (currentQuantity >= quantityToSubtract)  // Kiểm tra nếu số lượng đủ để ghép
                    {
                        currentQuantity -= quantityToSubtract;
                        quantityLines[0] = currentQuantity.ToString();
                        File.WriteAllLines(quantityFilePath, quantityLines);

                        itemQuantityText.text = $"Số lượng: {currentQuantity}";

                        //  cập nhật số lượng item thứ 2
                        UpdateItemQuantity(secondItemId);
                        UpdateSecondaryItemQuantityDisplay(secondItemId);

                        resultMessageText.text = "Ghép đồ thành công!";
                    }
                    else
                    {
                        resultMessageText.text = "Không đủ số lượng để ghép đồ.";
                    }
                }
                else
                {
                    resultMessageText.text = "Dữ liệu số lượng không hợp lệ.";
                }
            }
            else
            {
                resultMessageText.text = "Không thể ghép item này.";
            }
        }
        else
        {
            resultMessageText.text = "Không tìm thấy item đã chọn.";
        }
    }
    else
    {
        resultMessageText.text = "Không có item nào được chọn.";
    }

    RefreshInventoryUI();
}


    //update số lượng item2
    private void UpdateItemQuantity(string itemId)
    {
        string quantityFilePath = Path.Combine(folderPathQuan, $"{itemId}.txt");

        if (TryReadItemFile(quantityFilePath, out string[] lines) && lines.Length > 0 && int.TryParse(lines[0], out int currentQuantity))
        {
            currentQuantity++;
            lines[0] = currentQuantity.ToString();
            File.WriteAllLines(quantityFilePath, lines);

            if (DetailPanelController.instance != null && itemId == InventoryController.instance.idItemSelected)
            {
                DetailPanelController.instance.UpdateItemQuantity(currentQuantity);
            }
        }
        else
        {
            File.WriteAllText(quantityFilePath, "1\n");
        }
    }

    public void RefreshInventoryUI()
    {
        
        foreach (var item in InventoryController.instance.itemObjects)
        {
            var itemSlot = item.GetComponent<ItemSlot>();
            string quantityFilePath = Path.Combine(folderPathQuan, $"item{itemSlot.id}.txt");

            if (TryReadItemFile(quantityFilePath, out string[] lines) && lines.Length > 0 && int.TryParse(lines[0], out int quantity))
            {
                itemSlot.UpdateQuantity(quantity);
            }
        }
     
    }

}
