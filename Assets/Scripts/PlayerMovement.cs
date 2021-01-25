using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState
    {
        walk,
        attack,
        interact,
        stagger,
        idle
    }

    public PlayerState currentState;
    public float moveSpeed = 7f;
    public Rigidbody2D rb;
    public Animator animator;
    public float fAttackDirection = 0;
    public float attackRange = 0.5f;
    public float thrust;
    public float knockTime;
    public FloatValue damage;
    public float health;
    public FloatValue currentHealth;
    public FloatValue heartContainers;
    public bool attacking = false;
    public LayerMask enemyLayers;
    public Transform AttackUp, AttackRight, AttackDown, AttackLeft;
    private Vector3 change;
    public Vector2 lastDirection = new Vector2(0, 0);
    public Item apollosFlask;
    public Item bookOfPower;
    public Item chrysaor;
    public Item focusRing;
    public Item fontOfLife;
    public BoolValue aPCheck;
    public BoolValue bPCheck;
    public BoolValue chryCheck;
    public BoolValue fRCheck;
    public BoolValue fLCheck;
    public Signal heartSignal;
    public Signal heartContainerSignal;
    public Inventory playerInventory;
    public LootTable enemyDrops;
    public Coin goldCoin;
    public Coin platCoin;
    public VectorValue playerPosition;
    public Signal bossDeath;
    public Signal finalBossDeath;
    public BossBar bossBar;
    // Start is called before the first frame update

    // Update is called once per frame
    void Start()
    {
        transform.position = playerPosition.RuntimeValue;
        StartCoroutine(attack());
        currentState = PlayerState.idle;
    }
    void Update()
    {
        playerPosition.RuntimeValue = transform.position;
        change = Vector3.zero;
        if (currentState != PlayerState.stagger && currentState != PlayerState.attack)
        {
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");
        }
        if (change != Vector3.zero)
        {
            ChangeState(PlayerState.walk);
            MoveCharacter(change);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) && currentState != PlayerState.stagger)
        {
            ChangeState(PlayerState.attack);
            if (Input.GetKey(KeyCode.UpArrow))
            {
                fAttackDirection = 1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                fAttackDirection = 2;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                fAttackDirection = 3;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                fAttackDirection = 4;
            }
        }
        if (playerInventory.CheckForItem(apollosFlask) && aPCheck.RuntimeValue == false)
        {
            heartContainers.RuntimeValue += 1;
            currentHealth.RuntimeValue = heartContainers.RuntimeValue * 2;
            heartContainerSignal.Raise();
            heartSignal.Raise();
            aPCheck.RuntimeValue = true;
        }
        if (playerInventory.CheckForItem(bookOfPower) && bPCheck.RuntimeValue == false)
        {
            damage.RuntimeValue += 1;
            bPCheck.RuntimeValue = true;
        }
        if (playerInventory.CheckForItem(chrysaor) && chryCheck.RuntimeValue == false)
        {
            goldCoin.coinValue.RuntimeValue *= 2;
            platCoin.coinValue.RuntimeValue *= 2;
            enemyDrops.loots[0].lootChance.RuntimeValue += 20;
            enemyDrops.loots[1].lootChance.RuntimeValue += 10;
            enemyDrops.loots[2].lootChance.RuntimeValue = 0;
            chryCheck.RuntimeValue = true;
        }
        if (playerInventory.CheckForItem(focusRing) && fRCheck.RuntimeValue == false)
        {
            damage.RuntimeValue += 2;
            heartContainers.RuntimeValue -= 1;
            if(currentHealth.RuntimeValue / 2 > heartContainers.RuntimeValue)
            {
                currentHealth.RuntimeValue = heartContainers.RuntimeValue * 2;
            }
            heartContainerSignal.Raise();
            heartSignal.Raise();
            fRCheck.RuntimeValue = true;
        }
        if (playerInventory.CheckForItem(fontOfLife) && fLCheck.RuntimeValue == false)
        {
            heartContainers.RuntimeValue += 1;
            heartContainerSignal.Raise();
            heartSignal.Raise();
            fLCheck.RuntimeValue = true;
        }

            animator.SetFloat("Horizontal", change.x);
        animator.SetFloat("Vertical", change.y);
        animator.SetFloat("Speed", change.sqrMagnitude);
        animator.SetFloat("AttackDirection", fAttackDirection);

    }
    void MoveCharacter(Vector2 direction)
    {
        rb.MovePosition(transform.position + change.normalized * moveSpeed * Time.fixedDeltaTime);
        animator.SetFloat("Last Direction", direction.x);
    }
    private void ChangeState(PlayerState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    IEnumerator attack()
    {
        while (true)
        {
            
            Collider2D[] hitEnemies = null;
            if (fAttackDirection == 1)
            {
                hitEnemies = Physics2D.OverlapCircleAll(AttackUp.position, attackRange, enemyLayers);
                
            }
            else if (fAttackDirection == 2)
            {
                hitEnemies = Physics2D.OverlapCircleAll(AttackRight.position, attackRange, enemyLayers);
               
            }
            else if (fAttackDirection == 3)
            {
                hitEnemies = Physics2D.OverlapCircleAll(AttackDown.position, attackRange, enemyLayers);
                
            }
            else if (fAttackDirection == 4)
            {
                hitEnemies = Physics2D.OverlapCircleAll(AttackLeft.position, attackRange, enemyLayers);
                
            }
            
            if (fAttackDirection > 0)
            {
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    Rigidbody2D enemyBody = enemy.GetComponent<Rigidbody2D>();
                    Vector2 difference = enemy.transform.position - transform.position;
                    difference = difference.normalized * thrust;
                    enemyBody.AddForce(difference, ForceMode2D.Impulse);
                    StartCoroutine(KnockCo(enemyBody));
                    TakeDamage(enemyBody);
                    Debug.Log(enemyBody.GetComponent<Enemy>().health);

                }
                
                yield return new WaitForSecondsRealtime(0.44f);
                fAttackDirection = 0;
                currentState = PlayerState.idle;
                
                
            }
            yield return null;
        }
    }
    private void TakeDamage(Rigidbody2D enemy)
    {
        string enemyName = enemy.GetComponent<Enemy>().enemyName;
        
        enemy.GetComponent<Enemy>().health -= damage.RuntimeValue;
        if (enemyName == "Mushroom")
        {
            enemy.GetComponent<Mushroom>().startHit(fAttackDirection);
        }
        else if(enemyName == "Goblin")
        {
            enemy.GetComponent<Goblin>().startHit(fAttackDirection);
        }
        else if (enemyName == "Eye")
        {
            enemy.GetComponent<Eye>().startHit(fAttackDirection);
        }
        else if (enemyName == "Skeleton")
        {
            enemy.GetComponent<Skeleton>().startHit(fAttackDirection);
        }
        else if(enemyName == "Slime Boss")
        {
            enemy.GetComponent<SlimeBoss>().startHit(fAttackDirection);
            bossBar.SetHealth(enemy.GetComponent<Enemy>().health);
        }
        else if (enemyName == "Skeleton Boss")
        {
            enemy.GetComponent<SkeletonBoss>().startHit(fAttackDirection);
            bossBar.SetHealth(enemy.GetComponent<Enemy>().health);
        }
        if (enemy.GetComponent<Enemy>().health <= 0)
        {
           StartCoroutine(EnemyDeath(enemy));
        }
    }
    private IEnumerator KnockCo(Rigidbody2D enemy)
    {
        yield return new WaitForSeconds(knockTime);
        enemy.velocity = Vector2.zero;
        enemy.GetComponent<Enemy>().currentState = EnemyState.idle;
        enemy.velocity = Vector2.zero;
    }
    IEnumerator EnemyDeath(Rigidbody2D enemy)
    {
        string enemyName = enemy.GetComponent<Enemy>().enemyName;
        if (enemyName == "Mushroom")
        {
            enemy.GetComponent<Mushroom>().animator.SetBool("Alive", false);
            foreach (var c in enemy.gameObject.GetComponentsInChildren<Collider2D>())
            {
                c.enabled = false;
            }
        }
        else if (enemyName == "Goblin")
        {
            enemy.GetComponent<Goblin>().animator.SetBool("Alive", false);
            foreach (var c in enemy.gameObject.GetComponentsInChildren<Collider2D>())
            {
                c.enabled = false;
            }
        }
        else if (enemyName == "Eye")
        {
            enemy.GetComponent<Eye>().animator.SetBool("Alive", false);
            foreach (var c in enemy.gameObject.GetComponentsInChildren<Collider2D>())
            {
                c.enabled = false;
            }
        }
        else if (enemyName == "Skeleton")
        {
            enemy.GetComponent<Skeleton>().animator.SetBool("Alive", false);
            foreach (var c in enemy.gameObject.GetComponentsInChildren<Collider2D>())
            {
                c.enabled = false;
            }
        }
        else if (enemyName == "Slime Boss")
        {
            bossDeath.Raise();
            bossBar.gameObject.SetActive(false);
            enemy.GetComponent<SlimeBoss>().animator.SetBool("Alive", false);
            foreach (var c in enemy.gameObject.GetComponentsInChildren<Collider2D>())
            {
                c.enabled = false;
            }
        }
        else if (enemyName == "Skeleton Boss")
        {
            finalBossDeath.Raise();
            bossBar.gameObject.SetActive(false);
            enemy.GetComponent<SkeletonBoss>().animator.SetBool("Alive", false);
            foreach (var c in enemy.gameObject.GetComponentsInChildren<Collider2D>())
            {
                c.enabled = false;
            }
        }

        enemy.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.8f);
        enemy.GetComponent<Enemy>().SetBool();
        enemy.gameObject.SetActive(false);
        enemy.GetComponent<Enemy>().MakeLoot();
    }
    public void StartDeath()
    {
        StartCoroutine(StartDeathCo());
    }
    public IEnumerator StartDeathCo()
    {
        Debug.Log("player death");
        animator.SetBool("Alive", false);
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("DeathScene");
    }
}
