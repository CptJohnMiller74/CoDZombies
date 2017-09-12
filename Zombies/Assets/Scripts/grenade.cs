using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{

    public float blastRadius;
    public int damage;
    public float delay;
    public float throwSpeed;

    private Transform grenadeSpawn;
    private ParticleSystem explosion;
    private float explosionTime;
    private SphereCollider sphereCollider;
    private List<EnemyHealth> enemiesInRange;
    private bool playerInRange;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private Camera fpsCamera;
    private PlayerShooterNew playerShooter;
    private bool isThrown;

    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = blastRadius;
        explosionTime = Time.time + delay;
        enemiesInRange = new List<EnemyHealth>();
        this.rb = GetComponent<Rigidbody>();
        playerInRange = true;
        explosion = GetComponent<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), capsuleCollider);
        fpsCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        grenadeSpawn = GameObject.FindGameObjectWithTag("grenadeSpawn").transform;
        playerShooter = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerShooterNew>();
        isThrown = false;
    }

    void Update()
    {
        if (playerShooter.getIsHoldingGrenade() && !isThrown)
        {
            gameObject.transform.position = grenadeSpawn.position;
        }
        
        if (Input.GetKeyUp(KeyCode.G) && !this.isThrown)
        {
            this.throwGrenade();
        }

        if (Time.time >= explosionTime)
        {
            explode();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            enemiesInRange.Add(other.gameObject.GetComponent<EnemyHealth>());
        }

        if (other.tag == "Player")
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "enemy")
        {
            enemiesInRange.Remove(other.gameObject.GetComponent<EnemyHealth>());
        }

        if (other.tag == "Player")
        {
            playerInRange = false;
        }
    }

    public void explode()
    {
        this.explosion.Play();
        this.sphereCollider.radius = 0;
        if (enemiesInRange.Count > 0)
        {
            Vector3 rayOrigin = this.transform.position;

            foreach (EnemyHealth enemyHealth in enemiesInRange)
            {
                if (enemyHealth != null && !enemyHealth.getIsDead())
                {
                    Vector3 rayEnd = enemyHealth.gameObject.GetComponent<Rigidbody>().worldCenterOfMass;
                    RaycastHit shotHit;
                    if (Physics.Raycast(rayOrigin, rayEnd, out shotHit, Vector3.Distance(rayEnd, rayOrigin)))
                    {
                        continue;
                    }
                    else
                    {
                        enemyHealth.takeDamage(damage, "grenade", shotHit);
                    }
                }
            }
        }
        if (playerInRange)
        {
            PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
            playerHealth.takeDamage(damage);
        }
        Destroy(gameObject, 2f);
    }
    
    public void throwGrenade()
    {
        this.isThrown = true;
        playerShooter.setIsHoldingGrenade(false);
        Vector3 grenadeVector = fpsCamera.transform.forward;
        this.rb.AddForce(grenadeVector * throwSpeed);
    }
}