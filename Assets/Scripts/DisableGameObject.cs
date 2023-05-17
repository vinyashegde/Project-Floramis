using UnityEngine;

public class DisableGameObject : MonoBehaviour
{
    public float disableTime = 3.0f; // Set the time to disable the object
    private float timer = 0.0f; // Initialize the timer

    public GameObject loadingscreen;

    private void Update()
    {
        // Increment the timer every frame
        timer += Time.deltaTime;

        // Check if the timer has reached the disable time
        if (timer >= disableTime)
        {
            // Disable the game object
            loadingscreen.SetActive(false);
        }
    }
}
