using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    private float fadeInDuration = 2;
    private bool gameStarted;

    private void Start()
    {
        //get the only canvasGroup in the scene
        fadeGroup = FindObjectOfType<CanvasGroup>();

        //set the fade to full opacity
        fadeGroup.alpha = 1;
    }

    private void Update()
    {
        if(Time.timeSinceLevelLoad <= fadeInDuration)
        {
            //initial fade-in
            fadeGroup.alpha = 1 - (Time.timeSinceLevelLoad / fadeInDuration);
        }
        // if initial time fade-in is completed, and the game has not been started yet
        else if (!gameStarted)
        {
            //ensure the fade is completely gone
            fadeGroup.alpha = 0;
            gameStarted = true;
        }
    }

    public void CompleteLevel()
    {
        //complete the level and save the progress
        SaveManager.Instance.CompleteLevel(Manager.Instance.currentLevel);

        // focus the level selection when we return to the menu scene
        Manager.Instance.menuFocus = 1;

        ExitScene();
    }

    public void ExitScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
