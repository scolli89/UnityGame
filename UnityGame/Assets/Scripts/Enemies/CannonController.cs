﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    // Start is called before the first frame update
    [Space]
    [Header("Cannon Statistics:")]
    public Vector2 shootingDirection;
    public bool shootAtPlayer;
    public float fireRate; // in frames 
    public bool justFired;
    private float count;

    [Space]
    [Header("Enemy Attributes:")]

    public float ARROW_BASE_SPEED = 1.0f;
    public float ARROW_DOWN_OFFSET = 1.0f;
    public float DISTANCE_FROM_PLAYER = 1.0f;
    public float ARROW_OFFSET = 1.2f;



    [Space]
    [Header("References:")]
    private Rigidbody2D rb;
    public GameObject player;

    [Space]
    [Header("Prefabs:")]
    public GameObject projectilePrefab;


    void Start()
    {

    }

    // Update is called once per frame, but it is tied to frame rate. 
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            float deg = 0.0f;

            if (shootAtPlayer)
            {

                float opposite;
                float ajacent;
                opposite = player.transform.position.x - transform.position.x;
                ajacent = player.transform.position.y - transform.position.y;
                float rot;
                rot = Mathf.Atan(opposite / ajacent);
                if (ajacent < 0)
                {
                    rot = -rot;
                }
                else
                {
                    rot = Mathf.PI / 2 + ((Mathf.PI / 2) - rot);
                }
                deg = rot * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, deg);
                // transform.Rotate(rota//);
            }

            if (justFired)
            {
                if (justFired)
                {

                    if (count < fireRate)
                    {
                        count++;
                    }
                    else
                    {
                        count = 0;
                        justFired = false;
                    }

                }
                else
                {
                    count = 0;
                    justFired = false;
                }

            }
            else
            {
                if (checkDistance())
                {
                    fire(deg);
                    justFired = true;
                }
            }
        }
    }

    bool checkDistance()
    {

        //if player is too close, fire arrow down. 
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < DISTANCE_FROM_PLAYER &&
                Mathf.Abs(player.transform.position.y - transform.position.y) < DISTANCE_FROM_PLAYER)
        {
            return true;
        }
        // otherwise 
        return false;
    }


    void fire(float degree)
    {


        Vector2 iPosition = transform.position;
        if (shootingDirection.y < 0)
        { //down
            iPosition.y = iPosition.y - ARROW_DOWN_OFFSET;
        }
        else if (shootingDirection.y > 0)
        { // up
            iPosition.y = iPosition.y + ARROW_DOWN_OFFSET;
        }
        if (shootingDirection.x < 0)
        {
            iPosition.x = iPosition.x - ARROW_DOWN_OFFSET;
        }
        else if (shootingDirection.x > 0)
        {
            iPosition.x = iPosition.x - ARROW_DOWN_OFFSET;
        }

        iPosition = transform.position;
        if (shootAtPlayer)
        {
            // aim at make ball go towards player direction. 
            // rotate the sprite to look at player. 

            shootingDirection = new Vector2(player.transform.position.x,player.transform.position.y) - iPosition;
            shootingDirection.Normalize();

        }

       

        iPosition = iPosition + shootingDirection * ARROW_OFFSET; // this prevents it from hitting the player


        GameObject arrow = Instantiate(projectilePrefab, iPosition, Quaternion.identity);
        LaserController arrowController = arrow.GetComponent<LaserController>();
        arrowController.shooter = gameObject;
        arrowController.velocity = shootingDirection * ARROW_BASE_SPEED; // adjust velocity
        arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        Destroy(arrow, 2.0f);

        // the next thing long run i see happening is that we can use this script for enemy decision. 
        //but in addition to being close enough, the player needs to be within an invisible game object called like their line of sight or something. 



        // fires arrow Vector3 iPosition = transform.position; 



    }

}
