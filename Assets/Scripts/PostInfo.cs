using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class PostInfo : MonoBehaviour
{
    private string baseUrl = "https://jsonplaceholder.typicode.com/users/";
    public string postId = "1";

    public Text plantInfoText;

    void Start()
    {
        //StartCoroutine(GetInformation());
    }

    
    private void Update()
    {
        plantInfoText = GameObject.FindGameObjectWithTag("Plantext").GetComponent<Text>();
    }

public IEnumerator GetInformation()
{
    string url = baseUrl + postId;
    UnityWebRequest www = UnityWebRequest.Get(url);
    yield return www.SendWebRequest();

    if (www.isNetworkError || www.isHttpError)
    {
        Debug.Log("Error retrieving post information: " + www.error);
    }
    else
    {
        // Parse the JSON response
        PostData postData = JsonUtility.FromJson<PostData>(www.downloadHandler.text);

        string plantInfoString = "User_Id: " + postData.id + "\n" +
                                     "Name: " + postData.name + "\n" +
                                     "Username: " + postData.username + "\n" ;
                                              

            // Set the plant information text
            plantInfoText.text = plantInfoString;
        
        
        // Display the post information
        Debug.Log("User ID: " + postData.name);
        Debug.Log("Post ID: " + postData.id);
        Debug.Log("Title: " + postData.username);
        // Debug.Log("Body: " + postData.city);


        // Debug.Log("User ID: " + Address);
        // Debug.Log("Post ID: " + postData.id);
        // Debug.Log("Title: " + postData.username);
        // Debug.Log("Body: " + postData.city);
    }
}

    }

    [System.Serializable]
    public class PostData
    {
        public string name;
        public int id;
        public string username;
        public string city;
    }

     public class Address
    {
        public string street;
        public string suite;
        public string city;
        public string zipcode;
    }

