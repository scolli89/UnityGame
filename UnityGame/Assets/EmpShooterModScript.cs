using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpShooterModScript : PlayerClass
{
    // Start is called before the first frame update

    const float LASER_OFFSET = 1.2f;
    const float ARROW_BASE_SPEED = 50f;
    const int AMMO_REQUIRED = 0; // 4;
    public GameObject classPrefab;
    public override void usePower(Vector2 v)//,GameObject g)
    {
        Vector2 shootingDirection = v;
        shootingDirection.Normalize();
        // need to determine which if the player is shooting down. 
        Transform parentTransform = this.gameObject.transform.parent;
        Vector2 iPosition = new Vector2(parentTransform.position.x, parentTransform.position.y);
        // Vector2 iPosition = transform.position;
        iPosition = iPosition + shootingDirection * LASER_OFFSET; // this prevents it from hitting the player


        GameObject laser = Instantiate(classPrefab, iPosition, Quaternion.identity);
        EmpLaserScript laserScript = laser.GetComponent<EmpLaserScript>();
        laserScript.shooter = this.transform.parent.gameObject;
        laserScript.velocity = shootingDirection * ARROW_BASE_SPEED; // adjust velocity
        laser.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        Destroy(laser, 2.0f);

    }


    public override int getAmmoReq()
    {
        return AMMO_REQUIRED;
    }

}
