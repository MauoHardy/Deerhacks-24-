using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBookController : MonoBehaviour
{
    private Animator animator;
    private void Awake(){
        animator = this.GetComponent<Animator>();
    }

    public void TriggerAnimation(){
        animator.SetTrigger("StartPressed");
    }
}
