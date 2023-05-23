using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject otherGameObject;

    public void Resume()
    {
        otherGameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public void Restart()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }
    
    public void Pause () 
    {
        otherGameObject.SetActive (true);
        Time.timeScale = 0f;
    }
    
    public void Quit () {

        Time.timeScale = 1f;
        int index = SceneManager.GetActiveScene ().buildIndex;

        if ((index > 5) && (index < 8)) {
            SceneManager.LoadScene ("Level Selection Menu (Easy)");    
        }

        if (index == 8) {
            SceneManager.LoadScene ("Level Selection Menu (Normal)");    
        }
        
        if (index == 9) {
            SceneManager.LoadScene ("Level Selection Menu (Hard)");    
        }
        
        if (index == 10) {
            SceneManager.LoadScene ("Level Selection Menu (Insane)");    
        }
    }
}
