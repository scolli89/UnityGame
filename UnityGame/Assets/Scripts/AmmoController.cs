using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    // Start is called before the first frame update
    [Space]
    [Header("Sprites:")]

    public Sprite[] spriteArray;
   
    [Space]
    [Header("References:")]
    private SpriteRenderer tensSprite;
    private SpriteRenderer onesSprite;
    public GameObject ones;
    public GameObject tens;

    void Start()
    {
        onesSprite = ones.GetComponent<SpriteRenderer>();
        tensSprite = tens.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setAmmo(int ammo){
        int[] values = new int[2]; 
        values[0] = ammo % 10;
        values[1] = ammo % 100; 
        values[1] -= values[0];
        values[1] /= 10; 

        onesSprite.sprite = spriteArray[values[0]];
        tensSprite.sprite = spriteArray[values[1]];
        
      //143 modulo 10 is 3, 
      //143 modulo 100 is 43, 
      //143 modulo 1000 is 143, etc.
      // So if you're looking for the second digit, you would 
      //subtract 143 modulo 10 from 143 modulo 100 to get 40, then divide by 10.


    }
}
