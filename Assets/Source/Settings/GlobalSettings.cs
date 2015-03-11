// AGR2280 2012 - 2015
// Created by Vonsnake

using UnityEngine;
using System.Collections;

/// <summary>
/// Contains all of the variables needed for shared functionality across all classes.
/// </summary>

public class GlobalSettings : MonoBehaviour
{

    // Controller
    public enum InputController
    {
        Player = 0,
        AI = 1
    };

    // Speed Classes
    public enum SpeedClasses
    {
        D = 0,
        C = 1,
        B = 2,
        A = 3,
        AP = 4,
        APP = 5
    };

    // Global Ship Settings
    // Pitching
    public static float shipPitchAirAmount = 0.2f;
    public static float shipPitchGroundAmount = 1.0f;
    public static float shipPitchDamping = 5.0f;
    public static float shipAntiGravHeightAdjust = 1.0f;

    // Brakes
    public static float shipBrakesAmount = 100.0f;
    public static float shipBrakesGain = 100.0f;
    public static float shipBrakesFalloff = 100.0f;

    // Gravity Multiplier
    public static float airGravityMulD = 0.95f;
    public static float airGravityMulC = 1.0f;
    public static float airGravityMulB = 1.1f;
    public static float airGravityMulA = 1.2f;
    public static float airGravityMulAP = 1.3f;
    public static float airGravityMulAPP = 1.5f;

    // Track Gravity
    public static float shipTrackGravityD = 80.0f;
    public static float shipTrackGravityC = 80.0f;
    public static float shipTrackGravityB = 85.0f;
    public static float shipTrackGravityA = 85.0f;
    public static float shipTrackGravityAP = 85.0f;
    public static float shipTrackGravityAPP = 85.0f;

    // Flight Gravity
    public static float shipFlightGravityD = 110.0f;
    public static float shipFlightGravityC = 110.0f;
    public static float shipFlightGravityB = 115.0f;
    public static float shipFlightGravityA = 115.0f;
    public static float shipFlightGravityAP = 115.0f;
    public static float shipFlightGravityAPP = 115.0f;

    // Other Gravity
    public static float shipMass = 1.0f;
    public static float shipNormalGravity = 5.0f;

    // Other Settings
    public static float shipRideHeight = 5.5f;

    // Global Mods
    // Specials
    public static float rollCost = 15f;
    public static float rollSpeed = 1.5f;
    public static float rollTurboTime = 0.3f;
    public static float boostJump = 0.05f;

    // Zone
    public static float zoneIncrement = 0.4f;
    public static float zoneRecharge = 10.0f;
    public static float zoneStart = 34.0f;

    // Camera Pitch Mod
    public static float camPitchModDownMult = 0.35f;
    public static float camPitchModDownOffset = 3.0f;
    public static float camPitchModUpMult = 0.35f;
    public static float camPitchModUpOffset = 3.0f;

    // Start Boost
    public static float sbTimeLength = 0.4f;
    public static float sbBoostMultiplier = 1.7f;

    // Collision
    public static float WeaponHitFactor = 0.9f;
    public static float ShipCoF = 1.0f;
    public static float ShipCoR = 0.8f;

    // Other
    public static float speedPadAmount = 600.0f;
    public static float speedPadTime = 0.27f;

    public static float weaponPadRefreshTime = 0.75f;
    public static float weaponPadEliminationRefreshTime = 0.15f;

}
