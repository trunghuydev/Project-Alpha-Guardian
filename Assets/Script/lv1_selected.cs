using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lv1_selected : MonoBehaviour
{
    // Start is called before the first frame update

    public bool islv1Selected = false;
    public Animator cassette_anim;
    public Animator play_anim;
    public AudioSource audioSource;
    public AudioClip clip;
    public GameObject lv1_bg;
    public GameObject button_text;
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource == null)
        {
            audioSource = GameObject.FindObjectOfType<AudioSource>();
        }
    }

    private void OnMouseDown()
    {
        if (!islv1Selected)
        {
            islv1Selected = true;
            cassette_anim.SetBool("isSelected", true);
            audioSource.PlayOneShot(clip);
            lv1_bg.SetActive(true);
            play_anim.SetBool("isAnyLvSelected", true);
            button_text.SetActive(true);
        } else
        {
            islv1Selected = false;
            cassette_anim.SetBool("isSelected", false);
            lv1_bg.SetActive(false);
            play_anim.SetBool("isAnyLvSelected", false);
            button_text.SetActive(false);
        }
    }
}
