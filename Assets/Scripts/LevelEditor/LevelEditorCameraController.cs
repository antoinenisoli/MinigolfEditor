using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorCameraController : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    private void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");
        Vector3 move = (transform.forward * yAxis) + (transform.right * xAxis);
        move.y = 0;
        transform.position += move.normalized * Time.deltaTime * speed;
    }
}
