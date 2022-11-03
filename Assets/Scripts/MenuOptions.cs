using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    public GameOverScreen creditScreen;
    public void ExitButton()
    {
        AudioManager.Instance.PlayClip("SelectSfx");
        if(AudioManager.Instance.IsPlaying("MenuBGM"))
            AudioManager.Instance.StopClip("MenuBGM");
            AudioManager.Instance.PlayClip("EndBGM");
        creditScreen.SetUp();
        //Application.Quit();
    }

    public void PlayButton()
    {
        AudioManager.Instance.PlayClip("SelectSfx");
        AudioManager.Instance.StopClip("EndBGM");
    }

    public void MenuButton()
    {
        AudioManager.Instance.PlayClip("SelectSfx");
        AudioManager.Instance.StopClip("EndBGM");
        AudioManager.Instance.PlayClip("MenuBGM");
        SceneManager.LoadScene("StartScene");
    }
}
    
