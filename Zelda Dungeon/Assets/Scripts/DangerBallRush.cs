using System;
using UnityEngine;

public class DangerBallRush : MonoBehaviour
{
    public float speedMultiplier = 2;
    public float speed = 60f;
    public bool canMove;

    private Rigidbody rb;
    Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        
        if (!canMove)
        {
            rb.isKinematic = true;
        }
    }
    private void Start()
    {
        
    }


    public void StartBall()
    {
        canMove = true;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }
    public void FixedUpdate()
    {
        if (canMove)
        {
            rb.isKinematic = false;
            rb.AddForce(transform.forward * (speed * speedMultiplier * Time.fixedDeltaTime), ForceMode.VelocityChange);
        }
    }

    public void ResetPosition()
    {
        canMove = false;
        transform.position = startPos;
    }
}
