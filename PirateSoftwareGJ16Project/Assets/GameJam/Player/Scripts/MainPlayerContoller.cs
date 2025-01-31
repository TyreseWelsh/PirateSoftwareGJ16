using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
// UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class MainPlayerController : MonoBehaviour, IMobile
{
    [SerializeField] private GameObject mesh;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject player;
    private Animator animator;
    [SerializeField] private Transform cameraTransform;
    private StatManagerComponent statManager;
    
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpHeight = 12f;
    private float gravity = 34f;
    [SerializeField] private float lookRotationSpeed = 40f;
    [HideInInspector] public Vector3 moveDirection;
    [HideInInspector]public bool isPaused = false;
    
    [SerializeField] GameObject spawner;

    [HideInInspector] public CharacterController controller;
    PlayerInput playerInput;
    InputAction moveAction;

    private Vector2 lastMousePosition;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = mesh.GetComponent<Animator>();
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
        animator.SetBool("isGrounded", controller.isGrounded);
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
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (controller.isGrounded)
            {
                jumping = true;
                animator.SetTrigger(name: "isJumping");
            }
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isPaused)
            {
                Debug.Log("Pause");
                pause();
            }
            else
            {
                Debug.Log("UnPause");
                unPause();
            }
        }

    }

    public void pause()
    {
        
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
            
        
    }

    public void unPause()
    {

        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
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
