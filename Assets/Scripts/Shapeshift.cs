using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shapeshift : MonoBehaviour
{
    public List<GameObject> shapes;
    int shapeIndex;
    float rayLength = 20.0f;

    Mesh originalMesh;
    Renderer originalRenderer;
    MeshFilter meshFilter;

    Ray ray;
    RaycastHit hit;

    private void Start()
    {
        shapes = new List<GameObject>();
        originalMesh = gameObject.GetComponent<MeshFilter>().mesh;
        originalRenderer = gameObject.GetComponent<Renderer>();
        ray = new Ray(transform.position, transform.forward);
    }

    private void Update()
    {
        ChangeShape();
        Debug.DrawRay(transform.position, transform.forward * rayLength);
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            Debug.Log("You Hit a " + hit.collider.name);
            if (hit.collider.gameObject.CompareTag("Copiable"))
            {
                shapes.Add(hit.collider.gameObject);
            }
        }
    }

    void ChangeShape()
    {
        if (shapes != null && shapes.Count > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            shapeIndex = (shapeIndex + 1) % shapes.Count;
            gameObject.GetComponent<MeshFilter>().mesh = shapes[shapeIndex].GetComponent<MeshFilter>().mesh;
            gameObject.GetComponent<Renderer>().material = shapes[shapeIndex].GetComponent<Renderer>().material;
            gameObject.transform.localScale = shapes[shapeIndex].transform.localScale;

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.GetComponent<MeshFilter>().mesh = originalMesh;
        }
    }
}
