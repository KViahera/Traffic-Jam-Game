using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Workshop
{
    public class WorkshopMenu : MonoBehaviour
    {
        public void Back()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public GameObject savePanel, errorPanel;

        public void Save()
        {
            List <GameObject> listOfGameObjects = new List<GameObject>();
            foreach (var currentGameObject in GameObject.FindGameObjectsWithTag("Game object"))
            {
                listOfGameObjects.Add(currentGameObject);
            }

            if (GameLevelValidator.isValid(listOfGameObjects))
            {
                savePanel.SetActive(true);
            }

            else
            {
                errorPanel.SetActive(true);
            }
        }
    }
}
