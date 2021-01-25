using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    public Transform target;
    public Vector3 homePosition;
    public float chaseRadius, attackRadius;
    public Animator animator;
    private Rigidbody2D rb;
    public Vector2 lastDirection = new Vector2(0, 0);
    public GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        homePosition = this.transform.position;
        StartCoroutine(FireProjectile());
    }
    private IEnumerator FireProjectile()
    {
        
        while (true)
        {
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) >= attackRadius && (currentState == EnemyState.idle || currentState == EnemyState.walk) && currentState != EnemyState.stagger)
            {
                Vector3 tempVector = target.transform.position - transform.position;
                float temp = 0;
                if(tempVector.x > 0)
                {
                    temp = 1;
                } else if(tempVector.x <= 0)
                {
                    temp = -1;
                }
                animator.SetFloat("Projectile Direction", temp);
                animator.SetBool("Firing", true);
                yield return new WaitForSeconds(1f);
                animator.SetBool("Firing", false);
                tempVector = target.transform.position - transform.position;
                GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
                current.GetComponent<Projectile>().Launch(tempVector);
                yield return new WaitForSeconds(2);
            }
            yield return null;
        }
        
    }
    private void changeAnim(Vector2 direction)
    {
        if (direction.x > 0)
        {
            Debug.Log("right");
            animator.SetFloat("MoveX", 1f);
        }
        else if (direction.x < 0)
        {
            Debug.Log("left");
            animator.SetFloat("MoveX", -1f);
        }
    }
    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
    public void startHit(float attDirection)
    {
        StartCoroutine(HitAnim(attDirection));
    }
    public IEnumerator HitAnim(float attDirection)
    {
        if (health != 0)
        {
            if (attDirection == 1)
            {
                animator.SetFloat("Attack Direction", 1);
            }
            if (attDirection == 2)
            {
                animator.SetFloat("Attack Direction", 1);
            }
            if (attDirection == 3)
            {
                animator.SetFloat("Attack Direction", -1);
            }
            if (attDirection == 4)
            {
                animator.SetFloat("Attack Direction", -1);
            }
        }
        yield return new WaitForSeconds(0.60f);
        animator.SetFloat("Attack Direction", 0);
    }
}
