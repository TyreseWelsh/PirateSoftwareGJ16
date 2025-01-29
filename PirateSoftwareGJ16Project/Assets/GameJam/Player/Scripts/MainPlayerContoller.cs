using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class MainPlayerController : MonoBehaviour, IMobile
{
    [SerializeField] private GameObject mesh;
    [SerializeField] private GameObject AnimatedMesh;
    [SerializeField] private Transform cameraTransform;
    private StatManagerComponent statManager;
    
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpHeight = 12f;
    private float gravity = 34f;
    [SerializeField] private float lookRotationSpeed = 40f;
    [HideInInspector] public Vector3 moveDirection;

    [SerializeField] GameObject spawner;

    [HideInInspector] public CharacterController controller;
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
        Move();
        
        // Rotate to camera forward
        Quaternion newRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation, newRotation, Time.deltaTime * lookRotationSpeed);
    }

    private bool jumping = false;
    
    public void Move()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), moveDirection.y, Input.GetAxisRaw("Vertical"));

            moveDirection = moveDirection.x * cameraTransform.right.normalized + moveDirection.z * cameraTransform.forward.normalized;
            float currentSpeed = GetMoveSpeed(true);
            moveDirection.x *= currentSpeed;
            moveDirection.z *= currentSpeed;
            if (jumping)
            {
                jumping = false;
                moveDirection.y = jumpHeight;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(Time.deltaTime * moveDirection);
        Debug.Log(moveDirection);
    }

    // TODO: HEEEEEEEELLLLLLLLLLLLPPPPPPPPP
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (controller.isGrounded)
            {
                jumping = true;
                /*Vector3 jumpVector = Vector3.zero;
                jumpVector.y += Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y));
                controller.Move(jumpVector);*/
                //Debug.Log("After jump= " + jumpHeight * -6 * (Physics.gravity.y));
                //moveDirection.y += jumpHeight * -24 * (Physics.gravity.y); 
            }
        }
    }
    
    public float GetMoveSpeed(bool modified)
    {
        if (!modified)
        {
            return moveSpeed;
        }
        
        return statManager.ApplyStatIncrease("MoveSpeed", moveSpeed);
    }

    public void SetMoveSpeed(float _speed)
    {
        //
    }

    public void DamageSelf(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GetComponent<HealthComponent>()?.TakeDamage(10, gameObject);
        }
    }

}
