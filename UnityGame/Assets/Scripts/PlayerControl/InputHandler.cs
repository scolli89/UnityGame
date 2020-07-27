using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    // this variable exists to prevent the last player to ready up fire an arrow due to unity believing they have not released their button yet
    public bool aimFirst = false;
    public bool powerFirst = false;
    public int press = 0;
    private PlayerController playerController;
    private PlayerConfiguration playerConfig;
    //public GameObject playerEmptyPrefab; 
    //public GameObject[] classMods; 
    private bool aimingMouse=false;
    [SerializeField]
    private PlayerControls controls;

    // Start is called before the first frame update
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        controls = new PlayerControls();
        press = 0;

        // this is where you need to build a player. 
        // Vector2 iPosition = new Vector2(0,0); 
        // GameObject player = Instantiate(playerEmptyPrefab, iPosition, Quaternion.identity);
        // GameObject mod = Instantiate(classMods[0], iPosition, Quaternion.identity);
        // player.addChild(mod); 
    }

    public void InitializePlayer(PlayerConfiguration pc)
    {
        playerConfig = pc;
        // = pc.PlayerClass;
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(CallbackContext obj)
    {        
        // if(obj.canceled){
        //     Debug.Log("#CANCELED");
        // }
        // else if(obj.performed){
        //     Debug.Log("#performed");
        // }
        // else if(obj.started){
        //     Debug.Log("#started");

        // }
        // else if(obj.phase == InputActionPhase.Canceled){
        //      Debug.Log("phase CANCELED");
        // }
        // else if(obj.phase == InputActionPhase.Disabled){
        //      Debug.Log("pase Disabled");
        // }
        // else if(obj.phase == InputActionPhase.Performed){Debug.Log("phase performed");}
        // else if(obj.phase == InputActionPhase.Started){Debug.Log("phase started");}
        // else if(obj.phase == InputActionPhase.Waiting){Debug.Log("#waiting");}

        // get name of action just triggerred and compare it to the input list
        var action = obj.action.name;
        if (action  == controls.Player.Movement.name)
        {
            OnMove(obj);
        }

        // this is a catch to prevent inputs from being triggerred twice
        if(!obj.performed){
            return;
        }
        
        if (action == controls.Player.MousePosition.name)
        {
            MousePosition();
        }
        if (action == controls.Player.Aim.name)
        {
            OnAim();
        }
        if (action == controls.Player.Fire.name)
        {
            OnFire();
        }
        if (action == controls.Player.AimPower.name)
        {
            OnAimPower();
        }
        if (action == controls.Player.FirePower.name)
        {
            OnFirePower();
        }
        if (action == controls.Player.Dash.name)
        {
            OnDash();
        }
        if (action == controls.Player.Pause.name)
        {
            //OnPause();
        }
    }

    public void OnMove(CallbackContext context)
    {
        if (playerController != null){
            Vector2 movement = context.ReadValue<Vector2>();
            movement.Normalize();
            playerController.setMovementDirection(movement);
        }
    }

    public void MousePosition()
    {
        if (aimingMouse == true && playerController != null)
        {
            playerController.setAimDirection(Mouse.current.position.ReadValue());
        }
    }

    private void OnAim()
    {
        if(!aimFirst){
            aimFirst = true;
        }
        playerController.setIsAiming();
    }

    private void OnFire()
    {
        if(aimFirst){
            playerController.setIsFiring();
        }
    }

    private void OnAimPower()
    {
        if(!powerFirst){
            powerFirst = true;
        }
        playerController.setIsAimingPower();
    }

    private void OnFirePower()
    {
        if(powerFirst){
            playerController.setIsFiringPower();
        }
    }    

    public void OnDash()
    {
        if (playerController != null){
            playerController.setIsDashing();
        }
    }
}