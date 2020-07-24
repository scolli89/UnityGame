using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    protected string className;
    protected string classMod;

    public PlayerClass(){
        className = null;
        classMod = null;
    }
    public virtual void usePower(Vector2 v){//, GameObject g){
        Debug.Log(" PLAYER CLASS Using Power");
    }
    public virtual int getAmmoReq(){
        Debug.Log("PLAYER CLASS Ammo req");
        return 0; 
    }
    public string getClassName(){
        return className;
    }
    public string getClassMod(){
        return classMod;
    }
    public void setClassName(string className){
        this.className = className;
    }
    public void setClassMod(string classMod){
        this.classMod = classMod;
    }


}
