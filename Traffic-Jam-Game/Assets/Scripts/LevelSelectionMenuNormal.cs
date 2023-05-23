using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuNormal : MonoBehaviour
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
    public void Eleven()
    {
        SceneManager.LoadScene ("11");
    }
    
    public void Back()
    {
        SceneManager.LoadScene ("Difficulty Selection Menu");
    }
}