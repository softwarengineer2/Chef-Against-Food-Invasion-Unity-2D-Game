using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {

    [Header("GUI Elements")]
    public Text ammoText;

    [Header("Object Constants")]
    private Transform firePoint;
    public WeaponController.Guns gunType;
    public int priceOfWeapon;
    public float ammoCapasity = 14;
    public float leftAmmo;
    public float fireRate = 0;
	public int damage = 10;
    public LayerMask whatToHit;
    public bool isChangingAmmo;
    private float timeToFire = 0;
    private float timeToSpawnEffect = 0f;
	public float effectSpawnRate = 10f;
    public float camShakeAmount = 0.03f;
    public float camShakeLength = 0.06f;
    CameraShake camShake;

    [Header("Prefabs")]
	public Transform bulletTrailPrefab;
	public Transform muzzlleFlashPrefab;
	public AudioClip fireSound;
    public AudioClip changeMagazineSound;

	[Header("Temporary Variables")]
	private float tSize;
	private Transform tBullet;
	private Transform tClone;
	private Vector3 tHitPos;
	private Vector2 tMousePosition;
	private Vector2 tFirePointPosition;
	private RaycastHit2D tHit;
	private Vector3 tHitNormal;
	private Enemy tEnemy;
	private LineRenderer tLr;
    private int tPhaseCount = 0;
    public Vector3 tShootPosition;
    public Vector3 tClickPos;

    void Awake() {
		firePoint = transform.GetChild (0);
		if(firePoint == null) {
			Debug.LogError ("There is no firepoint of this weapon!");
		}
    }

    void initializeObject()
    {
        isChangingAmmo = false;

        leftAmmo = ammoCapasity + GameManager.instance.bonusAmmoCapasity;

        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();

        ammoText.text = "AMMO  : " + leftAmmo.ToString();
    }

    void Start () {
		camShake = GameManager.instance.GetComponent<CameraShake>();

        initializeObject();
    }

	void Update () {
        if(GameManager.instance.isPaused != true && isChangingAmmo != true) {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (GameManager.instance.isInShootingArea(touch.position))
                    {
                        if (fireRate == 0)
                        {
                            if (touch.phase == TouchPhase.Began && tPhaseCount == 0) {
                                Shoot();
                                tPhaseCount = 1;
                            }
                            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                            {
                                tPhaseCount = 0;
                            }

                        }
                        else { 
                            if (Time.time > timeToFire) { 
                                timeToFire = Time.time + 1 / (fireRate + GameManager.instance.bonusMuzzleVelocity);
                                Shoot();
                            }
                        }
                        tShootPosition = touch.position;
                    } 
                }
            }
            else { 
                if (fireRate == 0) {
			        if(Input.GetButtonDown ("Fire1")) {

                        tClickPos = Input.mousePosition;

                        if (GameManager.instance.isInShootingArea(Input.mousePosition)) { 
				            Shoot ();

                            tShootPosition = Input.mousePosition;
                        }
                    }
		        }
		        else {
			        if(Input.GetButton ("Fire1") && Time.time > timeToFire) {
                        if (GameManager.instance.isInShootingArea(Input.mousePosition))
                        {
                            timeToFire = Time.time + 1 / (fireRate + GameManager.instance.bonusMuzzleVelocity);
                            Shoot();

                            tShootPosition = Input.mousePosition;
                        }
			        }
		        }
            }
        }
    }

	void Shoot() {
		tMousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (tShootPosition).x, Camera.main.ScreenToWorldPoint (tShootPosition).y);
		tFirePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		tHit = Physics2D.Raycast(firePoint.position, tMousePosition - tFirePointPosition, 100, whatToHit);


		if(Time.time > timeToSpawnEffect) {
			
			if(tHit.collider == null) {
				tHitPos = (tMousePosition - tFirePointPosition) * 30;
				tHitNormal = new Vector3 (9999, 9999, 9999);
			}
			else{
				tHitPos = tHit.point;
				tHitNormal = tHit.normal;
			}

			GameManager.instance.playAudioClip (fireSound);

			BulletEffect (tHitPos, tHitNormal);
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}
        
		if(tHit.collider != null) {
			if(tHit.collider.tag == "Enemy") {
				tEnemy = tHit.collider.GetComponent<Enemy> ();
				tEnemy.damageEnemy (damage);
			}
		}
	}

	void BulletEffect (Vector3 hitPos, Vector3 hitNormal) {
		tBullet = (Transform)Instantiate (bulletTrailPrefab, firePoint.position, firePoint.rotation);
		tLr = tBullet.GetComponent<LineRenderer> ();
		if(tLr != null) {
			tLr.SetPosition (0, firePoint.position);
			tLr.SetPosition (1, hitPos);
		}

		Destroy (tBullet.gameObject, 0.04f);

        leftAmmo -= 1;

        if(leftAmmo == 0)
        {
            isChangingAmmo = true;

            StartCoroutine(changeMagazine());
        }

        if (ammoText != null)
        {
            ammoText.text = "AMMO  : "+leftAmmo.ToString();
        }

        if (hitNormal == new Vector3(9999,9999,9999)) {
			
		}
		
		tClone = (Transform)Instantiate (muzzlleFlashPrefab, firePoint.position, firePoint.rotation);
		tClone.parent = firePoint;
		tSize = Random.Range (0.08f, 0.15f);
		tClone.localScale = new Vector3 (tSize, tSize, tSize);
		Destroy(tClone.gameObject, 0.2f);

		camShake.Shake (camShakeAmount, camShakeLength);
	}

    public IEnumerator changeMagazine()
    {

        GameManager.instance.playAudioClip(changeMagazineSound);

        yield return new WaitForSeconds(changeMagazineSound.length);

        isChangingAmmo = false;

        leftAmmo = ammoCapasity + GameManager.instance.bonusAmmoCapasity;

        ammoText.text = "AMMO  : " + leftAmmo.ToString();
    }
}
