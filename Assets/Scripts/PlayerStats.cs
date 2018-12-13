using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Factory Element")]
    public static PlayerStats instance;

    [Header("Object Constants")]
    public float regenRate = 3.0f;
    public int _curHealth = 100;
    private int _maxHealth = 100;

    public int curHealth
    {
        get
        {
            return _curHealth;
        }
        set
        {
            _curHealth = (int)Mathf.Clamp(value, 0f, maxHealth);
        }
    }

    public int maxHealth
    {
        get
        {
            return _maxHealth;
        }

        set
        {
            _maxHealth = value;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Player Stats Controller in scene!");
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
    }
}
