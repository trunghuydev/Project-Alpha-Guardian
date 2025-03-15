using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMinionMovement : MonoBehaviour
{
    private RangedMinionStats rangedMinion;
    private Animator anim;
    public float _movementSpeed;
    
    private void Awake()
    {
        rangedMinion = FindObjectOfType<RangedMinionStats>();
        anim = GetComponent<Animator>();
    }
    public void Run()
    {
        transform.position += Vector3.left * _movementSpeed * Time.deltaTime;
    }
    public void ContinueMoving()
    {
        _movementSpeed = rangedMinion.movementSpeed;
    }
    public void Stop()
    {
        _movementSpeed = 0;
    }
}
