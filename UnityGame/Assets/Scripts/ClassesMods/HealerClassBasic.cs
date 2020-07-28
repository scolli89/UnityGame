using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealerClassBasic : PlayerClass
{
    const int AMMO_REQUIRED = 2; 
    public GameObject classPrefab; 

    void Start()
    { // like our contr

    }

    // Start is called before the first frame update

    public override void usePower(Vector2 v)//,GameObject g)
    {
        Debug.Log("Healing peeps.");
        Transform parentTransform = this.gameObject.transform.parent; 
        
        Vector2 iPosition = new Vector2(parentTransform.position.x,parentTransform.position.y);
        //TIME TO BUILD 
        Vector2 aimDirection = v;
        //Vector2 iPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        
        //Vector2 iPosition = aimDirection + buildOffset;
        //iPosition.Normalize();

        //GameObject aura = Instantiate(classPrefab, iPosition, Quaternion.identity);
        Transform aura = ((GameObject)Instantiate (classPrefab, iPosition, transform.rotation)).transform;
        aura.parent = transform; 
        PlayerController pc = this.gameObject.GetComponentInParent<PlayerController>();
        pc.healing = true;  
        
    

    }

    public override int getAmmoReq(){
        return AMMO_REQUIRED;
    }
// 

// lets say im the bubble. 
// bubble requirement is 3
// basic wall is 1
// i have 2
// what happens when i use my power, do you default? 

// lets just do one for now. 

// but we would then need another level of classes for each mod, orlike a funciton for 
// useDefaultPower()
// and getDefaultAmmoReq()

}

