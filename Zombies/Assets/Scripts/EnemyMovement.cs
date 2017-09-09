using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

    public float [] enemySpeed;
    public float enemyAngularSpeed;
    public float rotation;

    private gameController gameController;
    private Transform player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;
    private EnemyAttack enemyAttack;
    private NavMeshAgent nav;
    private int speedIndex;
    private Transform spawn;
    private Transform spawnEnd;
    private bool isOutOfSpawn;
    //private float moveSpeed = .5f;

    void Start () {
        GameObject gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObj != null)
        {
            gameController = gameControllerObj.GetComponent<gameController>();
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        nav = GetComponent<NavMeshAgent>();
        assignSpeed();
        nav.angularSpeed = enemyAngularSpeed;
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttack = GetComponent<EnemyAttack>();
        isOutOfSpawn = false;
	}
	
	void Update () {

        if (spawn.GetComponent<spawnWindow>().getIsDestroyed() && enemyAttack.getSpawnInRange() && !isOutOfSpawn)
        {
            moveThroughDoor();
        }

        if (!isOutOfSpawn && !player.GetComponent<PlayerMovement>().getIsInEnemySpawn())
        {
            nav.SetDestination(this.spawn.position);
        }

        else if (playerHealth.getCurrentHP() > 0 && enemyHealth.getCurrentHealth() > 0)
        {
            nav.SetDestination(player.position);
        }

        else
        {
            nav.enabled = false;
        }

        InstantlyTurn(nav.destination);
    }

    private void InstantlyTurn(Vector3 destination)
    {
        if ((destination - transform.position).magnitude < 0.1f)
        {
            return;
        }

        Vector3 direction = (destination - transform.position).normalized;
        Quaternion qDir = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, qDir, Time.deltaTime * rotation);
    }

    private void assignSpeed()
    {
        if (gameController.getWave() >= 10)
        {
            speedIndex = enemySpeed.Length - 1;
        }

        else if (gameController.getWave() <= 3) 
        {
            speedIndex = 0;
        }

        else
        {
            float rF = Random.Range(0.0f, 1.0f);
            if (rF < .25)
            {
                speedIndex = 0;
            }

            else if (rF >= .75)
            {
                speedIndex = enemySpeed.Length - 2;
            }

            else
            {
                speedIndex = enemySpeed.Length - 1;
            }
        }
        nav.speed = enemySpeed[speedIndex];
    }

    public void moveThroughDoor()
    {
        nav.enabled = false;
        gameObject.transform.position = spawnEnd.position;
        nav.enabled = true;
        isOutOfSpawn = true;
    }

    public void setSpawn(Transform currSpawn)
    {
        this.spawn = currSpawn;
        this.spawnEnd = currSpawn.GetChild(0);
    }

    public GameObject getSpawn()
    {
        return this.spawn.gameObject;
    }
}
