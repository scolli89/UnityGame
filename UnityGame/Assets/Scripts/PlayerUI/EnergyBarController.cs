using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBarController : MonoBehaviour
{
    public Sprite[] energySprites;
    public Sprite[] healthSprites; 
    private SpriteRenderer energySpriteRenderer;
    private SpriteRenderer healthSpriteRenderer; 
    private int TOP_HEALTH_BOUND = 3; 
    // Start is called before the first frame update
    void Start()
    {
        energySpriteRenderer = GetComponent<SpriteRenderer>();
        healthSpriteRenderer = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void setEnergy(int nrg){

        if(energySpriteRenderer == null){
            Debug.Log("Energy null catch");
            Start();
        }
        if(nrg < 0){
            energySpriteRenderer.sprite = energySprites[0];
        } else if(nrg > 7){
            energySpriteRenderer.sprite = energySprites[7];
        }
        else{
            Debug.Log(nrg);
            energySpriteRenderer.sprite = energySprites[nrg]; 
        }
    }

    public void setHealth(int health){
        if(healthSpriteRenderer == null){
            Debug.Log("Energy null catch");
            Start();
        }
        if(health < 0){
            healthSpriteRenderer.sprite = healthSprites[0];
        }
        else if(health > TOP_HEALTH_BOUND){
            healthSpriteRenderer.sprite = healthSprites[TOP_HEALTH_BOUND];
        }
        else {
            healthSpriteRenderer.sprite = healthSprites[health];
        }

    }


}
