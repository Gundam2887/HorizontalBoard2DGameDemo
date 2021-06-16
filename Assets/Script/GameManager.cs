using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public static bool GameIsDie = false;

    public GameObject dieUI;

    public AudioSource music;

    public GameObject audioB;

    public float playerLife = LifeBar.lifeCurrent;

    void Start()
    {
        DontDestroyOnLoad(audioB);
        Cursor.visible = false;
        Debug.Log("playerLife3:" + playerLife);
    }

    void Update()
    {
        playerLife = LifeBar.lifeCurrent;
        Debug.Log("playerLife1:" + playerLife);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (playerLife <= 0)
        {
            Debug.Log("playerLife2:" + playerLife);

            SetGameIsDie();

        }
    }
     public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        music.Play();
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
     void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
        music.Play();
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        dieUI.SetActive(false);
        GameIsDie = false;
        SceneManager.LoadScene("Demo");
        music.Play();
        Cursor.visible = false;
    }

    void SetGameIsDie()
    {
        dieUI.SetActive(true);
        GameIsDie = true;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Start");
        music.Play();
        Cursor.visible = true;
    }

}