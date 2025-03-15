using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiegeMinionMovement : MonoBehaviour
{
    private SiegeMinionStats siegeMinion;
    private Animator anim;
    public float _movementSpeed;
    private void Awake()
    {
        siegeMinion = FindObjectOfType<SiegeMinionStats>();
        anim = GetComponent<Animator>();
    }
    public void Run()
    {
        transform.position += Vector3.left * _movementSpeed * Time.deltaTime;
    }
    public void ContinueMoving()
    {
        _movementSpeed = siegeMinion.movementSpeed;
    }
    public void Stop()
    {
        _movementSpeed = 0;
    }
}
