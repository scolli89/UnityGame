using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    private int press = 0;
    private PlayerController playerController;
    
    // Start is called before the first frame update
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    public void OnMove(CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        movement.Normalize();
        playerController.setMovementDirection(movement);
    }

    // because input manager package is garbage and hold doesn't work
    public void StupidAssFix() //for some reason the function is called twice on every button press, and twice on every button release
    {
        press++;
        if (press == 2)
        {
            //can assume player is holding the button
            OnAim();
        }
        else if (press > 2)
        {
            OnFire();
            press = 0;
        }
    }

    public void StupidAssFixPowers()
    {
        press++;
        if (press == 2)
        {
            OnAimPower();
        }
        else if (press > 2)
        {
            OnPower();
            press = 0;
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

    private void OnPower()
    {
        playerController.setIsFiringPower();
    }    

    public void OnDash()
    {
        playerController.setIsDashing();
    }
}