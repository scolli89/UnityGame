using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockClassBasic : PlayerClass
{
    const int AMMO_REQUIRED = 3; 
    public GameObject classPrefab; 
    
    public override void usePower(Vector2 v){
        Debug.Log("Shocking peeps.");
        Transform parentTransform = this.gameObject.transform.root; 
        
        Vector2 iPosition = new Vector2(parentTransform.position.x,parentTransform.position.y); 
        //new Vector2(this.transform.position.x, this.transform.position.y);
        Transform aura = ((GameObject)Instantiate (classPrefab, iPosition, transform.rotation)).transform;
        aura.parent = transform; 
       
    }

    public override int getAmmoReq(){
        return AMMO_REQUIRED;
    }
}
