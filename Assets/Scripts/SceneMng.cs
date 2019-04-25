using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMng : MonoBehaviour
{

    public void ResetGame()
    {
        SceneManager.LoadScene("scene");
    }
}
