using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class MainPlayerController : MonoBehaviour
{
    [SerializeField] private GameObject mesh;
    [SerializeField] private Transform cameraTransform;
    private StatManagerComponent statManager;
    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float lookRotationSpeed = 40f;
    private Vector3 moveDirection;

    [SerializeField] GameObject spawner;
    
    CharacterController controller;
    PlayerInput playerInput;
    InputAction moveAction;

    private Vector2 lastMousePosition;    

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        statManager = GetComponent<StatManagerComponent>();
        
        // Make sure to unlock Cursor when attempting to use UI
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDirection = moveDirection.x * cameraTransform.right.normalized + moveDirection.z * cameraTransform.forward.normalized;
        moveDirection.y = Physics.gravity.y / 6;
        controller.Move(GetMoveSpeed(true) * Time.deltaTime * moveDirection);
        
        // Rotate to camera forward
        Quaternion newRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation, newRotation, Time.deltaTime * lookRotationSpeed);
    }

    // TEMP
    public void CalculateEnemySpawn(InputAction.CallbackContext context)
    {
        /*if (context.performed)
        {
            spawner.GetComponent<EnemySpawner>()?.CalculateWeighting();
        }*/
    }

    public float GetMoveSpeed(bool modified)
    {
        if (!modified)
        {
            return moveSpeed;
        }
        
        return statManager.ApplyStatIncrease("MoveSpeed", moveSpeed);
    }
    

}
