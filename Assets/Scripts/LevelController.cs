using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelController : MonoBehaviour {
	
	[Header("Factory Element")]
	public static LevelController instance;

	[Header("Object Constants")]
	public GameObject gmObject;
	private Scene lastScene;
    private int restartCount = 0;

	void Awake () {
		if (instance != null) {
			Debug.Log ("More than one Game Controller in scene!");
			DestroyImmediate (gameObject);
			return;
		}

		initializeObject ();

		instance = this;
		DontDestroyOnLoad (gameObject);
	}

	void initializeObject() {
		SceneManager.sceneLoaded += isSceneLoaded;
        SceneManager.sceneUnloaded += isSceneUnloaded;
        SceneManager.activeSceneChanged += isActiveSceneChanged;
    }

	public void loadLevel(int levelIndex) {
        SceneManager.LoadScene (levelIndex);
	}

	public void restartScene() {

		destroyGM ();
		Time.timeScale = 1.0f;
		int scene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}

	void isSceneLoaded(Scene loadedScene, LoadSceneMode y) {

        if (restartCount == 0)
        {
            GameManager.instance.startGameMenu();
        }
        restartCount++;

        if (loadedScene.name == lastScene.name) {
           // Debug.Log("Different Scene Loaded!");
		}
        //Debug.Log("isSceneLoaded Fired!");
        //lastScene = loadedScene;
	}


    void isSceneUnloaded(Scene unloadedScene)
    {
        //Debug.Log("isSceneUnloaded Fired!");
        lastScene = unloadedScene;
    }

    void isActiveSceneChanged(Scene x, Scene loadedScene) {
        
        //if(lastScene.name != null && loadedScene.name != lastScene.name)
        //{
        //    Debug.Log("Different Scene was Loaded!");
        //}

		if(loadedScene.buildIndex == 0) {
			gmObject = GameObject.Find ("_GM");
			if(gmObject != null) {
				//Debug.Log ("Active Scene Changed");
			}
		}
	}

	void destroyGM() {
		gmObject = GameObject.Find ("_GM");
		if(gmObject != null) {
			Destroy (gmObject);
		}
	}

	void OnDestroy()
    {
        //Debug.Log("Scene Destroyed!" + SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded -= isSceneLoaded;
        SceneManager.sceneUnloaded -= isSceneUnloaded;
        SceneManager.activeSceneChanged -= isActiveSceneChanged;
    }
}
