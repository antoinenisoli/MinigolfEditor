using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    BoxCollider boxCollider;

    private void OnDrawGizmos()
    {
        Color c = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        c.a = 0.3f;
        Gizmos.color = c;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    private void Start()
    {
        if (!boxCollider)
            boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        BallController ball = other.GetComponent<BallController>();
        if (ball)
        {
            if (LevelManager.instance)
                LevelManager.instance.LevelComplete();
        }
    }
}
