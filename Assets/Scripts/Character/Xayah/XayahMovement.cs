using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XayahMovement : MonoBehaviour
{
    [SerializeField] Selected Xayah_Selected;
    // Start is called before the first frame update
    public Rigidbody2D rb;

    public Animator anim;

    public float runspd = 12f;

    bool isselected = false;

    Vector3 targetPosition;

    void Start()
    {
       Xayah_Selected = GameObject.FindObjectOfType<Selected>();
       targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Xayah_Selected.isSelected && Input.GetKeyDown(KeyCode.Mouse1))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z;
            targetPosition.y = transform.position.y;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, runspd * Time.deltaTime);

        
        anim.SetFloat("Movement", Mathf.Abs(transform.position.x - targetPosition.x));

        if(transform.position.x - targetPosition.x > 0.1f)
        {
            transform.localScale = new Vector3(-1,transform.localScale.y, transform.localScale.z);  
        }
        else
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }

    void OnMouseDown()
    {
         if(isselected)
        {
            isselected = false;
        }
        else
        {
            isselected = true;
        }

    }
}
