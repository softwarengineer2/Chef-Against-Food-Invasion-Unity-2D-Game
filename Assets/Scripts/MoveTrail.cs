using UnityEngine;
using System.Collections;

public class MoveTrail : MonoBehaviour {

    [Header("Object Constants")]
	public int moveSpeed = 230;

	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
		Destroy (gameObject, 1f);
	}
}
