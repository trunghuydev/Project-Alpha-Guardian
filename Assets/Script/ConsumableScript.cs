using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ConsumableScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject invent;
    public Animator animator;
    public GameObject itemSlot0;
    public GameObject itemSlot1;
    public GameObject itemSlot2;
    public GameObject itemSlot3;
    public GameObject itemSlot4;

    public Sprite five_stars_bg;
    public Sprite four_stars_bg;
    public Sprite three_stars_bg;
    public Sprite two_stars_bg;

    public GameObject Confirmation;



    int quantity;

    bool isExpanded = false;

    string folderPath = "Assets/Data/ingame_data/consume_item";

    string itemCurrentPath = "Assets/Data/receive/itemCurrent.txt";

    void Update()
    {
        LoadItemsFromFolder();
    }

    void LoadItemsFromFolder()
    {
        if (Directory.Exists(folderPath))
        {
            // Lấy tất cả các file không phải đuôi .meta trong thư mục
            string[] files = Directory.GetFiles(folderPath).Where(f => !f.EndsWith(".meta")).ToArray();
            for (int i = 0; i < files.Length; i++)
            {
                string itemName = Path.GetFileNameWithoutExtension(files[i]);
                int.TryParse(File.ReadAllText(files[i]), out int quantity);
                DeclareSlot(itemName, quantity, i);
            }
        }
        else
        {
            Debug.LogError("Directory does not exist at path: " + folderPath);
        }
    }


    public void DeclareSlot(string itemName, int quantity, int index)
    {
        switch (index)
        {
            case 0:
                AddItem(itemName, quantity, itemSlot0);
                break;
            case 1:
                AddItem(itemName, quantity, itemSlot1);
                break;
            case 2:
                AddItem(itemName, quantity, itemSlot2);
                break;
            case 3:
                AddItem(itemName, quantity, itemSlot3);
                break;
            case 4:
                AddItem(itemName, quantity, itemSlot4);
                break;
        }
    }

    public void AddItem(string itemName, int quantity, GameObject item)
    {
        item.SetActive(true);
        string info_path = "Assets/Data/item_info/" + itemName + ".txt";
        string image_path = "Assets/Data/item_sprite/" + itemName + ".png";

        string[] lines = File.ReadAllLines(info_path);

        GameObject BG = item.gameObject.transform.Find("BG").gameObject;
        int.TryParse(lines[3], out int rarity);

        switch (rarity)
        {
            case 5:
                BG.GetComponent<Image>().sprite = five_stars_bg;
                break;
            case 4:
                BG.GetComponent<Image>().sprite = four_stars_bg;
                break;
            case 3:
                BG.GetComponent<Image>().sprite = three_stars_bg;
                break;
            case 2:
                BG.GetComponent<Image>().sprite = two_stars_bg;
                break;
        }

        GameObject itemSprite = item.gameObject.transform.Find("Sprite").gameObject;

        // Check if file exists
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
                    // Create a new sprite from the texture
                    Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                    // Assign the sprite to the Image component
                    if (itemSprite != null && itemSprite.GetComponent<Image>() != null)
                    {
                        itemSprite.GetComponent<Image>().sprite = newSprite;
                    }
                    else
                    {
                        Debug.LogError("Image component is missing on the target object.");
                    }
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


        GameObject Quantity = item.gameObject.transform.Find("Quantity").gameObject;

        Quantity.GetComponent<TextMeshProUGUI>().text = quantity.ToString();

            item.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(itemName));

    }

    public void OnButtonClick(string itemid)
    {
        File.WriteAllText(itemCurrentPath, itemid);
        Confirmation.SetActive(true);
    }

    public void ExpandInventory()
    {
        if (!isExpanded)
        {
            animator.SetTrigger("trigger");
            isExpanded = true;
        }
        else
        {
            animator.SetTrigger("trigger");
            isExpanded = false;
        }
    }
}
