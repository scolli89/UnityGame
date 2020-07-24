using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderClassBasic : PlayerClass
{
    const string BUILDER_CLASS_NAME = "BUILDER";
    const string BUILDER_MOD_ONE = "B_MOD_ONE";
    const string BUILDER_MOD_TWO = "B_MOD_TWO";
    const string BUILDER_MOD_THREE = "B_MOD_THREE";
    const string HEALER_CLASS_NAME = "HEALER";
    const string SHOCK_CLASS_NAME = "SHOCK";
    const int AMMO_REQUIRED = 2; 
    public GameObject classPrefab; 

    void Start()
    { // like our contr

    }

    // Start is called before the first frame update

    public override void usePower(Vector2 v)//,GameObject g)
    {
        Debug.Log("BUILDING SHIT");
        //TIME TO BUILD 
        Vector2 aimDirection = v;
        Vector2 buildOffset = new Vector2(transform.position.x, transform.position.y);
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

