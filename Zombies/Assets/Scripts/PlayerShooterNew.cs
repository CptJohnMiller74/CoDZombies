
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
    public Image sniperScope;

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
    private Camera fpsCamera;
    private float startFov;
    private float zoomFov;
    private float startSensitivity;
    private float zoomSensitivity;
    private SkinnedMeshRenderer arms;

    void Awake()
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
        fpsCamera = GetComponentInParent<Camera>();
        startFov = fpsCamera.fieldOfView;
        zoomFov = startFov / 2.7f;
        startSensitivity = fpsCamera.GetComponent<mouseLook>().sensitivity;
        zoomSensitivity = startSensitivity / 2f;
        arms = GameObject.FindGameObjectWithTag("Arms").GetComponent<SkinnedMeshRenderer>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isHoldingGrenade && !activeGun.getIsReloading() && activeGun.getBulletsInMag() < activeGun.magazineSize && activeGun.getTotalBullets() > 0)
        {
            StartCoroutine(activeGun.reload());
        }

        if (activeGun.isFullAuto)
        {
            if (Input.GetButton("Fire1") && Time.time > nextFire && !playerMovement.getisSprinting())
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
        }

        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire && !playerMovement.getisSprinting())
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
        }


        if (Vector3.Distance(transform.localPosition, activeGun.aimPos) <= .0013f && activeGun.gameObject.tag == "L96Sniper")
        {
            sniperScope.gameObject.SetActive(true);
            fpsCamera.fieldOfView = zoomFov;
            fpsCamera.GetComponent<mouseLook>().sensitivity = zoomSensitivity;
            activeGun.GetComponent<MeshRenderer>().enabled = false;
            arms.gameObject.SetActive(false);
        }

        else if (transform.localPosition != activeGun.aimPos && activeGun.gameObject.tag == "L96Sniper" && sniperScope.gameObject.activeSelf)
        {
            sniperScope.gameObject.SetActive(false);
            fpsCamera.fieldOfView = startFov;
            fpsCamera.GetComponent<mouseLook>().sensitivity = startSensitivity;
            activeGun.GetComponent<MeshRenderer>().enabled = true;
            arms.gameObject.SetActive(true);
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
        gun tempGun = activeGun;
        activeGun = otherGun;
        otherGun = tempGun;
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
        otherGun = prevGun.GetComponent<gun>();
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

