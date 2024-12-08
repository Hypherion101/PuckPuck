using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform target; // Target to follow
    public float moveSpeed = 5f; // Movement speed
    public float rotationSpeed = 360f; // Rotation speed (degrees per second)
    public float avoidanceStrength = 1f; // Strength of avoidance force
    public float avoidanceRadius = 0.5f;
    public float wallCheckDistance = 0.6f; // Distance to check for walls (slightly larger than the step size)
    public float wallPushbackStrength = 0.1f; // Strength to push back when stuck in a wall

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

        // Check for obstacles and move only if clear
        Vector3 movement = transform.up * moveSpeed * Time.deltaTime;
        if (!IsObstacleInPath(movement))
        {
            transform.position += movement;
        }
        else
        {
            ResolveCollision();
        }
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

    private bool IsObstacleInPath(Vector3 movement)
    {
        // Perform a raycast to check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(transform.position, movement.normalized, wallCheckDistance);
        if (hit.collider != null && hit.collider.CompareTag("Wall"))
        {
            return true; // Obstacle detected
        }
        return false; // No obstacle in path
    }

    private void ResolveCollision()
    {
        // Check if the object is stuck in a wall and push it out slightly
        Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(transform.position, wallCheckDistance);
        foreach (Collider2D collider in overlappingColliders)
        {
            if (collider.CompareTag("Wall"))
            {
                // Push back along the normal of the wall
                Vector2 pushDirection = (Vector2)(transform.position - collider.transform.position).normalized;
                transform.position += (Vector3)(pushDirection * wallPushbackStrength);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the avoidance radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);

        // Visualize the wall detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * wallCheckDistance);
    }
}
