using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shapeshift : MonoBehaviour
{
    public CinemachineFreeLook mainCamera;
    public PlayerController playerController;
    public List<GameObject> learnedShapes = new List<GameObject>();
    public List<string> learnedShapesNames = new List<string>();
    public string shapeName;

    public int currentShapeIndex = 0;
    private RaycastHit hitData;
    private Ray playerRay;
    private float rayDistance = 15.0f;

    private Transform playerTransform;
    public MeshRenderer playerMeshRenderer;
    public MeshFilter playerMeshFilter;

    private Transform learnedShapesTransform;
    private MeshFilter learnedShapesMeshFilter;
    private MeshRenderer learnedShapesMeshRenderer;

    private void Start()
    {
        learnedShapes.Add(gameObject);
        learnedShapesNames.Add(gameObject.name);

        playerTransform = GetComponent<Transform>();
        playerMeshRenderer = GetComponent<MeshRenderer>();
        playerMeshFilter = GetComponent<MeshFilter>();
        playerRay = new Ray(transform.position, transform.forward);
        playerController = GetComponent<PlayerController>();
        mainCamera = GameObject.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        LearnShape();
        ChangeShape();
        Debug.DrawRay(transform.position, transform.forward * rayDistance);
    }

    void LearnShape()
    {
        //checking Hit data
        if (Physics.Raycast(transform.position, transform.forward, out hitData,  rayDistance))
        {
            if(hitData.collider.CompareTag("Copiable") && !learnedShapesNames.Contains(hitData.collider.gameObject.name))
            {
                Debug.Log("Press 'Q' to Learn Shape");
                if (Input.GetKeyDown(KeyCode.Q))
                {                    
                    learnedShapes.Add(hitData.collider.gameObject);
                    learnedShapesNames.Add(hitData.collider.gameObject.name);
                    GameDataManager.instance.SaveList(learnedShapesNames);
                }
            }
            else
            {
                Debug.Log("You already know this shape!");
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
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            //trying to create a data persistece for learned shapes so that each instance of newPlayer will have that knowledge
           GameDataManager.instance.SaveList(learnedShapesNames);
           GameDataManager.instance.LoadGameData();
           
            GameObject selectedShape = learnedShapes[currentShapeIndex];
            string shapeName = selectedShape.name;

            switch (shapeName)
            {
                case "Player":
                    CreateNewPlayer("Player");
                break;
                case "Cube":
                    CreateNewPlayer("Cube");
                    break;
                case "Sphere":
                    CreateNewPlayer("Sphere");
                    break;
                case "Capsule":
                    CreateNewPlayer("Capsule");
                    break;
            }
        }
    }

    void CreateNewPlayer(string shape)
    {
        //destroying current player and replacing it with intatiated prefabs
        Destroy(gameObject);
        GameObject newPlayer = Instantiate(GameObject.Find(shape), gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        newPlayer.SetActive(true);
        //giving instantiated prefabs same abilities as OG player
        newPlayer.AddComponent<PlayerController>();
        newPlayer.AddComponent<CharacterController>();
        newPlayer.AddComponent<Shapeshift>();
        for (int i = 0; i < learnedShapesNames.Count; i++)
        {
            //jerry-rigged data persistence, which doesn't work
            if (!learnedShapes.Contains(gameObject))
            {
                learnedShapes.Add(GameObject.Find(learnedShapesNames[i]));
            }
        }
        mainCamera.LookAt = newPlayer.transform;
        mainCamera.Follow = newPlayer.transform;
    }
}
