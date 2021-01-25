using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public Signal startBoss;
    public FloatValue bossHealth;
    public BossBar bossBar;
    public BoolValue alive;
   void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && alive.RuntimeValue == true)
        {
            foreach (var c in this.gameObject.GetComponentsInChildren<Collider2D>())
            {
                if (c.isTrigger == false)
                {

                    c.enabled = true;
                } else if (c.isTrigger == true && c.isActiveAndEnabled)
                {
                    Debug.Log("trigger");
                    startBoss.Raise();
                    bossBar.gameObject.SetActive(true);
                    bossBar.SetMaxHealth(bossHealth.intialValue);
                    c.enabled = false;
                }
            }
            
        }
    }
    public void DestroyTrigger()
    {
        Destroy(this.gameObject);
    }
}
