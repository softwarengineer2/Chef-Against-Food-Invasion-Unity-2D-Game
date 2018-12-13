using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	[Header("Object Constants")]
	public GameObject mainPanel;
    public GameObject upgradePanel;
    public GameObject optionsPanel;
	public GameObject resumeButton;
    public GameObject startButton;
    public GameObject upgradeButton;
    public Text menuMainTitle;
	public GameObject mainMenu;

    [Header("Additional Upgrade Panel Variables")]
    public GameObject newGunPanel;
    public GameObject healthUpgradePanel;
    public GameObject gunsUpradePanel;
    public Text newGunAlertText;
    public Text gunUpgradeAlertText;
    public Text healthUpgradeAlertText;
    public void startPanelOpen()
    {
        mainMenu.SetActive(true);
        optionsPanel.SetActive(false);
        mainPanel.SetActive(true);
        resumeButton.SetActive(false);
        startButton.SetActive(true);
        upgradeButton.SetActive(false);

        upgradePanel.SetActive(false);
        newGunPanel.SetActive(false);
        healthUpgradePanel.SetActive(false);
        gunsUpradePanel.SetActive(false);

        menuMainTitle.text = "CHEF AGAINST FOOD INVASION";
    }

    public void pausePanelOpen() {
		mainMenu.SetActive (true);
		optionsPanel.SetActive (false);
		mainPanel.SetActive (true);
		resumeButton.SetActive (true);
        startButton.SetActive(false);
        upgradeButton.SetActive(true);

        upgradePanel.SetActive(false);
        newGunPanel.SetActive(false);
        healthUpgradePanel.SetActive(false);
        gunsUpradePanel.SetActive(false);

        menuMainTitle.text = "PAUSED";
    }

	public void pausePanelClose() {
		mainMenu.SetActive (false);
	}

	public void gameOverPanelOpen() {
		mainMenu.SetActive (true);
		optionsPanel.SetActive (false);
		mainPanel.SetActive (true);
		resumeButton.SetActive (false);
        startButton.SetActive(false);
        upgradeButton.SetActive(false);
        upgradePanel.SetActive(false);
        menuMainTitle.text = "GAME OVER";
    }

	public void menuButtonClick(string pButtonName) {

		if(pButtonName == "back") {
			optionsPanel.SetActive (false);
            upgradePanel.SetActive(false);
            mainPanel.SetActive (true);
        }
        else if (pButtonName == "upgrade")
        {
            upgradePanel.SetActive(true);
            mainPanel.SetActive(false);
        }
        else if(pButtonName == "options") {
			optionsPanel.SetActive (true);
			mainPanel.SetActive (false);
		}
		else if(pButtonName == "exit") {
			Application.Quit ();
		}
		else if(pButtonName == "start") {
			LevelController.instance.loadLevel(1);
		}
		else if(pButtonName == "resume") {
			GameManager.instance.resumeGame ();
		}
		else if(pButtonName == "restart") {
			LevelController.instance.restartScene();
		}
        else if (pButtonName == "returngame")
        {
            newGunPanel.SetActive(false);
            healthUpgradePanel.SetActive(false);
            gunsUpradePanel.SetActive(false);
            upgradePanel.SetActive(false);
            optionsPanel.SetActive(false);
            mainPanel.SetActive(false);

            GameManager.instance.resumeGame();
        }
        else {
            checkAdditionalButtonClick(pButtonName);
        }
    }

    void checkAdditionalButtonClick(string pButtonName)
    {

        newGunAlertText.text = "";
        healthUpgradeAlertText.text = "";
        gunUpgradeAlertText.text = "";

        if (pButtonName == "newgun")
        {
            newGunPanel.SetActive(true);
            upgradePanel.SetActive(false);
            optionsPanel.SetActive(false);
            mainPanel.SetActive(false);
        }
        else if (pButtonName == "healthupgrade")
        {
            healthUpgradePanel.SetActive(true);
            upgradePanel.SetActive(false);
            optionsPanel.SetActive(false);
            mainPanel.SetActive(false);
        }
        else if (pButtonName == "gunsupgrade")
        {
            gunsUpradePanel.SetActive(true);
            upgradePanel.SetActive(false);
            optionsPanel.SetActive(false);
            mainPanel.SetActive(false);
        }
        else if (pButtonName == "backtoupgradepanel")
        {
            newGunPanel.SetActive(false);
            upgradePanel.SetActive(true);
            healthUpgradePanel.SetActive(false);
            gunsUpradePanel.SetActive(false);
            optionsPanel.SetActive(false);
            mainPanel.SetActive(false);
        }
    }

    public void healthControl(string pButtonName)
    {
        if (pButtonName == "increasehealth5")
        {
            GameManager.instance.upgradeHealth(HealthUpgradeController.upgradeType.increaseHealth5);
        }
        else if (pButtonName == "increasehealth10")
        {
            GameManager.instance.upgradeHealth(HealthUpgradeController.upgradeType.increaseHealth10);
        }
        else if (pButtonName == "newlife")
        {
            GameManager.instance.upgradeHealth(HealthUpgradeController.upgradeType.newLife);
        }
    }

    public void gunControl(string pButtonName)
    {
        if(pButtonName == "pistol")
        {
            GameManager.instance.changeWeapon(WeaponController.Guns.Pistol);
        }
        else if (pButtonName == "m16")
        {
            GameManager.instance.changeWeapon(WeaponController.Guns.M16);
        }
        else if (pButtonName == "famas")
        {
            GameManager.instance.changeWeapon(WeaponController.Guns.Famas);
        }
        else if (pButtonName == "shotgun")
        {
            GameManager.instance.changeWeapon(WeaponController.Guns.Shotgun);
        }
        else if (pButtonName == "p90")
        {
            GameManager.instance.changeWeapon(WeaponController.Guns.P90);
        }
        else if (pButtonName == "ak47")
        {
            GameManager.instance.changeWeapon(WeaponController.Guns.AK47);
        }
    }

    public void gunUpgradeControl(string pButtonName)
    {
        if (pButtonName == "ammocapasity2")
        {
            GameManager.instance.upgradeGuns(GunsUpgradeController.upgradeType.ammoCapasity2);
        }
        else if (pButtonName == "ammocapasity4")
        {
            GameManager.instance.upgradeGuns(GunsUpgradeController.upgradeType.ammoCapasity4);
        }
        else if (pButtonName == "muzzlevelocity")
        {
            GameManager.instance.upgradeGuns(GunsUpgradeController.upgradeType.muzzleVelocity);
        }
    }

    public void chooseJoystick(int joystickType)
    {
        GameManager.instance.activateJoystick(joystickType);
    }
}
