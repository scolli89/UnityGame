using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderClassBasic : PlayerClass
{
    const int AMMO_REQUIRED = 2; 
    public GameObject classPrefab; 

    void Start()
    { // like our contr

    }


    public override void usePower(Vector2 v)//,GameObject g)
    {
       
        //TIME TO BUILD 
        Vector2 aimDirection = v;

        Transform parentTransform = this.gameObject.transform.parent; 
        
        Vector2 buildOffset = new Vector2(parentTransform.position.x,parentTransform.position.y);


        //Vector2 buildOffset = new Vector2(transform.position.x, transform.position.y);
        Vector2 iPosition = aimDirection + buildOffset;
        //iPosition.Normalize();
        
        GameObject wall = Instantiate(classPrefab, iPosition, Quaternion.identity);
        //wall.transform.Rotate(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) *Mathf.Rad2Deg + 90 );

        //original
        wall.transform.Rotate(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);

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

