using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    private GameObject followTarget;                    
    private Vector3 basePos;                             

    private void Awake()
    {
        if (instance == null) 
            instance = this;
        else 
            Destroy(gameObject);

        SetTarget(FindObjectOfType<BallController>().gameObject);
    }

    public void SetTarget(GameObject target)
    {
        followTarget = target;                                          
        basePos = transform.position;                                
    }

    private void LateUpdate()
    {
        if (followTarget)                                              
        {
            transform.position = followTarget.transform.position;                             
        }
    }
}
