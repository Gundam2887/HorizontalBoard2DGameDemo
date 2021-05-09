using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitUI : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public static bool GameIsDie;

    public GameObject dieUI;

    public AudioSource music;

    public GameObject audio_b;

    void Start()
    {
        DontDestroyOnLoad(audio_b);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {          
            if (GameIsPaused)
            {
                Resume();
                Time.timeScale = 1f;
            }
            else
            {
                Pause();
                Time.timeScale = 0f;
            }
        }
        if (LifeBar.lifeCurrent <= 0)
        {
            dieUI.SetActive(true);
        }
        else
        {
            dieUI.SetActive(false);
        }
        lockState();
    }
     public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        music.Play();
    }
     void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        music.Play();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Demo");
        music.Play();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start");
        music.Play();
    }

    void lockState()
    {
        if(Time.timeScale == 1f)
        {
            Cursor.visible = false;
        }
        else if (Time.timeScale == 0f)
        {
            Cursor.visible = true;
        }
    }
}