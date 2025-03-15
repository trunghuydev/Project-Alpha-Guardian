using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Selected : MonoBehaviour
{

    public GameObject Xayah;

    bool isSpawned = false;

    public bool isSelected = false;

    public GameObject message;

    public Sprite sprite;
    public Sprite highlightSprite;

    // Start is called before the first frame update

    // Update is called once per frame
    public void Update()
    {

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnMouseDown();
        }


        if (isSelected && !isSpawned && Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 offset = new Vector3(0, 0, 10);
            Instantiate(Xayah, mousePosition + offset, Quaternion.identity);
            Debug.Log("1");
            isSpawned = true;

            isSelected = false;
            transform.GetComponent<SpriteRenderer>().sprite = sprite;
            message.SetActive(false);
        }


        if (isSelected)
        {
            transform.GetComponent<SpriteRenderer>().sprite = highlightSprite;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sprite = sprite;
        }

    }

    private void OnMouseDown()
    {
        Debug.Log("clicked");
        if (!isSelected)
        {
            isSelected = true;
            

            if(!isSpawned)
            {

                message.SetActive(true);
                message.GetComponent<Text>().text = "Right click to spawn!";
            }
            else
            {

                message.SetActive(true);
                message.GetComponent<Text>().text = "Right click to move!";
            }
        }
        else
        {
            isSelected = false;
            message.SetActive(false);
            transform.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        
    }

    private void OnMouseOver()
    {
        if(!isSelected)
        {
            transform.GetComponent<SpriteRenderer>().sprite = highlightSprite;
        }
        
    }

    private void OnMouseExit() 
    {
        if (!isSelected)
        {
            transform.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        
    }

   
}
