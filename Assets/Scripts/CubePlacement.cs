using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlacement : MonoBehaviour
{
    public LayerMask mask;
    public Material matGrid, matDefault;

    private PostInfo objectInfo;
    
    public GameObject ObjectButtonInfo;

    public Color selectedColor = Color.red;
    public Color unselectedColor = Color.green;

    private GameObject selectedObj = null;
    private Vector3 lastMousePos;
    private Renderer groundRenderer;
    private float gridSize = 1.0f;
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();

    // Reference to the ZoomAndPan script
    private PanAndZoom zoomAndPanScript;
    private bool zoomAndPanEnabled = true;

    void Start()
    {
        groundRenderer = GameObject.Find("Ground").GetComponent<Renderer>();

        // Get the ZoomAndPan script
        zoomAndPanScript = Camera.main.GetComponent<PanAndZoom>();
    }

    void Update()
    {
        Vector3 inputPos = Vector3.zero;
        if (Input.GetMouseButton(0))
        {
            inputPos = Input.mousePosition;
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            inputPos = Input.GetTouch(0).position;
        }

        if (inputPos != Vector3.zero)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                int posX = (int)Mathf.Round(hit.point.x / gridSize);
                int posZ = (int)Mathf.Round(hit.point.z / gridSize);
                Vector3 pos = new Vector3(posX * gridSize, 0, posZ * gridSize);


                if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    GameObject clickedObj = hit.transform.gameObject;

                    if (selectedObj == null && clickedObj.CompareTag("Selectable"))
                    {
                        // Select object if clicking on one
                        selectedObj = clickedObj;
                        lastMousePos = inputPos;
                        SetObjectColor(selectedObj, selectedColor);

                        // Disable zoom and pan script
                        //zoomAndPanScript.enabled = false;
                        //zoomAndPanEnabled = false;
                        ObjectButtonInfo.SetActive(true);

                        objectInfo = selectedObj.GetComponent<PostInfo>();
                        if (objectInfo != null)
                        {
                            Debug.Log("Selected Object Information: " + StartCoroutine(objectInfo.GetInformation()));
                        }
                    }
                    else if (selectedObj != null && clickedObj == selectedObj)
                    {
                        // Unselect object if clicking on it again
                        selectedObj = null;
                        SetObjectColor(clickedObj, unselectedColor);

                        // Enable zoom and pan script
                        //zoomAndPanScript.enabled = true;
                        //zoomAndPanEnabled = true;
                        ObjectButtonInfo.SetActive(false);
                    }

                    
                }


                if (selectedObj != null)
                {
                    // Move selected object if it exists and is selected
                    //Vector3 deltaMouse = inputPos - lastMousePos;
                    //selectedObj.transform.position += new Vector3(deltaMouse.x, 0, deltaMouse.y) * 0.1f;
                    //selectedObj.transform.position = pos; // Snap to grid
                    //lastMousePos = inputPos;
                    groundRenderer.material.color = selectedColor;
                }
                else
                {
                    groundRenderer.material.color = unselectedColor;
                }
            }
        }
    }

    void SetObjectColor(GameObject obj, Color color)
    {
        if (!originalMaterials.ContainsKey(obj))
        {
            originalMaterials[obj] = obj.GetComponent<Renderer>().materials;
        }

        Material[] materials = new Material[originalMaterials[obj].Length];

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = new Material(Shader.Find("Standard"));
            materials[i].color = color;
        }

        obj.GetComponent<Renderer>().materials = materials;
    }
}
