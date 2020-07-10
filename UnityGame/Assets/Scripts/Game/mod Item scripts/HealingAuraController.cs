using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAuraController : MonoBehaviour
{
    // Start is called before the first frame update
    public float AURA_DURATION = 5.0f; 


    void Start()
    {
        Destroy(this.gameObject, AURA_DURATION);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
