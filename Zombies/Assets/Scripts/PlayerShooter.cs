/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour {

    public int gunDamage;
    public float verticalRecoil;
    public float horizontalRecoil;
    public float maxVerticalRecoil;
    public float maxHorizontalRecoil;
    public float fireRate;
    public Transform gunEnd;
    public Transform grenadeSpawn;
    public float gunRange;
    public int magazineSize;
    public int maxReserve;
    public float reloadSpeed;
    public AudioClip emptyMag;
    public AudioClip reloadSound;
    public AudioClip shot;
    public AudioClip hitmarkerSound;
    public float adsSpeed;
    public Vector3 aimPos;
    public Text bulletCount;
    public Text grenadeCount;
    public Image hitmarker;
    public PlayerMovement playerMovement;
    public scoreController scoreController;
    public float maxSpread;
    public float minSpread;
    public float spreadPerShot;
    public float spreadResetDelay;
    public GameObject grenadeObj;
    public crosshairController crosshairController;
    public int maxGrenades;

    //private gun activeGun;
    // private gun otherGun;

    private int numGrenades;
    private float spreadResetTime;
    private float currentSpread;
    private Transform model;
    private ParticleSystem flash;
    private Animation anim;
    private Camera fpsCamera;
    private AudioSource audio;
    private float nextFire;
    public int bulletsInMag;
    private bool isReloading;
    private Vector3 startPos;
    private Quaternion startRotation;
    private bool isFiring;
    private int totalBullets;
    private int bulletsInReserve;
    private bool isAds;
    private LineRenderer gunLine;
    private bool isHoldingGrenade;
    private RectTransform hudCanvas;
    private float timeParam = 0.3f;
    private Vector3 recoilIncrement;
    private float recoilTimeParam = 0.3f;
    //private Vector3 totalRecoil;

    void Start () {
        fpsCamera = GetComponentInParent<Camera>();
        audio = GetComponent<AudioSource>();
        flash = GetComponentInChildren<ParticleSystem>();
        anim = GetComponent<Animation>();
        model = GameObject.FindGameObjectWithTag("model").transform;
        bulletsInMag = magazineSize;
        startPos = model.localPosition;
        startRotation = model.localRotation;
        totalBullets = maxReserve + magazineSize;
        bulletCount.text = bulletsInMag + " / " + maxReserve;
        bulletsInReserve = maxReserve;
        gunLine = GetComponent<LineRenderer>();
        isAds = false;
        hudCanvas = hitmarker.gameObject.GetComponent<RectTransform>();
        currentSpread = minSpread;
        numGrenades = maxGrenades;
        grenadeCount.text = numGrenades.ToString();
    }
	

	void Update () {


        if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isHoldingGrenade && bulletsInMag < magazineSize && totalBullets > 0)
        {
            StartCoroutine(reload());
        }

        if (Input.GetButton("Fire1") && Time.time > nextFire && !isHoldingGrenade)
        {
            if (totalBullets <= 0 && bulletsInMag <= 0)
            {
                emptyReload();
            }

            else if (bulletsInMag <= 0 && !isReloading)
            {
                StartCoroutine(reload());               
            }

            else if (!isReloading && !playerMovement.getisSprinting())
            {
                spreadResetTime = Time.time + spreadResetDelay;
                isFiring = true;
                incrementSpread();
                fire();
                isFiring = false;
            }
        }

        if (Time.time > spreadResetTime)
        {
            resetSpread();
        }

        if (Input.GetKeyDown(KeyCode.G) && !isReloading && !isAds && !isHoldingGrenade && numGrenades > 0)
        {
            Instantiate(grenadeObj, grenadeSpawn.position, Quaternion.identity);
            isHoldingGrenade = true;
            numGrenades -= 1;
        }

        if (!isReloading)
        {
            model.localPosition = startPos;
            model.localRotation = startRotation;
        }

        ads();
        updateText();
        resetRecoil();
    }

    public bool getIsAds()
    {
        return isAds;
    }

    public int getTotalBullets()
    {
        return totalBullets;
    }

    public float getCurrentSpread()
    {
        return currentSpread;
    }

    public bool getIsHoldingGrenade()
    {
        return isHoldingGrenade;
    }

    public void setIsHoldingGrenade(bool x)
    {
        isHoldingGrenade = x;
    }

    public void updateText()
    {
        bulletCount.text = bulletsInMag + " / " + bulletsInReserve;
        grenadeCount.text = numGrenades.ToString();
    }

    public void fire()
    {
        nextFire = Time.time + fireRate;// activeGun.getFireRate();
        Vector3 rayOrigin = fpsCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Vector3 rayEnd = fpsCamera.transform.forward;
        if (!isAds)
        {
            rayEnd.x += Random.Range(-currentSpread, currentSpread);
            rayEnd.y += Random.Range(-currentSpread, currentSpread);
            rayEnd.z += Random.Range(-currentSpread, currentSpread);
        }
        RaycastHit shotHit;
        flash.Play();
        anim.Play();
        if (bulletsInMag > 0)
        {
            bulletsInMag -= 1;
        }
        audio.clip = shot;
        audio.Play();
        gunLine.SetPosition(0, gunEnd.position);

        if (Physics.Raycast(rayOrigin, rayEnd, out shotHit, gunRange))
        {
            EnemyHealth enemyHealth = shotHit.collider.GetComponentInParent<EnemyHealth>();
            gunLine.SetPosition(1, shotHit.point);

            if (enemyHealth != null && enemyHealth.getIsDead() == false)  
            {
                enemyHealth.takeDamage(gunDamage, "bullet", shotHit);
                StartCoroutine(showHitmarker());
                
            }
        }

        if (isAds)
        {
            recoilUp();
        }
    }

    public void emptyReload()
    {
        audio.clip = emptyMag;
        audio.Play();
        return;
    }

    public IEnumerator reload()
    {
        anim.Play("Reload");
        audio.clip = reloadSound;
        audio.Play();
        isReloading = true;
        yield return new WaitForSeconds(anim["Reload"].length);
        isReloading = false;
        model.position = startPos;
        model.rotation = startRotation;

        int bulletsUsed = magazineSize - bulletsInMag;

        if (totalBullets <= bulletsUsed)
        {
            bulletsInMag += totalBullets;
            totalBullets = 0;
        }
        else
        {
            totalBullets -= bulletsUsed;
            bulletsInReserve -= bulletsUsed;
            bulletsInMag = magazineSize;
        }

        anim.PlayQueued("Idle");
    }

    public IEnumerator showHitmarker()
    {
        hitmarker.color = Color.red;
        audio.PlayOneShot(hitmarkerSound, 0.7f);
        //audio.Play();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        hitmarker.color = Color.clear;
    }

    public void ads()
    {
        if (Input.GetButton("Fire2") && !isReloading && !playerMovement.getisSprinting())
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPos, Time.deltaTime * adsSpeed);
            crosshairController.clearCrosshairs();
            isAds = true;
        }

        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * adsSpeed);
            crosshairController.makeCrosshairsVisible();
            isAds = false;
        }
    }

    public void incrementSpread()
    {
        currentSpread += spreadPerShot;
        if (currentSpread > maxSpread) {
            currentSpread = maxSpread;
        }
    }

    public void resetSpread()
    {
        timeParam += Time.deltaTime * .03f;
        currentSpread = Mathf.Lerp(currentSpread, minSpread, timeParam);
    }

    public void refillAmmo()
    {
        bulletsInReserve = maxReserve;
        totalBullets = bulletsInReserve + bulletsInMag;
    }

    public void recoilUp()
    {
        recoilIncrement += new Vector3 (-Random.Range(verticalRecoil / 2, verticalRecoil), Random.Range(-horizontalRecoil, horizontalRecoil), 0);
        Mathf.Clamp(recoilIncrement.y, -maxHorizontalRecoil, maxHorizontalRecoil);
        Mathf.Clamp(recoilIncrement.x, 0, maxVerticalRecoil);
    }

    public void resetRecoil()
    {
        //recoilTimeParam += Time.deltaTime * .000005f;
        recoilIncrement = Vector3.Lerp(recoilIncrement, Vector3.zero, Time.deltaTime * 2f);
    }

    public Vector3 getRecoilIncrement()
    {
        return this.recoilIncrement;
    }
}
*/