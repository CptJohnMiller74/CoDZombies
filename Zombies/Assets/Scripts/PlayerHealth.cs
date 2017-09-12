using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int startingHP = 100;
    public Image damageImg;
    public float flashSpeed = 5f;
    public float regenDelay = 5f;
    public mouseLook fpsCamera;

    private  Color damageColor = new Color(1f, 0f, 0f, 0.3f);
    private PlayerMovement playerMovement;                              
    private PlayerShooterNew playerShooter;
    private AudioSource death;
    private bool isDead;                                                
    private bool damaged;
    private float startRegen;
    private int currentHP;

    void Start () {
        currentHP = startingHP;
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponentInChildren<PlayerShooterNew>();
        death = GetComponent<AudioSource>();
    }

    void Update () {
		
        if (damaged)
        {
            startRegen = Time.time + regenDelay;
            damageImg.color = damageColor;
        }

        if (Time.time > startRegen)
        {
            currentHP = startingHP;
            damageImg.color = Color.Lerp(damageImg.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;

        if (currentHP <= 0)
        {
            die();
        }
	}

    public int getCurrentHP()
    {
        return currentHP;
    }

    public bool getIsDead()
    {
        return isDead;
    }

    public void takeDamage(int attack)
    {
        currentHP -= attack;
        damaged = true;
    }

    public void die()
    {
        isDead = true;
        playerMovement.enabled = false;
        playerShooter.enabled = false;
        death.Play();
        fpsCamera.enabled = false;
        this.enabled = false;
        gameObject.GetComponentInChildren<gunSwayController>().enabled = false;
    }
}
