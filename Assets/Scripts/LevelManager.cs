using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void OpenScanScreen()
    {    
		SceneManager.LoadScene ("MainScreen");	
    }

    public void OnStartLevel2 ()
	{
		SceneManager.LoadScene ("LeftRight");
	}

    public void QuitApp()
    {
        Application.Quit();
    }
}
