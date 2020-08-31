using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailDotController : MonoBehaviour
{
    // Start is called before the first frame update
    // public GameObject nextDot;
    // public GameObject prevDot;
    public GameObject explosionEffect;
    public GameObject sploder; 

    private bool explode = false; // trigger to explode this gameObject. 
    private bool hasExploded = false; 
    private float countDown; // number of frames before it starts to explode. 
    public float delay = 0.1f; 
    
    public float EXPLOSION_RADIUS = 0.4f;
    public float DESTROY_DOT_TIME = 15f;
    public int PLY_DMG = 2; 
    public int WALL_DMG = 1;

    public AudioManager audioManager;

    void Start()
    {
        countDown = delay; 
        Destroy(this.gameObject,DESTROY_DOT_TIME); 
    }

    // Update is called once per frame
    void Update()
    {
        if (explode)
        {
            countDown -=Time.deltaTime; 
            if(countDown <= 0 && !hasExploded){
                BrakeysExplode(); 
            }
        }
    }

    // void Explode()
    // {
    //     Vector2 thisPos = Vector2.zero;
    //     if (nextDot != null)
    //     {
    //         Vector2 nextPos = new Vector2(nextDot.transform.position.x, nextDot.transform.position.y);
    //         thisPos = new Vector2(this.transform.position.x, this.transform.position.y);

    //         Vector2 difference = thisPos - nextPos;

    //         if (difference.magnitude <= EXPLOSISON_DISTANCE)
    //         {
    //             // if the next dot is within the explosion radius. 
    //             nextDot.GetComponent<TrailDotController>().setExplode();
    //             // or
    //             // nextDot.GetComponent<TrailDotController>().Explode(); 
    //             /*
    //             Talk about the delay between explosions. 
    //             */


    //         }

    //     }
    //     if (prevDot != null)
    //     {
    //         if (thisPos.magnitude == 0)
    //         {
    //             thisPos = new Vector2(this.transform.position.x, this.transform.position.y);
    //         }

    //         Vector2 prevPos = new Vector2(prevDot.transform.position.x, prevDot.transform.position.y);
    //         Vector2 diff2 = thisPos - prevPos;
    //         if (diff2.magnitude <= EXPLOSISON_DISTANCE)
    //         {
    //             prevDot.GetComponent<TrailDotController>().setExplode();
    //         }
    //     }





    //     Destroy(this.gameObject);

    // }
    public void setExplode()
    {
        if(!explode && !hasExploded){
            explode = true;
        }
        
        /*
        StartExplosision animation .

        */
    }

    void BrakeysExplode()
    {
        hasExploded = true; 
        // show explosion effect and play sound
        audioManager.playSound("Explosion (trail)");
        GameObject i = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(i,0.4f);
        // get all the hits in the area.
        LayerMask lm = LayerMask.GetMask("Dots", "Player","Enemy");
        //lm.value = 768; 
        //lm = (1<<9) | (1<<8); 
        
        //layerMask = (1<<9) | (1<<0)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, EXPLOSION_RADIUS,lm);
        
        foreach (Collider2D hit in hits)
        {

            //add damange to the other gameObject. 

            GameObject other = hit.gameObject;
          

            if (other.CompareTag("TrailDot"))
            {
                TrailDotController o = other.GetComponent<TrailDotController>();
                o.sploder = this.sploder; 
                o.setExplode();
                //break 
            }
            if (other.CompareTag("Player"))
            {
                //todo,
                // have it look like the plasma is splashing aroud the players shield, especially if it isn't visiable, when hitting it. 
                // https://www.youtube.com/watch?v=FFzyHDrgDc0
                //
               
                
                //other.gameObject.GetComponent<PlayerController>().takeDamage(PLY_DMG,sploder);
                other.gameObject.GetComponent<PlayerController>().takeDamage(PLY_DMG,this.gameObject);
                break;
            }
            if (other.CompareTag("Enemy"))
            { // right now just the cannon. 
                Destroy(gameObject);
                other.gameObject.GetComponent<RobotDroneController>().takeDamage(WALL_DMG);
                break;
            }
            if (other.CompareTag("Environment"))
            {
                Destroy(this.gameObject);
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
                other.gameObject.GetComponent<BuilderWallController>().takeDamage(WALL_DMG);

                break;
            }
        }
       
        Destroy(this.gameObject); 
    
    }
}