using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosedBookController : MonoBehaviour
{
    private Animator animator;
    public Button button;

    [SerializeField] private OpenBookController openBookController;
    private void Awake(){
        animator = this.GetComponent<Animator>();
        button.onClick.AddListener(()=>{animator.SetTrigger("StartPressed");
                openBookController.TriggerAnimation();});
    }

}
