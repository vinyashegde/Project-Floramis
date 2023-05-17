using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class PlantInfo : MonoBehaviour
{
    private string baseUrl = "https://trefle.io/api/v1/plants/";
    private string token = "6ZyL8ZedbFUYU1BUBjG-1q_pYsMOTckGfqC711BU1rM";
    public string plantId = "183086"; // replace with the ID of the plant you want to get information on
    public Text plantInfoText; // reference to the Text UI element

    void Start()
    {
        
    }

    public IEnumerator GetInformation()
    {
        string url = baseUrl + plantId + "?token=" + token;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error retrieving plant information: " + www.error);
        }
        else
        {
            // Parse the JSON response
            PlantData plantData = JsonUtility.FromJson<PlantData>(www.downloadHandler.text);
            Debug.Log(www.downloadHandler.text);
            // Build the plant information string
            string plantInfoString = "Common name: " + plantData.data.common_name + "\n" +
                                     "Scientific name: " + plantData.data.scientific_name + "\n" +
                                     "Family: " + plantData.data.family + "\n" +
                                     "Growth habit: " + plantData.data.specifications.growth_habit + "\n" +
                                     "Light preference: " + plantData.data.growth.light + "\n" +
                                     "Soil pH preference: " + plantData.data.growth.soil_ph + "\n";
                                     

            // Set the plant information text
            plantInfoText.text = plantInfoString;
        }
    }

    [System.Serializable]
    public class PlantData
    {
        public PlantInfoData data;
    }

    [System.Serializable]
    public class PlantInfoData
    {
        public string common_name;
        public string scientific_name;
        public string family;
        public PlantGrowthSpecifications specifications;
        public PlantGrowthRequirements growth;
        public PlantMainSpecies main_species;
    }

    [System.Serializable]
    public class PlantGrowthSpecifications
    {
        public string growth_habit;
    }

    [System.Serializable]
    public class PlantGrowthRequirements
    {
        public string light;
        public string soil_ph;
    }

    [System.Serializable]
    public class PlantMainSpecies
    {
        public PlantTemperatureTolerance growth;
    }

    [System.Serializable]
    public class PlantTemperatureTolerance
    {
        public float deg_c;
    }
}
