using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gun : MonoBehaviour{

    public int gunDamage;
    public float verticalRecoil;
    public float horizontalRecoil;
    public float maxVerticalRecoil;
    public float maxHorizontalRecoil;
    public float fireRate;
    public Transform gunEnd;
    public int gunRange;
    public int magazineSize;
    public int maxReserve;
    public float reloadSpeed;
    public float adsSpeed;
    public Vector3 aimPos;
    public AudioClip emptyMag;
    public AudioClip reloadSound;
    public AudioClip shot;
    public AudioClip hitmarkerSound;
    public float minSpread;
    public float maxSpread;
    public float spreadPerShot;
    public float spreadResetDelay;
    public bool isFullAuto;
    public Vector3 gunStartPos;

    private crosshairController crosshairController;
    private scoreController scoreController;
    private PlayerMovement playerMovement;
    private float timeParam = 0.3f;
    private Animation anim;
    private AudioSource audio;
    private PlayerShooterNew playerShooter;
    private float spreadResetTime;
    private float currentSpread;
    private ParticleSystem flash;
    private Camera fpsCamera;
    private bool isReloading;
    private Vector3 startPos;
    private Quaternion startRotation;
    private int totalBullets;
    private bool isFiring;
    private int bulletsInMag;
    private int bulletsInReserve;
    private Vector3 recoilIncrement;
    private bool isAds;

    public void Start ()
    {
        playerShooter = GetComponentInParent<PlayerShooterNew>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        flash = GetComponentInChildren<ParticleSystem>();
        anim = GetComponentInParent<Animation>();
        fpsCamera = GetComponentInParent<Camera>();
        bulletsInMag = magazineSize;
        totalBullets = maxReserve + magazineSize;
        bulletsInReserve = maxReserve;
        currentSpread = minSpread;
        startPos = playerShooter.transform.localPosition;
        startRotation = playerShooter.transform.localRotation;
        audio = GetComponent<AudioSource>();
        crosshairController = GameObject.FindGameObjectWithTag("crosshairController").GetComponent<crosshairController>();
        scoreController = GameObject.FindGameObjectWithTag("ScoreController").GetComponent<scoreController>();
    }

    private void Update()
    {
        if (Time.time > spreadResetTime)
        {
            resetSpread();
        }

        resetRecoil();
    }

    public int getBulletsInReserve()
    {
        return this.bulletsInReserve;
    }

    public void fire()
    {
        Vector3 rayOrigin = fpsCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Vector3 rayEnd = fpsCamera.transform.forward;
        if (!isAds)
        {
            rayEnd.x += Random.Range(-currentSpread, currentSpread);
            rayEnd.y += Random.Range(-currentSpread, currentSpread);
            rayEnd.z += Random.Range(-currentSpread, currentSpread);
        }
        RaycastHit shotHit;
        //audio.Play();
        flash.Play();
        anim.Play();
        if (bulletsInMag > 0)
        {
            bulletsInMag -= 1;
        }
        audio.clip = shot;
        audio.Play();
        //gunLine.SetPosition(0, gunEnd.position);

        if (Physics.Raycast(rayOrigin, rayEnd, out shotHit, gunRange))
        {
            EnemyHealth enemyHealth = shotHit.collider.GetComponentInParent<EnemyHealth>();
            //gunLine.SetPosition(1, shotHit.point);

            if (enemyHealth != null && enemyHealth.getIsDead() == false)
            {
                enemyHealth.takeDamage(gunDamage, "bullet", shotHit);
                StartCoroutine(playerShooter.showHitmarker());
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

    public void ads()
    {
        if (Input.GetButton("Fire2") && !isReloading && !playerMovement.getisSprinting())
        {
            playerShooter.transform.localPosition = Vector3.Lerp(playerShooter.transform.localPosition, aimPos, Time.deltaTime * adsSpeed);
            crosshairController.clearCrosshairs();
            isAds = true;
        }

        else
        {
            playerShooter.transform.localPosition = Vector3.Lerp(playerShooter.transform.localPosition, startPos, Time.deltaTime * adsSpeed);
            //crosshairController.makeCrosshairsVisible();
            isAds = false;
        }
    }

    public void incrementSpread()
    {
        currentSpread += spreadPerShot;
        if (currentSpread > maxSpread)
        {
            currentSpread = maxSpread;
        }
    }

    public void resetSpread()
    {
        timeParam += Time.deltaTime * .003f;
        currentSpread = Mathf.Lerp(currentSpread, minSpread, Time.deltaTime * 2);//timeParam);
    }

    public void recoilUp()
    {
        recoilIncrement += new Vector3(-Random.Range(verticalRecoil / 2, verticalRecoil), Random.Range(-horizontalRecoil, horizontalRecoil), 0);
        Mathf.Clamp(recoilIncrement.y, -maxHorizontalRecoil, maxHorizontalRecoil);
        Mathf.Clamp(recoilIncrement.x, 0, maxVerticalRecoil);
    }

    public void resetRecoil()
    {
        recoilIncrement = Vector3.Lerp(recoilIncrement, Vector3.zero, Time.deltaTime * 2f);
    }

    public Vector3 getRecoilIncrement()
    {
        return this.recoilIncrement;
    }

    public void refillAmmo()
    {
        bulletsInReserve = maxReserve;
        totalBullets = bulletsInReserve + bulletsInMag;
    }

    public int getTotalBullets()
    {
        return totalBullets;
    }

    public int getBulletsInMag()
    {
        return bulletsInMag;
    }

    public bool getIsReloading()
    {
        return isReloading;
    }

    public int getMagazineSize()
    {
        return magazineSize;
    }

    public float getFireRate()
    {
        return fireRate;
    }

    public float getCurrentSpread()
    {
        return this.currentSpread;
    }

    public Vector3 getStartPos()
    {
        return this.startPos;
    }
}
