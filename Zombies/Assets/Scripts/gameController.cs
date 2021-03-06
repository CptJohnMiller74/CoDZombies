﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour {

    public PlayerHealth playerHealth;
    public GameObject enemy;
    public Transform[] spawns;
    public Text waveDisplay;
    public int maxEnemies;
    public Image gameOverImg;

    private int enemiesInWave;
    private int enemiesLeft;
    private int wave;
    private float spawnDelay;
    private int initialDelay;
    private int currentEnemies;
    private bool isNewWave;
    private bool isSpawning;
    //private float restartDelay = 5f;
    //private float restartTime;

	void Start () {

        initialDelay = 5;
        currentEnemies = 0;
        enemiesLeft = 0;
        wave = 0;
        spawnDelay = 5f;
        isNewWave = false;
        isSpawning = false;
        waveDisplay.text = "Wave " + 0;
    }

    void Update()
    {
        if (playerHealth.getIsDead())
        {
            return;
        }

        else if (enemiesLeft > 0 && currentEnemies < maxEnemies && !isSpawning)
        {
            StartCoroutine(spawn()); 
        }

        else if (enemiesLeft == 0 && currentEnemies == 0 && !isNewWave) 
        {
            StartCoroutine(newWave());
        }
    }

    public IEnumerator spawn()
    {
        isSpawning = true;
        currentEnemies += 1;
        enemiesLeft -= 1;
        yield return new WaitForSeconds(spawnDelay);
        int spawnIndex = Random.Range(0, spawns.Length);
        GameObject newEnemy = Instantiate(enemy, spawns[spawnIndex].position, spawns[spawnIndex].rotation);
        newEnemy.GetComponent<EnemyMovement>().setSpawn(spawns[spawnIndex].gameObject.transform.GetChild(0));
        newEnemy.GetComponent<EnemyAttack>().setSpawnTarget(spawns[spawnIndex].gameObject.transform.GetChild(0).gameObject);
        isSpawning = false;
    }

    public IEnumerator newWave()
    {
        isNewWave = true;
        yield return new WaitForSeconds(initialDelay);
        wave += 1;
        waveDisplay.text = "Wave " + wave;
        enemiesInWave = wave * 10;
        spawnDelay = 5f / wave;
        if (spawnDelay < 1)
        {
            spawnDelay = 1;
        }
        enemiesLeft = enemiesInWave;
        isNewWave = false;
    }

    public int getWave() //for the enemy health
    {
        return wave;
    }

    public int getEnemiesLeft()
    {
        return enemiesLeft;
    }

    public void setEnemiesLeft(int i)
    {
        enemiesLeft = i;
    }

    public int getCurrentEnemies()
    {
        return currentEnemies;
    }

    public void setCurrentEnemies(int i)
    {
        currentEnemies = i;
    }

}
