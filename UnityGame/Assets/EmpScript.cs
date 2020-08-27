using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject shooter;



    private bool explode = false; // trigger to explode this gameObject. 
    private bool hasExploded = false;
    private float countDown; // number of frames before it starts to explode. 
    public float delay = 0.1f;

    public float EMP_RADIUS = 1f;


    public AudioManager audioManager;

    void Start()
    {
        countDown = delay;
        Destroy(this.gameObject, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {

        countDown -= Time.deltaTime;
        if (countDown <= 0)
        {
            BrakeysExplode();
        }

    }



    void BrakeysExplode()
    {
        hasExploded = true;
        //audioManager.playSound("Explosion (trail)");

        // get all the hits in the area.
        LayerMask lm = LayerMask.GetMask("Dots", "Player","BuilderWall");

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, EMP_RADIUS, lm);

        foreach (Collider2D hit in hits)
        {

            //add damange to the other gameObject. 

            GameObject other = hit.gameObject;
            Debug.Log(other.name);


            if (other.CompareTag("TrailDot"))
            {
                TrailDotController o = other.GetComponent<TrailDotController>();
                o.sploder = this.shooter;
                o.setExplode();
                //break 
            }
            if (other.CompareTag("Player"))
            {
                Debug.Log("HitPlayer");
                other.gameObject.GetComponent<PlayerController>().setEmpEffect(10f);
                break;
            }
            if (other.CompareTag("Enemy"))
            { // right now just the cannon. 
                other.gameObject.GetComponent<RobotDroneController>().setEmpEffect(10f);
                break;
            }
            if (other.CompareTag("Environment"))
            {

                break;
            }
            if (other.CompareTag("Shockwave"))
            { // if we don't want the shock wave to block things, remove this if tree

                break;
            }
            if (other.CompareTag("BuilderWall"))
            {

                other.gameObject.GetComponent<BuilderWallController>().takeDamage(-1);

                break;
            }
        }

        Destroy(this.gameObject);

    }
}