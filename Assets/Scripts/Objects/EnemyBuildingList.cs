using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBuildingList : MonoBehaviour
{

    public static EnemyBuildingList enemyBuildingList;

    private List<GameObject> enemyBuildings;
    private string[] enemyBuildingTags = new string[] { "EnemyZone" }; // assigns all the possible friendly building tags to the list

    public List<GameObject> ListOfEnemyBuildings
    {
        get { return enemyBuildings; }
    }


    // Use this for initialization
    public void Awake() // called once before start
    {
        if (enemyBuildingList != null) // singleton format if there is an instance already destroy this object, else this is the instance
        {
            Destroy(this);
        }
        else
        {
            enemyBuildingList = this;
        }
        UpdateBuildingList();
    }

    // Update is called once per frame
    void Start()
    {
        UpdateBuildingList();
    }

    void UpdateBuildingList() // clears list, then updates the building list - call on start, and whe a building is created or destroyed!
    {
        enemyBuildingList.enemyBuildings = new List<GameObject>();
        for (int i = 0; i < enemyBuildingTags.Length; i++) // add buildings with a friendly tag to the list
        {
            enemyBuildings.AddRange(GameObject.FindGameObjectsWithTag(enemyBuildingTags[i])); // fill the list with all game objects tagged friendly buildings

        }
    }




}