using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    [Space]
    [Header("References:")]
    [Header("References:")]
    private static SpriteRenderer tensSprite;
    private static SpriteRenderer onesSprite;
    public static GameObject ones;
    public static GameObject tens;
    
    
    
    [Space]
    [Header("Sprites:")]

    public Sprite[] spriteArray;

    public Sprite errorSprite;
   
    

    void Start()
    {
        Debug.Log("Ammo Start");
        onesSprite = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        tensSprite = this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    public void setAmmo(int ammo){

        if (onesSprite == null || tensSprite == null)
        {
            /* getting an error where the player was calling UpdateUI() and it was throwing a null exception  because
           * it hadnt been assigned yet. The object existed but its start funciton was not yet called. 
           */
            Debug.Log("ammo null catch ");
            Start();
        }
        int[] values = new int[2];
        values[0] = ammo % 10;
        values[1] = ammo % 100;
        values[1] -= values[0];
        values[1] /= 10;

        if (values[0] > 9 || values[0] < 0)
        {
            onesSprite.sprite = errorSprite;
        }
        else if (values[1] > 9 || values[1] < 0)
        {
            tensSprite.sprite = errorSprite;
        }

        onesSprite.sprite = spriteArray[values[0]];
        tensSprite.sprite = spriteArray[values[1]];
        
      //143 modulo 10 is 3, 
      //143 modulo 100 is 43, 
      //143 modulo 1000 is 143, etc.
      // So if you're looking for the second digit, you would 
      //subtract 143 modulo 10 from 143 modulo 100 to get 40, then divide by 10.


    }
}
