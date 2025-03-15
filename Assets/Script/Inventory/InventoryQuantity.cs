using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class InventoryQuantity : MonoBehaviour
{

    public GameObject ItemSlot;
    public Sprite five_stars_bg;
    public Sprite four_stars_bg;
    public Sprite three_stars_bg;
    public Sprite two_stars_bg;
    public TextMeshProUGUI quantity;
    public GameObject Image;
    public GameObject Bg;

    void Start()
    {

       
        string folderPathQuan = "Assets/Data/InventoryData";

        // Đếm số lượng file trong thư mục InventoryData
        var Items = Directory.GetFiles(folderPathQuan).Where(file => !file.EndsWith(".meta")).ToArray();
        int num = Items.Length;

        // Hiển thị số lượng file ra console
        Debug.Log($"Số lượng file trong thư mục {folderPathQuan}: {num}");

        // Hiển thị tên các file ra console
        foreach (var file in Items)
        {
            Debug.Log($"Tên file: {Path.GetFileName(file)}");
        }

    }


    void Update()
    {
        string folderPathLib = "Assets/Data/LibraryData";
        string folderPathQuan = "Assets/Data/InventoryData";
        var Items = Directory.GetFiles(folderPathQuan).Where(file => !file.EndsWith(".meta")).ToArray();
        // soố lượng items
        int num = Items.Length;
        //foreach(string item in Items)
        //{
        int.TryParse(File.ReadAllText(folderPathQuan + "/item00000.txt"), out int Quantity);
        quantity.text = Quantity.ToString();
        //}


        // Đọc và đặt hình ảnh của vật phẩm từ file ảnh
        string image_path = "Assets/Resources/item/item" + "00000" + ".png";
        if (File.Exists(image_path))
        {
            byte[] imageData = File.ReadAllBytes(image_path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Image.GetComponent<Image>().sprite = newSprite;
        }
        else
        {
            Debug.LogError($"Không tìm thấy ảnh tại: {image_path}");
        }



        string[] lines = File.ReadAllLines(folderPathLib + "/item00000.txt");
        switch (lines[3])
        {
            case "5":
                Bg.GetComponent<Image>().sprite = five_stars_bg;
                break;
            case "4":
                Bg.GetComponent<Image>().sprite = four_stars_bg;
                break;
            case "3":
                Bg.GetComponent<Image>().sprite = three_stars_bg;
                break;
            case "2":
                Bg.GetComponent<Image>().sprite = two_stars_bg;
                break;
        }
        Debug.Log(lines[3]);

    }
}
