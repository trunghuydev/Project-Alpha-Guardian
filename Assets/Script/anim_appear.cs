using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class anim_appear : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if ((stateInfo.IsName("textbg") && stateInfo.normalizedTime >= 1.0f))
        {
            gameObject.transform.Find("Text_bg").gameObject.transform.Find("Image").gameObject.SetActive(false);
        }
    }
}
