using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnCharacter : MonoBehaviour
{
      
    public Vector2 spawnLocation = new Vector2(-6, 1.11f);
    public ChangeBorder spawn;
    public GameObject currentCharacter;

    private Vector3 targetPosition;
    public bool isMoving = false;
    public LayerMask groundLayer;
    public Animator anim;

    [Header("Xayah")]
    //Cooldown Xayah
    private float moveSpeed;
    XayahStats xayahStats;

    public GameObject characterPrefab;
    public float cooldownXayah;
    private float cooldownTimerXayah;
    public Image cooldownImageXayah;
    public Text cooldownTextXayah;
    private bool isCooldownActive = false;

    private void Awake()
    {
        
    }
    private void Start()
    {
        DisableCooldownImage();
        Spawn();
    }

    
    void Update()
    {
        if (isCooldownActive)
        {
            cooldownTimerXayah -= Time.deltaTime; // Decrease the timer
            if (cooldownTimerXayah <= 0)
            {
                isCooldownActive = false;
                DisableCooldownImage();
                Spawn();
            }
            else
            {
                cooldownTextXayah.text = Mathf.CeilToInt(cooldownTimerXayah).ToString();
            }
        }

        

        spawn = FindObjectOfType<ChangeBorder>();
        
        if (spawn.selected == true)
        {
            //if (Input.GetKeyDown(KeyCode.P) && currentCharacter == null)
            //{
            //    Spawn();
            //}

            if (Input.GetMouseButtonDown(1) && currentCharacter != null)
            {
                SetTargetPosition();
            }
        }

        if (isMoving && currentCharacter != null)
        {
            MoveCharacterToTarget();
            if (currentCharacter.GetComponent<XayahAttack>() != null)
            {
                currentCharacter.GetComponent<XayahAttack>().StopAttacking();
            }
        }
        else if (currentCharacter != null)
        {
            anim.SetFloat("Movement", 0f);  
        }

        
    }

    private void EnableCooldownImage()
    {
        cooldownImageXayah.gameObject.SetActive(true);
        cooldownTextXayah.gameObject.SetActive(true);
    }

    
    private void DisableCooldownImage()
    {
        cooldownImageXayah.gameObject.SetActive(false);
        cooldownTextXayah.gameObject.SetActive(false);
    }

  

    public void StartCooldown()
    {
        isCooldownActive = true;
        cooldownTimerXayah = cooldownXayah; 
        EnableCooldownImage();
    }

    

    public void Spawn()
    {
        if (currentCharacter != null)
        {
            Debug.Log("Xayah already exists");
            return;
        }
        if(cooldownTimerXayah > 0)
        {
            return;
        }
        currentCharacter = Instantiate(characterPrefab, spawnLocation, Quaternion.identity);
        xayahStats = currentCharacter.GetComponent<XayahStats>();
        moveSpeed = xayahStats.movementSpeed;
        anim = currentCharacter.GetComponent<Animator>();
    }

    

    void SetTargetPosition()
    {
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;  
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.down, Mathf.Infinity, groundLayer);

        if (hit.collider != null)
        {
            targetPosition = new Vector3(mouseWorldPos.x, hit.point.y + 1, 0);
            //Debug.Log("Target Position: " + targetPosition);
        }
        FaceTarget(targetPosition);
        isMoving = true;

        anim.SetFloat("Movement", Mathf.Abs(transform.position.x - targetPosition.x));
    }

  

    void MoveCharacterToTarget()
    {
        float leftBoundary = spawnLocation.x;
        targetPosition.x = Mathf.Max(targetPosition.x, leftBoundary);

        currentCharacter.transform.position = Vector3.MoveTowards(
            currentCharacter.transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime 
        );

        float distanceToTarget = Mathf.Abs(currentCharacter.transform.position.x - targetPosition.x);
        anim.SetFloat("Movement", distanceToTarget);

        if (Vector3.Distance(currentCharacter.transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
            anim.SetFloat("Movement", 0f);  
        }
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 direction = target - currentCharacter.transform.position;
        if (direction.x > 0)
        {
            currentCharacter.transform.localScale = new Vector3(1, 1, 1);  
        }
        else
        {
            currentCharacter.transform.localScale = new Vector3(-1, 1, 1); 
        }
    }
}
