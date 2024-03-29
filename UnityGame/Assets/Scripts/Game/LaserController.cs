﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public Vector2 velocity = new Vector2(0.0f, 0.0f);
    public GameObject shooter;
    public Vector2 offset = new Vector2(0.0f, 0.0f);
    public AudioManager audioManager;

    void Update()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPosition = currentPosition + velocity * Time.deltaTime;
        Debug.DrawLine(currentPosition + offset, newPosition + offset, Color.red);

        RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition + offset, newPosition + offset);
        //Debug.Log("laser hits size:"+hits.Length);
        foreach (RaycastHit2D hit in hits)
        {
            GameObject other = hit.collider.gameObject;
            if (other != shooter)
            { // do the interaction here. 
              //Debug.Log("laser:"+other.tag);
                if (other.CompareTag("Teleporter"))
                {
                    Debug.Log("LaserController Teleporting");
                    Debug.Log("Before teleporting: " + transform.position);
                    GameObject temp = other.GetComponent<TeleporterScript>().teleportLaser(this.gameObject);
                    if(temp != null){
                        newPosition = temp.transform.position;
                    }
                    
                    break;
                    
                }
                if (other.CompareTag("Switch"))
                {
                    Debug.Log("Hit Switch");
                    audioManager.playSound("Laser Dissapate");
                    other.gameObject.GetComponent<SwitchScript>().ToggleSwitch();
                    Destroy(this.gameObject);
                    break;

                }
                if (other.CompareTag("TrailDot"))
                {

                    TrailDotController t = other.GetComponent<TrailDotController>();
                    t.sploder = shooter;

                    t.setExplode();

                    break;

                    /*
                    does colliding with a traildot destroy the laser?
                        -> I don't think that it should. 

                    if yes ->
                        Destroy(this.gameObject);
                        break; 

                    if no -> 
                        DoNothing(); 
                    */



                }
                if (other.CompareTag("Player"))
                {
                    //todo,
                    // have it look like the plasma is splashing aroud the players shield, especially if it isn't visiable, when hitting it. 
                    // https://www.youtube.com/watch?v=FFzyHDrgDc0
                    //

                    audioManager.playSound("Laser Hit");

                    Destroy(gameObject);
                    other.gameObject.GetComponent<PlayerController>().takeDamage(1, shooter);
                    break;
                }
                if (other.CompareTag("Enemy"))
                { // right now just the cannon.
                    audioManager.playSound("Laser Hit");
                    Destroy(gameObject);
                    if (shooter.CompareTag("Player"))
                    {
                        // player killed drone
                        PlayerController pc = shooter.GetComponent<PlayerController>();

                        other.gameObject.GetComponent<RobotDroneController>().takeDamage(1, pc.getPlayerIndex());
                        break;
                    }
                    else
                    {
                        // drone accidentally killed drone
                        other.gameObject.GetComponent<RobotDroneController>().takeDamage(1);
                        break;
                    }

                }

                if (other.CompareTag("SpaceMarble"))
                {
                    Destroy(other.gameObject);
                    break;
                }
                if (other.CompareTag("Environment"))
                {
                    audioManager.playSound("Laser Dissapate");
                    Destroy(this.gameObject);
                    break;
                }
                if (other.CompareTag("Shockwave"))
                { // if we don't want the shock wave to block things, remove this if tree
                    audioManager.playSound("Laser Dissapate");
                    Destroy(gameObject);
                    break;
                }
                if (other.CompareTag("BuilderWall"))
                {

                    BuilderWallController wallController = other.gameObject.GetComponent<BuilderWallController>();

                    if (wallController.builder == shooter)
                    {
                        break;
                    }
                    else
                    {
                        audioManager.playSound("Laser Dissapate");
                        Destroy(gameObject);
                        //other.gameObject.GetComponent<BuilderWallController>().takeDamage(1);
                        wallController.takeDamage(1);
                        break;

                    }
                }

            }
        }
        transform.position = newPosition;
    }
}