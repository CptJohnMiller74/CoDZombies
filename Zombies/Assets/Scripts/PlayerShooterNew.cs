
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooterNew : MonoBehaviour
{

    public PlayerMovement playerMovement;
    public scoreController scoreController;
    public Transform grenadeSpawn;
    public Text grenadeCount;
    public Text bulletCount;
    public GameObject grenadeObj;
    public int maxGrenades;
    public AudioClip hitmarkerSound;
    public Image hitmarker;
    public crosshairController crosshairController;

    private AudioSource audio;
    private gun activeGun;
    private gun otherGun;
    private bool isAds;
    private int numGrenades;
    private float nextFire;
    private bool isFiring;
    private int totalBullets;
    private bool isHoldingGrenade;
    private Transform model;
    private Vector3 startPos;
    private Quaternion startRotation;

    void Start()
    {
        activeGun = GetComponentInChildren<gun>();
        otherGun = null;
        bulletCount.text = activeGun.getBulletsInMag() + " / " + totalBullets;
        numGrenades = maxGrenades;
        grenadeCount.text = numGrenades.ToString();
        audio = GetComponent<AudioSource>();
        isHoldingGrenade = false;
        model = GameObject.FindGameObjectWithTag("model").transform;
        startPos = model.localPosition;
        startRotation = model.localRotation;
    }


    void Update()
    {


        if (Input.GetKeyDown(KeyCode.R) && !isHoldingGrenade && !activeGun.getIsReloading() && activeGun.getBulletsInMag() < activeGun.magazineSize && activeGun.getTotalBullets() > 0)
        {
            StartCoroutine(activeGun.reload());
        }

        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            if (activeGun.getTotalBullets() <= 0 && activeGun.getBulletsInMag() <= 0)
            {
                activeGun.emptyReload();
            }

            else if (activeGun.getBulletsInMag() <= 0 && !activeGun.getIsReloading())
            {
                StartCoroutine(activeGun.reload());
            }

            else if (!activeGun.getIsReloading())
            {
                isFiring = true;
                nextFire = Time.time + activeGun.fireRate;
                activeGun.incrementSpread();
                activeGun.fire();
                isFiring = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && !activeGun.getIsReloading() && !isAds && !isHoldingGrenade && numGrenades > 0)
        {
            Instantiate(grenadeObj, grenadeSpawn.position, Quaternion.identity);
            isHoldingGrenade = true;
            numGrenades -= 1;
        }

        if (Input.GetKeyDown(KeyCode.Q) && otherGun != null)
        {
            switchWeapons();
        }

        if (!activeGun.getIsReloading())
        {
            model.localPosition = startPos;
            model.localRotation = startRotation;
        }

        activeGun.ads();
        updateText();
    }

    public void updateText()
    {
        bulletCount.text = activeGun.getBulletsInMag() + " / " + activeGun.getBulletsInReserve();
        grenadeCount.text = numGrenades.ToString();
    }

    public void switchWeapons()
    {
        otherGun.gameObject.SetActive(true);
        activeGun.gameObject.SetActive(false);
        activeGun = otherGun;
        otherGun = activeGun;
    }

    public IEnumerator showHitmarker()
    {
        hitmarker.color = Color.white;
        audio.PlayOneShot(hitmarkerSound, 0.7f);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        hitmarker.color = Color.clear;
    }

    public gun getActiveGun()
    {
        return this.activeGun;
    }

    public void setActiveGun(gun newActiveGun)
    {
        activeGun.gameObject.SetActive(false);
        GameObject prevGun = activeGun.gameObject;
        Destroy(prevGun, 2f);
        newActiveGun.GetComponent<gun>().enabled = true;
        this.activeGun = newActiveGun;
        activeGun.transform.localPosition = activeGun.gunStartPos;
    }

    public bool getIsHoldingGrenade()
    {
        return this.isHoldingGrenade;
    }

    public void setIsHoldingGrenade(bool b)
    {
        this.isHoldingGrenade = b;
    }
}

