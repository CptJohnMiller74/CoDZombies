using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunSwayController : MonoBehaviour {

    public float moveAmount;
    public float moveSpeed;
    public Vector3 leftPos;
    public Vector3 leftRotation;
    public Vector3 rightPos;
    public Vector3 rightRotation;
    public float swayDelay;

    private float swayStart;
    private Vector3 startPos;
    private Quaternion startRotation = Quaternion.identity;
    private PlayerShooterNew playerShooter;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private PlayerMovement playerMovement;
    private float sprintSmoother;
    private float moveSmoother;
    private float sprintSwaySpeed;

    void Start () {
        startPos = transform.localPosition;
        playerShooter = GetComponentInChildren<PlayerShooterNew>();
        targetPosition = leftPos;
        targetRotation = Quaternion.Euler(leftRotation);
        playerMovement = GetComponentInParent<PlayerMovement>();
        sprintSwaySpeed = playerMovement.speed / 12;
	}
	
	
	void Update () {

        if (!playerMovement.getisSprinting() && Time.time >= swayStart)
        {
            sprintSmoother = 0;
            swayGun();
        }

        if (playerMovement.getisSprinting() && playerMovement.getIsMoving())
        {
            sprintSway();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            swayStart = Time.time + swayDelay;
            targetPosition = leftPos;
            targetRotation = Quaternion.Euler(leftRotation);
        }

        if (Time.time < swayStart)
        {
            resetGun();
        }
    }

    public void swayGun()
    {
        float moveOnX = Input.GetAxis("Mouse X") * Time.deltaTime * moveAmount;
        float moveOnY = Input.GetAxis("Mouse Y") * Time.deltaTime * moveAmount;
        if (playerShooter.getActiveGun().getIsAds())
        {
            moveOnX /= 4;
            moveOnY /= 4;
        }
        Vector3 newPos = new Vector3(startPos.x + moveOnX, startPos.y + moveOnY, startPos.z);
        newPos = Vector3.ClampMagnitude(newPos, 1.6f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, moveSpeed * Time.deltaTime);
    }

    public void resetGun()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Mathf.SmoothStep(0.0f, 1.0f, sprintSmoother));
        transform.localRotation = Quaternion.Lerp(transform.localRotation, startRotation, Mathf.SmoothStep(0.0f, 1.0f, sprintSmoother));
        sprintSmoother += Time.deltaTime / sprintSwaySpeed;
    }

    public void sprintSway()
    {
        if (transform.localPosition == targetPosition && targetPosition == leftPos)
        {
            targetPosition = rightPos;
            targetRotation = Quaternion.Euler(rightRotation);
            sprintSmoother = 0;
        }

        else if (transform.localPosition == targetPosition && targetPosition == rightPos)
        {
            targetPosition = leftPos;
            targetRotation = Quaternion.Euler(leftRotation);
            sprintSmoother = 0;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Mathf.SmoothStep(0.0f, 1.0f, sprintSmoother));
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Mathf.SmoothStep(0.0f, 1.0f, sprintSmoother));
        sprintSmoother += Time.deltaTime / sprintSwaySpeed;
    }
}
