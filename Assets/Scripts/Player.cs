using UnityEngine;
public class Player : MonoBehaviour {

    [Header("Object Constants")]
    public PlayerStats playerStats;
	public int fallBoundary = -4;
	private bool isDying;
	public AudioClip deathSound;
    public WeaponController.Guns activeGun;
    public GameObject playerArm;

    [Header("Temporary Variables")]
    private GameObject tObject;
    private Vector3 tWeaponPoint;

    [Header("Optional Variables")]
	[SerializeField]
	private StatusIndicator statusIndicator;

	void Start () {
        initializeObject();

        if (statusIndicator == null) {
			Debug.LogError ("No Status Indicator on Player");
		}
		else {
			statusIndicator.SetHealth (playerStats.curHealth, playerStats.maxHealth);
		}

        InvokeRepeating("regeneradeHealth", 1f / playerStats.regenRate, 1f / playerStats.regenRate);
	}

    void initializeObject()
    {
        isDying = false;

        playerStats = PlayerStats.instance;

        playerStats.curHealth = playerStats.maxHealth;

        playerArm = transform.Find("PlayerArm").gameObject;

        activeGun = GameManager.instance.lastActiveGun;

        if(activeGun != playerArm.transform.GetChild(0).gameObject.GetComponent<Weapon>().gunType)
        {
            changeWeapon(activeGun);
        }

    }

    public void damagePlayer(int damageValue) {
		playerStats.curHealth -= damageValue;

		statusIndicator.SetHealth (playerStats.curHealth, playerStats.maxHealth);

		if(playerStats.curHealth <= 0) {
			killPlayer ();
			Destroy (gameObject);
		}
	}

    public void changeWeapon(WeaponController.Guns newGun)
    {
        if (playerArm.transform.childCount > 0) {
            Destroy(playerArm.transform.GetChild(0).gameObject);
        }

        tWeaponPoint = playerArm.transform.position;
        tWeaponPoint.x += 0.094f;
        tWeaponPoint.y -= 0.063f;

        tObject = (GameObject)Instantiate(WeaponController.instance.gunPrefabs[(int)newGun], tWeaponPoint, WeaponController.instance.gunPrefabs[(int)newGun].transform.rotation);
        tObject.transform.parent = playerArm.transform;
        tObject.transform.localRotation = Quaternion.Euler(0f,0f,-90f);

        activeGun = newGun;
    }

	public void killPlayer() {
		GameManager.instance.playAudioClip (deathSound);
		GameManager.instance.killPlayer ();
	}

	void regeneradeHealth() {
        if(playerStats.curHealth < playerStats.maxHealth) { 
            playerStats.curHealth += 1;
            statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
        }
    }

	void Update () {

        if (Input.GetKeyDown(KeyCode.R) && isDying == false)
        {
            StartCoroutine(playerArm.transform.GetChild(0).gameObject.GetComponent<Weapon>().changeMagazine());
        }

        if (transform.position.y <= fallBoundary && isDying == false) {
			isDying = true;
			damagePlayer (999);
        }
	}
}
