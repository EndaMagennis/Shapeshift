using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player data
    private Vector3 m_Position;
    private int m_PlayerHealth = 100;
    public bool isMoving;
    private bool m_isWorking;
    private bool isRunning;

    //player movement
    private Rigidbody playerRB;
    private CharacterController controller;
    private Transform cam;
    public float speed = 5.0f;
    float verticalInput;
    float horizontalInput;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

 
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_Position = gameObject.transform.position;
        m_Position = GameDataManager.instance.playerPosition;
        m_PlayerHealth = GameDataManager.instance.playerHealth;
        playerRB = gameObject.GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<CharacterController>();
        cam = GameObject.Find("Main Camera").GetComponent<Transform>();
        
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 20.0f);
        PlayerMove();
    }

    public void PlayerMove()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

    }
        
}
