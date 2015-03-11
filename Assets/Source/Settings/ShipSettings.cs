// AGR2280 2012 - 2015
// Created by Vonsnake


using UnityEngine;
using System.Collections;

/// <summary>
/// Stores settings for the ship simulator to read.
/// </summary>
public class ShipSettings : MonoBehaviour {

    // Turning
    public float turnAmount;
    public float turnGain;
    public float turnFalloff;

    // Acceleration Cap
    public float engineAccelCapD;
    public float engineAccelCapC;
    public float engineAccelCapB;
    public float engineAccelCapA;
    public float engineAccelCapAP;
    public float engineAccelCapAPP;

    // Thrust Cap
    public float engineThrustCapD;
    public float engineThrustCapC;
    public float engineThrustCapB;
    public float engineThrustCapA;
    public float engineThrustCapAP;
    public float engineThrustCapAPP;

    // Others
    public float engineFalloff;
    public float engineGain;
    public float engineTurbo;

    // Air Grip
    public float agGripAirD;
    public float agGripAirC;
    public float agGripAirB;
    public float agGripAirA;
    public float agGripAirAP;
    public float agGripAirAPP;

    // Ground Grip
    public float agGripGroundD;
    public float agGripGroundC;
    public float agGripGroundB;
    public float agGripGroundA;
    public float agGripGroundAP;
    public float agGripGroundAPP;

    // Rebound
    public float agReboundLanding;
    public float agRebound;
    public float agReboundJumpTime;

    // Close Camera
    public float camCloseFoV;
    public Vector3 camCloseLA;
    public Vector3 camCloseOffset;
    public float camCloseSpringHor;
    public float camCloseSpringVert;

    // Far Camera
    public float camFarFoV;
    public Vector3 camFarLA;
    public Vector3 camFarOffset;
    public float camFarSpringHor;
    public float camFarSpringVert;

    // Internal Camera
    public float camIntFoV;
    public Vector3 camIntOffset;

    // Backward Camera
    public float camBackFoV;
    public Vector3 camBackOffset;

    // Bonnet Camera
    public float camBonnetFoV;
    public Vector3 camBonnetOffset;

    // Airbrakes
    public float airbrakesAmount;
    public float airbrakesDrag;
    public float airbrakesFalloff;
    public float airbrakesGain;
    public float airbrakesTurn;
    public float airbrakesSlidegrip;
    public float airbrakesSideshift;

    // Airbrake visual
    public float airbrakeUpSpeed;
    public float airbrakeDownSpeed;
    public float airbrakeAmount;

    // Front-end
    public int feSpeed;
    public int feThrust;
    public int feHandling;
    public int feShield;

    // Tilt
    public float tiltInternalSpeed;
    public float tiltInternalAmount;
    public float tiltShipSpeed;
    public float tiltShipAmount;

    // Physical
    public Vector3 physColliderSize;
    public float physShieldAmount;
    public float physWeightDist;

    // Audio
    public AudioClip audioAirbrake;
    public AudioClip audioEngine;
    public AudioClip audioEngineStartup;
    public AudioClip audioEngineCooler;

    // References
    public GameObject refMeshContainers;
    public GameObject refEngineLight;
    public GameObject refEngineTrail;

}
