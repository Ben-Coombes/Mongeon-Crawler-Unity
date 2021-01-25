using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : Enemy
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
                if (tempVector.x > 0)
                {
                    temp = 1;
                }
                else if (tempVector.x <= 0)
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
    // Update is called once per frame
    void Update()
    {

    }
    public void startHit(float attDirection)
    {
        StartCoroutine(HitAnim(attDirection));
    }
    public IEnumerator HitAnim(float attDirection)
    {
        if (health != 0)
        {
            animator.SetBool("takeHit", true);
            yield return new WaitForSeconds(0.4f);
            animator.SetBool("takeHit", false);
        }
    }
}
