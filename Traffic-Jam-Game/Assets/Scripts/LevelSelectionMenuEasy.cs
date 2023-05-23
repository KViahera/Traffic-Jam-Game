using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuEasy : MonoBehaviour
{
    public List<Button> buttons;
    public void Update()
    {
        foreach (Button i in buttons)
        {
            if (PlayerPrefs.GetInt(i.name) > 0)
            {
                i.interactable = true;
            }

            else
            {
                i.interactable = false;
            }
        }
    }
    public void One()
    {
        SceneManager.LoadScene ("1");
    }
    
    public void Back()
    {
        SceneManager.LoadScene ("Difficulty Selection Menu");
    }
}
