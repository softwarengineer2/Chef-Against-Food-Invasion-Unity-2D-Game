using UnityEngine;

public class GunsUpgradeController : MonoBehaviour {

    [Header("Factory Element")]
    public static GunsUpgradeController instance;

    public enum upgradeType { ammoCapasity2 = 0, ammoCapasity4 = 1, muzzleVelocity = 2 };

    [System.Serializable]
    public class UpgradeOptions
    {
        public upgradeType type;

        public int incrementAmmoCapasity;

        public int incrementMuzzleVelocity;

        public int upgradePrice;
    }

    [Header("Object Constants")]
    public UpgradeOptions[] options;

    void Start()
    {
        if (instance != null)
        {
            Debug.Log("More than one Guns Upgrade Controller in scene!");
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
    }
}
