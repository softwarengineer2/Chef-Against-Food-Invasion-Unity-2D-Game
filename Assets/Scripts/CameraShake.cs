using UnityEngine;

public class CameraShake : MonoBehaviour {

    [Header("Object Constants")]
    public Camera mainCam;
	float shakeAmount = 0;

    [Header("Temporary Variables")]
    Vector3 tCamPos;
    float tOffsetX;
    float tOffsetY;


    void Awake () {
		if(mainCam == null) {
			mainCam = Camera.main;
		}
	}

	void Update() {
		if(Input.GetKeyDown (KeyCode.T)) {
			Shake (0.1f, 0.2f);
		}
	}


	public void Shake(float amt, float length) {
		shakeAmount = amt;
		InvokeRepeating ("BeginShake", amt, length);
		Invoke ("StopShake", length);
	}

	void BeginShake() {
		if(shakeAmount > 0) {
			tCamPos = mainCam.transform.position;

			tOffsetX = Random.value * shakeAmount * 2 - shakeAmount;
			tOffsetY = Random.value * shakeAmount * 2 - shakeAmount;

            tCamPos.x += tOffsetX;
            tCamPos.y += tOffsetY;

			mainCam.transform.position = tCamPos;
		}
	}

	void StopShake() {
		CancelInvoke ("BeginShake");	
		mainCam.transform.localPosition = Vector3.zero;
	}

}
	
