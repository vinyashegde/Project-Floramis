using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInformation : MonoBehaviour
{
    public string information;

    public void SetInformation(string info)
    {
        information = info;
    }

    public string GetInformation()
    {
        return information;
    }
}
