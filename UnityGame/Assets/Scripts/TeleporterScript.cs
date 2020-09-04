using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject sisterPanel;
    TeleporterScript sisterScript;
    public float startTeleportCoolDown = 2f;
    private float teleportCoolDown;
    private GameObject particles;
    void Start()
    {
        teleportCoolDown = startTeleportCoolDown;
        particles = this.transform.GetChild(0).gameObject;
        particles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (teleportCoolDown >= 0)
        {
            teleportCoolDown -= Time.deltaTime;
        }
        else
        {
            particles.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            if (sisterPanel != null)
            {

                if (teleportCoolDown <= 0)
                {
                    teleporting(other.gameObject);


                }
                // hopefully teleports us. 

            }
            else
            {
                Debug.Log("No sister");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // if (other.gameObject.CompareTag("Player"))
        // {
        //     if (sisterPanel != null)
        //     {

        //         if (teleportCoolDown <= 0)
        //         {
        //             resetSisterCoolDown();
        //             other.transform.position = sisterPanel.transform.position;
        //             teleportCoolDown = startTeleportCoolDown;

        //         }
        //         // hopefully teleports us. 

        //     }
        //     else
        //     {
        //         Debug.Log("No sister");
        //     }
        // }
    }

    public void coolDownReset()
    {
        if(particles == null){
            Start(); 
        }
        particles.SetActive(false);
        teleportCoolDown = startTeleportCoolDown;
    }
    private void teleporting(GameObject other)
    {
        other.transform.position = sisterPanel.transform.position;
        particles.SetActive(false);

        teleportCoolDown = startTeleportCoolDown;
        sisterScript.coolDownReset();
    }
    public void setSisterPanel(GameObject newPanel)
    {
        this.sisterPanel = newPanel;
        sisterScript = newPanel.GetComponent<TeleporterScript>();
        Debug.Log(this.sisterPanel);

    }
    public void setBothSisters(TeleporterScript other)
    {

        sisterPanel = other.gameObject;

        other.setSisterPanel(this.gameObject);


    }

    
    // add shooting through teleporters. 
    // add being able to blow up with dash trail. 




}
