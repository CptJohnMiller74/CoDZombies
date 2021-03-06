﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour {

    public float sensitivity;
    public float smoothing;

    private Vector3 mouseLookV;
    private Vector3 smoothingV;
    private Transform player;
    private PlayerShooterNew playerShooter;
    private Vector3 recoilVelocity = Vector3.zero;
    private Transform gunModel;
    private bool isTurningLeft;
    private bool isTurningRight;

    void Start () {
        player = this.transform.parent.gameObject.transform;
        playerShooter = GetComponentInChildren<PlayerShooterNew>();
        gunModel = GameObject.FindGameObjectWithTag("armsAndGun").transform;
	}
	
	
	void Update () {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        //Debug.Log(mouseDelta.x);
        if (mouseDelta.x < 0)
        {
            isTurningLeft = true;
        }
        else
        {
            isTurningLeft = false;
        }
        if (mouseDelta.x > 0)
        {
            isTurningRight = true;
        }
        else
        {
            isTurningRight = false;
        }
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothingV.x = Mathf.Lerp(smoothingV.x, mouseDelta.x, 1f / smoothing);
        smoothingV.y = Mathf.Lerp(smoothingV.y, mouseDelta.y, 1f / smoothing);
        mouseLookV += smoothingV;

        mouseLookV.y = Mathf.Clamp(mouseLookV.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(-mouseLookV.y, Vector3.right);

        //transform.localEulerAngles += playerShooter.getRecoilIncrement();
        transform.localEulerAngles = Vector3.SmoothDamp(transform.localEulerAngles, transform.localEulerAngles + playerShooter.getActiveGun().getRecoilIncrement(), ref recoilVelocity, 0.04f);

        player.localRotation = Quaternion.AngleAxis(mouseLookV.x, player.up);
    }

    public bool getIsTurningLeft()
    {
        return isTurningLeft;
    }

    public bool getIsTurningRight()
    {
        return isTurningRight;
    }
}
