using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    private float disappearTimerMax = 1;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        float moveSpeed = 0.5f;
        transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;

        if (disappearTimer > disappearTimerMax * 0.5f)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime; //Reduce text's alpha
            textMesh.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetUp(float damageAmount, bool isCrit)
    {
        int roundedDamage = Mathf.RoundToInt(damageAmount);
        if (!isCrit)
        {
            textMesh.fontSize = 3;
            textMesh.SetText(roundedDamage.ToString());
            textColor = new Color(250f / 255f, 120f / 255f, 0f / 255f);
        }
        else
        {
            textMesh.fontSize = 4;
            textMesh.SetText(roundedDamage.ToString() + "!!");
            textColor = new Color(174f / 255f, 45f / 255f, 0f / 255f);
        }


        textMesh.color = textColor;
        disappearTimer = disappearTimerMax;
    }

    public static DamagePopUp Create(Vector3 position, float damage, bool isCrit)
    {
        Transform damagePopUpTransform = Instantiate(GameAssets.i.pfDamagePopUp, position, Quaternion.identity);
        DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
        damagePopUp.SetUp(damage, isCrit);

        return damagePopUp;
    }

    
}
