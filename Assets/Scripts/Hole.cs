using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hole : MonoBehaviour
{
    BoxCollider boxCollider;
    [SerializeField] Transform flag;

    private void OnDrawGizmos()
    {
        Color c = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        c.a = 0.9f;
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
            ball.Stop();
            flag.DOComplete();
            flag.DOShakeRotation(0.3f, 10, 10);
            if (LevelManager.instance)
                LevelManager.instance.LevelComplete();
        }
    }
}
