using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        PlayerPrefs.SetInt("1", 1);
        PlayerPrefs.SetInt("11", 1);
        PlayerPrefs.SetInt("21", 1);
        PlayerPrefs.SetInt("30", 1);
    }
    public void SinglePlayer()
    {
        SceneManager.LoadScene ("Difficulty Selection Menu");
    }
    
    public void Workshop()
    {
        SceneManager.LoadScene ("Workshop");
    }

    public void AboutTheGame()
    {
        SceneManager.LoadScene ("About The Game");
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
