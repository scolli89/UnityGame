﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HealingLaserController : MonoBehaviour
{
    /*

     only difference between this script and the arrow controller is
     that when an object with laserScript 'collides' with a Player or enemy,
      they give 

    */
    public Vector2 velocity = new Vector2(0.0f, 0.0f);
    public GameObject shooter;
    public Vector2 offset = new Vector2(0.0f, 0.0f);
    void Update()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPosition = currentPosition + velocity * Time.deltaTime;
        Debug.DrawLine(currentPosition + offset, newPosition + offset, Color.red);

        RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition + offset, newPosition + offset);
        foreach (RaycastHit2D hit in hits)
        {
            GameObject other = hit.collider.gameObject;
            // do the interaction here. 
            if (other.CompareTag("Player"))
            {

                //todo,
                // have it look like the plasma is splashing aroud the players shield, especially if it isn't visiable, when hitting it. 
                // https://www.youtube.com/watch?v=FFzyHDrgDc0
                //
                Destroy(gameObject);
                Debug.Log(other.name);
                other.gameObject.GetComponent<PlayerController>().rechargeEnergyFull();
                break;
            }
            if (other.CompareTag("Enemy"))
            { // right now just the cannon. 
                Destroy(gameObject);
                Debug.Log(other.name);
                break;
            }

            if (other.CompareTag("Environment"))
            {
                Destroy(gameObject);
                break;
            }
            if (other.CompareTag("Shockwave"))
            { // if we don't want the shock wave to block things, remove this if tree
                Destroy(gameObject);
                break;
            }
            if (other.CompareTag("BuilderWall"))
            {
                Destroy(gameObject);
                other.gameObject.GetComponent<BuilderWallController>().takeDamage(-1);
                break;
            }





            Debug.Log(hit.collider.gameObject);
        }
        transform.position = newPosition;
    }
}

