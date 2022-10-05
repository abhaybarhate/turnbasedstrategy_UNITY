using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform CameraTransform;


    private void Awake() {
        CameraTransform = Camera.main.transform;
    }

    private void LateUpdate() {
        //Vector3 directionToCamera = (CameraTransform.position - transform.position).normalized;
        
        transform.LookAt(CameraTransform);
    }

}
