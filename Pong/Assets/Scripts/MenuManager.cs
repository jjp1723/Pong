using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    // ----- Fields -----

    // limit, timed, crazy, & cpu fields - Static fields the user can change in the options menu which are used when loading the game
    public static float limit = 3.0f;
    public static bool timed = true;
    public static bool crazy = false;
    public static bool cpu = true;

    // tempLimit & tempTimed fields - Hold temporary values from the options screen which are either assigned to their static counterparts if confirmed or discarded if canceled
    protected float tempLimit = limit;
    protected bool tempTimed = timed;

    // newSceneID field - A public field used to determine the id of the scene to be loaded next
    public int newSceneID;

    // options field - Used to toggle the options menu
    protected bool options = false;

    // mode, timeLimiter, parLimiter, crazyToggle, cpuToggle, optionsMenu, optionsButton, & startButton fields - Used to alter the displayed UI
    public TMP_Dropdown mode;
    public TMP_Dropdown timeLimiter;
    public TMP_Dropdown parLimiter;
    public Toggle crazyToggle;
    public Toggle cpuToggle;
    public GameObject optionsMenu;
    public GameObject optionsButton;
    public GameObject startButton;



    // ----- Methods -----

    // ChangeToScene method - Unpauses runtime if needed and loads a new scene using newSceneID
    public void ChangeToScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(newSceneID);
    }

    // ToggleOptions method - Toggles whether the options menu is visible
    public void ToggleOptions()
    {
        // Inverting the value of options, and using to set whether optionsMenu or startButton and optionsButton are visible
        options = !options;
        optionsButton.SetActive(!options);
        startButton.SetActive(!options);
        optionsMenu.SetActive(options);

        // Assigning the temp fields the values of their static counterparts
        tempLimit = limit;
        tempTimed = timed;

        // Assigning the currently used parameters to their repective UI element
        if (tempTimed)
        {
            mode.SetValueWithoutNotify(0);
            timeLimiter.SetValueWithoutNotify((int)tempLimit - 1);
        }
        else
        {
            mode.SetValueWithoutNotify(1);
            parLimiter.SetValueWithoutNotify((int)tempLimit - 1);
        }
        crazyToggle.isOn = crazy;
        cpuToggle.isOn = cpu;
    }

    // ChangeMode method - Used to change which mode is currently active and calls ChangeLimit
    public void ChangeMode()
    {
        tempTimed = (mode.options[mode.value].text == "Timed");
        ChangeLimit();
    }

    // ChangeLimit method - Used to update the current limit based on whether the active mode is timed or par
    public void ChangeLimit()
    {
        if (tempTimed)
        {
            tempLimit = float.Parse(timeLimiter.options[timeLimiter.value].text);
        }
        else
        {
            tempLimit = float.Parse(parLimiter.options[parLimiter.value].text);
        }
    }

    // Confirm method - Updates static fields based on selected options and calls ToggleOptions
    public void Confirm()
    {
        limit = tempLimit;
        timed = tempTimed;
        crazy = crazyToggle.isOn;
        cpu = cpuToggle.isOn;

        ToggleOptions();
    }
}
