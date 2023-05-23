using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionMenu : MonoBehaviour
{
    public GameObject otherGameObject;

    public void GoToTheNextLevel ()
    {
        otherGameObject.SetActive(false);
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex + 1);
    }

    public void Quit () {

        Time.timeScale = 1f;
        int index = SceneManager.GetActiveScene ().buildIndex;
        
        SceneManager.LoadScene ("Main Menu");   

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
