using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public Vector2 velocity = new Vector2(0.0f,0.0f);
    public GameObject shooter;
    public Vector2 offset = new Vector2(0.0f,0.0f);
    void Update()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPosition = currentPosition + velocity * Time.deltaTime; 
        Debug.DrawLine(currentPosition + offset, newPosition + offset, Color.red);
        
        RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition + offset, newPosition+ offset);
        foreach (RaycastHit2D hit in hits){
            GameObject other = hit.collider.gameObject;
            if(other != shooter){ // do the interaction here. 
                if(other.CompareTag("TrailDot")){
                    other.GetComponent<TrailDotController>().setExplode();  
                    
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
                if(other.CompareTag("Player")){
                    //todo,
                    // have it look like the plasma is splashing aroud the players shield, especially if it isn't visiable, when hitting it. 
                    // https://www.youtube.com/watch?v=FFzyHDrgDc0
                    //
                    Destroy(gameObject);
                    other.gameObject.GetComponent<PlayerController>().takeDamage(1);
                    break; 
                } 
                if(other.CompareTag("Enemy")){ // right now just the cannon. 
                    Destroy(gameObject);
                    other.gameObject.GetComponent<RobotDroneController>().takeDamage(1);
                    break; 
                }

                if(other.CompareTag("Environment")){
                    Destroy(this.gameObject);
                    break;
                }
                if(other.CompareTag("Shockwave")){ // if we don't want the shock wave to block things, remove this if tree
                    Destroy(gameObject);
                    break; 
                }
                if(other.CompareTag("BuilderWall")){
                    Destroy(gameObject);
                    other.gameObject.GetComponent<BuilderWallController>().takeDamage(1);

                    break; 
                }
            }
        }
        transform.position = newPosition; 
    }
}
