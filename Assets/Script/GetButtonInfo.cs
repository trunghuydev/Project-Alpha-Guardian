using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GetButtonInfo : MonoBehaviour
{
    public GameObject successfullyPanel;
    public GameObject notificationPanel;
    //public GameObject failurePanel;
    public TextMeshProUGUI item_name;
    public TextMeshProUGUI item_description;
    public GameObject image_line;
    public GameObject image;
    public GameObject sprite;
    public Sprite five_stars_bg;
    public Sprite four_stars_bg;
    public Sprite three_stars_bg;
    public Sprite two_stars_bg;

    public TextMeshProUGUI Quantity;

    string itemCurrentPath = "Assets/Data/receive/itemCurrent.txt";

    string currentItemId;
    // Start is called before the first frame update
    void Update()
    {
        string itemid = File.ReadAllText(itemCurrentPath);

        currentItemId = itemid;

        string pathToDescription = "Assets/Data/item_info/" + itemid + ".txt";

        string pathToImage = "Assets/Data/item_sprite/" + itemid + ".png";

        string[] lines = File.ReadAllLines(pathToDescription);

        int line_num = 0;

        foreach (string line in lines)
        {
            line_num++;

            switch (line_num)
            {
                case 1:
                    item_name.text = line;
                    break;
                case 2:
                    item_description.text = line;
                    break;
                case 3:
                    item_description.text += "\n\n" + line;
                    break;
                case 4:
                    switch (line)
                    {
                        case "5":
                            image.GetComponent<Image>().sprite = five_stars_bg;
                            image_line.GetComponent<Image>().color = new Color(209 / 255f, 211 / 255f, 110 / 255f);
                            break;
                        case "4":
                            image.GetComponent<Image>().sprite = four_stars_bg;
                            image_line.GetComponent<Image>().color = new Color(192 / 255f, 142 / 255f, 184 / 255f);
                            break;
                        case "3":
                            image.GetComponent<Image>().sprite = three_stars_bg;
                            image_line.GetComponent<Image>().color = new Color(97 / 255f, 203 / 255f, 225 / 255f);
                            break;
                        case "2":
                            image.GetComponent<Image>().sprite = two_stars_bg;
                            image_line.GetComponent<Image>().color = new Color(154 / 255f, 169 / 255f, 180 / 255f);
                            break;
                    }
                    break;
            }
        }
        line_num = 0;

        string image_path = pathToImage;

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
                    if (sprite != null && sprite.GetComponent<Image>() != null)
                    {
                        sprite.GetComponent<Image>().sprite = newSprite;
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

        Quantity.text = File.ReadAllText("Assets/Data/ingame_data/consume_item/" + itemid + ".txt");
    }

    // Update is called once per frame

    public void ConsumableUse()
    {
        switch (currentItemId)
        {
            case "item91000":
                int.TryParse(File.ReadAllText("Assets/Data/ingame_data/consume_item/item91000.txt"), out int quantity);
                if(quantity > 0)
                {
                    int.TryParse(File.ReadAllText("Assets/Data/ingame_data/system_point.txt"), out int systemPoint);
                    if(systemPoint <= 60) 
                    {
                        quantity--;
                        File.WriteAllText("Assets/Data/ingame_data/system_point.txt", (systemPoint+40).ToString());
                        successfullyPanel.SetActive(true);
                        if(quantity > 0)
                        {
                            File.WriteAllText("Assets/Data/ingame_data/consume_item/item91000.txt",(quantity).ToString());
                        }
                        else
                        {
                            File.Delete("Assets/Data/ingame_data/consume_item/item91000.txt");
                        }
                    }

                    if (systemPoint > 60)
                    {
                        notificationPanel.SetActive(true);
                    }
                }
                break;
        }
    }

    public void ConsumableConfirmUse()
    {
        switch (currentItemId)
        {
            case "item91000":
                int.TryParse(File.ReadAllText("Assets/Data/ingame_data/consume_item/item91000.txt"), out int quantity);
                if (quantity > 0)
                {
                    int.TryParse(File.ReadAllText("Assets/Data/ingame_data/system_point.txt"), out int systemPoint);
                    if (systemPoint > 60)
                    {
                        quantity--;
                        File.WriteAllText("Assets/Data/ingame_data/system_point.txt", (100).ToString());
                        successfullyPanel.SetActive(true);
                        if (quantity > 0)
                        {
                            File.WriteAllText("Assets/Data/ingame_data/consume_item/item91000.txt", (quantity).ToString());
                        }
                        else
                        {
                            File.Delete("Assets/Data/ingame_data/consume_item/item91000.txt");
                        }
                    }


                }
                break;
        }
    }
}
