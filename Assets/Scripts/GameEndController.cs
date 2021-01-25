using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameEndController : Interactable
{
    public GameObject interactButton;
    public TextMeshProUGUI textDescription;
    public TextMeshProUGUI textName;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange == true && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("VictoryScene");
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            playerInRange = true;
            interactButton.SetActive(true);
            textName.text = "exit dungeon";
            textDescription.text = "";


        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            playerInRange = false;
            interactButton.SetActive(false);

        }
    }
}
