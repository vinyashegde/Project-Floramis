using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
   public GameObject prefab; // The prefab to spawn

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Check if space key is pressed
        {
            Vector3 spawnPosition = GetGroundPosition(); // Get position on ground
            Instantiate(prefab, spawnPosition, Quaternion.identity); // Spawn object at position
        }
    }

    Vector3 GetGroundPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create ray from mouse position
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) // Check if ray hits anything
        {
            if (hit.collider.CompareTag("Ground")) // Check if hit object is tagged as ground
            {
                return hit.point; // Return the position of the hit point
            }
        }

        return Vector3.zero; // If no ground hit, return zero vector
    }
}
