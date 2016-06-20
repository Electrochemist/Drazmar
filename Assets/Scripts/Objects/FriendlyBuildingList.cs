using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendlyBuildingList : MonoBehaviour
{

    public static FriendlyBuildingList friendlyBuildingList;

    private List<GameObject> friendlyBuildings;
    private string[] friendlyBuildingTags = new string[] { "FriendZone" }; // assigns all the possible friendly building tags to the list
    
    public List<GameObject> ListOfFriendlyBuildings
    {
        get { return friendlyBuildings; }
    }
        

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
        UpdateBuildingList();
    }

    // Update is called once per frame
    void Start ()
    {
        UpdateBuildingList();
    }

    void UpdateBuildingList() // clears list, then updates the building list - call on start, and whe a building is created or destroyed!
    {
        friendlyBuildingList.friendlyBuildings = new List<GameObject>();
        for (int i = 0; i < friendlyBuildingTags.Length; i++) // add buildings with a friendly tag to the list
        {
            friendlyBuildings.AddRange(GameObject.FindGameObjectsWithTag(friendlyBuildingTags[i])); // fill the list with all game objects tagged friendly buildings

        }
    }


    

}
