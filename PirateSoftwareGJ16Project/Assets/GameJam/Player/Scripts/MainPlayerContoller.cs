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
    
    [SerializeField] private float moveSpeed;
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
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Physics.gravity.y / 5, Input.GetAxisRaw("Vertical"));
        //controller.SimpleMove(moveSpeed * Time.deltaTime * moveDirection);
        controller.Move(moveSpeed * Time.deltaTime * moveDirection);
    }

    
    public void MovePlayer(InputAction.CallbackContext context)
    {
    }

    public void Look(InputAction.CallbackContext context)
    {
        /*if (mesh != null)
        {
            Vector2 currentMousePosition = context.ReadValue<Vector2>();
            Vector2 mouseMovement = currentMousePosition - lastMousePosition;
            mouseMovement *= 3;
            Debug.Log(currentMousePosition - lastMousePosition);
            mesh.transform.rotation = Quaternion.Euler(mesh.transform.rotation.x + mouseMovement.y, mesh.transform.rotation.y + mouseMovement.x, 0f);
        
            lastMousePosition = currentMousePosition;
        }*/
    }
}
