using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour {

	[Header("Object Constants")]
	public Transform target;
	public float updateRate = 2f;
	private Seeker seeker;  //Caching
    private Rigidbody2D rb;
	public Path path;  //Calculated path   
    public float speed = 300f; //AI Speed and Force
    public ForceMode2D fMode;

	[HideInInspector]
	public bool pathIsEnabled = false;
	private bool pathIsEnded = false;

	public float nextWaypointDistance = 3; //The Max Distance AI to Waypoint for it to continue to the next waypoint
    private int currentWaypoint = 0; //The waypoint we are currently moving toward

    [Header("Temporary Variables")]
    private Vector3 tDir;
	private float tDistance;

	void Start () {
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();

		if(target == null){
			findPlayer ();
			//Debug.LogError ("No Player Found");
			return;
		}

		seeker.StartPath (transform.position, target.position, OnPathComplete);

		StartCoroutine (UpdatePath());
 	}

	void findPlayer() {
		if(GameManager.instance.tNewPlayer != null) {
			target = GameManager.instance.tNewPlayer.transform;

			StartCoroutine (UpdatePath());
		}
	}

	public void OnPathComplete(Path p) {
		if(!p.error){
			path = p;
			currentWaypoint = 0;
		}
		else {
			//Debug.LogError ("Error : "+p.error);
		}
	}

	IEnumerator UpdatePath() {
		if(target == null) {
			findPlayer ();
		}
        else { 
		    seeker.StartPath (transform.position, target.position, OnPathComplete);
		    yield return new WaitForSeconds (1f / updateRate);
		    StartCoroutine (UpdatePath ());
        }
    }

	void FixedUpdate() {
		if(target == null) {
			findPlayer ();
			return;
		}

		if(path == null) {
			return;
		}

		if(currentWaypoint >= path.vectorPath.Count) {
			if(pathIsEnded) {
				return;
			}
			//Debug.LogError ("End of path reached");
			pathIsEnded = true;
			return;
		}
		pathIsEnded = false;

		//Direction
		tDir = (path.vectorPath [currentWaypoint] - transform.position).normalized;

		tDir *= speed * Time.fixedDeltaTime;

		//Move to AI
		rb.AddForce (tDir, fMode);
		tDistance = Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]);
		if(tDistance < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}	
	}

}
