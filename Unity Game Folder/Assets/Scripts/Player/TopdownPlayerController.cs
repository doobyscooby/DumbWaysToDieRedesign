using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownPlayerController : MonoBehaviour
{
    float moveSpeed = 120.0f;
    float rotationSpeed = 150.0f;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Get axis
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // If moving diagonally normalise vector so that speed remains the same
        if (dir.magnitude > 1.0f)
            dir.Normalize();
        Vector3 vel = ((transform.forward * dir.y) * moveSpeed) * Time.fixedDeltaTime;
        vel.y = rb.velocity.y;
        // Apply velocity
        rb.velocity = vel;
        // Rotate
        transform.Rotate(Vector3.up, dir.x * rotationSpeed * Time.fixedDeltaTime);
    }
}
