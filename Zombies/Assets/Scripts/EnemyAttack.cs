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
            StartCoroutine(attack());
        }
        /*
        else if (spawnInRange && !enemyMovement.getSpawnDestroyed())
        {
            attackSpawn();
        }*/
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            this.playerInRange = true;
        }
        /*
        else if (other.tag == "SpawnWindow" && !enemyMovement.getSpawnDestroyed())
        {
            spawnInRange = true;
        }*/
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            this.playerInRange = false;
        }
    }

    public IEnumerator attack()
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
    /*
    public IEnumerator attackSpawn()
    {
        GameObject spawn = enemyMovement.getSpawn();
        foreach (Transform child in spawn.transform)
        {
            child.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.5f);
        }

        if (spawn.transform.childCount == 0)
        {
            enemyMovement.setSpawnDestroyed(true);
        }

        else
        {
            GameObject board = spawn.transform.GetChild(0).gameObject;
            board.SetActive(false);
            nextAttack = Time.time + attackSpeed;
        }
    }*/
}
