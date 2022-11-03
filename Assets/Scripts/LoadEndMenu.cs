using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadEndMenu : MonoBehaviour
{
    public SceneManagement sceneManagement;
    public GameObject GameOverScreen;
    public void TriggerEndMenu() {
        //sceneManagement.LoadNextScene();
        GameOverScreen.SetActive(true) ;
    }
}
