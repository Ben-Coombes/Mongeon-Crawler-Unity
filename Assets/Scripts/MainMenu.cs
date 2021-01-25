using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void New()
    {
        SceneManager.LoadScene("Level");
    }
    public void Load()
    {
        SceneManager.LoadScene("Level");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
