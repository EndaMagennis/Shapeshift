using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shapeshift : MonoBehaviour
{
    public List<GameObject> learnedShapes = new List<GameObject>();
    public List<GameObject> possibleShapes = new List<GameObject>();
    [Serialize] List<GameObject> prefabs = new List<GameObject>();

    public int currentShapeIndex = 0;
    private RaycastHit hitData;
    private Ray playerRay;
    private float rayDistance = 15.0f;

    private Transform playerTransform;
    private MeshRenderer playerMeshRenderer;
    private MeshFilter playerMeshFilter;

    private Transform learnedShapesTransform;
    private MeshFilter learnedShapesMeshFilter;
    private MeshRenderer learnedShapesMeshRenderer;

    private void Start()
    {
        learnedShapes.Add(gameObject);

        playerTransform = GetComponent<Transform>();
        playerMeshRenderer = GetComponent<MeshRenderer>();
        playerMeshFilter = GetComponent<MeshFilter>();
        playerRay = new Ray(transform.position, transform.forward);
    }

    private void Update()
    {
        LearnShape();
        ChangeShape();
        Debug.DrawRay(transform.position, transform.forward * rayDistance);
    }

    void LearnShape()
    {
        if (Physics.Raycast(playerRay, out hitData, rayDistance))
        {
            Debug.Log("Press 'Q' to LearnShape");

            // checks if thing hit is able to be copied using the 'copiable' tag and limiting it to only be added to possible shapes once
            if (hitData.collider.CompareTag("Copiable") && possibleShapes.Count <= 1)
            {
                //adds the copiable shape to the List of possible shapes
                possibleShapes.Add(hitData.collider.gameObject);
            }
        }
        if (possibleShapes.Count > 0 && Input.GetKeyDown(KeyCode.Q))
        {
            //iterating through possible shapes
            for (int j = 0; j < possibleShapes.Count; j++)
            {
                //checking if the possible shape has already been learned
                if (learnedShapes.Contains(possibleShapes[j]))
                {
                    Debug.Log("You already know this shape");
                    possibleShapes.Clear();
                }
                else
                {
                    learnedShapes[j] = ObjectPool.SharedInstance.GetPooledObject();
                    learnedShapes.Add(possibleShapes[j]);
                }
            }
        }

    }
    void ChangeShape()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentShapeIndex <= learnedShapes.Count - 1)
            {
                currentShapeIndex++;
            }
            else
            {
                currentShapeIndex = 0;
            }

        }
        if (learnedShapes.Count > 0 && Input.GetKeyDown(KeyCode.E))
        {
            GameObject doppleganger = Instantiate(learnedShapes[currentShapeIndex]);

            gameObject.SetActive(false);
            doppleganger.SetActive(true);

            doppleganger.transform.position = transform.position;
            doppleganger.transform.rotation = transform.rotation;
            doppleganger.transform.localScale = transform.localScale;
        }
    }
}
