using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneGameLevel
{
    public class CreatingALevel : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            listOfGameObjects = AnnealingMethod.Launch(20);
            //Debug.Log(listOfGameObjects.Count);

            //CreatingThePlayingField();
            CreatingGameObjects();
        }

        // private void CreatingThePlayingField()
        // {
        //
        // }

        private void CreatingGameObjects()
        {
            foreach ((float, float, string, string, int) currentGameObject in listOfGameObjects)
            {
                GameObject newGameObject = Instantiate(basicGameObject, parentGameObject.transform);

                newGameObject.GetComponent<Transform>().position =
                    new Vector3(currentGameObject.Item1, 0.5f, currentGameObject.Item2);
    
                if (currentGameObject.Item3 == "Player")
                {
                    newGameObject.GetComponent<Transform>().localScale = new Vector3(2f, 1f, 1f);
                    newGameObject.AddComponent<Rigidbody>();
                    newGameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY |
                                                                          RigidbodyConstraints.FreezePositionZ |
                                                                          RigidbodyConstraints.FreezeRotationX |
                                                                          RigidbodyConstraints.FreezeRotationY |
                                                                          RigidbodyConstraints.FreezeRotationZ;
                }
                
                else
                {
                    if (currentGameObject.Item4 == "Horizontal")
                    {
                        newGameObject.GetComponent<Transform>().localScale =
                            new Vector3((float)(currentGameObject.Item5), 1f, 1f);
                    }
                
                    else
                    {
                        newGameObject.GetComponent<Transform>().localScale =
                            new Vector3(1f, 1f, (float)(currentGameObject.Item5));
                    }
                }
                
                newGameObject.GetComponent<Renderer>().material.color =
                    new Color(Random.value, Random.value, Random.value);

                newGameObject.SetActive(true);
            }
        }

        public GameObject basicGameObject;
        public GameObject parentGameObject;

        private List<(float, float, string, string, int)> listOfGameObjects =
            new List<(float, float, string, string, int)>();
    }
}