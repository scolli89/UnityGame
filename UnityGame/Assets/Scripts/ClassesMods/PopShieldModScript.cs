using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopShieldModScript : PlayerClass
{
    // Start is called before the first frame update
    const int AMMO_REQUIRED = 3;
    public GameObject classPrefab;

    public override void usePower(Vector2 v)//,GameObject g)
    {
        

        Transform parentTransform = this.gameObject.transform.parent; 
        
        Vector2 iPosition = new Vector2(parentTransform.position.x,parentTransform.position.y); 
        //new Vector2(this.transform.position.x, this.transform.position.y);
        Transform aura = ((GameObject)Instantiate (classPrefab, iPosition, transform.rotation)).transform;
        aura.parent = transform; 

    }

    public override int getAmmoReq()
    {
        return AMMO_REQUIRED;
    }
    // 

}
