using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMinionMovement : MonoBehaviour
{
    private SuperMinionStats superMinion;
    private Animator anim;
    public float _movementSpeed;
    private void Awake()
    {
        superMinion = FindObjectOfType<SuperMinionStats>();
        anim = GetComponent<Animator>();
    }
    public void Run()
    {
        transform.position += Vector3.left * _movementSpeed * Time.deltaTime;
    }
    public void ContinueMoving()
    {
        _movementSpeed = superMinion.movementSpeed;
    }
    public void Stop()
    {
        _movementSpeed = 0;
    }
}
