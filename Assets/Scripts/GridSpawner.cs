using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpawner : MonoBehaviour
{
    public GameObject[] prefabs; // The array of prefabs to spawn
    public int width = 10; // The width of the grid
    public int height = 10; // The height of the grid
    public float gridSize = 1.0f; // The size of each grid cell
    public float spawnOffset = 0.5f; // The offset from the center of each grid cell
    public GameObject ItemPopup;
    private Vector3 touchPosition; // Store the position where the player has touched the screen
    public bool popupActive; // Flag to check if popup is active

    private void Start()
    {
        ItemPopup.GetComponent<SlideUpAndDown>();
    }

    private void Update()
    {
        popupActive = ItemPopup.GetComponent<SlideUpAndDown>().isShown;

        if (!popupActive && Input.GetMouseButtonDown(0)) // Check for left mouse button click when popup is not active
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // Check if the ray hits something
            {
                Debug.Log("Raycast hit object: " + hit.transform.name);
                if (hit.transform.name == "Ground")
                {
                    if (hit.transform.childCount == 0) // Check if the grid cell is empty
                    {
                        // Calculate the grid cell position based on the hit point
                        int x = Mathf.FloorToInt(hit.point.x / gridSize);
                        int z = Mathf.FloorToInt(hit.point.z / gridSize);
                        touchPosition = new Vector3(x * gridSize, 0, z * gridSize) + new Vector3(spawnOffset, 0, spawnOffset);
                        touchPosition += new Vector3(Random.Range(-spawnOffset, spawnOffset), 0, Random.Range(-spawnOffset, spawnOffset)); // Add random offset to position

                        // Open the popup to search for a plant
                        ItemPopup.GetComponent<SlideUpAndDown>().OnButtonClick();
                        popupActive = true; // Set the flag to true
                    }
                }
                else
                {
                    //Debug.Log("Raycast hit object: " + hit.transform.name);
                }
            }
        }
    }

    public void SpawnPrefab(int index)
    {
        popupActive = false; // Set the flag to false when popup is closed
        if (index >= 0 && index < prefabs.Length)
        {
            //string searchQuery = searchField.text;
            // Use the search query to find a plant and add it to the garden at the given position
            // ...
            ItemPopup.GetComponent<SlideUpAndDown>().OnButtonClick();
            // Spawn the prefab at the touch position
            Instantiate(prefabs[index], touchPosition, Quaternion.identity);
        }
    }
}
