using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBorder : MonoBehaviour
{
    public Button button; // Assign the Button component in the Inspector
    public Sprite normalBorder; // Assign the normal border sprite in the Inspector
    public Sprite highlightedBorder; // Assign the highlighted border sprite in the Inspector
    public ChangeBorderRakan rakanScript;
    private Image buttonImage;
    public bool selected = false;
    

    private void Start()
    {
        rakanScript = FindObjectOfType<ChangeBorderRakan>();
        buttonImage = button.GetComponent<Image>();
        buttonImage.sprite = normalBorder;
    }

    public void SelectAvatar()
    {
        if (rakanScript.selected)
        {
            rakanScript.DeselectAvatar();
        }

        selected = !selected;
        if (selected == true)
        {
            buttonImage.sprite = highlightedBorder;
        }
        else
        {
            buttonImage.sprite = normalBorder;
        }
        //Debug.Log(selected);
    }

    public void DeselectAvatar()
    {
        selected = false;
        buttonImage.sprite = normalBorder;
    }
}
