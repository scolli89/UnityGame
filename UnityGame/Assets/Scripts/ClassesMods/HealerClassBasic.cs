using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealerClassBasic : PlayerClass
{
    const int AMMO_REQUIRED = 6;
    public GameObject classPrefab;
    private GameObject buffZone;
    private float AuraDuration = 15f;
    private float radius = 1f;

    void Start()
    { // like our contr

    }

    // Start is called before the first frame update

    public override void usePower(Vector2 v)//,GameObject g)
    {
        if (buffZone != null)
        {
            BrakeysExplode(); 
            Destroy(buffZone);
        }
        Transform parentTransform = this.gameObject.transform.parent;

        Vector2 iPosition = new Vector2(parentTransform.position.x, parentTransform.position.y);
        //TIME TO BUILD 
        Vector2 aimDirection = v;
        //Vector2 iPosition = new Vector2(this.transform.position.x, this.transform.position.y);

        //Vector2 iPosition = aimDirection + buildOffset;
        //iPosition.Normalize();

        //GameObject aura = Instantiate(classPrefab, iPosition, Quaternion.identity);
        buffZone = Instantiate(classPrefab, iPosition, transform.rotation);
        Destroy(buffZone, AuraDuration);
        //Transform aura = ((GameObject)Instantiate (classPrefab, iPosition, transform.rotation)).transform;
        //aura.parent = transform; 
        PlayerController pc = this.gameObject.GetComponentInParent<PlayerController>();
        pc.toggleShotBuff(true);
        pc.healing = true;



    }

    public override int getAmmoReq()
    {
        return AMMO_REQUIRED;
    }
    // 
    void BrakeysExplode()
    {
      
        //audioManager.playSound("Explosion (trail)");

        // get all the hits in the area.
        LayerMask lm = LayerMask.GetMask("Player");

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, lm);

        foreach (Collider2D hit in hits)
        {

            //add damange to the other gameObject. 

            GameObject other = hit.gameObject;
            Debug.Log(other.name);

            if (other.CompareTag("Player"))
            {
               // Debug.Log("Debuffng player");
                other.gameObject.GetComponent<PlayerController>().toggleShotBuff(false);
                //break;
            }
            
        }

        Destroy(buffZone);

    }
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

