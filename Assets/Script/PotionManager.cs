using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionManager : MonoBehaviour
{
    // Start is called before the first frame update

    public class Potion
    {
        public string potionName;
        public int potionCount;
        public string potionEffect;

        public Potion(string name, int count, string effect)
        {
            potionName = name;
            potionEffect = effect;
            potionCount = count;
        }
    }

    string potionDataPath = "Assets/Data/ingame_data/potion";

    public GameObject content;

    string potionLibPath = "Assets/Data/Library_data/Potion_lib";

    public List<Potion> potionList = new List<Potion>();

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
        string[] dataFiles = Directory.GetFiles(potionDataPath).Where(f => !f.EndsWith(".meta")).ToArray();

        foreach (string file in dataFiles)
        {
            string potionid = Path.GetFileNameWithoutExtension(file);

            int.TryParse(File.ReadAllText(file), out int potionCount);

            string[] lines = File.ReadAllLines(potionLibPath + "/" + potionid + ".txt");

            string name = lines[0];

            string effect = lines[3];


            AssignInformationToPotionList(name, potionCount, effect);
        }
        AssignInformationToPotionSlot(content, potionList);
    }

    void AssignInformationToPotionSlot(GameObject content, List<Potion> potions)
    {
        int i = 0;

        for (i = 0; i < potions.Count; i++)
        {
            content.transform.Find("Potion" + (i+1)).gameObject.SetActive(true);
        }

        i = 0;
        foreach (Potion potion in potions)
        {
            GameObject potionSlot;
            i++;
            potionSlot = content.transform.Find("Potion" + i).gameObject;
            potionSlot.transform.Find("Image and Name").gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = potion.potionName;
            potionSlot.transform.Find("Image and Name").gameObject.transform.Find("Image").GetComponent<Image>().sprite = GetImage(potion.potionName);
            potionSlot.transform.Find("Description").gameObject.transform.Find("Scroll View").gameObject.transform.Find("Viewport").gameObject.transform.Find("Content").GetComponent<TextMeshProUGUI>().text = "Hiện có: <color=yellow>" + potion.potionCount + "</color>\n\n" + potion.potionEffect;

        }

    }

    Sprite GetImage(string name)
    {
        string imagePath = "Assets/Sprite/Dược phẩm/" + name + ".png";

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

    void AssignInformationToPotionList(string potionName, int potionCount, string potionEffect)
    {
        AddPotionToList(new Potion(potionName, potionCount, potionEffect));
        potionList.Sort((a, b) => b.potionCount.CompareTo(a.potionCount));
    }

    void AddPotionToList(Potion newPotion)
    {
        // Kiểm tra xem đối tượng Curio đã có trong danh sách chưa
        bool exists = potionList.Exists(potion => potion.potionName == newPotion.potionName
                                                && potion.potionEffect == newPotion.potionEffect
                                                && potion.potionCount == newPotion.potionCount);
        if (!exists)
        {
            potionList.Add(newPotion);
        }
    }
}