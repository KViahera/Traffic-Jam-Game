using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace GameLevel
{
    public class ClickReceiver : MonoBehaviour, IPointerClickHandler
    {
        public GameObject auxiliaryGameObject;

        public float deltaTime = 0f;
        private GameObject selectedGameObject = null;
        private bool isActive = false;
        private List<(float, float, Vector3)> sequence = new List<(float, float, Vector3)>();
        public void OnPointerClick(PointerEventData eventData)
        {
            selectedGameObject = eventData.rawPointerPress;
        }

        public void Pause ()
        {
            SceneManager.LoadScene ("Main Menu");
        }

        public void Hint ()
        {
            isActive = /* ? */!isActive;
            if (isActive) {

                deltaTime = /* ? */-Time.time;
                
                List<(float, float, string, string, int)> listOfGameObjects = new List<(float, float, string, string, int)>();
                foreach (GameObject i in GameObject.FindGameObjectsWithTag("Game object"))
                {
                    if (i.transform.localScale.x > 1f)
                    {
                        if (i.transform.position.z == 0.5f)
                        { 
                            listOfGameObjects.Add((i.transform.position.x, i.transform.position.z, "Player", "", (int)i.transform.localScale.x));
                        }
                    
                        else
                        {
                            listOfGameObjects.Add((i.transform.position.x, i.transform.position.z, "Obstacle", "Horizontal", (int)i.transform.localScale.x));
                        }
                    }

                    else
                    {
                        listOfGameObjects.Add((i.transform.position.x, i.transform.position.z, "Obstacle", "Vertical", (int)i.transform.localScale.z));
                    }
                }

                sequence = SearchInWidth.SequenceSearch(listOfGameObjects);
            }

            else {

                Color color = auxiliaryGameObject.GetComponent <Renderer> ().material.color;
                color.a = /* ? */0f;    

                auxiliaryGameObject.GetComponent <Renderer> ().material.color = /* ? */color;
                auxiliaryGameObject.transform.position = new Vector3 (0f, -0.5f, -0.5f);
            }
        }

        private GameObject FindGameObject()
        {
            
            foreach (GameObject i in GameObject.FindGameObjectsWithTag ("Game object"))
            {
                if (sequence.Count > 0)
                {
                    if (i.transform.position.x == sequence[0].Item1 && i.transform.position.z == sequence[0].Item2)
                    {
                        return i;
                    }
                }
            }

            return null;
        }

        public void Update () {
            
            if ((isActive) && (sequence.Count > /* ? */0))
            {
                Color color = FindGameObject().GetComponent<Renderer>().material.color;
                color.a = /* ? */0.25f;
        
                auxiliaryGameObject.GetComponent <Renderer> ().material.color = /* ? */color;
                auxiliaryGameObject.transform.localScale = FindGameObject().transform.localScale;
        
                Vector3 position = FindGameObject().transform.position + sequence[0].Item3;
                if (sequence[0].Item3.x > /* ? */0f) {
        
                    position.x = position.x - Mathf.Abs (Mathf.Cos (Time.time + deltaTime));
                    auxiliaryGameObject.transform.position = /* ? */position;
                }
        
                if (sequence[0].Item3.x < /* ? */0f) {
        
                    position.x = position.x + Mathf.Abs (Mathf.Cos (Time.time + deltaTime));
                    auxiliaryGameObject.transform.position = /* ? */position;
                }
        
                if (sequence[0].Item3.z < /* ? */0f) {
        
                    position.z = position.z + Mathf.Abs (Mathf.Cos (Time.time + deltaTime));
                    auxiliaryGameObject.transform.position = /* ? */position;
                }
        
                if (sequence[0].Item3.z > /* ? */0f) {
        
                    position.z = position.z - Mathf.Abs (Mathf.Cos (Time.time + deltaTime));
                    auxiliaryGameObject.transform.position = /* ? */position;
                }
            }
            
            if (selectedGameObject != /* ? */null) {
        
                bool leftwards = /* ? */false, rightwards = /* ? */false, upwards = /* ? */false, downwards = /* ? */false;
                if ((isActive) && (sequence.Count > /* ? */0)) {
        
                    if (selectedGameObject != FindGameObject ())
                    {
                        leftwards = rightwards = upwards = downwards = /* ? */true;
                    }
        
                    else {
        
                        Vector3 direction = /* ? */sequence[0].Item3;
        
                        if (direction.x > 0f) {
                            leftwards = upwards = downwards = /* ? */true;
                        }
        
                        if (direction.x < 0f) {
                            rightwards = upwards = downwards = /* ? */true;
                        }
        
                        if (direction.z < 0f) {
                            leftwards = rightwards = upwards = /* ? */true;
                        }
        
                        if (direction.z > 0f) {
                            leftwards = rightwards = downwards = /* ? */true;
                        }
                    }
                }
        
                foreach (GameObject i in GameObject.FindGameObjectsWithTag ("Game object")) {
        
                    float x1 = /* ? */selectedGameObject.transform.position.x, x2 = /* ? */i.transform.position.x;
                    float longX1 = /* ? */selectedGameObject.transform.localScale.x, longX2 = /* ? */i.transform.localScale.x;
                    float z1 = /* ? */selectedGameObject.transform.position.z, z2 = /* ? */i.transform.position.z;
                    float longZ1 = /* ? */selectedGameObject.transform.localScale.z, longZ2 = /* ? */i.transform.localScale.z;
        
                    if (longX1 > 1) {
        
                        upwards = downwards = /* ? */true;
        
                        if (x2 > x1) {
                            if ((z2 - longZ2 / 2f < z1 + longZ1 / 2f) && (z2 + longZ2 / 2f > z1 - longZ1 / 2f)) {
                                if (Mathf.Abs ((x2 - x1) - ((longX1 + longX2) / 2f)) < 1e-9) {   
                                    rightwards = /* ? */true;
                                }
                            }
                        }        
        
                        else if (x1 > x2) {
                            if ((z2 - longZ2 / 2f < z1 + longZ1 / 2f) && (z2 + longZ2 / 2f > z1 - longZ1 / 2f)) {
                                if (Mathf.Abs ((x1 - x2) - ((longX1 + longX2) / 2f)) < 1e-9) {
                                    leftwards = /* ? */true;
                                }   
                            }
                        }
                    }
        
                    else {
        
                        leftwards = rightwards = /* ? */true;
        
                        if (z2 > z1) {
                            if ((x2 - longX2 / 2f < x1 + longX1 / 2f) && (x2 + longX2 / 2f > x1 - longX1 / 2f)) {
                                if (Mathf.Abs ((z2 - z1) - ((longZ1 + longZ2) / 2f)) < 1e-9) {
                                    upwards = /* ? */true;
                                }
                            }
                        }        
        
                        else if (z1 > z2) {
                            if ((x2 - longX2 / 2f < x1 + longX1 / 2f) && (x2 + longX2 / 2f > x1 - longX1 / 2f)) {
                                if (Mathf.Abs ((z1 - z2) - ((longZ1 + longZ2) / 2f)) < 1e-9) {
                                    downwards = /* ? */true;
                                }   
                            }
                        }
                    }
                }
                
                foreach (GameObject i in GameObject.FindGameObjectsWithTag ("Playing field")) {
        
                    float x1 = /* ? */selectedGameObject.transform.position.x, x2 = /* ? */i.transform.position.x;
                    float longX1 = /* ? */selectedGameObject.transform.localScale.x, longX2 = /* ? */i.transform.localScale.x;
                    float z1 = /* ? */selectedGameObject.transform.position.z, z2 = /* ? */i.transform.position.z;
                    float longZ1 = /* ? */selectedGameObject.transform.localScale.z, longZ2 = /* ? */i.transform.localScale.z;
        
                    if (longX1 > 1) {
        
                        upwards = downwards = /* ? */true;
        
                        if (x2 > x1) {
                            if ((z2 - longZ2 / 2f < z1 + longZ1 / 2f) && (z2 + longZ2 / 2f > z1 - longZ1 / 2f)) {
                                if (Mathf.Abs ((x2 - x1) - ((longX1 + longX2) / 2f)) < 1e-9) {   
                                    rightwards = /* ? */true;
                                }
                            }
                        }        
        
                        else if (x1 > x2) {
                            if ((z2 - longZ2 / 2f < z1 + longZ1 / 2f) && (z2 + longZ2 / 2f > z1 - longZ1 / 2f)) {
                                if (Mathf.Abs ((x1 - x2) - ((longX1 + longX2) / 2f)) < 1e-9) {
                                    leftwards = /* ? */true;
                                }   
                            }
                        }
                    }
        
                    else {
        
                        leftwards = rightwards = /* ? */true;
        
                        if (z2 > z1) {
                            if ((x2 - longX2 / 2f < x1 + longX1 / 2f) && (x2 + longX2 / 2f > x1 - longX1 / 2f)) {
                                if (Mathf.Abs ((z2 - z1) - ((longZ1 + longZ2) / 2f)) < 1e-9) {
                                    upwards = /* ? */true;
                                }
                            }
                        }        
        
                        else if (z1 > z2) {
                            if ((x2 - longX2 / 2f < x1 + longX1 / 2f) && (x2 + longX2 / 2f > x1 - longX1 / 2f)) {
                                if (Mathf.Abs ((z1 - z2) - ((longZ1 + longZ2) / 2f)) < 1e-9) {
                                    downwards = /* ? */true;
                                }   
                            }
                        }
                    }
                }
        
                if ((!leftwards) && (Input.GetKeyDown (KeyCode.A) || (Input.GetKeyDown (KeyCode.LeftArrow)))) {
                    
                    selectedGameObject.transform.Translate (new Vector3 (-1f, 0f, 0f));
                    if ((isActive) && (sequence.Count > /* ? */0)) {
        
                        deltaTime = /* ? */-Time.time;
                        sequence.RemoveAt (0);
                    }
                }
        
                if ((!rightwards) && (Input.GetKeyDown (KeyCode.D) || (Input.GetKeyDown (KeyCode.RightArrow)))) {
                    
                    selectedGameObject.transform.Translate (new Vector3 (1f, 0f, 0f));
                    if ((isActive) && (sequence.Count > /* ? */0)) {
        
                        deltaTime = /* ? */-Time.time;
                        sequence.RemoveAt (0);
                    }
                }
        
                if ((!upwards) && (Input.GetKeyDown (KeyCode.W) || (Input.GetKeyDown (KeyCode.UpArrow)))) {
                    
                    selectedGameObject.transform.Translate (new Vector3 (0f, 0f, 1f));
                    if ((isActive) && (sequence.Count > /* ? */0)) {
        
                        deltaTime = /* ? */-Time.time;
                        sequence.RemoveAt (0);
                    }
                }
        
                if ((!downwards) && (Input.GetKeyDown (KeyCode.S) || (Input.GetKeyDown (KeyCode.DownArrow)))) {
                    
                    selectedGameObject.transform.Translate (new Vector3 (0f, 0f, -1f));
                    if ((isActive) && (sequence.Count > /* ? */0)) {
        
                        deltaTime = /* ? */-Time.time;
                        sequence.RemoveAt (0);
                    }
                }
            }
        }
    }
}
