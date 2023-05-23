using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuHard : MonoBehaviour
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
    
    public void TwentyOne()
    {
        SceneManager.LoadScene ("21");
    }
    public void Back()
    {
        SceneManager.LoadScene ("Difficulty Selection Menu");
    }
}