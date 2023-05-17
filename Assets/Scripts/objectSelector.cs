using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectSelector : MonoBehaviour
{
    public LayerMask layerMask;

    private GameObject selectedObject;
    private ObjectInformation objectInfo;
    private Color defaultColor;
    private Vector3 originalPosition;

    void Update()
{
    // Check if the left mouse button is pressed down
    if (Input.GetMouseButtonDown(0))
    {
        // Cast a ray from the mouse position and check if it hits an object in the mask
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // Get the selected object and its renderer component
            GameObject selectedObject = hit.transform.gameObject;
            Renderer selectedRenderer = selectedObject.GetComponent<Renderer>();

            // If there is a selected object, set its position to the hit position and change its color
            if (selectedObject != null)
            {
                // If there is already a selected object, unselect it and restore its default color and position
                if (this.selectedObject != null)
                {
                    this.selectedObject.GetComponent<Renderer>().material.color = defaultColor;
                    this.selectedObject.transform.position = originalPosition;
                }

                // Set the new selected object and save its original position and color
                this.selectedObject = selectedObject;
                defaultColor = selectedRenderer.material.color;
                originalPosition = selectedObject.transform.position;
                selectedRenderer.material.color = Color.red;
            }
        }
        else
        {
            // If the raycast doesn't hit an object, unselect the selected object (if any)
            if (selectedObject != null)
            {
                selectedObject.GetComponent<Renderer>().material.color = defaultColor;
                selectedObject = null;
            }
        }
    }

    // Check if there is a selected object and the left mouse button is held down
    if (selectedObject != null && Input.GetMouseButton(0))
    {
        // Calculate the new position of the selected object based on the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distanceToPlane = (selectedObject.transform.position.y - Camera.main.transform.position.y) / ray.direction.y;
        Vector3 newPosition = ray.GetPoint(distanceToPlane);

        // Set the position of the selected object to the new position
        selectedObject.transform.position = newPosition;
    }
}


    private void SetSelectedObject(GameObject obj)
{
    if (selectedObject != null)
    {
        // If there is a selected object, unselect it and restore its default color and position
        selectedObject.GetComponent<Renderer>().material.color = defaultColor;
        selectedObject.transform.position = originalPosition;
        selectedObject = null;
    }

    if (obj != null)
    {
        // If a new object is selected, set its color and save its original position
        selectedObject = obj;
        Material selectedMaterial = selectedObject.GetComponent<Renderer>().material;
        defaultColor = selectedMaterial.color;
        selectedMaterial.color = Color.red;
        originalPosition = selectedObject.transform.position;

        // Get the information from the ObjectInformation component and print it to the console
        ObjectInformation objectInfo = selectedObject.GetComponent<ObjectInformation>();
        if (objectInfo != null)
        {
            Debug.Log("Selected Object Information: " + objectInfo.GetInformation());
        }
        else
        {
            Debug.Log("Selected Object does not have ObjectInformation component.");
        }
    }
}


}
