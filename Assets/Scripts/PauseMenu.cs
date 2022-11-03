using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public bool isPause = false;

    public GameObject pauseScreen;

    private bool isGrass = false;
    private bool isDesert = false;
    public void Press_Pause()
    {
        if (!isPause)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            isPause = true;
            if (AudioManager.Instance.IsPlaying("BGM"))
            {
                isDesert = true;
                AudioManager.Instance.PauseClip("BGM");
            }
            else
            {
                isGrass = true;
                AudioManager.Instance.PauseClip("GrasslandBGM");
            }

        }
    }
    public void Press_Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
        if (isDesert == true)
        {
            AudioManager.Instance.PlayClip("BGM");
        }
        if (isGrass == true)
        {
            AudioManager.Instance.PlayClip("GrasslandBGM");
        }

    }

    public void Press_Menu()
    {
        Time.timeScale = 1f;
        isPause = false;
        SceneManager.LoadScene("StartScene");
        if (isDesert == true)
        {
            AudioManager.Instance.StopClip("BGM");
            isDesert = false;
        }
        if (isGrass == true)
        {
            AudioManager.Instance.StopClip("GrasslandBGM");
            isDesert = false;
        }

        AudioManager.Instance.PlayClip("MenuBGM");
    }
}
