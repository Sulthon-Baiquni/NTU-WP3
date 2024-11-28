using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void LoadNewWTPScene()
    {
        SceneManager.LoadScene("NEW WTP");
    }

    public void LoadSimulationScene()
    {
        SceneManager.LoadScene("SIMULATION");
    }
}
