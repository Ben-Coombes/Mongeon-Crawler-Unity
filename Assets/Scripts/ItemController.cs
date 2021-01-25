using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemController : Interactable
{
    public GameObject itemObject;
    public bool isShopItem;
    public Signal ItemSignal;
    public Signal coinSignal;
    public Item currentItem;
    public IntValue rand;
    public TextMeshProUGUI textDescription;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textShopPrice;
    public GameObject interactButton;
    public Inventory playerInventory;
    public BoolValue isSpawned;
    public BoolValue isTaken;
    public Inventory itemList;
    
    // Start is called before the first frame update
    void Start()
    {
       
        if(isSpawned.RuntimeValue == false && isTaken.RuntimeValue == false)
        {
            rand.RuntimeValue = Random.Range(0, itemList.items.Count);
            currentItem = itemList.items[rand.RuntimeValue];
            itemObject.GetComponent<SpriteRenderer>().sprite = currentItem.itemSprite;
            itemList.items.RemoveAt(rand.RuntimeValue);
            isSpawned.RuntimeValue = true;
        }
        else if(isSpawned.RuntimeValue == true && isTaken.RuntimeValue == false)
        {
            currentItem = itemList.items[rand.RuntimeValue];
            itemObject.GetComponent<SpriteRenderer>().sprite = currentItem.itemSprite;
            itemList.items.RemoveAt(rand.RuntimeValue);
            
        }
        else
        {
            itemObject.SetActive(false);
        }
        if (isShopItem)
        {
            textShopPrice.text = "$" + currentItem.itemPrice;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if(playerInRange == true && Input.GetKeyDown(KeyCode.E) && isTaken.RuntimeValue == false)
        {
            isTaken.RuntimeValue = true;
            if (playerInventory.coins.RuntimeValue >= currentItem.itemPrice && isShopItem == true)
            {
                playerInventory.coins.RuntimeValue -= currentItem.itemPrice;
                coinSignal.Raise();
                playerInventory.addItem(currentItem);
                itemObject.SetActive(false);
                foreach (var c in this.gameObject.GetComponents<Collider2D>())
                {
                    if (c.isTrigger == true)
                    {
                        c.enabled = false;
                    }
                   
                }
            } else if(isShopItem == false)
            {
                playerInventory.addItem(currentItem);
                itemObject.SetActive(false);
                foreach (var c in this.gameObject.GetComponents<Collider2D>())
                {
                    if (c.isTrigger == true)
                    {
                        c.enabled = false;
                    }
                }
            }
        }
    }
    public void CheckActive()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            playerInRange = true;
            interactButton.SetActive(true);
            textName.text = currentItem.itemName;
            textDescription.text = currentItem.itemDescription;
            

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
