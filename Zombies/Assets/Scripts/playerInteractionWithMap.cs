using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerInteractionWithMap : MonoBehaviour {

    public Text alertText;
    public scoreController scoreController;

    private PlayerShooterNew playerShooter;
    private string activeGun;
    private string otherGun;
    private bool inRangeToInteractWithGun;
    private float holdTime;
    private float triggerTime;
    private bool isHolding;
    private bool inRangeToInteractWithSpawn;
    private gunPickup targetPickup;
    private spawnWindow targetSpawn;

    void Start () {
        playerShooter = GetComponentInChildren<PlayerShooterNew>();
        alertText.text = "";
        holdTime = 1f;
        isHolding = false;
	}

	void Update () {

        activeGun = playerShooter.getActiveGun().gameObject.tag;

        if (playerShooter.getOtherGun() != null)
        {
            otherGun = playerShooter.getOtherGun().gameObject.tag;
        }

        if (Input.GetKeyDown(KeyCode.F) && (inRangeToInteractWithGun || inRangeToInteractWithSpawn))
        {
            triggerTime = Time.time + holdTime;
            isHolding = true;
        }

        if (Input.GetKey(KeyCode.F) && Time.time >= triggerTime && inRangeToInteractWithGun && isHolding)
        {
            interactWithGunPickup();
            isHolding = false;
        }

        else if (Input.GetKey(KeyCode.F) && Time.time >= triggerTime && inRangeToInteractWithSpawn && isHolding)
        {
            if (!targetSpawn.getIsFullHealth())
            {
                targetSpawn.repair();
            }
        }

        if (targetSpawn != null && targetSpawn.getIsFullHealth() && inRangeToInteractWithSpawn)
        {
            alertText.text = "";
        }
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "gunPickup")
        {
            inRangeToInteractWithGun = true;
            targetPickup = other.gameObject.GetComponent<gunPickup>();
            if (activeGun == targetPickup.gunName || otherGun == targetPickup.gunName)
            {
                alertText.text = "Hold F to refill ammo (" + targetPickup.ammoCost.ToString() + ")";
            }

            else
            {
                alertText.text = "Hold F to buy " + targetPickup.gunName + " (" + targetPickup.gunCost.ToString() + ")";
            }
        }

        if (other.tag == "SpawnWindow")
        {
            inRangeToInteractWithSpawn = true;
            targetSpawn = other.gameObject.GetComponent<spawnWindow>();

            if (!targetSpawn.getIsFullHealth())
            {
                alertText.text = "Hold F to repair";
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "gunPickup")
        {
            inRangeToInteractWithGun = false;
            targetPickup = null;
            alertText.text = "";
        }

        if (other.tag == "SpawnWindow")
        {
            inRangeToInteractWithSpawn = false;
            targetSpawn = null;
            alertText.text = "";
        }
    }

    public void interactWithGunPickup()
    {
        if ((activeGun == targetPickup.gunName && playerShooter.getActiveGun().getTotalBullets() < playerShooter.getActiveGun().getMaxBullets()) || (otherGun == targetPickup.gunName && playerShooter.getOtherGun().getTotalBullets() < playerShooter.getOtherGun().getMaxBullets()) && scoreController.getScore() >= targetPickup.ammoCost)
        {
            scoreController.spendScore(targetPickup.ammoCost);
            if (activeGun == targetPickup.gunName)
            {
                playerShooter.getActiveGun().refillAmmo();
            }

            else if (otherGun == targetPickup.gunName)
            {
                playerShooter.getOtherGun().refillAmmo();
            }

        }

        else if (scoreController.getScore() >= targetPickup.gunCost && activeGun != targetPickup.gunName && otherGun != targetPickup.gunName)
        {
            scoreController.spendScore(targetPickup.gunCost);
            activeGun = targetPickup.gunName;
            Debug.Log(activeGun);
            playerShooter.setActiveGun(Instantiate(targetPickup.gun, GameObject.FindGameObjectWithTag("model").transform).GetComponent<gun>());
        }
    }

    public void setActiveGunString(string s)
    {
        this.activeGun = s;
    }
}
