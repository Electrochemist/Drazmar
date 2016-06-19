using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendlyBuildingList : MonoBehaviour
{

    public static FriendlyBuildingList friendlyBuildingList;

    private List<GameObject> friendlyBuildings;
    private string[] friendlyBuildingTags = new string[] { "FriendZone", "FriendBuilding" }; // assigns all the possible friendly building tags to the list
    

    // Use this for initialization
    public void Awake() // called once before start
    {
        if (friendlyBuildingList != null) // singleton format if there is an instance already destroy this object, else this is the instance
        {
            Destroy(this);
        }
        else
        {
            friendlyBuildingList = this;
        }
    }

    // Update is called once per frame
    void Start ()
    {
        UpdateBuildingList();
    }

    void UpdateBuildingList() // clears list, then updates the building list - call on start, and whe a building is created or destroyed!
    {
        friendlyBuildings = null;
        for (int i = 0; i < friendlyBuildingTags.Length; i++)
        {
            friendlyBuildings.Add(GameObject.FindGameObjectsWithTag(friendlyBuildingTags[i])); // fill the list with all game objects tagged friendly buildings
        }
    }

}
