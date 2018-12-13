using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

    [Header("Object Constants")]
	public int offsetX = 2;
	public bool hasARightBuddy = false;
	public bool hasALeftBuddy = false;
	public bool reverseScale = false;
	public float spriteWidth = 0f;
	private Camera cam; 
	private Transform myTransform;
	public Vector3 camPos;


	[Header("Temporary Variables")]
	public float tCamHorizantalExtent;
	public float tEdgeVisiblePositionRight;
	public float tEdgeVisiblePositionLeft;
	public Transform tNewBuddy;
	public Vector3 tNewPosition;
	public int tRes = 0;

	void Awake() {
		cam = Camera.main;
		myTransform = transform;
	}

	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> ();
		spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	void Update () {
		camPos = cam.transform.position;
		if(hasALeftBuddy == false || hasARightBuddy == false) {
			tCamHorizantalExtent = cam.orthographicSize * Screen.width / Screen.height;

			tEdgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - tCamHorizantalExtent;
			tEdgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + tCamHorizantalExtent;


			if(cam.transform.position.x >= tEdgeVisiblePositionRight - offsetX && hasARightBuddy == false) {
				makeNewBuddy (1);
				hasARightBuddy = true;

				tRes = 1;
			}

			if(cam.transform.position.x <= tEdgeVisiblePositionLeft + offsetX && hasALeftBuddy == false) {
				makeNewBuddy (-1);
				hasALeftBuddy = true;


                tRes = 2;
			}
		}
	}

	void makeNewBuddy(int rightOrLeft) {
		tNewPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
		tNewBuddy = (Transform)Instantiate (myTransform, tNewPosition, myTransform.rotation);

		if(reverseScale == true) {
			tNewBuddy.localScale = new Vector3 (tNewBuddy.localScale.x * -1, tNewBuddy.localScale.y, tNewBuddy.localScale.z);
		}
		tNewBuddy.parent = myTransform.parent;

		if(rightOrLeft > 0) {
			tNewBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
		}
		else {
			tNewBuddy.GetComponent<Tiling>().hasARightBuddy = true;
		}
	}
}
