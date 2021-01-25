using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Pickup
{
    public FloatValue playerHealth;
    public FloatValue heartContainers;
    public float healAmount;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player")&& !collider.isTrigger)
        {
            if(playerHealth.RuntimeValue != heartContainers.RuntimeValue *2)
            {
                playerHealth.RuntimeValue += healAmount;
                
                if (playerHealth.RuntimeValue > heartContainers.RuntimeValue * 2)
                {
                    playerHealth.RuntimeValue = heartContainers.RuntimeValue * 2;
                }
                pickupSignal.Raise();
                if (taken != null)
                {
                    taken.RuntimeValue = true;
                }
                Destroy(this.gameObject);
            }
        } 
    }
}
