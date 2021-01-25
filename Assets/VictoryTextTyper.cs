using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryTextTyper : MonoBehaviour
{
    TextMeshProUGUI txt;
    string textToType;
    public GameObject saveManager;
    public IntValue playerCoins;
    void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
        textToType = "You escaped with " + playerCoins.RuntimeValue + " gold but could it have been more";
        txt.text = "";
        StartCoroutine("PlayText");
       
    }

    IEnumerator PlayText()
    {
        foreach (char c in textToType)
        {
            txt.text += c;
            if (txt.text == textToType)
            {
                yield return new WaitForSeconds(4);
                saveManager.GetComponent<SaveManager>().ResetScriptables();
                SceneManager.LoadScene("StartMenu");
            }
            yield return new WaitForSeconds(0.125f);
        }
    }
}
