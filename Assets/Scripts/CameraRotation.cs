using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.2f; 
    public static CameraRotation instance;
    Vector3 smoothRot;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void RotateCamera()           
    {
        smoothRot.y += Input.GetAxis("Mouse X") * rotationSpeed;
    }

    private void LateUpdate()
    {
        Quaternion rot = Quaternion.Euler(smoothRot);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
    }
}
