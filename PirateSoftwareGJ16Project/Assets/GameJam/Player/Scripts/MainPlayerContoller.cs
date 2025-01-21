using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class MainPlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject mesh;
    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float lookRotationSpeed = 40f;
    private Vector3 moveDirection;
    
    CharacterController controller;
    PlayerInput playerInput;
    InputAction moveAction;

    private Vector2 lastMousePosition;

    [Header("Stats")]
    [SerializeField] private int MAX_HEALTH = 100;
    public float health { get; set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        
        // Make sure to unlock Cursor when attempting to use UI
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
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
        mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation, newRotation, Time.deltaTime * lookRotationSpeed);
    }

    public void EmptyInputFunction(InputAction.CallbackContext context)
    {
    }

    public void TakeDamage(float _damage)
    {
        health -= _damage;

        if (health <= 0)
        {
            Debug.Log("PLAYER DEAD");
            Destroy(gameObject);
        }
    }
}
