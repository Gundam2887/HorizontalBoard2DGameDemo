using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public AudioSource music;
    public GameObject audio_a;
    void Start()
    {
        DontDestroyOnLoad(audio_a);
        Cursor.visible = true;
    }
    public void OnButtonGameStart()
    {
        music.Play();
        SceneManager.LoadScene("demo");
        Time.timeScale = 1f;
    }
    public void EndGame()
    {
        Application.Quit();
        music.Play();
    }
}
