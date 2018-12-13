using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public enum Guns { Pistol = 0, Shotgun = 1, AK47 = 2, M16  = 3, P90 = 4, Famas = 5 };
    [Header("Factory Element")]
    public static WeaponController instance;

    [Header("Prefabs")]
    public GameObject[] gunPrefabs;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Weapon Controller in scene!");
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
