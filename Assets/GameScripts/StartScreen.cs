using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public void OnClickReset()
    {
        FileOps.DeleteFile(GameConstants.DATA_CHARACTERDATA_FILEPATH);
        FileOps.DeleteFile(GameConstants.DATA_OBJECTSDATA_FILEPATH);
        SceneManager.LoadScene(0);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
