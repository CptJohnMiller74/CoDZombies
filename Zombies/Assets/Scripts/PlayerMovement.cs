﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;
    public crosshairController crosshairController;
    public float jumpSpeed;

    private Vector3 movement;
    private bool isSprinting;
    private Rigidbody rb;
    private bool isJumping;
    private bool isMovingHorizontal;
    private bool isMoving;
    private bool isInEnemySpawn;
    private gunSwayController gunSway;

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        isJumping = false;
        gunSway = GetComponentInChildren<gunSwayController>();
    }

    public void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == 0 && vertical == 0)
        {
            isMoving = false;
        }

        else
        {
            isMoving = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
            crosshairController.makeCrosshairsVisible();
        }

        if (Input.GetKey(KeyCode.LeftShift) && (horizontal != 0 || vertical < 0))
        {
            gunSway.resetGun();
            crosshairController.makeCrosshairsVisible();
        }

        if (horizontal != 0)
        {
            isMovingHorizontal = true;
        }

        else
        {
            isMovingHorizontal = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && vertical > 0 && horizontal == 0)
        {
            isSprinting = true;
            crosshairController.clearCrosshairs();
        }

        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            jump();
        }

        if (isJumping && rb.velocity.y <0)
        {
            rb.AddForce(Vector3.down * 900);
        }

        move(horizontal, vertical);
    }

    public bool getisSprinting()
    {
        return isSprinting;
    }

    public void move(float h, float v)
    {
        movement.Set(h, 0.0f, v);
        if (isSprinting && v > 0)
        {
            movement = movement.normalized * (speed * 1.5f) * Time.deltaTime;
            movement.x /= 2;
        }
        else
        {
            movement = movement.normalized * speed * Time.deltaTime;
        }

        transform.Translate(movement);

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

    public void jump()
    {
        isJumping = true;
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
    }

    public void OnCollisionStay(Collision collision)
    {
        isJumping = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.down);
    }

    public bool getIsMovingHorizontal()
    {
        return isMovingHorizontal;
    }

    public bool getIsMoving()
    {
        return isMoving;
    }

    public bool getIsInEnemySpawn()
    {
        return this.isInEnemySpawn;
    }
}
