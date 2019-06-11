using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }


    public int currentLevel = 0;  //used when changing from menu to gme scene
    public int menuFocus = 0;     //used when entering the menu scene, to know which menu focus
}
