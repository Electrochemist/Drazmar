using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {

    // may need agent.UpdateRotation = true; and agent.UpdatePosition = true; but not sure as the agent is attached to the gameobject...

    private Transform target; // create empty transform for the target
    public Transform Target
    {
        get { return target; }        
    }

    private NavMeshAgent agent; // create empty nav mesh agent

    private bool tracking; // checks if the unit is tracking the target or not (i.e. moving towards and enemy that it can see is true, building or last known position false)
    public bool Tracking
    {
        get { return tracking; }
        set { tracking = value; }
    }

    // Use this for initialization
    void Awake () {
        agent = GetComponent<NavMeshAgent>(); // sets the instance of the navmesh attached to the game object to instance called agent
        //target = transform; // sets the destination to the position of the target transform
    }
	
	// Update is called once per frame
	void Update () {

        if (tracking && target != null) // if we are tracking the target
        {
            if (target.position != agent.destination) // and the target has moved position
            {
                agent.destination = target.position; // update the target position
            }
        }
    }

    public void UpdateTarget(Transform _target, float _speed, bool _tracking) // new target location, movement speed, and if we need to track the location
    {
        agent.speed = _speed;
        target = _target;
        tracking = _tracking;

        if (agent.isActiveAndEnabled)
        {
            agent.destination = target.position; // update the target location for the unit
        }
        
    }

    public float ProximityToTargetSquare() // returns the distance squared to the navigation target
    {
       
            Vector3 distance = agent.destination - transform.position; // call the destination rather than the transform that may or may not exist!
            return distance.sqrMagnitude; // returns the square magnitude of the distance vector
        
    }
}
