using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using UnityStandardAssets._2D;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("Factory Element")]
    public static GameManager instance;

    [Header("GUI Elements")]
    public Text elapsedLivesText;
    public Text totalPointsText;
    public MenuController menuController;
    public Slider mainVolumeChange;
    public Slider otherVolumeChange;
    public Text newGunAlertText;
    public Text gunUpgradeAlertText;
    public Text healthUpgradeAlertText;
    public Text levelText;
    public Image gunInfoImage;
    public RectTransform rootCanvas;
    public RectTransform shootingArea;
    public GameObject[] joysticks;
    public Dropdown chooseJoystick;

    [Header("Object Constants")]
    public bool isDead = false;
    public bool isPaused = false;
    public Transform spawnPoint;
    public float spawnDelay = 1.7f;
    private GameObject mainCamera;
    private Camera2DFollow cameraScript;
    public int totalPoints = 0;
    public int totalLives = 3;
    public int elapsedLives = 0;
    public float currentTimeScale;
    private AudioSource audioSource;
    public int bonusAmmoCapasity;
    public int bonusMuzzleVelocity;

    [Header("Prefabs")]
    public GameObject prefabPlayer;
    public GameObject prefabRespawnEffect;
    public GameObject[] enemyPrefabs;

    [Header("Temporary Variables")]
    //public AudioSource tCompAudioSource;
    public float tTime;
    public GameObject tNewPlayer;
    private GameObject tRespawnParticle;
    private GameObject tWeapon;
    private HealthUpgradeController.UpgradeOptions tHealthUpgrade;
    private GunsUpgradeController.UpgradeOptions tGunsUpgrade;
    [SerializeField]
    public Rect tShootArea;
    public WeaponController.Guns lastActiveGun;
    public int joystickType;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Game Controller in scene!");
            DestroyImmediate(gameObject);
            return;
        }

        initializeObject();

        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    void initializeObject()
    {

        totalPoints = 100;

        bonusAmmoCapasity = 0;

        bonusMuzzleVelocity = 0;

        isPaused = false;

        isDead = false;

        audioSource = GetComponent<AudioSource>();

        currentTimeScale = 1.0f;

        mainCamera = GameObject.FindWithTag("MainCamera");

        cameraScript = mainCamera.GetComponentInParent<Camera2DFollow>();

        lastActiveGun = WeaponController.Guns.Pistol;

        elapsedLives = totalLives;

        changeUserInfo();

        changeActiveGunSprite();
    }

    public void initializeMusicSource()
    {

        if (MusicManager.instance != null)
        {
            mainVolumeChange.onValueChanged.AddListener(MusicManager.instance.changeMainVolume);
            mainVolumeChange.value = MusicManager.instance.GetComponent<AudioSource>().volume;

            otherVolumeChange.onValueChanged.AddListener(MusicManager.instance.changeOtherVolume);
            otherVolumeChange.value = audioSource.volume;
        }
    }

    public void activateJoystick(int joystickID)
    {
        if(joystickID != joystickType) {

            joysticks[joystickType].SetActive(false);
            joysticks[joystickID].SetActive(true);

            joystickType = joystickID;
            chooseJoystick.value = joystickID;
            /*for (int i = 0; i < joysticks.Length; i++)
            {
                if (joystickID == i)
                {
                    joysticks[i].SetActive(true);
                }
                else
                {
                    joysticks[i].SetActive(false);
                }
            }*/
        }

        if (joystickType == 0)
        {
            tNewPlayer.GetComponent<PlatformerCharacter2D>().m_JumpForce = 190;
        }
        else if (joystickType == 1)
        {
            tNewPlayer.GetComponent<PlatformerCharacter2D>().m_JumpForce = 800;
        }
    }

    public void killPlayer()
    {

        tNewPlayer = null;

        elapsedLives -= 1;

        changeUserInfo();

        if (elapsedLives <= 0)
        {
            gameOver();
        }
        else
        {
            StartCoroutine(respawnPlayer());
        }
    }

    public bool isInShootingArea(Vector3 position)
    {
        tShootArea = shootingArea.rect;

        tShootArea.width = Screen.width;

        tShootArea.height = (Screen.height / 600f) * tShootArea.height;

        tShootArea.position = shootingArea.position;

        if (tShootArea.Contains(position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void gameOver()
    {
        isDead = true;
        isPaused = true;
        menuController.gameOverPanelOpen();
        //Debug.Log("Game is Over!");
    }

    void pauseGame()
    {
        menuController.pausePanelOpen();
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void startGameMenu()
    {
        menuController.startPanelOpen();
        Time.timeScale = 0f;
        isPaused = true;
    }

    void restartGame()
    {
        menuController.pausePanelClose();
        Time.timeScale = currentTimeScale;
        isDead = false;
        isPaused = false;
    }
    public void resumeGame()
    {
        menuController.pausePanelClose();
        Time.timeScale = currentTimeScale;
        isPaused = false;
    }

    public void changeActiveGunSprite()
    {
        gunInfoImage.sprite = WeaponController.instance.gunPrefabs[(int)lastActiveGun].GetComponent<SpriteRenderer>().sprite;
    }

    public void changeUserInfo()
    {
        elapsedLivesText.text = "LIVES   : " + elapsedLives.ToString();
        totalPointsText.text = "POINTS : " + totalPoints.ToString();
    }

    public void changeLevelInfo(int level)
    {
        levelText.text = "LEVEL  : " + level.ToString();
    }

    IEnumerator respawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);

        tNewPlayer = (GameObject)Instantiate(prefabPlayer, spawnPoint.position, spawnPoint.rotation);
        tRespawnParticle = (GameObject)Instantiate(prefabRespawnEffect, spawnPoint.position, spawnPoint.rotation);

        cameraScript.target = tNewPlayer.transform;

        Destroy(tRespawnParticle.gameObject, 1f);

        changeActiveGunSprite();

        isDead = false;

        isPaused = false;

        if (joystickType == 0)
        {
            tNewPlayer.GetComponent<PlatformerCharacter2D>().m_JumpForce = 190;
        }
        else if (joystickType == 1)
        {
            tNewPlayer.GetComponent<PlatformerCharacter2D>().m_JumpForce = 800;
        }
    }

    public void playAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void upgradeHealth(HealthUpgradeController.upgradeType upgradeType)
    {
        tHealthUpgrade = HealthUpgradeController.instance.options[(int)upgradeType];

        if (totalPoints >= tHealthUpgrade.upgradePrice)
        {
            PlayerStats.instance.maxHealth += tHealthUpgrade.incrementHealthValue;

            totalLives += tHealthUpgrade.incrementLifeValue;

            elapsedLives += tHealthUpgrade.incrementLifeValue;

            totalPoints -= tHealthUpgrade.upgradePrice;

            gunUpgradeAlertText.text = "Upgrade Completed!";

            changeUserInfo();

        }
        else
        {
            healthUpgradeAlertText.text = "Insufficient Funds!";
        }
    }

    public void changeWeapon(WeaponController.Guns newGun)
    {
        tWeapon = WeaponController.instance.gunPrefabs[(int)newGun];

        if (totalPoints >= tWeapon.GetComponent<Weapon>().priceOfWeapon)
        {

            if (tNewPlayer.GetComponent<Player>().activeGun != newGun)
            {
                lastActiveGun = newGun;

                tNewPlayer.GetComponent<Player>().changeWeapon(newGun);

                changeActiveGunSprite();

                changeUserInfo();

                totalPoints -= tWeapon.GetComponent<Weapon>().priceOfWeapon;


                gunUpgradeAlertText.text = "Your New Gun Set!";
            }
            else
            {
                newGunAlertText.text = "It's your active gun!";
            }
        }
        else
        {
            newGunAlertText.text = "Insufficient Funds!";
        }

    }


    public void upgradeGuns(GunsUpgradeController.upgradeType upgradeType)
    {
        tGunsUpgrade = GunsUpgradeController.instance.options[(int)upgradeType];

        if (totalPoints >= tGunsUpgrade.upgradePrice)
        {
            bonusAmmoCapasity += tGunsUpgrade.incrementAmmoCapasity;

            bonusMuzzleVelocity += tGunsUpgrade.incrementMuzzleVelocity;

            gunUpgradeAlertText.text = "Upgrade Completed!";

        }
        else
        {
            gunUpgradeAlertText.text = "Insufficient Funds!";
        }
    }

    void Update()
    {
        if (isDead == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused == false)
                {
                    pauseGame();
                }
                else
                {
                    resumeGame();
                }
            }
        }
    }
}
