using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailPanelController : MonoBehaviour
{
    public static DetailPanelController instance;
    public Image ItemImage;                     // Hình ảnh của item
    public TextMeshProUGUI ItemName;            // Tên item
    public TextMeshProUGUI ItemDes;             // Mô tả item
    public TextMeshProUGUI ItemQuans;           // Số lượng item

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogWarning("Đã tồn tại .");
            Destroy(gameObject);
        }
    }


    public void ShowDetails(Sprite itemImage, string name, string des, int quans)
    {
        ItemImage.gameObject.SetActive(false);
        if (itemImage != null)
        {
            ItemImage.gameObject.SetActive(true);
            ItemImage.sprite = itemImage;
        }
        else
        {
            Debug.LogWarning("Sprite của item không tìm thấy.");
        }

        ItemName.text = !string.IsNullOrEmpty(name) ? name : "Tên không xác định";
        ItemDes.text = !string.IsNullOrEmpty(des) ? des : "Không có mô tả";
        ItemQuans.text = "Số lượng: " + quans;
    }

   //cập nhật số lượng item sau khi ghép
    public void UpdateItemQuantity(int newQuantity)
    {
        if (ItemQuans != null)
        {
            ItemQuans.text = "Số lượng: " + newQuantity;
        }
        else
        {
            Debug.LogWarning("dữ liệu không hợp lệ");
        }
    }
}
