using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    private int press = -1;
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
        var action = obj.action.name;
        if (action  == controls.Player.Movement.name)
        {
            OnMove(obj);
        }
        if (action == controls.Player.MousePosition.name)
        {
            MousePosition();
        }
        if (action == controls.Player.Aim.name)
        {
            OnAimFix();
        }
        if (action == controls.Player.AimPower.name)
        {
            OnAimPowerFix();
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

    private void OnAimFix()
    {
        if (playerController != null){
            press++;
            if(press == 2){
                if(Mouse.current.leftButton.IsPressed()){
                    aimingMouse = true;
                }
                OnAim();  
            }
            else if(press > 2){
                aimingMouse=false;
                OnFire();
                press = 0;
            }
        }
    }

    private void OnAimPowerFix()
    {
        if (playerController != null){
            press++;
            if(press == 2){
                if(Mouse.current.rightButton.IsPressed()){
                    aimingMouse = true;
                }
                OnAimPower();
            }
            else if(press > 2){
                aimingMouse=false;
                OnFirePower();
                press = 0;
            }
        }
    }

    private void OnAim()
    {
        playerController.setIsAiming();
    }

    private void OnFire()
    {
        playerController.setIsFiring();
    }

    private void OnAimPower()
    {
        playerController.setIsAimingPower();
    }

    private void OnFirePower()
    {
        playerController.setIsFiringPower();
    }    

    public void OnDash()
    {
        if (playerController != null){
            playerController.setIsDashing();
        }
    }
}