using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShieldModScript : PlayerClass
{
    const int AMMO_REQUIRED = 6;
    public GameObject classPrefab;

    public override void usePower(Vector2 v)//,GameObject g)
    {
        

        Transform parentTransform = this.gameObject.transform.parent; 
        
        Vector2 iPosition = new Vector2(parentTransform.position.x,parentTransform.position.y); 
        //new Vector2(this.transform.position.x, this.transform.position.y);
        Transform aura = ((GameObject)Instantiate (classPrefab, iPosition, transform.rotation)).transform;
     

    }

    public override int getAmmoReq()
    {
        return AMMO_REQUIRED;
    }



}
