using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    public Transform target;
    public Vector3 homePosition;
    public float chaseRadius, attackRadius;
    public Animator animator;
    private Rigidbody2D rb;
    public Vector2 lastDirection = new Vector2(0, 0);
    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        homePosition = this.transform.position;
    }
    void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius && (currentState == EnemyState.idle || currentState == EnemyState.walk) && currentState != EnemyState.stagger)
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            changeAnim(temp - transform.position);
            lastDirection = temp - transform.position;
            rb.MovePosition(temp);
            ChangeState(EnemyState.walk);
            animator.SetBool("Moving", true);
        }
        else if (this.transform.position != homePosition && currentState != EnemyState.stagger && Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, homePosition, moveSpeed * Time.deltaTime);
            changeAnim(temp - transform.position);
            lastDirection = temp - transform.position;
            rb.MovePosition(temp);
            ChangeState(EnemyState.walk);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetFloat("Last Direction", lastDirection.x);
            animator.SetBool("Moving", false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();
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
