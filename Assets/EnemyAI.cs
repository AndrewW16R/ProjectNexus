using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public Transform target;

    public float speed = 100f;
    public float nextWaypointDistance = 3f; //How close the enemy needs to be to the waypoint before moving on to the next one

    Path path; //The current path which the enemy is following
    int currentWaypoint = 0; //The current waypoint along the path which is being targetted
    bool reachedEndOfPath = false; //Whether or not the end of the path has been reached

    Seeker seeker;
    Rigidbody2D rb;

    public Transform enemySprite;
    public EnemySight enemySight;

    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        enemySight = GetComponent<EnemySight>();

        InvokeRepeating("UpdatePath", 0f, .5f); //Specified methed to invoke, amount of time to wait before calling method, repeat rate (currently set to every half-second
       
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && enemySight.playerInSight == true) //if a path is not currently being calculated
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete); //Starts calculating path
        }
        
    }

    void OnPathComplete(Path p)
    {
        if(!p.error) //if there are no errors calculating the path
        {
            path = p;
            currentWaypoint = 0; //Start at beginning of newly calculated path
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null) //end update if there is no path to follow
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count) //returns if enemy has reach the end of their path and there are no more waypoints
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized; //Draws direction from position to the next waypoint
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);
      // rb.velocity = force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]); //sets distance float to distance from enemy to their next waypoint

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;//Waypoint has been reached, time to move to next waypoint
        }


        if (force.x >= 0.001f || force.x <= -0.001f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        /*
        if (rb.velocity.x >= 0.01f) //Flips enemy sprite based on movement as needed
        {
            enemySprite.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemySprite.localScale = new Vector3(1f, 1f, 1f);
        }
        */
    }
}
