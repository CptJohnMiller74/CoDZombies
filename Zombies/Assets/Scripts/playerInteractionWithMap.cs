using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerInteractionWithMap : MonoBehaviour {

    public Text alertText;
    public scoreController scoreController;
    public int AKAmmoCost;
    public int AKCost;
    public int M4AmmoCost;
    public int M4Cost;
    public GameObject AK47;
    public GameObject M4;

    private PlayerShooterNew playerShooter;
    private string activeGun;
    private string otherGun;
    private bool inRange;
    private bool inRangeAK;
    private bool inRangeM4;
    private int holdTime;
    private float triggerTime;
    private bool isHolding;
    private GameObject weaponInInteractionRange;

	void Start () {
        playerShooter = GetComponentInChildren<PlayerShooterNew>();
        activeGun = playerShooter.getActiveGun().gameObject.tag;
        alertText.text = "";
        holdTime = 1;
        isHolding = false;
	}

	void Update () {
		
        if (Input.GetKeyDown(KeyCode.F) && inRange)
        {
            triggerTime = Time.time + holdTime;
            isHolding = true;
        }

        if (Input.GetKey(KeyCode.F) && Time.time >= triggerTime && inRange && isHolding)
        {
            if (inRangeAK)
            {
                interactAK();
                isHolding = false;
            }

            if (inRangeM4)
            {
                interactM4();
                isHolding = false;
            }
        }
        //Debug.Log(weaponInInteractionRange);
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AKPickup")
        {
            inRange = true;
            inRangeAK = true;
            if (activeGun == "AK47" || otherGun == "AK47")
            {
                alertText.text = "Hold F to refill ammo (" + AKAmmoCost.ToString() + ")";
            }

            else
            {
                alertText.text = "Hold F to buy AK-47 (" + AKCost + ")";
            }
        }

        else if (other.tag == "M4Pickup")
        {
            inRange = true;
            inRangeM4 = true;
            if (activeGun == "M4" || otherGun == "M4")
            {
                alertText.text = "Hold F to refill ammo (" + M4AmmoCost.ToString() + ")";
            }

            else
            {
                alertText.text = "Hold F to buy M4 (" + M4Cost + ")";
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "AKPickup")
        {
            inRangeAK = false;
            alertText.text = "";
        }

        else if (other.tag == "M4Pickup")
        {
            inRangeM4 = false;
            alertText.text = "";
        }

    }

    public void interactAK()
    {
        if (activeGun == "AK47" && playerShooter.getActiveGun().getTotalBullets() < playerShooter.getActiveGun().maxReserve && scoreController.getScore() > AKAmmoCost)
        {
            scoreController.spendScore(AKAmmoCost);
            playerShooter.getActiveGun().refillAmmo();
        }

        else if (scoreController.getScore() > AKCost)
        {
            scoreController.spendScore(AKCost);
            activeGun = "AK47";
            playerShooter.setActiveGun(Instantiate(AK47, GameObject.FindGameObjectWithTag("model").transform).GetComponent<gun>());
        }
    }

    public void interactM4()
    {
        if (activeGun == "M4" && playerShooter.getActiveGun().getTotalBullets() < playerShooter.getActiveGun().maxReserve && scoreController.getScore() > M4AmmoCost)
        {
            scoreController.spendScore(M4AmmoCost);
            playerShooter.getActiveGun().refillAmmo();
        }

        else if (scoreController.getScore() > M4Cost)
        {
            scoreController.spendScore(M4Cost);
            activeGun = "M4";
            playerShooter.setActiveGun(Instantiate(M4, GameObject.FindGameObjectWithTag("model").transform).GetComponent<gun>());
        }
    }

    public void setActiveGunString(string s)
    {
        this.activeGun = s;
    }
}
