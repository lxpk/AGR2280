// AGR2280 2012 - 2015
// Created by Vonsnake

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Simulates the ship physics.
/// </summary>
public class ShipSimulator : MonoBehaviour {

    // Debugging
    public bool bDebugVisualizeHovering;

    // Ship References
    private Rigidbody shipBody;
    private PhysicMaterial shipPhysicsMaterial;

    private GameObject shipAxis;
    private GameObject shipCamera;
    private ShipController controller;
    public ShipSettings settings;

    public GlobalSettings.InputController inputControl;
    public GlobalSettings.SpeedClasses eventSpeedClass;

    public RaceManager manager;
    private FFManager vibrationManager;

    // Booleans
    public bool bShipHasCameraControl;
    public bool bShipIsGrounded;
    public bool bShipIsOnMagStrip;
    public bool bShipIsBraking;
    public bool bShipIsBoosting;
    public bool bShipBoostingOverride;

    private bool bIsShipSS;
    private bool bSSCoolingDown;

    private bool bCanShipBR;
    private bool bHasShipBR;
    private bool bBRSuccess;

    private bool bShipHitPad;

    private bool bIsShipColliding;

    public bool bLookingBehind;

    private bool turnGainAntiBanding;
    private bool airbrakeGainAntiBanding;
    private bool bankingGainAntiBanding;

    // Vectors
    private Vector3 shipGravityForce;
    private Vector3 speedPadDirection;
    private Vector3 sideShiftDirection;
    private Vector3 shipHoverAnimOffset;
    private Vector3 shipCameraBoostOffset;

    // Floats
    public float shipIntegrity;

    public float shipTurningAmount;
    private float shipTurningGain;
    private float shipTurningFalloff;
    private float shipTurningPrevious;

    public float shipAirbrakeAmount;
    private float shipAirbrakeGain;
    private float shipAirbrakeFalloff;
    private float shipAirbrakePrevious;
    private float shipAirbrakeResistance;

    private float shipAirDrag;

    private float shipPitchAmount;
    private float shipGripAmount;

    private float shipCameraSpringTight;
    private float shipCameraStablizerTight;
    private float shipCameraSpringFree;
    private float shipCameraStablizerFree;
    private float shipCameraSpring;

    private float shipCameraTurnOffset;

    private float shipCameraAccelerationLength;
    private float shipCameraChaseLag;
    private float shipCameraOffsetLag;
    private float shipCameraHeightLag;
    private float shipCameraBoostChase;
    private float shipCameraPitchModify;

    private float shipCameraFoV;
    private float shipCameraBoostFoV;

    private float shipBankingAmount;
    private float shipBankingVelocity;
    private float shopBankingGain;
    private float shipBankingFalloff;
    private float shipBankingAirbrakeAmount;

    private float shipEngineThrust;
    private float shipEngineAcceleration;
    private float shipEngineGain;
    private float shipEngineFalloff;

    private float shipBrakesAmount;
    private float shipBrakesGain;
    private float shipBrakesFalloff;

    private float shipReboundJump;
    private float shipReboundLand;

    private float shipBoostTimer;

    private float shipSideshiftCooler;
    private float shipSideshiftCountdown;

    private float shipHoverAnimTimer;
    private float shipHoverAnimSpeed = 1.0f;
    private float shipHoverAnimAmount = 0.2f;

    private float shipBRProgress;
    private float shipBRActual;
    private float shipBRTimer;
    private float shipBRLastValue;
    private float shipBRBoostTimer;

    private float shipCollisionFriction;

    // Ints
    public int shipCurrentCamra;
    private int shipBRState;

    void Start()
    {
        SetupShip();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        Gravity();
        Handling();
        Acceleration();
        Drag();
        FixedUpdateCamera();
        ShipAxisAnimations();
    }

    /// <summary>
    /// Handles the gravity and hovering for the ship.
    /// </summary>
    public void Gravity()
    {
        // Reset gravity/magstrip
        bShipIsGrounded = false;
        bShipIsOnMagStrip = false;

        // Increase the rideheight slightly (keeps the ship at the actual wanted height)
        float rideHeight = GlobalSettings.shipRideHeight * 1.2f;

        // Pitching
        if (controller.inputPitch != 0)
        {
            shipPitchAmount = Mathf.Lerp(shipPitchAmount, controller.inputPitch * GlobalSettings.shipPitchGroundAmount, Time.deltaTime * GlobalSettings.shipPitchDamping);
        } else
        {
            shipPitchAmount = Mathf.Lerp(shipPitchAmount, 0.0f, Time.deltaTime * GlobalSettings.shipPitchDamping);
        }
        shipBody.AddTorque(transform.right * (shipPitchAmount * 10));

        // Get the current class and set the gravity multipliers
        float gravityMul = 0;
        float trackGrav = 0;
        float flightGrav = 0;
        switch(eventSpeedClass)
        {
            case GlobalSettings.SpeedClasses.D:
                // Gravity Multiplier
                gravityMul = GlobalSettings.airGravityMulD;
                // Track Gravity
                trackGrav = GlobalSettings.shipTrackGravityD;
                // Flight Gravity
                flightGrav = GlobalSettings.shipFlightGravityD;
                break;
            case GlobalSettings.SpeedClasses.C:
                // Gravity Multiplier
                gravityMul = GlobalSettings.airGravityMulC;
                // Track Gravity
                trackGrav = GlobalSettings.shipTrackGravityC;
                // Flight Gravity
                flightGrav = GlobalSettings.shipFlightGravityC;
                break;
            case GlobalSettings.SpeedClasses.B:
                // Gravity Multiplier
                gravityMul = GlobalSettings.airGravityMulB;
                // Track Gravity
                trackGrav = GlobalSettings.shipTrackGravityB;
                // Flight Gravity
                flightGrav = GlobalSettings.shipFlightGravityB;
                break;
            case GlobalSettings.SpeedClasses.A:
                // Gravity Multiplier
                gravityMul = GlobalSettings.airGravityMulA;
                // Track Gravity
                trackGrav = GlobalSettings.shipTrackGravityA;
                // Flight Gravity
                flightGrav = GlobalSettings.shipFlightGravityA;
                break;
            case GlobalSettings.SpeedClasses.AP:
                // Gravity Multiplier
                gravityMul = GlobalSettings.airGravityMulAP;
                // Track Gravity
                trackGrav = GlobalSettings.shipTrackGravityAP;
                // Flight Gravity
                flightGrav = GlobalSettings.shipFlightGravityAP;
                break;
            case GlobalSettings.SpeedClasses.APP:
                // Gravity Multiplier
                gravityMul = GlobalSettings.airGravityMulAPP;
                // Track Gravity
                trackGrav = GlobalSettings.shipTrackGravityAPP;
                // Flight Gravity
                flightGrav = GlobalSettings.shipFlightGravityAPP;
                break;
        }

        // Create hover points list
        List<Vector3> hoverPoints = new List<Vector3>();
        // Get a slightly smaller area of the ship size
        Vector2 hoverArea = new Vector2(settings.physColliderSize.x / 2, settings.physColliderSize.z / 2.05f);
        // Add hover points
        hoverPoints.Add(transform.TransformPoint(-hoverArea.x, Mathf.Clamp(shipPitchAmount, 0.0f, 0.5f), hoverArea.y)); // Front Left
        hoverPoints.Add(transform.TransformPoint(hoverArea.x, Mathf.Clamp(shipPitchAmount, 0.0f, 0.5f), hoverArea.y)); // Front Right
        hoverPoints.Add(transform.TransformPoint(hoverArea.x, Mathf.Clamp(-shipPitchAmount, 0.0f, 0.7f), -hoverArea.y)); // Back Right
        hoverPoints.Add(transform.TransformPoint(-hoverArea.x, Mathf.Clamp(-shipPitchAmount, 0.0f, 0.7f), -hoverArea.y)); // Back Left

        // Run through each hover point and apply hover forces if needed
        for (int i = 0; i < hoverPoints.Count; i++)
        {
            // Bitshift track floor and mag lock layers into a new integer for the ship to only check for those two collision layers
            int trackFloorMask = 1 << LayerMask.NameToLayer("TrackFloor") | 1 << LayerMask.NameToLayer("Maglock");
            // Create raycasthit to store hit data into
            RaycastHit hoverHit;
            // Raycast down for track
            if(Physics.Raycast(hoverPoints[i], -transform.up, out hoverHit, rideHeight, trackFloorMask))
            {
                // Get the distance to the ground
                float dist = hoverHit.distance;

                // Normalize the distance into a compression ratio
                float compressionRatio = ((dist / rideHeight) - 1) * -1;

                // Get the velocity at the hover point
                Vector3 hoverVel = shipBody.GetPointVelocity(hoverPoints[i]);

                // Project the velocity onto the ships up vector
                Vector3 projForce = Vector3.Project(hoverVel, transform.up);

                // Check to see if the ship is on a magstrip
                if (hoverHit.collider.gameObject.layer == LayerMask.NameToLayer("Maglock"))
                    bShipIsOnMagStrip = true;

                /* Calculate force needed to push ship away from track */
                // Set a force multiplier
                float forceMult = 5;
                // Calcaulate a ratio to multiply the compression ratio by (drasticly increases the hover force as it gets closer to the track)
                float forceRatio = ((dist / 2) / (rideHeight / 2) - 1) * -forceMult;
                // Clamp the ratio
                forceRatio = Mathf.Clamp(forceRatio, 2.0f, 10.0f);
                // Create the new force
                float force = 200.0f * (compressionRatio * forceRatio);

                /* Calculate damping amount to bring the ship to rest quicker */
                // Set a damping multiplier
                float dampMult = 6.5f;
                // Calculate a ratio to multiply the compression ratio by
                float dampRatio = ((dist / 2) / (rideHeight / 2) - 1) * -dampMult;
                // Clamp the ratio
                dampRatio = Mathf.Clamp(dampRatio, 1.0f, 5.5f);
                // Clamp the compression ratio, min being the base damp amount
                float clampedCR = Mathf.Clamp(compressionRatio, 0.5f, 1.0f);

                // Get the landing rebound to dampen forces straight after a long fall (stops the ship rebounding high off the track)
                float reboundDamper = Mathf.Clamp(shipReboundLand * 15, 1.0f, Mathf.Infinity);
                // Create the new damping force
                float damping = (1 * (clampedCR * dampRatio) * reboundDamper);

                // If the ship is over a magstrip then we want the ship to act a bit differently
                if (bShipIsOnMagStrip)
                {
                    // Calculate how strong the magstrip is based on the ships current angular velocity and speed
                    float magStrength = Mathf.Abs(transform.InverseTransformDirection(shipBody.angularVelocity).x * Time.deltaTime)
                        * Mathf.Abs(transform.InverseTransformDirection(shipBody.velocity).z * Time.deltaTime);
                    // Multiply the strength as the above calculate will result in a terrible lock strength
                    magStrength *= 1000;
                    // Clamp the strength so it can't be too strong
                    magStrength = Mathf.Clamp(magStrength, 0.0f, (transform.InverseTransformDirection(shipBody.velocity).z * Time.deltaTime) * 3.8f);

                    // Set new force and damping multipliers
                    force = (trackGrav * (5.0f + magStrength));
                    damping = 2f;

                    // Apply the magstrip lock force
                    shipBody.AddForceAtPosition(-transform.up * (trackGrav * (magStrength * 4.0f)), hoverPoints[i], ForceMode.Acceleration);
                }

                // Calculate the suspension force
                Vector3 suspensionForce = transform.up * compressionRatio * force;

                // Now apply damping
                Vector3 dampedForce = suspensionForce - (projForce * damping);

                // Finally, apply the force
                shipBody.AddForceAtPosition(dampedForce, hoverPoints[i], ForceMode.Acceleration);

                // Set the grounded boolean to true
                bShipIsGrounded = true;

                // Debug line
                if (bDebugVisualizeHovering)
                {
                    Debug.DrawLine(hoverPoints[i], hoverHit.point);
                }

                // Force feedback
                if (dist < rideHeight * 0.6f)
                {
                    vibrationManager.vibLeftMotor = 0.3f;
                    vibrationManager.vibRightMotor = 0.3f;
                    vibrationManager.timerMotor = 1.1f;
                }

            }
        }

        /* If the ship is grounded then we also want to have the ship be pulled down by gravity
        We want to do another raycast for the trackfloor layer only from the centre of the ship
        then calculate everything from that.

        Don't worry, we are only casting for one layer over a short distance, this isn't very
        expensive to do. */
        // Create raycasthit to store hit data into
        RaycastHit centreHit;
        // Raycast down
        if (Physics.Raycast(transform.position, -transform.up, out centreHit, rideHeight, 1 << LayerMask.NameToLayer("TrackFloor")))
        {
            // Use a dot product to find out how much we need to rotate the ship to get it to face down
            Vector3 trackRot = transform.forward - (Vector3.Dot(transform.forward, centreHit.normal) * centreHit.normal);
            // Use the track rotation to calculate how much we are going to rotate the ship
            float rotAmount = (trackRot.x * trackRot.z) * (Mathf.Clamp(Mathf.Abs(trackRot.y), 0.0f, 0.1f) * 10);
            // Increase the rotation amount as the ship speeds up
            float rotForce = 20 + (transform.InverseTransformDirection(shipBody.velocity).z * Time.deltaTime) * 1.2f;
            // Apply the rotation
            transform.Rotate(Vector3.up * (rotAmount * Time.deltaTime) * rotForce);

            // Apply gravity force 
            shipBody.AddForce(new Vector3(centreHit.normal.x, 0, centreHit.normal.z) * (Mathf.Abs(trackRot.x)) * 22);
        }
        

        /* We also want to check for pads on the track. We'll do exactly the same as above to do this.
           Also we can recycle the centrehit variable! */
        // Bitshift pads into integer
        int padMask = 1 << LayerMask.NameToLayer("SpeedPad") | 1 << LayerMask.NameToLayer("WeaponPad");
        // Raycast down
        if (Physics.Raycast(transform.position, -transform.up, out centreHit, rideHeight + 2, padMask))
        {
            // If the ship hits a speed pad then apply the appropiate force
            if (centreHit.collider.gameObject.layer == LayerMask.NameToLayer("SpeedPad"))
            {
                // Get the pad's forward direction
                speedPadDirection = centreHit.collider.gameObject.transform.right * GlobalSettings.speedPadAmount;
                // Set the ships boost timer
                shipBoostTimer = GlobalSettings.speedPadTime;
                // Set boosting to true
                bShipIsBoosting = true;

                // If we havn't hit this pad already, play the boost sound
                if (!bShipHitPad)
                {
                    bShipHitPad = true;
                    
                    // Get sound to play
                    AudioClip boostSFX = Resources.Load("Audio/Ships/BoostSpeedPad") as AudioClip;
                    
                    // Play the sound
                    PlayOneShot.PlayShot("BoostSFX", 1.0f, 1.0f, 50.0f, 500.0f, transform, transform.position, boostSFX);
                }
            }

        } else
        {
            // We are not hitting a pad, set the hitting pad boolean to false
            bShipHitPad = false;
        }

        // If the boost timer is over zero then we want to decrease it and apply any boost the ship has
        if (shipBoostTimer > 0)
        {
            // Decrease timer
            shipBoostTimer -= Time.deltaTime;
            // Apply pad boost
            shipBody.AddForce(speedPadDirection, ForceMode.Acceleration);
        } else
        {
            // Make sure that the pad hit boolean is no longer true
            bShipHitPad = false;
            
            // Override boost if the ship has boost power from a barrel rol
            if (shipBRBoostTimer > 0)
            {
                bShipIsBoosting = true;
                // Apply the barrel roll boost
                shipBody.AddForce(transform.forward * settings.engineTurbo, ForceMode.Acceleration);
            } else
            {
                // Otherwise the boosting is set to false
                bShipIsBoosting = false;
            }
        }

        // Allow the turbo pickup to override the boost
        if (bShipBoostingOverride)
        {
            bShipIsBoosting = true;
        }

        // Boosting Force Feedback
        if (bShipIsBoosting)
        {
            vibrationManager.timerMotor = 1.1f;
            vibrationManager.vibLeftMotor = 0.4f;
            vibrationManager.vibRightMotor = 0.4f;
        }

        /* And finally we want to apply gravity, we want to swap out the gravity between two different variables
        which allows the ship to feel much lighter on the track, this also makes the hovering feel much better too */
        // Check to see if the ship is grounded
        if (bShipIsGrounded)
        {
            // If the ship is on a magstrip then the gravity wants to be heavier
            if (bShipIsOnMagStrip)
            {
                // Set gravity
                shipGravityForce = -transform.up * (trackGrav * 5);
            } else
            {
                // Set gravity
                shipGravityForce = -transform.up * trackGrav;
            }

            // Set the rebound jump time so the stick force is always maxed when the ship enters flight
            shipReboundJump = settings.agReboundJumpTime;

            // Decrease the rebound land
            shipReboundLand -= Time.deltaTime;
        } else
        {
            // Set the gravity
            shipGravityForce = Vector3.down * (flightGrav * gravityMul);

            // Decrease the jump
            shipReboundJump -= Time.deltaTime;            
            // Apply the stick force
            if (shipReboundJump > 0)
            {
                // Apply the force to keep the ship grounded
                shipBody.AddForce(Vector3.down * ((10 * GlobalSettings.shipNormalGravity) * settings.agRebound), ForceMode.Acceleration);
            }

            // Set rebound to zero if it has gone under zero
            if (shipReboundLand < 0)
                shipReboundLand = 0;

            // Increase the rebound land
            shipReboundLand += Time.deltaTime;
            // Clamp the rebound land
            if (shipReboundLand > settings.agReboundLanding)
                shipReboundLand = settings.agReboundLanding;

            // Slowly rotate the ship forward
            transform.Rotate(Vector3.right * 0.05f);
        }

        // And finally, apply the gravity
        shipBody.AddForce(shipGravityForce, ForceMode.Acceleration);

    }

    /// <summary>
    /// Handles the turning and airbrakes for the ship.
    /// </summary>
    public void Handling()
    {
        // Anti-gain banding
        if ((controller.inputSteer > 0 && shipTurningAmount < 0) || (controller.inputSteer < 0 && shipTurningAmount > 0))
        {
            if (!turnGainAntiBanding)
            {
                turnGainAntiBanding = true;
                shipBankingVelocity = 0;
                shipTurningGain = 0;
            }
        } else
        {
            turnGainAntiBanding = false;
        }

        // Turning
        if (controller.inputSteer != 0)
        {
            // Calculate the turn amount
            float calculatedTurnAmount = settings.turnAmount * Time.deltaTime * Mathf.PI * 7.7f * 2;

            // Increase the turning gain
            shipTurningGain = Mathf.Lerp(shipTurningGain, settings.turnGain * Time.deltaTime, Time.deltaTime * 4);
            // Set falloff to zero to create a gradual slowdown every time we stop turning
            shipTurningFalloff = 0;
            // Increase the turning amount
            shipTurningAmount = Mathf.Lerp(shipTurningAmount, controller.inputSteer * calculatedTurnAmount ,Time.deltaTime * shipTurningGain);
            // Set the previous turning to the current turning
            shipTurningPrevious = Mathf.Abs(shipTurningAmount);

            // Increase the banking velocity
            shipBankingVelocity = Mathf.Lerp(shipBankingVelocity, settings.tiltShipSpeed, Time.deltaTime * settings.tiltShipAmount);
            // Set banking falloff to zero
            shipBankingFalloff = 0;
            // Increase the banking amount
            shipBankingAmount = Mathf.Lerp(shipBankingAmount, controller.inputSteer * 45.0f, Time.deltaTime * (shipBankingVelocity * 1.2f));
        } else
        {
            // Increase the turning falloff
            shipTurningFalloff = Mathf.Lerp(shipTurningFalloff, settings.turnFalloff * Time.deltaTime, Time.deltaTime * 5);
            // Set gain to zzero to create a gradual increase every time we start turning
            shipTurningGain = 0;
            
            shipTurningAmount = Mathf.Lerp(shipTurningAmount, 0.0f, Time.deltaTime * shipTurningFalloff);

            // Increase banking fallof
            shipBankingFalloff = Mathf.Lerp(shipBankingFalloff, settings.tiltShipSpeed, Time.deltaTime * settings.tiltShipAmount);
            // Set banking velocity to zero
            shipBankingVelocity = 0;
            // Decrease the banking amount
            shipBankingAmount = Mathf.Lerp(shipBankingAmount, 0.0f, Time.deltaTime * shipBankingFalloff);
        }

        // Apply the rotation on the up vector
        float calculatedAirbrakeTurn = settings.airbrakesTurn * Time.deltaTime * 2.673f;

        // Airbrakes
        // Get the overal airbrake input
        float airbrakeInput = controller.inputLeftAirbrake + controller.inputRightAirbrake;
        // Use the ships speed to calculate the sensitivity of the airbrakes
        float airbrakeAmount = ((transform.InverseTransformDirection(shipBody.velocity).z * Time.deltaTime) * calculatedAirbrakeTurn);

        if (airbrakeInput != 0)
        {
            // Increase airbrake gain
            shipAirbrakeGain = Mathf.Lerp(shipAirbrakeGain, settings.airbrakesGain * Time.deltaTime, Time.deltaTime * 6);
            // Set falloff to zero
            shipAirbrakeFalloff = 0;
            
            // Increase airbrake amount
            shipAirbrakeAmount = Mathf.Lerp(shipAirbrakeAmount, airbrakeInput * airbrakeAmount, Time.deltaTime * shipAirbrakeGain);

            // Set the previous airbrake amount
            shipAirbrakePrevious = Mathf.Abs(shipAirbrakeAmount);

            // Airbrake banking
            shipBankingAirbrakeAmount = Mathf.Lerp(shipBankingAirbrakeAmount, -airbrakeInput * (settings.airbrakesAmount * 1.5f), Time.deltaTime * settings.tiltShipSpeed);
        } else
        {
            // Increase airbrake falloff
            shipAirbrakeFalloff = Mathf.Lerp(shipAirbrakeFalloff, settings.airbrakesFalloff * Time.deltaTime, Time.deltaTime * 6);
            // Set gain to zero
            shipAirbrakeGain = 0;

            // If the previous airbake is over a certain threshold then increase how fast the airbrake amount decreases to 'spring'
            // the ship's airbrake rotation back to zero
            if (Mathf.Abs(shipAirbrakeAmount) > shipAirbrakePrevious * 0.7f)
            {
                // Over the threshold
                shipAirbrakeAmount = Mathf.Lerp(shipAirbrakeAmount, 0.0f, Time.deltaTime * (shipAirbrakeFalloff));
            } else
            {
                // Under the threshold
                shipAirbrakeAmount = Mathf.Lerp(shipAirbrakeAmount, 0.0f, Time.deltaTime * (shipAirbrakeFalloff * 2f));
            }


            // Airbrake banking
            shipBankingAirbrakeAmount = Mathf.Lerp(shipBankingAirbrakeAmount, 0.0f, Time.deltaTime * settings.tiltShipSpeed);
        }

        // Apply the overall rotation to the ship
        transform.Rotate(Vector3.up * (shipTurningAmount + shipAirbrakeAmount));

        // Apply the bank rotation to the ship axis
        shipAxis.transform.localRotation = Quaternion.Euler(0, 0, -shipBankingAmount + shipBankingAirbrakeAmount);
    }

    /// <summary>
    /// Handles the thruster for the ship.
    /// </summary>
    public void Acceleration()
    {
        // Create top speed values
        float topThrust = 0;
        float topAccel = 0;
        
        // Choose the top values depending on the speed class
        switch(eventSpeedClass)
        {
            case GlobalSettings.SpeedClasses.D:
                topThrust = settings.engineThrustCapD;
                topAccel = settings.engineAccelCapD;
                break;
            case GlobalSettings.SpeedClasses.C:
                topThrust = settings.engineThrustCapC;
                topAccel = settings.engineAccelCapC;
                break;
            case GlobalSettings.SpeedClasses.B:
                topThrust = settings.engineThrustCapB;
                topAccel = settings.engineAccelCapB;
                break;
            case GlobalSettings.SpeedClasses.A:
                topThrust = settings.engineThrustCapA;
                topAccel = settings.engineAccelCapA;
                break;
            case GlobalSettings.SpeedClasses.AP:
                topThrust = settings.engineThrustCapAP;
                topAccel = settings.engineAccelCapAP;
                break;
            case GlobalSettings.SpeedClasses.APP:
                topThrust = settings.engineThrustCapAPP;
                topAccel = settings.engineAccelCapAPP;
                break;
        }

        // Ship thrusting
        if (controller.btnThruster)
        {
            // Increase Gain
            shipEngineGain = Mathf.Lerp(shipEngineGain, (settings.engineGain * 0.1f) * Time.deltaTime, Time.deltaTime);
            // Set Falloff to zero
            shipEngineFalloff = 0;
            // Increase acceleration
            shipEngineAcceleration = Mathf.Lerp(shipEngineAcceleration, topAccel, Time.deltaTime * shipEngineGain);
            // Increase thrust
            shipEngineThrust = Mathf.Lerp(shipEngineThrust, topThrust, Time.deltaTime * (shipEngineAcceleration * 0.1f));
        }
        else
        {
            // Increase Falloff
            shipEngineFalloff = Mathf.Lerp(shipEngineFalloff, settings.engineFalloff * Time.deltaTime, Time.deltaTime);
            // Set Gain to zero
            shipEngineGain = 0;
            // Decrease acceleration
            shipEngineAcceleration = Mathf.Lerp(shipEngineAcceleration, 0.0f, Time.deltaTime * shipEngineFalloff);
            // Decrease thrust
            shipEngineThrust = Mathf.Lerp(shipEngineThrust, 0.0f, Time.deltaTime * shipEngineFalloff);
        }

        // Apply Thrust
        shipBody.AddRelativeForce(Vector3.forward * shipEngineThrust);

        // Side Shifting
        if (bIsShipSS)
        {
            shipBody.AddForce(sideShiftDirection * (settings.airbrakesSideshift * 1.5f), ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Calculates a custom drag to replace Unity's drag.
    /// </summary>
    public void Drag()
    {
        float gripAmount = 0;

        if (bShipIsGrounded)
        {
            switch (eventSpeedClass)
            {
                case GlobalSettings.SpeedClasses.D:
                    gripAmount = settings.agGripGroundD;
                    break;
                case GlobalSettings.SpeedClasses.C:
                    gripAmount = settings.agGripGroundC;
                    break;
                case GlobalSettings.SpeedClasses.B:
                    gripAmount = settings.agGripGroundB;
                    break;
                case GlobalSettings.SpeedClasses.A:
                    gripAmount = settings.agGripGroundA;
                    break;
                case GlobalSettings.SpeedClasses.AP:
                    gripAmount = settings.agGripGroundAP;
                    break;
                case GlobalSettings.SpeedClasses.APP:
                    gripAmount = settings.agGripGroundAPP;
                    break;
            }
        } else
        {
            switch (eventSpeedClass)
            {
                case GlobalSettings.SpeedClasses.D:
                    gripAmount = settings.agGripAirD;
                    break;
                case GlobalSettings.SpeedClasses.C:
                    gripAmount = settings.agGripAirC;
                    break;
                case GlobalSettings.SpeedClasses.B:
                    gripAmount = settings.agGripAirB;
                    break;
                case GlobalSettings.SpeedClasses.A:
                    gripAmount = settings.agGripAirA;
                    break;
                case GlobalSettings.SpeedClasses.AP:
                    gripAmount = settings.agGripAirAP;
                    break;
                case GlobalSettings.SpeedClasses.APP:
                    gripAmount = settings.agGripAirAPP;
                    break;
            }
        }

        float forwardVelocity = Mathf.Abs(transform.InverseTransformDirection(shipBody.velocity).z);
        float slideGrip = gripAmount * Time.deltaTime;
        if (bShipIsGrounded)
        {
            // Get turning input
            float turningInput = Mathf.Abs(controller.inputSteer);
            //slideGrip -= turningInput * Mathf.Abs(shipTurningAmount * Time.deltaTime);
            // Get airbrake input
            float airbrakeInput = Mathf.Abs(controller.inputLeftAirbrake + controller.inputRightAirbrake);
            //slideGrip -= airbrakeInput * ((Mathf.Abs(shipAirbrakeAmount) - (settings.airbrakesSlidegrip * Time.deltaTime)) * Time.deltaTime) * settings.airbrakesDrag;
            float absVelX = Mathf.Abs(transform.InverseTransformDirection(shipBody.velocity).x);
            slideGrip -= Mathf.Abs((shipTurningAmount / 2) * Time.deltaTime);
            slideGrip -= ((absVelX * Time.deltaTime) * Time.deltaTime) + ((Mathf.Abs(shipAirbrakeAmount* Time.deltaTime) * settings.airbrakesSlidegrip * Time.deltaTime) * settings.airbrakesDrag);
        }

        // Airbrake Resistance 
        if (controller.inputLeftAirbrake != 0 || controller.inputRightAirbrake != 0)
        {
            // Get the airbrake input 
            float airbrakeInput = Mathf.Abs(controller.inputLeftAirbrake + controller.inputRightAirbrake);
            airbrakeInput = Mathf.Clamp(airbrakeInput, 0.0f, 1.0f);

            // Increase resistance
            shipAirbrakeResistance = Mathf.Lerp(shipAirbrakeResistance, Mathf.Abs(shipAirbrakeAmount) * airbrakeInput, Time.deltaTime * 0.02f);

        } else
        {
            // Decrease resistance
            shipAirbrakeResistance = Mathf.Lerp(shipAirbrakeResistance, 0.0f, Time.deltaTime * 4.5f);
        }

        // Air resistance
        if (bShipIsGrounded)
        {
            shipAirDrag = Mathf.Lerp(shipAirDrag, 0.0f, Time.deltaTime * 4.5f);
        } else
        {
            shipAirDrag = Mathf.Lerp(shipAirDrag, 0.4f, Time.deltaTime * 0.04f);
        }

        // Braking Drag Increase
        if (controller.inputLeftAirbrake != 0 && controller.inputRightAirbrake != 0)
        {
            // Ship is now braking
            bShipIsBraking = true;
            // Set falloff to zero
            shipBrakesFalloff = 0;
            // Increase gain
            shipBrakesGain = Mathf.Lerp(shipBrakesGain, Time.deltaTime * GlobalSettings.shipBrakesGain * 0.1f, Time.deltaTime);
            // Increase brakes amount
            shipBrakesAmount = Mathf.Lerp(shipBrakesAmount, Time.deltaTime * GlobalSettings.shipBrakesAmount, Time.deltaTime * shipBrakesGain);

            // Decrease Acceleration and Thrust
            shipEngineAcceleration = Mathf.Lerp(shipEngineAcceleration, 0.0f, Time.deltaTime * shipBrakesGain);
            shipEngineThrust = Mathf.Lerp(shipEngineThrust, 0.0f, Time.deltaTime * shipBrakesGain);
        } else
        {
            // Ship is no longer breaking
            bShipIsBraking = false;
            // Set gain to zero
            shipBrakesGain = 0;
            // Increase falloff
            shipBrakesFalloff = Mathf.Lerp(shipBrakesFalloff, GlobalSettings.shipBrakesFalloff * 0.1f, Time.deltaTime);
            // Decrease brakes amount
            shipBrakesAmount = Mathf.Lerp(shipBrakesAmount, 0.0f, Time.deltaTime * shipBrakesFalloff);
        }

        Vector3 LocalVelocity = transform.InverseTransformDirection(shipBody.velocity);
        // Ship grip
        LocalVelocity.x *= (1 - slideGrip);
        // Natural Air drag
        LocalVelocity.y *= 1 - 0.003f;
        LocalVelocity.z *= 1 - (0.003f + ((forwardVelocity * Time.deltaTime) * 0.007f));
        // Drag Resistance
        LocalVelocity.z *= 1 - ((shipAirbrakeResistance + shipAirDrag) + shipBrakesAmount);
        Vector3 WorldVelocity = transform.TransformDirection(LocalVelocity);
        shipBody.velocity = WorldVelocity;

    }

    public void FixedUpdateCamera()
    {
        // Set default fail-safe spring values
        float camHorSpring = 12.0f;
        float camVertSpring = 12.0f;
        float camFoV = 60.0f;
        Vector3 camOffset = Vector3.zero;
        Vector3 camLA = Vector3.zero;

        // Check which camera is being used and use the right values
        switch(shipCurrentCamra)
        {
            case 0:
                camHorSpring = settings.camCloseSpringHor;
                camVertSpring = settings.camCloseSpringVert;

                camFoV = settings.camCloseFoV;
                camOffset = settings.camCloseOffset;
                camLA = settings.camCloseLA;
                break;
            case 1:
                camHorSpring = settings.camFarSpringHor;
                camVertSpring = settings.camFarSpringVert;
                camFoV = settings.camFarFoV;
                camOffset = settings.camFarOffset;
                camLA = settings.camFarLA;
                break;
        }

        if (shipCurrentCamra == 0 || shipCurrentCamra == 1)
        {
            settings.refMeshContainers.SetActive(true);
            // Camera acceleration length
            if (controller.btnThruster)
            {
                shipCameraAccelerationLength = Mathf.Lerp(shipCameraAccelerationLength, 2.0f, Time.deltaTime * 6);
            }
            else if (bShipIsBraking)
            {
                shipCameraAccelerationLength = Mathf.Lerp(shipCameraAccelerationLength, -2.0f, Time.deltaTime * 6);
            }
            else
            {
                shipCameraAccelerationLength = Mathf.Lerp(shipCameraAccelerationLength, 0.0f, Time.deltaTime * 6);
            }

            // Get the cameras position in the ships local space
            Vector3 camDistance = transform.InverseTransformPoint(shipCamera.transform.position);

            if (controller.inputLeftAirbrake != 0 || controller.inputRightAirbrake != 0)
            {
                float camSensitivity = 0.06f;
                shipCameraSpring = Mathf.Lerp(shipCameraSpring, camSensitivity, Time.deltaTime * (camHorSpring / 2));
            }
            else
            {
                shipCameraSpring = Mathf.Lerp(shipCameraSpring, 0.035f, Time.deltaTime * (camHorSpring / 6));
            }

            // Set the x offset
            float turnSensitivity = 1.5f;
            if (shipCurrentCamra == 1)
                turnSensitivity = 3.5f;
            shipCameraStablizerFree = Mathf.Lerp(shipCameraStablizerFree, Mathf.Abs(camDistance.x) * 2.5f, Time.deltaTime * (camHorSpring / 2));
            shipCameraOffsetLag = Mathf.Lerp
                (
                shipCameraOffsetLag,
                (-transform.InverseTransformDirection(shipBody.velocity).x * shipCameraSpring) + (shipTurningAmount * turnSensitivity),
                Time.deltaTime * (camHorSpring + (shipCameraStablizerFree))
                );

            // Set the y offset
            float heightHelper = camDistance.y;
            if (heightHelper > 0)
                heightHelper = 0;
            heightHelper = Mathf.Clamp(heightHelper, -1f, 0.0f);
            if (bShipIsGrounded)
            {
                shipCameraHeightLag = Mathf.Lerp
                    (
                    shipCameraHeightLag,
                    -transform.InverseTransformDirection(shipBody.velocity).y * (0.02f + heightHelper),
                    Time.deltaTime * (camVertSpring - (heightHelper * 10))
                    );
            }
            else
            {
                shipCameraHeightLag = Mathf.Lerp
                   (
                   shipCameraHeightLag,
                   -transform.InverseTransformDirection(shipBody.velocity).y * 0.05f,
                   Time.deltaTime * camVertSpring
                   );
            }

            // Set the z offset
            shipCameraChaseLag = Mathf.Lerp
            (
                shipCameraChaseLag,
                (transform.InverseTransformDirection(shipBody.velocity).z * Time.deltaTime) * 0.25f,
                Time.deltaTime * 2
            );

            // Parent the camera to the ship
            shipCamera.transform.parent = transform;

            // Boost Distance and FoV
            if (bShipIsBoosting)
            {
                shipCameraBoostChase = Mathf.Lerp(shipCameraBoostChase, 0.2f + ((transform.InverseTransformDirection(shipBody.velocity).z * Time.deltaTime) * 0.1f), Time.deltaTime * 15);
                shipCameraBoostFoV = 15 + ((transform.InverseTransformDirection(shipBody.velocity).z * Time.deltaTime) * 2);
            }
            else
            {
                shipCameraBoostChase = Mathf.Lerp(shipCameraBoostChase, 0.0f, Time.deltaTime * 4);
                shipCameraBoostFoV = Mathf.Lerp(shipCameraBoostFoV, 0.0f, Time.deltaTime * 5);
            }

            // Set the local offset
            Vector3 camLocalOffset = camOffset;
            camLocalOffset.x = shipCameraOffsetLag;
            camLocalOffset.y = camOffset.y + (shipCameraHeightLag - (transform.InverseTransformDirection(shipBody.velocity).y * Time.deltaTime)) - (shipPitchAmount * 0.1f);
            camLocalOffset.z = (camOffset.z - shipCameraChaseLag) + (Mathf.Abs(shipCameraOffsetLag) * 0.15f) + shipCameraBoostChase;

            // Position the camera
            shipCamera.transform.localPosition = camLocalOffset;

            // Get camera lookat position
            float pitchMult = GlobalSettings.camPitchModDownMult;
            if (!bShipIsGrounded)
            {
                pitchMult = 1f;
            }
            float camPitchMod = ((transform.InverseTransformDirection(shipBody.velocity).y * Time.deltaTime * pitchMult));
            shipCameraPitchModify = Mathf.Lerp(shipCameraPitchModify, camPitchMod, Time.deltaTime * camVertSpring);
            Vector3 localLA = transform.TransformPoint(camLA.x, camLA.y + shipCameraPitchModify + (shipPitchAmount * GlobalSettings.camPitchModDownMult), camLA.z + shipCameraAccelerationLength + (shipCameraBoostChase * 10));

            Quaternion LookAt = Quaternion.LookRotation(localLA - shipCamera.transform.position, transform.up);
            shipCamera.transform.rotation = LookAt;

            /* Camera Effects */
            // Set field of view
            shipCameraFoV = Mathf.Lerp(shipCameraFoV, camFoV + ((transform.InverseTransformDirection(shipBody.velocity).z * Time.deltaTime) * 3) + shipCameraBoostFoV, Time.deltaTime * camVertSpring);
            if (shipCameraFoV < 60)
                shipCameraFoV = 60;
        } else
        {
            shipCameraFoV = camFoV;
            shipCamera.transform.localPosition = settings.camIntOffset;
            shipCamera.transform.localRotation = Quaternion.Euler(0, 0, -shipBankingAmount);

            settings.refMeshContainers.SetActive(false);
        }

        if (bLookingBehind)
        {
            settings.refMeshContainers.SetActive(false);
            shipCameraFoV = settings.camBackFoV;
            shipCamera.transform.localPosition = settings.camBackOffset;
            shipCamera.transform.localRotation = Quaternion.Euler(0, 180, shipBankingAmount);
        }

        // Apply field of view
        shipCamera.GetComponent<Camera>().fieldOfView = shipCameraFoV;


    }

    public void UpdateCamera()
    {
        shipCurrentCamra++;
        if (shipCurrentCamra > 2)
        {
            shipCurrentCamra = 0;
        }
    }

    public void ShipAxisAnimations()
    {
        if (Mathf.Abs(transform.InverseTransformDirection(shipBody.velocity).z) < 100 && bShipIsGrounded)
        {
            // Anim Settings
            shipHoverAnimSpeed = 0.5f;
            shipHoverAnimAmount = 0.2f;

            shipHoverAnimTimer += Time.deltaTime * shipHoverAnimSpeed;
            shipHoverAnimOffset.x = (Mathf.Sin(shipHoverAnimTimer * 1.5f) * shipHoverAnimAmount) * Mathf.Sin(shipHoverAnimTimer);
            shipHoverAnimOffset.y = (Mathf.Cos(shipHoverAnimTimer * 4) * (shipHoverAnimAmount / 2));

        } else
        {
            shipHoverAnimTimer = 0;
            shipHoverAnimOffset = Vector3.Lerp(shipHoverAnimOffset, Vector3.zero, Time.deltaTime);
        }

        // Apply Hover Anim Offset
        shipAxis.transform.localPosition = shipHoverAnimOffset;
    }

    /// <summary>
    /// Checks if the ship can sideshift and then starts a sideshift if it can.
    /// Input -1 for left, 1 for right.
    /// </summary>
    /// <param name="direction"></param>
    public void StartSideShift(int direction)
    {
        // Only allow the ship to sideshift if it is not already sideshifting,
        // if the sideshifting is not cooling down at that the ship is grounded
        if (!bIsShipSS && !bSSCoolingDown && bShipIsGrounded)
        {
            // The ship is now sideshifting
            bIsShipSS = true;
            // Invoke the sideshift to stop in 0.28 seconds
            Invoke("StopSideShift", 0.28f);
            // Set direction of sideshift
            if (direction == 1)
            {
                sideShiftDirection = transform.right;
                // Controller Vibration
                vibrationManager.vibRightMotor = 0.1f;
                vibrationManager.timerMotor = 1.4f;
            } else
            {
                sideShiftDirection = -transform.right;
                // Controller Vibration
                vibrationManager.vibLeftMotor = 0.1f;
                vibrationManager.timerMotor = 1.4f;
            }
            // PLAY SOUND HERE
        }
    }

    /// <summary>
    /// Stops the ship from sideshifting.
    /// </summary>
    public void StopSideShift()
    {
        // The ship is no longer sideshifting
        bIsShipSS = false;
        // Activate the cooldown
        bSSCoolingDown = true;
        // Invoke the cooldown to stop in 1 second
        Invoke("SideShiftCoolDown", 1.0f);
        // Reset the sideshift direction
        sideShiftDirection = Vector3.zero;
    }

    /// <summary>
    /// Allow the ship to sideshift again.
    /// </summary>
    public void SideShiftCoolDown()
    {
        // The cooldown has ended
        bSSCoolingDown = false;
    }

    /// <summary>
    /// Handles barrel rolling for the ship.
    /// </summary>
    public void ShipBarrelRoll()
    {

    }

    /// <summary>
    /// Switch to the next camera.
    /// </summary>
    public void ProgressCamera()
    {

    }

    /// <summary>
    /// Returns a vector that can be used to make the ship hover.
    /// </summary>
    /// <param name="HoverLocation"></param>
    /// <param name="DistanceToGround"></param>
    /// <returns></returns>
    public Vector3 CalculateHoverForce(Vector3 HoverLocation, float DistanceToGround)
    {
        return Vector3.zero;
    }

    /// <summary>
    /// Returns a vector that can be used to make the ship lock above the track.
    /// </summary>
    /// <param name="HoverLocation"></param>
    /// <returns></returns>
    public Vector3 CalculateMagStripForce(Vector3 HoverLocation)
    {
        return Vector3.zero;
    }

    public void SetupShip()
    {
        // Create rigidbody
        shipBody = gameObject.AddComponent<Rigidbody>();
        shipBody.useGravity = false;
        shipBody.drag = 0;
        shipBody.angularDrag = 20;

        shipBody.constraints = RigidbodyConstraints.FreezeRotationY;

        // Create physics material
        shipPhysicsMaterial = new PhysicMaterial();
        shipPhysicsMaterial.bounciness = 0;
        shipPhysicsMaterial.staticFriction = 0;
        shipPhysicsMaterial.dynamicFriction = 0;
        shipPhysicsMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
        shipPhysicsMaterial.frictionCombine = PhysicMaterialCombine.Minimum;

        // Create ship axis
        shipAxis = new GameObject("Ship Axis");
        shipAxis.transform.parent = transform;
        shipAxis.transform.localPosition = Vector3.zero;
        
        // Load ship
        GameObject ShipObject = Instantiate(Resources.Load("Ships/Test/LoadMe") as GameObject) as GameObject;
        ShipObject.transform.parent = shipAxis.transform;
        ShipObject.transform.localPosition = Vector3.zero;

        // Get ship settings
        settings = ShipObject.GetComponent<ShipSettings>();

        // Create ship collider
        GameObject ShipCollider = new GameObject("Ship Collider");
        ShipCollider.transform.parent = transform;
        ShipCollider.transform.localPosition = Vector3.zero;
        ShipCollider.AddComponent<BoxCollider>();
        ShipCollider.GetComponent<BoxCollider>().size = settings.physColliderSize;
        ShipCollider.GetComponent<BoxCollider>().material = shipPhysicsMaterial;

        // Create controller
        controller = gameObject.AddComponent<ShipController>();
        controller.controller = inputControl;

        // Camera
        if (bShipHasCameraControl)
        {
            shipCamera = Camera.main.gameObject;
        }

        // Attach forcefeedback manager
        vibrationManager = gameObject.AddComponent<FFManager>();

        // Attach Audio/Visual Manager
        gameObject.AddComponent<ShipAVManager>();

    }

}
