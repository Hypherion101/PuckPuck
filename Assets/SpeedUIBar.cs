using UnityEngine;
using UnityEngine.UI;

public class BallSpeedUI : MonoBehaviour
{
    public Transform ball; // Reference to the ball object
    public Slider speedSlider; // Reference to the UI slider
    public float maxSpeed = 20f; // Maximum speed for the slider normalization
    public float updateInterval = 0.5f; // Time interval for updates (twice a second)

    private Vector3 lastPosition;
    private float speed;
    private float timer;

    void Start()
    {
        if (ball == null)
        {
            Debug.LogError("Ball Transform not assigned!");
        }
        if (speedSlider == null)
        {
            Debug.LogError("Speed Slider not assigned!");
        }

        lastPosition = ball.position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= updateInterval)
        {
            // Calculate the speed based on the change in position
            speed = (ball.position - lastPosition).magnitude / timer;

            // Normalize the speed for the slider (0 to 1 range)
            speedSlider.value = Mathf.Clamp(speed / maxSpeed, 0f, 1f);

            // Debug log the ball's speed
            Debug.Log($"Ball Speed: {speed:F2}");

            // Update the last position and reset the timer
            lastPosition = ball.position;
            timer = 0f;
        }
    }
}
