using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shapeshift : MonoBehaviour
{
    //references to player graphical info
    private Transform playerTransform;
    private Renderer playerRenderer;
    private Material playerMaterial;
    private Mesh playerMesh;

    //references to desired grapical info
    private Transform desiredTransform;
    private Material desiredMaterial;
    private Renderer desiredRenderer;
    private Mesh desiredMesh;

    //List to store GameObjects to access desired graphical info
    public List<GameObject> possibleShapes = new List<GameObject>();
    public List<GameObject> learnedShapes = new List<GameObject>();

    //reference to Ray to prompt player to learn shape
    private Ray playerRay;
    private RaycastHit hitData;
    private float rayDistance = 15.0f;
    private float proximityToShape = 15.0f;


    private void Awake()
    {
        playerTransform = GetComponent<Transform>();
        playerRenderer = GetComponent<Renderer>();
        playerMaterial = GetComponent<Material>();
        playerMesh = GetComponent<Mesh>();
        
    }

    private void Start()
    {
        learnedShapes.Add(gameObject);
    }

    private void Update()
    {
        playerRay = new Ray(transform.position, transform.forward * rayDistance);
        LearnShape();
        Debug.DrawRay(transform.position, transform.forward * rayDistance);
    }

    void LearnShape()
    {
        if(Physics.Raycast(playerRay, out hitData, rayDistance))
        {
            float distanceToShape = Vector3.Distance(transform.position, hitData.transform.position);
            if (hitData.collider.CompareTag("Copiable") && possibleShapes.Count < 1)
            {
                Debug.Log("Press 'Q' to LearnShape");
                possibleShapes.Add(hitData.collider.gameObject);
            }
            if(distanceToShape>proximityToShape)
            {
                possibleShapes.Clear();
            }
            
        }
        if (possibleShapes.Count > 0 && Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = 0; i < learnedShapes.Count; i++)
            {
                for (int j = 0; j < possibleShapes.Count; j++)
                {
                    if (learnedShapes.Contains(possibleShapes[i]))
                    {
                        Debug.Log("You already know this shape");
                        possibleShapes.Clear();
                    }
                    else
                    {
                        learnedShapes.Add(possibleShapes[0]);
                        possibleShapes.Clear();
                        Debug.Log("You Learned a new Shape");
                    }
                }
            }

        }
    }

}
