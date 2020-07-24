using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    private int press = 0;
    private PlayerController playerController;
    private PlayerConfiguration playerConfig;
    //public GameObject playerEmptyPrefab; 
    //public GameObject[] classMods; 
    public bool aimWithMouse = false;
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
        if (obj.action.name  == controls.Player.Movement.name)
        {
            OnMove(obj);
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

    // because input manager package is garbage and hold doesn't work
    public void OnAim() //for some reason the function is called twice on every button press, and twice on every button release
    {
        if (playerController != null){
            press++;
            if (press == 2)
            {
                //can assume player is holding the button
                OnAimFix();
            }
            else if (press > 2)
            {
                OnFire();
                press = 0;
                aimWithMouse=false;
            }
        }
    }

    public void OnAimPower()
    {
        if (playerController != null){
            press++;
            if (press == 2)
            {
                OnAimPowerFix();
            }
            else if (press > 2)
            {
                OnPower();
                press = 0;
                aimWithMouse=false;
            }
        }
    }

    public void UsingMouse()
    {
        aimWithMouse=true;
    }

    public void MousePosition()
    {
        if(aimWithMouse == true && playerController != null){
            playerController.setAimDirection(Mouse.current.position.ReadValue());
        }
    }

    private void OnAimFix()
    {
        playerController.setIsAiming();
    }

    private void OnFire()
    {
        playerController.setIsFiring();
    }

    private void OnAimPowerFix()
    {
        playerController.setIsAimingPower();
    }

    private void OnPower()
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