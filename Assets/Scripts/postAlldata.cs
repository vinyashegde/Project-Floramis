using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class postAlldata : MonoBehaviour
{
     private string baseUrl = "https://jsonplaceholder.typicode.com/users/";
    public int startId = 1;
    public int endId = 10;
    public List<Text> plantInfoTexts;

    void Start()
    {
        StartCoroutine(GetInformation());
    }

    public IEnumerator GetInformation()
    {
        for (int i = startId; i <= endId; i++)
        {
            string url = baseUrl + i;
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

                if (i <= plantInfoTexts.Count)
                {
                    // Set the plant information text
                    plantInfoTexts[i - 1].text = postData.name;
                }

                // Display the post information
                // Debug.Log("User ID: " + postData.name);
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
}
