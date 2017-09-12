using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnWindow : MonoBehaviour {

    public scoreController scoreController;
    public float repairDelay;

    private bool isDestroyed;
    private bool isFullHealth;
    private List<GameObject> boards;
    private int maxBoards;
    private int currIndex;
    private int numActiveBoards;
    private float nextRepair;

    const int repairValue = 10;

	void Start () {

        boards = new List<GameObject>();

        foreach (Transform child in transform)
        {
            if (child.tag == "board")
            {
                boards.Add(child.gameObject);
            }
        }
        maxBoards = boards.Count;
        isDestroyed = false;
        currIndex = maxBoards;
	}
	

	void Update () {
        //Debug.Log(isFullHealth);
        if (currIndex < maxBoards)
        {
            isFullHealth = false;
        } 

        if (currIndex == 0)
        {
            isDestroyed = true;
        }

        if (currIndex == maxBoards)
        {
            isFullHealth = true;
        }

	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            other.GetComponent<EnemyAttack>().setSpawnInRange(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "enemy")
        {
            other.GetComponent<EnemyAttack>().setSpawnInRange(false);
        }
    }

    public void takeDamage()
    {
        if (boards.Count > 0)
        {
            boards[currIndex - 1].SetActive(false);
            currIndex -= 1;
        }
    }

    public void repair()
    {
        if (Time.time > nextRepair)
        {
            isDestroyed = false;
            boards[currIndex].SetActive(true);
            currIndex += 1;
            scoreController.addScore(repairValue);
            nextRepair = Time.time + repairDelay;
        }
    }

    public bool getIsDestroyed()
    {
        return this.isDestroyed;
    }

    public bool getIsFullHealth()
    {
        return this.isFullHealth;
    }
}
