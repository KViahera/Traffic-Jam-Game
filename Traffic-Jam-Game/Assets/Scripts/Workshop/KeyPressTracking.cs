using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Workshop
{
    public class KeyPressTracking : MonoBehaviour
    {
        public GameObject parentGameObject, basicGameObject;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                GameObject finalGameObject = Instantiate(basicGameObject, parentGameObject.transform);

                finalGameObject.transform.position = new Vector3(5.5f, 0.5f, 5.5f);

                finalGameObject.GetComponent<Renderer>().material.color =
                    new Color(Random.value, Random.value, Random.value);
                
                finalGameObject.SetActive(true);
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                Destroy(ClickReceiver.SelectedGameObject);
            }
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                ClickReceiver.SelectedGameObject.transform.Translate(new Vector3(0f, 0f, 1f));
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                ClickReceiver.SelectedGameObject.transform.Translate(new Vector3(0f, 0f, -1f));
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                ClickReceiver.SelectedGameObject.transform.Translate(new Vector3(-1f, 0f, 0f));
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {    
                ClickReceiver.SelectedGameObject.transform.Translate(new Vector3(1f, 0f, 0f));
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Vector3 globalScale = ClickReceiver.SelectedGameObject.transform.localScale;
                if (globalScale.z != 3f)
                {
                    ClickReceiver.SelectedGameObject.transform.Translate(new Vector3(0f, 0f, 0.5f));
                }
                
                globalScale.z = Mathf.Min (3f, globalScale.z + 1f);
                ClickReceiver.SelectedGameObject.transform.localScale = globalScale;
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Vector3 globalScale = ClickReceiver.SelectedGameObject.transform.localScale;
                if (globalScale.z != 1f)
                {
                    ClickReceiver.SelectedGameObject.transform.Translate(new Vector3(0f, 0f, -0.5f));    
                }
                
                globalScale.z = Mathf.Max (1f, globalScale.z - 1f);
                ClickReceiver.SelectedGameObject.transform.localScale = globalScale;
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Vector3 globalScale = ClickReceiver.SelectedGameObject.transform.localScale;
                if (globalScale.x != 3f)
                {
                    ClickReceiver.SelectedGameObject.transform.Translate(new Vector3(-0.5f, 0f, 0f));
                }
                
                globalScale.x = Mathf.Min (3f, globalScale.x + 1f);
                ClickReceiver.SelectedGameObject.transform.localScale = globalScale;
            }
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Vector3 globalScale = ClickReceiver.SelectedGameObject.transform.localScale;
                if (globalScale.x != 1f)
                {
                    ClickReceiver.SelectedGameObject.transform.Translate(new Vector3(0.5f, 0f, 0f));
                }
                
                globalScale.x = Mathf.Max (1f, globalScale.x - 1f);
                ClickReceiver.SelectedGameObject.transform.localScale = globalScale;
            }
        }
    }
}
