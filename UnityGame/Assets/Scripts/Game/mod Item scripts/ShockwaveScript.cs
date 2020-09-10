using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ShockwaveScript : MonoBehaviour
{
    public float AURA_DURATION = 1f; 
    public float PUSH_BACK_FORCE = 10f; 
    // Start is called before the first frame update
    void Start()
    {
         Destroy(this.gameObject, AURA_DURATION);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        // if(other.gameObject.tag == "bullet"){
        //     Debug.Log("Shock box bullets");
        //     Destroy(other.gameObject);
        // }
    }
}
