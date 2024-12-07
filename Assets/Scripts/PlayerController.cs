using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 mouseDownPos;
    private Vector2 mouseUpPos;
    private Vector2 force = new Vector2(0f, 0f);
    private Vector2 velocity = new Vector2(0f, 0f);
    private float forceRemainder;
    [SerializeField] private float speedMultiplier = 0.1f;
    [SerializeField] private float friction = 0.1f;
    [SerializeField] private float frictionMultiplier = 0.9f;
    [SerializeField] private Vector2 maxSpeed = new Vector2(30f, 30f);
    [SerializeField] private Vector2 minSpeed = new Vector2(30f, 30f);
    [SerializeField] private float shieldPushForce = 5f;

    void Update()
    {
        // Capture mouse input and calculate initial velocity
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
            Debug.Log("MouseDownPos: " + mouseDownPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseUpPos = Input.mousePosition;
            Debug.Log("MouseUpPos: " + mouseUpPos);
            force = (mouseDownPos - mouseUpPos) * speedMultiplier;
            forceRemainder = (force.x + force.y) - (maxSpeed.x + maxSpeed.y);
            Debug.Log("forceRemainder: " + forceRemainder);
            velocity = force;
            if(velocity.x > maxSpeed.x)
            {
                velocity.x = maxSpeed.x;
            }
            if(velocity.y > maxSpeed.y)
            {
                velocity.y = maxSpeed.y;
            }
            if(velocity.x < minSpeed.x)
            {
                velocity.x = minSpeed.x;
            }
            if (velocity.y < minSpeed.y)
            {
                velocity.y = minSpeed.y;
            }
            Debug.Log("Velocity: " + velocity);
        }

        // Apply velocity and friction
        if (velocity != Vector2.zero)
        {
            transform.position += (Vector3)velocity * Time.deltaTime;
            velocity = Vector2.Lerp(velocity, Vector2.zero, friction * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reverse direction upon collision and reduce velocity slightly
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Shield") || collision.gameObject.CompareTag("Spawner"))
        {
            Vector2 normal = collision.contacts[0].normal; // Get collision normal
            velocity = Vector2.Reflect(velocity, normal);  // Reflect the velocity vector
            velocity *= frictionMultiplier; // Optional: Reduce velocity slightly to simulate energy loss
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            Vector2 normal = collision.contacts[0].normal; // Get collision normal
            velocity += normal * shieldPushForce; // Add push force to velocity
        }
    }
}
