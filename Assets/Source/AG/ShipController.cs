// AGR2280 2012 - 2015
// Created by Vonsnake

using UnityEngine;
using System.Collections;
using BSGTools.IO;
using IM = BSGTools.IO.InputMaster;

/// <summary>
/// Handles ship input and local race data such as lap times.
/// </summary>
public class ShipController : MonoBehaviour {

    // INPUT | Axis
    public float inputSteer;
    public float inputPitch;
    public float inputLeftAirbrake;
    public float inputRightAirbrake;

    // INPUT | Buttons
    public bool btnThruster;

    // INPUT | Sideshifting
    private int previousSSInput;
    private int ssTapAmount;
    private float ssTimer;
    private float ssCooler;

    private bool ssLeft;
    private bool ssLeftPressed;
    private bool ssRight;
    private bool ssRightPressed;

    // Controller
    public GlobalSettings.InputController controller;

    // Manager
    RaceManager manager;

    // Bools
    public bool bHasFinishedRace;
         
    void Awake()
    {
        // Check to see if controls have already been setup, if not then create them.
        // This is really only for use when using the Unity Editor since it's more then likely
        // that you havn't gone through the splash screen which is where the controls are setup
        if (!ControlsManager.bHasLoadedControls)
        {
            ControlsManager.SetDefaultControls();
        }
        IM.Initialize();
    }

    void Update()
    {
       // Use a switch to check whether to get player or AI input
       switch(controller)
       {
           case GlobalSettings.InputController.Player:
               GetPlayerInput();
               break;
           case GlobalSettings.InputController.AI:
               GetAIInput();
               break;
       }
    }

    private void GetPlayerInput()
    {
        // Get the input axis
        inputSteer = IM.GetAxis("Steer").value;
        inputPitch = IM.GetAxis("Pitch").value;
        inputLeftAirbrake = IM.GetAxis("LeftAirbrake").value;
        inputRightAirbrake = IM.GetAxis("RightAirbrake").value;

        // Get whether the player is accelerating
        btnThruster = false;
        if (IM.GetAction("Thrust").state == State.Held)
            btnThruster = true;

        // Reset sideshift inputs
        ssLeft = false;
        ssRight = false;

        // Left airbrake tap
        if ((inputLeftAirbrake != 0 && inputRightAirbrake == 0) && !ssLeftPressed)
        {
            ssLeft = true;
            ssLeftPressed = true;
            ssRight = false;
        }

        // Right airbrake tap
        if ((inputRightAirbrake != 0 && inputLeftAirbrake == 0) && !ssRightPressed)
        {
            ssRight = true;
            ssRightPressed = true;
            ssLeft = false;
        }

        // No airbrake taps
        if (inputLeftAirbrake == 0 && inputRightAirbrake == 0)
        {
            ssLeftPressed = false;
            ssRightPressed = false;
        }

        SideshiftInput();

        // Vibrate the controller when starting to thrust
        if (IM.GetAction("Thrust").state == State.Down)
        {
            GetComponent<FFManager>().timerMotor = 1.2f;
            GetComponent<FFManager>().vibLeftMotor = 0.4f;
            GetComponent<FFManager>().vibRightMotor = 0.4f;
        }

        // Thruster Events
        if (IM.GetAction("Thrust").state == State.Down)
        {
            GetComponent<ShipAVManager>().ThrusterDown();
        }

        if (IM.GetAction("Thrust").state == State.Up)
        {
            GetComponent<ShipAVManager>().ThrusterUp();
            GetComponent<FFManager>().timerMotor = 1.2f;
            GetComponent<FFManager>().vibLeftMotor = 0.3f;
            GetComponent<FFManager>().vibRightMotor = 0.3f;
        }

        // Braking vibration
        if (GetComponent<ShipSimulator>().bShipIsBraking)
        {
            GetComponent<FFManager>().timerMotor = 1.2f;
            GetComponent<FFManager>().vibLeftMotor = 0.1f;
            GetComponent<FFManager>().vibRightMotor = 0.1f;
        }

        // Camera Change
        if (IM.GetAction("CameraToggle").state == State.Down)
        {
            GetComponent<ShipSimulator>().UpdateCamera();
        }

        // Look Behind
        if (IM.GetAction("LookBehind").state == State.Held)
        {
            GetComponent<ShipSimulator>().bLookingBehind = true;
        } else
        {
            GetComponent<ShipSimulator>().bLookingBehind = false;
        }
    }

    private void SideshiftInput()
    {
        // Opposite sideshift inputs cancel each other
        if ((previousSSInput == -1 && ssRight) || (previousSSInput == 1 && ssLeft))
        {
            ssTapAmount = 0;
            previousSSInput = 0;
        }

        // Double tap left airbrake
        if (ssLeft)
        {
            previousSSInput = -1;
            if (ssCooler > 0 && ssTapAmount == 1)
            {
                GetComponent<ShipSimulator>().StartSideShift(previousSSInput);
            } else
            {
                ssCooler = 0.2f;
                ssTapAmount++;
            }
        }

        // Double tap right airbrake
        if (ssRight)
        {
            previousSSInput = 1;
            if (ssCooler > 0 && ssTapAmount == 1)
            {
                GetComponent<ShipSimulator>().StartSideShift(previousSSInput);
            } else
            {
                ssCooler = 0.2f;
                ssTapAmount++;
            }
        }

        // Sideshift Cooler
        if (ssCooler > 0)
        {
            ssCooler -= 1 * Time.deltaTime;
        } else
        {
            ssTapAmount = 0;
        }

    }

    private void GetAIInput()
    {

    }

}
