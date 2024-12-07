using System.Collections;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject prefab; // The prefab to spawn
    public float initialDelay = 3f; // Initial delay before spawning
    public float spawnInterval = 2f; // Interval between spawns
    [SerializeField] private Vector3 spawnRotationEuler = Vector3.zero; // Editable spawn rotation in Euler angles

    private Coroutine spawnEnemy; // To manage the coroutine

    private void Awake()
    {
        spawnEnemy = StartCoroutine(SpawnPrefabAtInterval());
    }

    public IEnumerator SpawnPrefabAtInterval()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            Quaternion spawnRotation = Quaternion.Euler(spawnRotationEuler);
            GameObject instance = Instantiate(prefab, transform.position, spawnRotation);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the rotation direction
        Gizmos.color = Color.red;

        // Convert spawn rotation to a forward direction
        Quaternion spawnRotation = Quaternion.Euler(spawnRotationEuler);
        Vector3 direction = spawnRotation * Vector3.right; // Use 'right' as it's the local X-axis in 2D

        // Draw a line representing the direction
        Gizmos.DrawLine(transform.position, transform.position + direction * 2f); // Arrow length is 2 units
        Gizmos.DrawSphere(transform.position + direction * 2f, 0.1f); // Add a sphere at the end of the arrow
    }
}
