using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}
public class Enemy : MonoBehaviour
{
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float damage;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public float knockTime;
    public float thrust;
    public Signal playerHealthSignal;
    public FloatValue playerHealth;
    public LootTable thisLoot;
    public BoolValue alive;
    public Signal playerDeath;
    // Start is called before the first frame update
    private void Awake()
    {
        health = maxHealth.intialValue;
        if (alive.RuntimeValue == false)
        {
            Destroy(this.gameObject);
        }
    }
    public void Knock(Rigidbody2D rb, float knockTime)
    {
        StartCoroutine(KnockCo(rb, knockTime));
        health = maxHealth.intialValue;
    }
    private IEnumerator KnockCo(Rigidbody2D rb, float knockTime)
    {
        yield return new WaitForSeconds(knockTime);
        rb.velocity = Vector2.zero;
        currentState = EnemyState.idle;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            if (collider.GetComponent<PlayerMovement>().currentState != PlayerMovement.PlayerState.stagger)
            {
                StartCoroutine(KnockCo(collider.GetComponent<Rigidbody2D>()));
                TakeDamage(collider.GetComponent<Rigidbody2D>());
            }
        }

    }
    public void MakeLoot()
    {
        if (thisLoot != null)
        {
            Pickup current = thisLoot.LootPickup();
            if(current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }
    private void TakeDamage(Rigidbody2D player) 
    {
        playerHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (playerHealth.RuntimeValue <= 0)
        {
            playerDeath.Raise();
           
        }
    }
    private IEnumerator KnockCo(Rigidbody2D player)
    {
            player.GetComponent<PlayerMovement>().currentState = PlayerMovement.PlayerState.stagger;
            Vector2 difference = player.transform.position - transform.position;
            difference = difference.normalized * thrust;
            player.AddForce(difference, ForceMode2D.Impulse);
            yield return new WaitForSeconds(knockTime);
            player.velocity = Vector2.zero;
            player.GetComponent<PlayerMovement>().currentState = PlayerMovement.PlayerState.idle;
            player.velocity = Vector2.zero;
        
    }
    public void SetBool()
    {
        alive.RuntimeValue = false;
    }
}
