using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysPointToPlayer : MonoBehaviour
{
    public Camera thirdPersonCam;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + thirdPersonCam.transform.rotation * Vector3.forward, thirdPersonCam.transform.rotation * Vector3.up);
    }
}
