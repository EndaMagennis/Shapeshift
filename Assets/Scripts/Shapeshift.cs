using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shapeshift : MonoBehaviour
{

    public float rayDistance = 10.0f;
    public LayerMask objectLayer;

    public Mesh originalMesh;
    public Material originalMaterial;

    public Mesh desiredMesh;
    public Material desiredMaterial;

    private void Start()
    {
        // Store a reference to the player's original mesh and material
        originalMesh = GetComponent<MeshFilter>().mesh;
        originalMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Create a ray from the camera's position in the direction that the camera is facing
        Ray ray = new Ray(gameObject.transform.position(0,-.5,0), gameObject.transform.forward);

        // Perform a raycast and store the result in a variable
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance, objectLayer))
        {
            Debug.Log("You hit a " + hit.collider.name);

            // Check if the player presses a button to change shape
            if (hit.collider.CompareTag("Copiable") && Input.GetKeyDown(KeyCode.Space))
            {
                desiredMesh = Instantiate(hit.collider.GetComponent<MeshFilter>().mesh);
                desiredMaterial = Instantiate(hit.collider.GetComponent<Renderer>().material);
                ChangeShape(hit);
            }

            // Check if the player wants to return to original shape
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReturnToOriginalShape();
            }
        }
    }

    private void ChangeShape(RaycastHit hit)
    {
        Destroy(GetComponent<MeshFilter>());
        Destroy(GetComponent<MeshRenderer>());

        // Change the player's transform properties to match the object's transform properties
        transform.position = hit.transform.position;
        transform.rotation = hit.transform.rotation;
        transform.localScale = hit.transform.localScale;

        // Add new mesh filter and mesh renderer components using the object's mesh and material
        MeshFilter newMeshFilter = gameObject.AddComponent<MeshFilter>();
        newMeshFilter.mesh = desiredMesh;
        MeshRenderer newMeshRenderer = gameObject.AddComponent<MeshRenderer>();
        newMeshRenderer.material = desiredMaterial;
    }

    private void ReturnToOriginalShape()
    {
        // Remove the player's current mesh filter and mesh renderer components
        Destroy(GetComponent<MeshFilter>());
        Destroy(GetComponent<MeshRenderer>());

        // Add the player's original mesh filter and mesh renderer components
        MeshFilter originalMeshFilter = gameObject.AddComponent<MeshFilter>();
        originalMeshFilter.mesh = originalMesh;
        MeshRenderer originalMeshRenderer = gameObject.AddComponent<MeshRenderer>();
        originalMeshRenderer.material = originalMaterial;
    }
}
