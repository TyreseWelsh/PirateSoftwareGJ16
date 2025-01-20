using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class MainPlayerController : MonoBehaviour
{
    [SerializeField] private GameObject cameraOrigin;
    [SerializeField] private GameObject mesh;
    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float lookRotationSpeed = 40f;
    private Vector3 moveDirection;
    
    CharacterController controller;
    PlayerInput playerInput;
    InputAction moveAction;

    private Vector2 lastMousePosition;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDirection = moveDirection.x * cameraTransform.right.normalized + moveDirection.z * cameraTransform.forward.normalized;
        moveDirection.y = Physics.gravity.y / 6;
        controller.Move(moveSpeed * Time.deltaTime * moveDirection);
        
        // Rotate to camera forward
        Quaternion newRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        Debug.Log(cameraTransform.eulerAngles.y);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * lookRotationSpeed);
    }

    
    public void MovePlayer(InputAction.CallbackContext context)
    {
    }

    public void Look(InputAction.CallbackContext context)
    {

    }
}
