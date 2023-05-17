using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    public GameObject ObjToMove;
    public LayerMask mask;
    public int LastPosX, LastPosZ;
    public float LastPosY;
    private Vector3 mousepos;

    private GameObject plain;
    private Renderer rend;
    public Material matGrid,matDefault;
    private bool isdraging = false;


    void Start()
    {
        plain = GameObject.Find("Ground");
        rend = plain.GetComponent<Renderer>();

    }  

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
             isdraging = true;
             Debug.Log("Draging");
             mousepos = Input.mousePosition;
             Ray ray = Camera.main.ScreenPointToRay(mousepos);
             RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
             int PosX = (int)Mathf.Round(hit.point.x);
             int PosZ = (int)Mathf.Round(hit.point.z);

            if(PosX != LastPosX || PosZ != LastPosZ)
            {
                LastPosX = PosX;
                LastPosZ = PosZ;
                ObjToMove.transform.position = new Vector3(PosX, LastPosY, PosZ);
            }
        }
        } else
        {
            Debug.Log("not Draging");
            isdraging = false;
        }
        if(isdraging)
        {
            rend.material = matGrid;
        } else if(!isdraging)
        {
            rend.material = matDefault;
        }
    }
}
