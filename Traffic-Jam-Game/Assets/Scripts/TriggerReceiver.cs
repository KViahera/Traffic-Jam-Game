using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TriggerReceiver : MonoBehaviour 
{
    
    public GameObject otherGameObject;
	public Text text;

    public void OnTriggerEnter (Collider other) 
	{
        otherGameObject.SetActive (true);
        Time.timeScale = 0f;

		if (otherGameObject.name != "Menu") {

			int index = SceneManager.GetActiveScene ().buildIndex - 5;
        	PlayerPrefs.SetInt(index.ToString(), 1);
		}

		else {
			text.text = "Your score: " + (Time.deltaTime * 1000).ToString ();
		}
    }
}
