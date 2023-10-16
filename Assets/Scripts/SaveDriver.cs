using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Temporary driver class to test the save system.
/// 
/// Please remove this class when you are done with it.
/// </summary>
public class SaveDriver : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        // If the player presses the "P" key, then save the game.
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveManager.WriteToDisk();
        }
    }
}
