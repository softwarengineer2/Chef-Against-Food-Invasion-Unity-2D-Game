using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {

	[Header("Factory Element")]
	public static MusicManager instance;

	[Header("Object Constants")]
	private AudioSource gameMusic;
    private AudioSource otherMusic;

    void Awake () {

		if (instance != null) {
			Debug.Log ("More than one Music Manager in scene!");
			DestroyImmediate (gameObject);
			return;
		}

        instance = this;
		DontDestroyOnLoad (gameObject);
        initializeObject();
    }

    void initializeObject()
    {
        gameMusic = GetComponent<AudioSource>();
        otherMusic = GameManager.instance.GetComponent<AudioSource>();

        GameManager.instance.initializeMusicSource();
    }

	public void changeMainVolume(float newValue) {
		gameMusic.volume = newValue;
	}

    public void changeOtherVolume(float newValue)
    {
        GameManager.instance.GetComponent<AudioSource>().volume = newValue;
    }
}
