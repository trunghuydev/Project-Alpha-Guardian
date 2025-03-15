using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class hero1_teaminfo : MonoBehaviour
{

    public GameObject hero_1;

    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets/Data/hero_select/hero1.txt";
        if (File.Exists(path))
        {
            if (File.ReadAllText(path) != "")
            {
                string hero_current = File.ReadAllText(path);

                string image_path = "Assets/Data/hero_sprite/hero" + hero_current + ".jpg";

                byte[] imageData = File.ReadAllBytes(image_path);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                hero_1.GetComponent<Image>().sprite = newSprite;
            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
