using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : Projectile
{
    public Animator animator;
    public Rigidbody2D player;
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (collider.GetComponent<PlayerMovement>().currentState != PlayerMovement.PlayerState.stagger)
            {
                StartCoroutine(KnockCo(collider.GetComponent<Rigidbody2D>()));
                TakeDamage(collider.GetComponent<Rigidbody2D>());
            }
            StartCoroutine(BombExplode());
            
        }
        if(collider.CompareTag("Ceiling"))
        {
            StartCoroutine(BombExplode());
        }

    }
    IEnumerator BombExplode()
    {
        this.GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        animator.SetBool("isExplode", true);
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
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
    private void TakeDamage(Rigidbody2D player)
    {
        playerHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (playerHealth.RuntimeValue <= 0)
        {
            playerDeath.Raise();
        }
    }
}
