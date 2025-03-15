using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit_teamselect : MonoBehaviour
{


    public AudioSource audioSource;
    public AudioClip clip;
    public GameObject exit_select;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        audioSource.PlayOneShot(clip);
        SceneManager.LoadScene("Level Select");
    }

    private void OnMouseOver()
    {
        exit_select.SetActive(true);
    }

    private void OnMouseExit()
    {
        exit_select.SetActive(false);
    }
}
