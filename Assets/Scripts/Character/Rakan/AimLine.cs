using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    public Transform player;  // Reference to the player's position
    public LineRenderer lineRenderer;  // Reference to the LineRenderer
    private float maxLength = 5;
    private bool isAiming = false;
    void Start()
    {
        
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;  
        lineRenderer.endWidth = 0.05f;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        Vector3 playerPosition = player.position;
        float direction = player.localScale.x;  

        Vector3 startPosition = new Vector3(playerPosition.x, playerPosition.y-0.5f, playerPosition.z);

        Vector3 endPosition = new Vector3(playerPosition.x + (direction * maxLength), playerPosition.y-0.5f, playerPosition.z);

        //if (Input.GetKey(KeyCode.E))
        //{
        //    isAiming = true;
        //    lineRenderer.enabled = true;
        //}
        //else if (Input.GetKeyUp(KeyCode.E)) 
        //{
        //    isAiming = false;
        //    lineRenderer.enabled = false;
        //}

        if (isAiming)
        {
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);
        }
    }

    public Vector3 GetAimEndPosition()
    {
        return lineRenderer.GetPosition(1);
    }
}
