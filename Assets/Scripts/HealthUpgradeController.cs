using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgradeController : MonoBehaviour {

    [Header("Factory Element")]
    public static HealthUpgradeController instance;

    public enum upgradeType { newLife = 0, increaseHealth5 = 1, increaseHealth10 = 2 };

    [System.Serializable]
    public class UpgradeOptions
    {
        public upgradeType type;

        public int incrementHealthValue;

        public int incrementLifeValue;

        public int upgradePrice;
    }
    [Header("Object Constants")]
    public UpgradeOptions[] options;
	
	void Start () {
        if (instance != null)
        {
            Debug.Log("More than one Health Upgrade Controller in scene!");
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
    }
}
