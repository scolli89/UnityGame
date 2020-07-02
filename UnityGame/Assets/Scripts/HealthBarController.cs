using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{

//todo make this an array

    public Sprite health6;
    public Sprite health5;
    public Sprite health4;
    public Sprite health3;
    public Sprite health2;
    public Sprite health1;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("health start");
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    public void setHealth(int h)
    {
      
        if (h == 6)
        {
            spriteRenderer.sprite = health6;
        }
        else if (h == 5)
        {
            spriteRenderer.sprite = health5;

        }
        else if (h == 4)
        {
            spriteRenderer.sprite = health4;
        }
        else if (h == 3)
        {
            spriteRenderer.sprite = health3;
        }
        else if (h == 2)
        {
            spriteRenderer.sprite = health2;
        }
        else if (h == 1)
        {
            spriteRenderer.sprite = health1;
        } else if(h == 0){
            spriteRenderer.sprite = null; 
        }

    }

    // Update is called once per frame

}
