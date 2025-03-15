using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SpawnRakan : MonoBehaviour
{
    public Vector2 spawnLocation = new Vector2(-6, 1.11f);
    public ChangeBorderRakan spawn;
    public GameObject currentCharacter;

    private Vector3 targetPosition;
    public bool isMoving = false;
    public LayerMask groundLayer;
    public Animator anim;

    [Header("Rakan")]
    //Cooldown Xayah
    private float moveSpeed;
    RakanStats rakanStats;
    public bool isUltActive = false;


    public GameObject characterPrefab;
    public float cooldownRakan;
    private float cooldownTimerRakan;
    public Image cooldownImageRakan;
    public Text cooldownTextRakan;
    private bool isCooldownActive = false;

    public float ultDuration = 12f;
    private float ultTimer = 0f;


    private void Awake()
    {

    }
    private void Start()
    {
        
        CheckRakanExist();
        Debug.Log("rakan exist:" + CheckRakanExist());
        DisableCooldownImage();
        Spawn();
        
    }


    void Update()
    {
        
        if (isCooldownActive)
        {
            cooldownTimerRakan -= Time.deltaTime; // Decrease the timer
            if (cooldownTimerRakan <= 0)
            {
                isCooldownActive = false;
                DisableCooldownImage();
                Spawn();
            }
            else
            {
                cooldownTextRakan.text = Mathf.CeilToInt(cooldownTimerRakan).ToString();
            }
        }



        spawn = FindObjectOfType<ChangeBorderRakan>();

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

        }
        else if (currentCharacter != null)
        {
            anim.SetFloat("Movement", 0f);
        }

        if (currentCharacter != null && rakanStats != null)
        {
            if (isUltActive)
            {
                ultTimer += Time.deltaTime;
                if (ultTimer >= ultDuration)
                {
                    anim.SetFloat("ult_run", 0f);
                    EndUltimate();
                }
                else
                {

                    moveSpeed = rakanStats.movementSpeed * 1.5f;
                }
            }
            else
            {

                moveSpeed = rakanStats.movementSpeed;
            }
        }
        

    }

    private void EnableCooldownImage()
    {
        cooldownImageRakan.gameObject.SetActive(true);
        cooldownTextRakan.gameObject.SetActive(true);
    }


    private void DisableCooldownImage()
    {
        cooldownImageRakan.gameObject.SetActive(false);
        cooldownTextRakan.gameObject.SetActive(false);
    }



    public void StartCooldown()
    {
        isCooldownActive = true;
        cooldownTimerRakan = cooldownRakan;
        EnableCooldownImage();
    }



    public void Spawn()
    {
        if (!CheckRakanExist())
        {
            return;
        }

        if (currentCharacter != null)
        {
            Debug.Log("Xayah already exists");
            return;
        }
        if (cooldownTimerRakan > 0)
        {
            return;
        }

        currentCharacter = Instantiate(characterPrefab, spawnLocation, Quaternion.identity);

        if(currentCharacter != null)
        {
            rakanStats = currentCharacter.GetComponent<RakanStats>();
            moveSpeed = rakanStats.movementSpeed;
            anim = currentCharacter.GetComponent<Animator>();
        }
            
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
        if (isUltActive)  
        {
            anim.SetFloat("ult_run", distanceToTarget);
        }
        else
        {
            anim.SetFloat("Movement", distanceToTarget);
        }

        if (Vector3.Distance(currentCharacter.transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
            if (isUltActive)
            {
                anim.SetFloat("ult_run", 0f);
            }
            else
            {
                anim.SetFloat("Movement", 0f);
            }
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

    private void EndUltimate()
    {
        anim.SetBool("isUlt", false);
        isUltActive = false;
        ultTimer = 0;
    }

    private bool CheckRakanExist()
    {
        string path = "Assets/Data/hero_select/hero1.txt";
        string path2 = "Assets/Data/hero_select/hero2.txt";

        if (File.Exists(path)){
            string id = File.ReadAllText(path);
            if (id.Equals("00002"))
            {
                return true;
            }
        }
        else
        {
            Debug.Log("file not exist");
        }

        if (File.Exists(path2))
        {
            string id = File.ReadAllText(path);
            if (id.Equals("00002"))
            {
                return true;
            }
        }
        else
        {
            Debug.Log("file not exist");
        }

        return false;
    }
}
