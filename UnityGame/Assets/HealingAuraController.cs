using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAuraController : MonoBehaviour
{
    // Start is called before the first frame update
    public float AURA_DURATION = 10f; 
    public float HEAL_TIME = 2.0f;


    void Start()
    {
        Destroy(this.gameObject, AURA_DURATION);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){
             Debug.Log("Plonk");
        //new WaitForSeconds(HEAL_TIME);
        // if (other.gameObject.tag == "Player")
        // {
        //     Debug.Log("Honk");
        //     PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        //     pc.setHealth(1+ pc.getHealth());
            
        //     //setHealthAmount(--health);

        // }


    }

}
