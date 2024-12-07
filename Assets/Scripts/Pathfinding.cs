using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform target; // Target to follow
    public float moveSpeed = 5f; // Movement speed
    public float rotationSpeed = 360f; // Rotation speed (degrees per second)

    void Update()
    {
        if (target == null) return; // Exit if no target is assigned

        // Calculate direction to the target
        Vector3 direction = target.position - transform.position;
        direction.z = 0; // Keep the movement on the 2D plane (if using 2D)

        // Rotate to face the target
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move toward the target
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }


}
