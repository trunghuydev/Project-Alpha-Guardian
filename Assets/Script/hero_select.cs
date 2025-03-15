using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
using static System.TimeZoneInfo;
using static UnityEditor.Progress;

public class hero_select : MonoBehaviour
{
    public int maxHero = 2;
    public GameObject hero_1;
    public GameObject hero_2;
    public Sprite default_sprite;
    public AudioSource source;
    public AudioClip clip;
    public GameObject play_active;
    public AudioClip start_clip;

    public GameObject transition;
    public float transitionTime2 = 3f;



    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets/Data/hero_select/hero1.txt";
        string path2 = "Assets/Data/hero_select/hero2.txt";
        LoadPreviousTeam(path); 
        LoadPreviousTeam(path2);
    }

    void LoadPreviousTeam(string path)
    {
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
        else
        {
            File.WriteAllText(path, "");
        }
    }

    // Update is called once per frame
    void Update()
    {
        string path = "Assets/Data/hero_select/hero1.txt";
        string path2 = "Assets/Data/hero_select/hero2.txt";
        setButtonActive(path, path2);
        
    }

    void setButtonActive(string path, string path2)
    {
        if (File.Exists(path))
        {
            if (File.ReadAllText(path) != "")
            {
                play_active.SetActive(true);
                return;
            }
        }

        if (File.Exists(path2))
        {
            if (File.ReadAllText(path2) != "")
            {
                play_active.SetActive(true);
                return;
            }
        }

        play_active.SetActive(false);
    }

    public void Hero_Select(string heroid)
    {
        string path = "Assets/Data/hero_select/"; 
        string path_hero1 = path + "hero1.txt";
        string path_hero2 = path + "hero2.txt";


        if (!File.Exists(path_hero1))
        {
            File.WriteAllText(path_hero1, "");
        }

        if (!File.Exists(path_hero2))
        {
            File.WriteAllText(path_hero2, "");
        }


        string hero_current_1 = File.ReadAllText(path_hero1);
        string hero_current_2 = File.ReadAllText(path_hero2);

        Debug.Log(hero_current_1);
        Debug.Log(hero_current_2);

        if (hero_current_1 == heroid)
        {
            KickHeroAwayFromSlot(hero_1, path_hero1);
            return;
        }
        if (hero_current_2 == heroid)
        {
            KickHeroAwayFromSlot(hero_2, path_hero2);
            return;
        }
        if (hero_current_1 == "")
        {
            AssignHeroToSlot(hero_1,path_hero1, heroid);
            return;
        }
        if (hero_current_2 == "")
        {
            AssignHeroToSlot(hero_2, path_hero2, heroid);
            return;
        }
        Debug.Log("Đội đã đầy!");
    }

    void AssignHeroToSlot(GameObject hero,string path_hero, string heroid)
    {
        File.WriteAllText(path_hero, heroid);
        Debug.Log("Thêm vào thành công");

        string image_path = "Assets/Data/hero_sprite/hero" + heroid + ".jpg";

        byte[] imageData = File.ReadAllBytes(image_path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        hero.GetComponent<Image>().sprite = newSprite;
    }

    void KickHeroAwayFromSlot(GameObject hero, string path_hero)
    {
        File.Delete(path_hero);
        hero.GetComponent<Image>().sprite = default_sprite;
        Debug.Log("Đã rời đội");
    }


    public void Hero_Delete()
    {
        string path = "Assets/Data/hero_select/";
        string path_hero1 = path + "hero1.txt";
        string path_hero2 = path + "hero2.txt";

        source.PlayOneShot(clip);

        HeroFileDelete(hero_1, path_hero1);
        HeroFileDelete(hero_2, path_hero2);
    }

    void HeroFileDelete(GameObject hero, string path_hero)
    {
        if (File.Exists(path_hero))
        {
            File.Delete(path_hero);
            hero.GetComponent<Image>().sprite = default_sprite;
            Debug.Log("Đã xoá thành công");
        }
        else
        {
            Debug.Log("Chưa có nhân vật để xoá");
        }
    }

    public void Start_Game()
    {
        string path = "Assets/Data/hero_select/hero1.txt";
        string path2 = "Assets/Data/hero_select/hero2.txt";

        if(!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }
        if (!File.Exists(path2))
        {
            File.WriteAllText(path2, "");
        }
        if (File.ReadAllText(path) != "" && File.ReadAllText(path2) != "")
        {
            source.PlayOneShot(start_clip);
            LoadAdventureScene();
        }
        
        
    }

    public void LoadAdventureScene()
    {        
        StartCoroutine(LoadLevel("Adventure"));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(transitionTime2);
        SceneManager.LoadScene(sceneName);
    }
}
