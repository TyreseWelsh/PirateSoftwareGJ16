using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScriptController : MonoBehaviour
{
    Animator animator;
    int isRunningHash;
    [SerializeField] private GameObject Player;
    private MainPlayerController PlayerScript;
    //private ShootComponent ShootComponent;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
        PlayerScript = Player.GetComponent<MainPlayerController>();
        //ShootComponent = Player.GetComponent<ShootComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        

        // if player is moving
        if (!isRunning && PlayerScript.moveDirection.magnitude > 1 && PlayerScript.controller.isGrounded)
        {
            // then set the isRunning boolean to be true
            animator.SetBool(isRunningHash, true);
        }

        // if player is not moving
        if (isRunning && PlayerScript.moveDirection.magnitude < 1 && !PlayerScript.controller.isGrounded)
        {
            // then set the isRunning boolean to be false
            animator.SetBool(isRunningHash, false);
        }
       
        // if player is shooting
        /*if (ShootComponent.bHoldingTrigger && ShootComponent.bCanShoot)
        {
            // play shooting animation
            Debug.Log("Shooting");
            animator.SetTrigger(name: "isShooting");
        }*/
        
    }
}
