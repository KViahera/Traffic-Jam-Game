using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelectionMenu : MonoBehaviour
{
    public void Easy()
    {
        SceneManager.LoadScene ("Level Selection Menu (Easy)");
    }
    
    public void Normal()
    {
        SceneManager.LoadScene ("Level Selection Menu (Normal)");
    }
    
    public void Hard()
    {
        SceneManager.LoadScene ("Level Selection Menu (Hard)");
    }
    
    public void Insane()
    {
        SceneManager.LoadScene ("Level Selection Menu (Insane)");
    }

    public void Back()
    {
        SceneManager.LoadScene ("Main Menu");
    }
}
