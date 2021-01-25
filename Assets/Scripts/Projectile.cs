using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public Vector2 directionToMove;
    public float lifetime;
    private float lifetimeCount;
    public Rigidbody2D rb;
    public float thrust;
    public float knockTime;
    public float damage;
    public FloatValue playerHealth;
    public Signal playerHealthSignal;
    public Signal playerDeath;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lifetimeCount = lifetime;   
    }

    // Update is called once per frame
    void Update()
    {
        lifetimeCount -= Time.deltaTime;
        if(lifetimeCount <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void Launch(Vector2 intialVel)
    {
        rb.velocity = intialVel.normalized * speed;
    }
    
}
