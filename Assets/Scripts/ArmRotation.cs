using UnityEngine;

public class ArmRotation : MonoBehaviour {

    [Header("Object Constants")]
    public int rotationOffset = 0;

	[Header("Temp Variables")]
	private float tRotZ;
	private Vector3 tDifference;
    public float tRotationzEuler;

    void Update () {
        if(GameManager.instance.isPaused != true) {

            if(transform.GetChild(0).gameObject != null) { 
                
                tDifference = Camera.main.ScreenToWorldPoint (transform.GetChild(0).gameObject.GetComponent<Weapon>().tShootPosition) - transform.position;
		        tDifference.Normalize ();

		        tRotZ = Mathf.Atan2 (tDifference.y, tDifference.x) * Mathf.Rad2Deg;

                tRotationzEuler = tRotZ + rotationOffset;

                transform.rotation = Quaternion.Euler (0f, 0f, tRotationzEuler);
            }
        }
    }
}
