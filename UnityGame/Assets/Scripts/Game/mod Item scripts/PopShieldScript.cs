using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopShieldScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float bubbleDuration = 0.5f;
    void Start()
    {
        Destroy(this.gameObject,bubbleDuration);
    }

    // Update is called once per frame
   
}
