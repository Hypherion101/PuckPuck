using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform target; // Target to follow
    public float moveSpeed = 5f; // Movement speed
    public float rotationSpeed = 360f; // Rotation speed (degrees per second)
    public float avoidanceStrength = 1f; // Strength of avoidance force
    public float avoidanceRadius = 0.5f;

    private void Awake()
    {
        // Find the GameObject titled "player" and get its Transform
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("GameObject titled 'player' not found!");
        }
    }

    void Update()
    {
        if (target == null) return; // Exit if no target is assigned

        // Calculate direction to the target
        Vector3 direction = target.position - transform.position;
        direction.z = 0; // Keep the movement on the 2D plane (if using 2D)

        // Avoid overlapping with other objects
        Vector3 avoidance = CalculateAvoidance();
        direction += avoidance;

        // Rotate to face the target
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move forward in the direction the object is facing
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    private Vector3 CalculateAvoidance()
    {
        Vector3 avoidance = Vector3.zero;

        // Detect other Pathfinding objects nearby
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.CompareTag("Pathfinder"))
            {
                // Calculate repulsion force
                Vector3 toOther = transform.position - collider.transform.position;
                avoidance += toOther.normalized / toOther.sqrMagnitude; // Stronger force when closer
            }
        }

        return avoidance * avoidanceStrength;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the avoidance radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }
}
