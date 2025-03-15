using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;

public class Quit_Game : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public GameObject transition;

    public GameObject quitPanel;

    public float transitionTime3 = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowQuitPanel()
    {
        quitPanel.SetActive(true);
    }

    public void Quit()
    {
        source.PlayOneShot(clip);
        LoadSceneWithDelay("Level Select", 1f);
        transition.SetActive(true);
    }

    public void ResultScreen()
    {
        source.PlayOneShot(clip);
        LoadSceneWithDelay("EndAdventure", 1f);
        transition.SetActive(true);

    }

    public void LoadSceneWithDelay(string sceneName, float delay)
    {
        StartCoroutine(LoadSceneAfterDelay(sceneName, delay));
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
