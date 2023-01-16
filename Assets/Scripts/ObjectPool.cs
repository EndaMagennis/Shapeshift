using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<GameObject> objects;
    public GameObject objectToPool;
    public int poolAmount;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        objects = new List<GameObject>();
        GameObject temp;
        for(int i = 0; i < poolAmount; i++)
        {
            string shapeName = objects[i].name;

            switch (shapeName)
            {
                case "Cube":
                    // instantiate prefab
                    temp = Instantiate(Resources.Load("Assets/Prefabs/Cube.prefab")) as GameObject;
                    temp.SetActive(false);
                    objects.Add(temp);
                    // add instantiated prefab to learnedShapes list
                    break;
                case "Sphere":
                    // instantiate prefab
                    temp = Instantiate(Resources.Load("Assets/Prefabs/Sphere.prefab")) as GameObject;
                    temp.SetActive(false);
                    objects.Add(temp);
                    // add instantiated prefab to learnedShapes list
                    break;
                case "Shpere":
                    // instantiate prefab
                    temp = Instantiate(Resources.Load("Assets/Prefabs/Capsule.prefab")) as GameObject;
                    temp.SetActive(false);
                    objects.Add(temp);
                    // add instantiated prefab to learnedShapes list
                    break;

            }

        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < poolAmount; i++)
        {
            if (!objects[i].activeInHierarchy)
            {
                return objects[i];
            }
        }
        return null;
    }
}
