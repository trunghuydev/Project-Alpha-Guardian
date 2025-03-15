using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeMinionMovement : MonoBehaviour
{
    private MeleeMinionStats meleeMinionStats;
    private Animator anim;
    float _movementSpeed;
    
    private void Awake()
    {
        meleeMinionStats = FindObjectOfType<MeleeMinionStats>();
        anim = GetComponent<Animator>();
    }
    
    public void Run()
    {
        transform.position += Vector3.left * _movementSpeed * Time.deltaTime;
        anim.SetTrigger("run");
    }

    public void ContinueMoving()
    {
        _movementSpeed = meleeMinionStats.movementSpeed;
    }
    public void Stop()
    {
        _movementSpeed = 0f;
    }
}
