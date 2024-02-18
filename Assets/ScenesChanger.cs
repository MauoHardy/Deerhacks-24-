using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesChanger : MonoBehaviour
{
    public Button StartGameButton;

    private Animator fadeOutanimator;

    private void Awake(){

        // fadeOutanimator = this.GetComponent<Animator>();
        // fadeOutanimator.enabled=false;
        StartGameButton.onClick.AddListener(()=>{
            // fadeOutanimator.enabled=true;
            ChangeScene();});
    }

    public void ChangeScene(){
        // fadeOutanimator.SetTrigger("MoveOut");
        // yield return new WaitForSeconds(2);
        SceneManager.LoadScene("StoryBookTitleScreen");
    }
}
