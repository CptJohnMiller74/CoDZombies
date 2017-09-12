using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    public float attackSpeed;
    public int attackDamage;

    private Animator anim;
    private GameObject player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;
    private EnemyMovement enemyMovement;
    private float nextAttack = 0;
    private bool playerInRange;
    private spawnWindow spawnTarget;
    private bool spawnInRange;

	void Start () {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyMovement = GetComponent<EnemyMovement>();
    }
	
	void Update () {

        if (Time.time > nextAttack && playerInRange && enemyHealth.getCurrentHealth() > 0 && playerHealth.getCurrentHP() > 0)
        {
            StartCoroutine(attackPlayer());
        }

        else if (Time.time > nextAttack && spawnInRange && enemyHealth.getCurrentHealth() > 0 && !spawnTarget.getIsDestroyed())
        {
            StartCoroutine(attackSpawn());
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            this.playerInRange = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            this.playerInRange = false;
        }
    }

    public IEnumerator attackPlayer()
    {
        nextAttack = Time.time + attackSpeed;
        anim.SetBool("attack", true);

        if (playerHealth.getCurrentHP() > 0)
        {
            playerHealth.takeDamage(attackDamage);
        }
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("attack", false);
    }
    
    public IEnumerator attackSpawn()
    {
        nextAttack = Time.time + attackSpeed;
        anim.SetBool("attack", true);
        spawnTarget.takeDamage();
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("attack", false);
    }

    public bool getSpawnInRange()
    {
        return this.spawnInRange;
    }

    public void setSpawnInRange(bool b)
    {
        this.spawnInRange = b;
    }

    public void setSpawnTarget(GameObject currSpawn)
    {
        this.spawnTarget = currSpawn.GetComponent<spawnWindow>();
    }
}
