using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // ----- Fields -----

    // newSceneID field - A public field used to determine the id of the scene to be loaded next
    public int newSceneID;



    // ----- Methods -----

    // ChangeToScene method - Unpauses runtime if needed and loads a new scene using newSceneID
    public void ChangeToScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(newSceneID);
    }
}
