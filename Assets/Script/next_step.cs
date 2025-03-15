using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class next_step : MonoBehaviour
{
    public GameObject transition;
    public GameObject button_select;
    public AudioSource audioSource;
    public AudioClip clip;

    public GameObject notificationPanel;

    public float transitionTime = 0.3f;

    string pathMap = "Assets/Data/ingame_data/map";
    string pathMapConst = "Assets/Data/ingame_data/map_const";

    // Start is called before the first frame update
    void Start()
    {
        if(audioSource == null)
        {
            audioSource = GameObject.FindObjectOfType<AudioSource>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        
        //transition.SetActive(true);
        //SceneManager.LoadScene("Team Select");
        CheckForOldGameData();
    }

    void CheckForOldGameData()
    {
        string[] dataMapFiles = Directory.GetFiles(pathMap).Where(f => !f.EndsWith(".meta")).ToArray();
        string[] dataMapConstFiles = Directory.GetFiles(pathMapConst).Where(f => !f.EndsWith(".meta")).ToArray();

        if(dataMapFiles.Length + dataMapConstFiles.Length == 38)
        {
            notificationPanel.SetActive(true);
        }
        else
        {
            button_select.SetActive(true);
            audioSource.PlayOneShot(clip);
            LoadTeamSelectScene();
        }
    }

    public void LoadTeamSelectScene()
    {
        StartCoroutine(LoadLevel("Team Select"));
    }

    public void LoadAdventureScene()
    {
        StartCoroutine(LoadLevel("Adventure"));
    }

    public void LoadResultScreen()
    {
        StartCoroutine(LoadLevel("EndAdventure"));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    private void OnMouseUp()
    {
        button_select.SetActive(false);
    }

    private void OnMouseOver()
    {
        button_select.SetActive(true);
    }

    private void OnMouseExit()
    {
        button_select.SetActive(false);
    }

}
