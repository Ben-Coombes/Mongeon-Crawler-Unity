using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextTypeWriter : MonoBehaviour
{

	TextMeshProUGUI txt;
	string textToType;
	public GameObject saveManager;

	void Awake()
	{
		txt = GetComponent<TextMeshProUGUI>();
		textToType = txt.text;
		txt.text = "";

		// TODO: add optional delay when to start
		StartCoroutine("PlayText");
	}

	IEnumerator PlayText()
	{
		foreach (char c in textToType)	
		{
			txt.text += c;
			if(txt.text == textToType)
            {
				yield return new WaitForSeconds(4);
				saveManager.GetComponent<SaveManager>().ResetScriptables();
				SceneManager.LoadScene("StartMenu");
			}
			yield return new WaitForSeconds(0.125f);
		}
	}

}
