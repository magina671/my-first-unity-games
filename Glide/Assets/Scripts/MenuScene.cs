using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    private float fadeInSpeed = 0.33f;

    public RectTransform menuContainer;
    public Transform levelPanel;
    public Transform colorPanel;
    public Transform trailPanel;

    public Text colorBuySetText;
    public Text trailBuySetText;
    public Text goldText;

    private int[] colorCost = new int[] { 0, 5, 5, 5, 10, 10, 10, 15, 15, 15 };
    private int[] trailCost = new int[] { 0, 20, 40, 40, 60, 60, 80, 80, 100, 100 };
    private int selectedColorIndex;
    private int selectedTrailIndex;
    private int activeColorIndex;
    private int activeTrailIndex;

    private Vector3 desiredMenuPosition;

    public AnimationCurve enteringLevelZoomCurve;
    private bool isEnteringLevel = false;
    private float zoomDuration = 3.0f;
    private float zoomTransition;

    private void Start()
    {
        //$$ temporary
        SaveManager.Instance.state.gold = 999;

        //position our camera on the focused menu
        SetCameraTo(Manager.Instance.menuFocus);

        //tell our gold text how much he should displaying
        UpdateGoldText();

        //Grab the only CanvasGroup in the scene
        fadeGroup = FindObjectOfType<CanvasGroup>();

        //Start with  white screen
        fadeGroup.alpha = 1;

        //Add button on-click events to shop buttons
        InitShop();

        //add buttons on-click events to levels
        InitLevel();

        //set players preferences (color & trail)
        OnColorSelect(SaveManager.Instance.state.activeColor);
        SetColor(SaveManager.Instance.state.activeColor);

        OnTrailSelect(SaveManager.Instance.state.activeTrail);
        SetTrail(SaveManager.Instance.state.activeTrail);

        //make the buttons bigger for the selcted items
        colorPanel.GetChild(SaveManager.Instance.state.activeColor).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        trailPanel.GetChild(SaveManager.Instance.state.activeTrail).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;

    }

    private void Update()
    {
        //Fade-in
        fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;

        //menu navigation (smooth)
        menuContainer.anchoredPosition3D = Vector3.Lerp(menuContainer.anchoredPosition3D, desiredMenuPosition, 0.1f);

        //entering level zoom 
        if (isEnteringLevel)
        {
            // add to the zoomTransition float
            zoomTransition += (1 / zoomDuration) * Time.deltaTime;

            //change the scale, following the animation curve
            menuContainer.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 5, enteringLevelZoomCurve.Evaluate(zoomTransition));

            //change the desired positon of the canvas , so it can follow the scale up
            // this zoom in the center 
            Vector3 newDesiredPosition = desiredMenuPosition * 5;
            //this adds to the specific of the level canvas
            RectTransform rt = levelPanel.GetChild(Manager.Instance.currentLevel).GetComponent<RectTransform>();
            newDesiredPosition -= rt.anchoredPosition3D * 5;

            //this line will override the previous position update
            menuContainer.anchoredPosition3D = Vector3.Lerp(desiredMenuPosition, newDesiredPosition, enteringLevelZoomCurve.Evaluate(zoomTransition));

            //Fade to white screen, this will override the first line of the update
            fadeGroup.alpha = zoomTransition;

            //Are we done with the animation
            if (zoomTransition >= 1)
            {
                //enter level
                SceneManager.LoadScene("Game");
            }
        }
    }

    private void InitShop()
    {
        //Just make sure we've assigned the references
        if (colorPanel == null || trailPanel == null)
            Debug.Log("You did not asign the color/trail panel in the inspector ");

        //For every children transform under our color panel, find the button and add onclick
        int i = 0;
        foreach (Transform t in colorPanel)
        {
            int currentIndex = i;

            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnColorSelect(currentIndex));

            //set color of the image, based on if owned or not
            Image img = t.GetComponent<Image>();
            img.color = SaveManager.Instance.IsColorOwned(i) ? Color.white : new Color(0.7f, 0.7f, 0.7f);

            i++;
        }

        //Reset index
        i = 0;
        //Do the same for the trail panel
        foreach (Transform t in trailPanel)
        {
            int currentIndex = i;

            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnTrailSelect(currentIndex));

            //set trail of the image, based on if owned or not
            Image img = t.GetComponent<Image>();
            img.color = SaveManager.Instance.IsTrailOwned(i) ? Color.white : new Color(0.7f, 0.7f, 0.7f);

            i++;
        }
    }
     
    private void InitLevel()
    {
        //Just make sure we've assigned the references
        if (levelPanel == null)
            Debug.Log("You did not asign the level panel in the inspector ");

        //For every children transform under our level panel, find the button and add onclick
        int i = 0;
        foreach (Transform t in levelPanel)
        {
            int currentIndex = i;

            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnLevelSelect(currentIndex));

            Image img = t.GetComponent<Image>();

            //is it unlocked?
            if ( i <= SaveManager.Instance.state.completedLevel)
            {
                //it is unlocked!
                if ( i== SaveManager.Instance.state.completedLevel)
                {
                    img.color = Color.white;
                }
                else
                {
                    // level is already completed!
                    img.color = Color.green;
                }
            }
            else
            {
                // level is not unlock, disable the button
                b.interactable = false;

                //set to dark color
                img.color = Color.grey;
            }

            i++;
        }
    }

    private void SetCameraTo(int menuIndex)
    {
        NavigateTo(menuIndex);
        menuContainer.anchoredPosition3D = desiredMenuPosition;
    }

    private void NavigateTo(int menuIndex)
    {
        switch (menuIndex)
        {
            // 0 && default case = Main Menu
            default:
            case 0:
                desiredMenuPosition = Vector3.zero;
                break;
            //1  = Play Menu
            case 1:
                desiredMenuPosition = Vector3.right * 1280;
                break;
            //2 = Shop MEnu
            case 2:
                desiredMenuPosition = Vector3.left * 1280;
                break;
        }
    }

    private void SetColor(int index)
    {
        //set the active index
        activeColorIndex = index;
        SaveManager.Instance.state.activeColor = index;

        //change the color on the palyer model

        //change but/set button text
        colorBuySetText.text = "Current";

        //remember preferences
        SaveManager.Instance.Save();
    }

    private void SetTrail(int index)
    {
        //set the active index
        activeTrailIndex = index;
        SaveManager.Instance.state.activeTrail = index;

        //change the trail on the palyer model

        //change but/set button text
        trailBuySetText.text = "Current";

        //remember preferences
        SaveManager.Instance.Save();
    }

    private void UpdateGoldText()
    {
        goldText.text = SaveManager.Instance.state.gold.ToString();
    }

    //Buttons
    public void OnPlayClick()
    {
        NavigateTo(1);
        Debug.Log("Play button has been clicked!");
    }

    public void OnShopClick()
    {
        NavigateTo(2);
        Debug.Log("Shop button has been clicked!");
    }

    public void OnBackClick()
    {
        NavigateTo(0);
        Debug.Log("Back button has been clicked!");
    }

    private void OnTrailSelect(int currentIndex)
    {
        Debug.Log("Selecting trail button : " + currentIndex);

        // if the button clicked is already selected, exit
        if (selectedTrailIndex == currentIndex)
            return;

        //make the icon slightly bigger
        trailPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        //put the previous one on normal scale
        trailPanel.GetChild(selectedTrailIndex).GetComponent<RectTransform>().localScale = Vector3.one;

        //set the selected trail
        selectedTrailIndex = currentIndex;

        // change the content of the buy/set button, depending on the state of the trail
        if (SaveManager.Instance.IsTrailOwned(currentIndex))
        {
            //trail is owned
            //is it already our current color?
            if (activeTrailIndex == currentIndex)
            {
                trailBuySetText.text = "Current";
            }
            else
            {
                trailBuySetText.text = "Select";
            }
        }
        else
        {
            // trail is not owned
            trailBuySetText.text = "Buy: " + trailCost[currentIndex].ToString();
        }
    }

    private void OnColorSelect(int currentIndex)
    {
        Debug.Log("Selecting color button : " + currentIndex);

        // if the button clicked is already selected, exit
        if (selectedColorIndex == currentIndex)
            return;

        //make the icon slightly bigger
        colorPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        //put the previous one on normal scale
        colorPanel.GetChild(selectedColorIndex).GetComponent<RectTransform>().localScale = Vector3.one;

        //set the selected color
        selectedColorIndex = currentIndex;

        // change the content of the buy/set button, depending on the state of the color
        if (SaveManager.Instance.IsColorOwned(currentIndex))
        {
            //color is owned
            //is it already our current color?
            if (activeColorIndex == currentIndex)
            {
                colorBuySetText.text = "Current";
            }
            else
            {
                colorBuySetText.text = "Select";
            }
        }
        else
        {
            // color is not owned
            colorBuySetText.text = "Buy: " + colorCost[currentIndex].ToString();
        }
    }

    private void OnLevelSelect(int currentIndex)
    {
        Manager.Instance.currentLevel = currentIndex;
        isEnteringLevel = true;
        Debug.Log("Selecting level : " + currentIndex);
    }

    public void OnColorBuySet()
    {
        Debug.Log("Buy/Set color");

        // is the selected color owned
        if (SaveManager.Instance.IsColorOwned(selectedColorIndex))
        {
            //set color!
            SetColor(selectedColorIndex);
        }
        else
        {
            //attempt to Buy the color
            if (SaveManager.Instance.BuyColor(selectedColorIndex, colorCost[selectedColorIndex]))
            {
                //success!
                SetColor(selectedColorIndex);

                //change the color of the button
                colorPanel.GetChild(selectedColorIndex).GetComponent<Image>().color = Color.white;

                //update gold text
                UpdateGoldText();
            }
            else
            {
                //do not have enough gold!
                //play sound feedback
                Debug.Log("Not enough gold");
            }
        }
    }

    public void OnTrailBuySet()
    {
        Debug.Log("Buy/Set trail");

        // is the selected trail owned
        if (SaveManager.Instance.IsTrailOwned(selectedTrailIndex))
        {
            //set trail!
            SetTrail(selectedTrailIndex);
        }
        else
        {
            //attempt to Buy the trail
            if (SaveManager.Instance.BuyTrail(selectedTrailIndex, trailCost[selectedTrailIndex]))
            {
                //success!
                SetTrail(selectedTrailIndex);

                //change the color of the button
                trailPanel.GetChild(selectedTrailIndex).GetComponent<Image>().color = Color.white;

                //update gold text
                UpdateGoldText();
            }
            else
            {
                //do not have enough gold!
                //play sound feedback
                Debug.Log("Not enough gold");
            }
        }
    }
}
