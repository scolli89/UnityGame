using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairScript : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    float aimProgress;
    float startAimProgress = 0.5f;
    bool animationActive = false;
    public bool shootFlag = false;
    Animator a;
    private void Start()
    {
        //a = this.
        a.enabled = false;

    }
    void Update()
    {
      
    }

    public void startAiming()
    {

        // call the animation to start;
        if (a == null)
        {
            Start();
        }

        if (animationActive == false && shootFlag == false)
        {
            Debug.Log("start");
            aimProgress = startAimProgress;
            this.gameObject.SetActive(true);
            a.enabled = true;
            animationActive = true;
            
            
        }


    }
    public void finishAiming()
    {
        if (animationActive == true && shootFlag == false)
        {
            Debug.Log("finished");
            // stop Animation. 
            // set sprite renderer inmage to image 8. 
            shootFlag = true; 
            animationActive = false;
            aimProgress = startAimProgress;
            a.enabled = false;
        }


    }
    public void stopAiming()
    {
        if(shootFlag == true){
            Debug.Log("Fired");
        } else {
            Debug.Log("cancled");
        }

        
        this.gameObject.SetActive(false);
        animationActive = false;
        a.enabled = false;
        shootFlag = false;
    }
}
