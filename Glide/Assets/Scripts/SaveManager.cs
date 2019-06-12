using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }
    public SaveState state;

    private void Awake()
    {
        //ResetSave();
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Load();

    }

    //Save the whole state of this SaveState script to the player pref
    public void Save()
    {
        PlayerPrefs.SetString("save",Helper.Serialize<SaveState>(state));
    }

    //Load the previous saved state from the player prefs 
    public void Load()
    {
        //Do we already have a save?
        if (PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveState();
            Save();
            Debug.Log("No save file found,creating a new one! ");
        }
    }

    //check if the color is owned
    public bool IsColorOwned(int index)
    {
        //check if the bit is set, if so the color owned
        return (state.colorOwned & (1 << index)) != 0;
    }
    //check if the trail is owned
    public bool IsTrailOwned(int index)
    {
        //check if the bit is set, if so the color owned
        return (state.trailOwned & (1 << index)) != 0;

    }

    //attempt buying color, return true/false
    public bool BuyColor(int index, int cost)
    {
        if (state.gold >= cost)
        {
            //Enough money, remove grom the current gold stack
            state.gold -= cost;
            UnlockColor(index);

            //saveprogress
            Save();
            return true;
        }
        else
        {
            //not enough money, return false
            return false;
        }
    }

    //attempt buying trail, return true/false
    public bool BuyTrail(int index, int cost)
    {
        if (state.gold >= cost)
        {
            //Enough money, remove grom the current gold stack
            state.gold -= cost;
            UnlockTrail(index);

            //saveprogress
            Save();
            return true;
        }
        else
        {
            //not enough money, return false
            return false;
        }
    }

    //unlock a color in the "colorOwned" int 
    public void UnlockColor(int index)
    {
        //toggle on the bit at index
        state.colorOwned |= 1 << index;
    }
    //unlock a trail in the "trailOwned" int 
    public void UnlockTrail(int index)
    {
        //toggle on the bit at index
        state.trailOwned |= 1 << index;
    }

    //Complete Level
    public void CompleteLevel(int index)
    {
        //if this is the current active level
        if (state.completedLevel == index)
        {
            state.completedLevel++;
            Save();
        }
    }

    //reset the whole save file
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }
}
