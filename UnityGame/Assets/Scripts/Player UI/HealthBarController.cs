using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{

    //todo make this an array

    public Sprite[] spriteArray;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("health start");
        
        spriteRenderer = GetComponent<SpriteRenderer>();
      //  Debug.Log("HB" + spriteRenderer);
    }
    public void setHealth(int h)
    {
       // Debug.Log("h" + h);
        if(spriteRenderer == null)
        {
            /* getting an error where the player was calling UpdateUI() and it was throwing a null exception on line ~40 because
            * it hadnt been assigned yet. The object existed but its start funciton was not yet called. 
            */
            Debug.Log("health null catch ");
            Start(); 
        }
        if (h < 0)
        {
            spriteRenderer.sprite = spriteArray[0];
        }
        else if (h > 6)
        {
            spriteRenderer.sprite = spriteArray[6];
        }
        else
        {
            spriteRenderer.sprite = spriteArray[h];
        }

        // if (h == 6)
        // {
        //     spriteRenderer.sprite = health6;
        // }
        // else if (h == 5)
        // {
        //     spriteRenderer.sprite = health5;

        // }
        // else if (h == 4)
        // {
        //     spriteRenderer.sprite = health4;
        // }
        // else if (h == 3)
        // {
        //     spriteRenderer.sprite = health3;
        // }
        // else if (h == 2)
        // {
        //     spriteRenderer.sprite = health2;
        // }
        // else if (h == 1)
        // {
        //     spriteRenderer.sprite = health1;
        // } else if(h == 0){
        //     spriteRenderer.sprite = null; 
        // }

    }

}
