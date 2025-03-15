using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBorderRakan : MonoBehaviour
{
    public Button button; // Assign the Button component in the Inspector
    public Sprite normalBorder; // Assign the normal border sprite in the Inspector
    public Sprite highlightedBorder; // Assign the highlighted border sprite in the Inspector
    public ChangeBorder xayah;

    private Image buttonImage;
    public bool selected = false;
    

    private void Start()
    {
        xayah = FindObjectOfType<ChangeBorder>();
        buttonImage = button.GetComponent<Image>();
        buttonImage.sprite = normalBorder;
    }

    public void SelectAvatar()
    {
        if (xayah.selected)
        {
            xayah.DeselectAvatar();
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
