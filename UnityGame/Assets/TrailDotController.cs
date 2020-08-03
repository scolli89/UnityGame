using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailDotController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextDot;
    public GameObject prevDot;
    public float explosionRadius = 1f;
    public bool explode = false;
    private int explodeCount = 0;
    public int explodeTime = 1; // number of frames before it starts to explode. 
    public float EXPLOSISON_DISTANCE = 2f;
    public float DESTROY_DOT_TIME = 15f;
    void Start()
    {
        //Destroy(this.gameObject, DESTROY_DOT_TIME);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (explode)
        {
            
            if (explodeCount >= explodeTime)
            {
                Explode();
            }
            else
            {
                explodeCount++;
            }
        }


    }

    void Explode()
    {
        Vector2 thisPos = Vector2.zero;
        if (nextDot != null)
        {
            Vector2 nextPos = new Vector2(nextDot.transform.position.x, nextDot.transform.position.y);
            thisPos = new Vector2(this.transform.position.x, this.transform.position.y);

            Vector2 difference = thisPos - nextPos;

            if (difference.magnitude <= EXPLOSISON_DISTANCE)
            {
                // if the next dot is within the explosion radius. 
                nextDot.GetComponent<TrailDotController>().setExplode();
                // or
                // nextDot.GetComponent<TrailDotController>().Explode(); 
                /*
                Talk about the delay between explosions. 
                */


            }

        }
        if (prevDot != null)
        {
            if (thisPos.magnitude == 0)
            {
                thisPos = new Vector2(this.transform.position.x, this.transform.position.y);
            }

            Vector2 prevPos = new Vector2(prevDot.transform.position.x, prevDot.transform.position.y);
            Vector2 diff2 = thisPos - prevPos;
            if (diff2.magnitude <= EXPLOSISON_DISTANCE)
            {
                prevDot.GetComponent<TrailDotController>().setExplode();
            }
        }





        Destroy(this.gameObject);

    }
    public void setExplode()
    {
        explode = true;
        /*
        StartExplosision animation .

        */

    }

}
