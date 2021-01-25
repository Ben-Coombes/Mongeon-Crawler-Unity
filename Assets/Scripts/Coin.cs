using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Pickup
{
    public Inventory playerInventory;
    public IntValue coinValue;
    public BoolValue taken;
    // Start is called before the first frame update
    void Start()
    {
        if (taken != null)
        {
            if (taken.RuntimeValue == true)
            {
                this.gameObject.SetActive(false);
            }
        }
        pickupSignal.Raise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            playerInventory.coins.RuntimeValue += coinValue.RuntimeValue;
            pickupSignal.Raise();
            if(taken != null)
            {
                taken.RuntimeValue = true;
            }
            Destroy(this.gameObject);
        }
    }
}
