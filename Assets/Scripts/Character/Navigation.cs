using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {

    // may need agent.UpdateRotation = true; and agent.UpdatePosition = true; but not sure as the agent is attached to the gameobject...

    private Transform target; // create empty transform for the target
    private NavMeshAgent agent; // create empty nav mesh agent

    // Use this for initialization
    void Awake () {
        agent = GetComponent<NavMeshAgent>(); // sets the instance of the navmesh attached to the game object to instance called agent
        //target = transform; // sets the destination to the position of the target transform
    }
	
	// Update is called once per frame
	void Update () {

        if (target != null)
        {
            if (target.position != agent.destination)
            {
                agent.destination = target.position;
            }
        }
    }

    public void UpdateTarget(Transform _target, float _speed)
    {
        agent.speed = _speed;
        target = _target;
    }
    
}
