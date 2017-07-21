using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {

    public float dealthDelay = 5f;
    public int scoreOnHit;
    public int scoreOnKill;
    public GameObject particles;

    private gameController gameController;
    private scoreController scoreController;
    private GameObject player;
    private Animator anim;
    private bool isDead;
    private CapsuleCollider capsule;
    private NavMeshAgent nav;
    private float dealthTime;
    private float bloodTme = .5f;
    private int currentHealth;
    private EnemyMovement enemyMovement;
    private Rigidbody rb;
    private int startingHealth;
    private List<GameObject> leftLeg;
    private List<GameObject> rightLeg;
    private List<GameObject> leftArm;
    private List<GameObject> rightArm;
    private List<CapsuleCollider> limbColliders;
    private BoxCollider headCollider;
    private GameObject head;
    private Transform leftLegTransform;
    private Transform rightLegTransform;


    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        GameObject gameControllerObj = GameObject.FindGameObjectWithTag("GameController");

        if (gameControllerObj != null)
        {
            gameController = gameControllerObj.GetComponent<gameController>();
        }

        GameObject scoreControllerObj = GameObject.FindGameObjectWithTag("ScoreController");

        if (scoreControllerObj != null)
        {
            scoreController = scoreControllerObj.GetComponent<scoreController>();
        }

        startingHealth = gameController.getWave() * 100 + 50;
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        isDead = false;
        capsule = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim.speed = 2f;
        enemyMovement = GetComponent<EnemyMovement>();
        leftLeg = new List<GameObject>();
        rightLeg = new List<GameObject>();
        leftArm = new List<GameObject>();
        rightArm = new List<GameObject>();
        limbColliders = new List<CapsuleCollider>();
        sortBodyParts();
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public bool getIsDead()
    {
        return isDead;
    }

    public int getScoreOnKill()
    {
        return scoreOnKill;
    }

    public void takeDamage(int damageValue, string source,RaycastHit shotHit)
    {
        bool isHeadshot = false;

        if (isDead)
        {
            return;
        }

        if (source == "bullet")
        {
            if (string.Compare(shotHit.collider.transform.ToString(), "mesh_Head (UnityEngine.Transform)") == 0)
            {
                currentHealth -= damageValue * 2;
                isHeadshot = true;
            }

            else
            {
                currentHealth -= damageValue;
            }
            GameObject blood = Instantiate(particles, shotHit.point, Quaternion.identity);
            Destroy(blood, bloodTme);
        }

        else
        {
            currentHealth -= damageValue;
            GameObject blood1 = Instantiate(particles, leftLegTransform.position, Quaternion.identity);
            GameObject blood2 = Instantiate(particles, rightLegTransform.position, Quaternion.identity);
            Destroy(blood1, bloodTme);
            Destroy(blood2, bloodTme);
        }

        scoreController.addScore(scoreOnHit);

        if (currentHealth <= 0)
        {
            if (source == "bullet")
            {
                if (isHeadshot)
                {
                    scoreController.addScore(scoreOnKill);
                    dismemberHead();
                }

                else if (string.Compare(shotHit.collider.transform.ToString(), "LLegCollider (UnityEngine.Transform)") == 0)
                {
                    dismemberLeftLeg();
                }

                else if (string.Compare(shotHit.collider.transform.ToString(), "RLegCollider (UnityEngine.Transform)") == 0)
                {
                    dismemberRightLeg();
                }

                else if (string.Compare(shotHit.collider.transform.ToString(), "LArmCollider (UnityEngine.Transform)") == 0)
                {
                    dismemberLeftArm();
                }

                else if (string.Compare(shotHit.collider.transform.ToString(), "RArmCollider (UnityEngine.Transform)") == 0)
                {
                    dismemberRightArm();
                }
            }

            else
            {
                dismemberLeftLeg();
                dismemberRightLeg();
            }

            die();
        }
    }

    public void die()
    {
        capsule.isTrigger = true;
        isDead = true;
        anim.SetTrigger("die");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Shootable"), LayerMask.NameToLayer("Player"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Shootable"), LayerMask.NameToLayer("Shootable"));
        nav.enabled = false;
        enemyMovement.enabled = false;
        Destroy(gameObject, dealthDelay);
        gameController.setCurrentEnemies(gameController.getCurrentEnemies() - 1);
        gameController.setEnemiesLeft(gameController.getEnemiesLeft() - 1);
        gameObject.layer = 2;
        scoreController.addScore(scoreOnKill);
        setBodyPartsToTrigger();
    }

    public void dismemberHead()
    {
        head.SetActive(false);
    }

    public void dismemberLeftLeg()
    {
        foreach (GameObject limb in leftLeg)
        {
            limb.SetActive(false);
        }
    }

    public void dismemberRightLeg()
    {
        foreach (GameObject limb in rightLeg)
        {
            limb.SetActive(false);
        }
    }

    public void dismemberLeftArm()
    {
        foreach (GameObject limb in leftArm)
        {
            limb.SetActive(false);
        }
    }

    public void dismemberRightArm()
    {
        foreach (GameObject limb in rightArm)
        {
            limb.SetActive(false);
        }
    }

    public void sortBodyParts()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Head")
            {
                head = child.gameObject;
                headCollider = head.GetComponent<BoxCollider>();
            }

            else if (child.tag == "Lthigh" || child.tag == "Lcalf")
            {
                leftLeg.Add(child.gameObject);
                leftLegTransform = child.transform;
            }

            else if (child.tag == "Rthigh" || child.tag == "Rcalf")
            {
                rightLeg.Add(child.gameObject);
                rightLegTransform = child.transform;
            }

            else if (child.tag == "Lhand" || child.tag == "Lforearm" || child.tag == "Lupperarm")
            {
                leftArm.Add(child.gameObject);
            }

            else if (child.tag == "Rhand" || child.tag == "Rforearm" || child.tag == "Rupperarm")
            {
                rightArm.Add(child.gameObject);
            }

            else if (child.tag == "LimbCollider")
            {
                limbColliders.Add(child.GetComponent<CapsuleCollider>());
            }
        }
    } 

    public void setBodyPartsToTrigger()
    {
        foreach (CapsuleCollider collider in limbColliders)
        {
            collider.isTrigger = true;
        }

        headCollider.isTrigger = true;
    }
}
