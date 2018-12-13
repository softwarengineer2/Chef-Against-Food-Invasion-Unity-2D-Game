
using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

    [Header("Object Constants")]
	public Transform[] backgrounds;
	private float[] parallaxScales; 
	public float smoothing = 1f; 
	private Transform cam;
	private Vector3 previousCamPos;

    [Header("Temporary Variables")]
    private float tParallax;
    private float tBackgroundTargetPosX;
    private Vector3 tBackgroundTargetPos;

    void Awake() {
		cam = Camera.main.transform;
	}
	
	void Start () {
		previousCamPos = cam.position;
		parallaxScales = new float[backgrounds.Length];

		for(int i=0;i<backgrounds.Length;i++) {
			parallaxScales [i] = backgrounds [i].position.z * -1;
		}
	}
	
	void Update () {
		for(int i=0;i<backgrounds.Length;i++) {
			tParallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
			tBackgroundTargetPosX = backgrounds [i].position.x + tParallax;
			tBackgroundTargetPos = new Vector3 (tBackgroundTargetPosX, backgrounds [i].position.y, backgrounds [i].position.z);
			backgrounds [i].position = Vector3.Lerp (backgrounds [i].position, tBackgroundTargetPos, smoothing * Time.deltaTime);
		}
		previousCamPos = cam.position;
	}
}