// AGR2280 2012 - 2015
// Created by Vonsnake


using UnityEngine;
using UnityEditor;
using System.Collections;

//[CustomEditor(typeof(ShipSettings))]
public class ShipInspector : Editor {

    private bool bEngineExpanded;
    private bool bEngineDClassExpanded;
    private bool bEngineCClassExpanded;
    private bool bEngineBClassExpanded;
    private bool bEngineAClassExpanded;
    private bool bEngineAPClassExpanded;
    private bool bEngineAPPClassExpanded;

    private bool bTurningExpanded;
    private bool bAirbrakesExpanded;

    private bool bAntiGravityExpanded;
    private bool bAntiGravityDClassExpanded;
    private bool bAntiGravityCClassExpanded;
    private bool bAntiGravityBClassExpanded;
    private bool bAntiGravityAClassExpanded;
    private bool bAntiGravityAPClassExpanded;
    private bool bAntiGravityAPPClassExpanded;

    private bool bCameraExpanded;
    private bool bCloseCamExpanded;
    private bool bFarCamExpanded;
    private bool bInternalCamExpanded;
    private bool bBackwardCamExpanded;
    private bool bBonnetCamExpanded;

    private bool bMiscExpanded;
    private bool bFEExpanded;

    public override void OnInspectorGUI()
    {
        // Set class target
        ShipSettings classTarget = (ShipSettings)target;

        // Get current editor GUI settings (for resetting after everything below)
        int origFoldoutFontSize = EditorStyles.foldout.fontSize;
        FontStyle origFoldoutFontStyle = EditorStyles.foldout.fontStyle;

        #region EngineFoldout
        // Engine
        EditorStyles.foldout.fontSize = 16;
        EditorStyles.foldout.fontStyle = FontStyle.Italic;

        bEngineExpanded = EditorGUILayout.Foldout(bEngineExpanded, "Engine Settings");
        if (bEngineExpanded)
        {
            // Labels
            GUIContent AccelCap = new GUIContent("Acceleration Cap", "The cap value for how quickly the ship reaches the thrust cap.");
            GUIContent ThrustCap = new GUIContent("Thrust Cap", "The top force that can be applied to the ship.");
            GUIContent Gain = new GUIContent("Engine Gain", "How quickly the acceleration reaches it's cap.");
            GUIContent Falloff = new GUIContent("Engine Falloff", "How quickly the acceleration and thrust decrease.");
            GUIContent Turbo = new GUIContent("Engine Turbo", "How powerful the Turbo pickup is.");

            // Font Styles
            EditorStyles.foldout.fontSize = 12;
            EditorStyles.foldout.fontStyle = FontStyle.Bold;

            EditorGUILayout.Separator();

            // Others
            classTarget.engineGain = EditorGUILayout.FloatField(Gain, classTarget.engineGain, GUILayout.ExpandWidth(false));
            classTarget.engineFalloff = EditorGUILayout.FloatField(Falloff, classTarget.engineFalloff, GUILayout.ExpandWidth(false));
            classTarget.engineTurbo = EditorGUILayout.FloatField(Turbo, classTarget.engineTurbo, GUILayout.ExpandWidth(false));

            EditorGUILayout.Separator();

            // D Class
            bEngineDClassExpanded = EditorGUILayout.Foldout(bEngineDClassExpanded, "D Class");
            if (bEngineDClassExpanded)
            {
                classTarget.engineAccelCapD = EditorGUILayout.FloatField(AccelCap, classTarget.engineAccelCapD, GUILayout.ExpandWidth(false));
                classTarget.engineThrustCapD = EditorGUILayout.FloatField(ThrustCap, classTarget.engineThrustCapD, GUILayout.ExpandWidth(false));
            }
            
            // C Class
            bEngineCClassExpanded = EditorGUILayout.Foldout(bEngineCClassExpanded, "C Class");
            if (bEngineCClassExpanded)
            {
                classTarget.engineAccelCapC = EditorGUILayout.FloatField(AccelCap, classTarget.engineAccelCapC, GUILayout.ExpandWidth(false));
                classTarget.engineThrustCapC = EditorGUILayout.FloatField(ThrustCap, classTarget.engineThrustCapC, GUILayout.ExpandWidth(false));
            }

            // B Class
            bEngineBClassExpanded = EditorGUILayout.Foldout(bEngineBClassExpanded, "B Class");
            if (bEngineBClassExpanded)
            {
                classTarget.engineAccelCapB = EditorGUILayout.FloatField(AccelCap, classTarget.engineAccelCapB, GUILayout.ExpandWidth(false));
                classTarget.engineThrustCapB = EditorGUILayout.FloatField(ThrustCap, classTarget.engineThrustCapB, GUILayout.ExpandWidth(false));
            }

            // A Class
            bEngineAClassExpanded = EditorGUILayout.Foldout(bEngineAClassExpanded, "A Class");
            if (bEngineAClassExpanded)
            {
                classTarget.engineAccelCapA = EditorGUILayout.FloatField(AccelCap, classTarget.engineAccelCapA, GUILayout.ExpandWidth(false));
                classTarget.engineThrustCapA = EditorGUILayout.FloatField(ThrustCap, classTarget.engineThrustCapA, GUILayout.ExpandWidth(false));
            }

            // A+ Class
            bEngineAPClassExpanded = EditorGUILayout.Foldout(bEngineAPClassExpanded, "A+ Class");
            if (bEngineAPClassExpanded)
            {
                classTarget.engineAccelCapAP = EditorGUILayout.FloatField(AccelCap, classTarget.engineAccelCapAP, GUILayout.ExpandWidth(false));
                classTarget.engineThrustCapAP = EditorGUILayout.FloatField(ThrustCap, classTarget.engineThrustCapAP, GUILayout.ExpandWidth(false));
            }

            // A++ Class
            bEngineAPPClassExpanded = EditorGUILayout.Foldout(bEngineAPPClassExpanded, "A++ Class");
            if (bEngineAPPClassExpanded)
            {
                classTarget.engineAccelCapAPP = EditorGUILayout.FloatField(AccelCap, classTarget.engineAccelCapAPP, GUILayout.ExpandWidth(false));
                classTarget.engineThrustCapAPP = EditorGUILayout.FloatField(ThrustCap, classTarget.engineThrustCapAPP, GUILayout.ExpandWidth(false));
            }

        }

        EditorGUILayout.Separator();

        #endregion

        #region TurningFoldout
        // Turning
        EditorStyles.foldout.fontSize = 16;
        EditorStyles.foldout.fontStyle = FontStyle.Italic;

        bTurningExpanded = EditorGUILayout.Foldout(bTurningExpanded, "Turning Settings");

        if (bTurningExpanded)
        {
            // Labels
            GUIContent Amount = new GUIContent("Turning Amount", "How fast the ship can turn.");
            GUIContent Gain = new GUIContent("Turning Gain", "How quickly the ship starts rotating.");
            GUIContent Falloff = new GUIContent("Turning Falloff", "How quickly the ship stops rotating.");

            EditorGUILayout.Separator();

            // Settings
            classTarget.turnAmount = EditorGUILayout.FloatField(Amount, classTarget.turnAmount, GUILayout.ExpandWidth(false));
            classTarget.turnGain = EditorGUILayout.FloatField(Gain, classTarget.turnGain, GUILayout.ExpandWidth(false));
            classTarget.turnFalloff = EditorGUILayout.FloatField(Falloff, classTarget.turnFalloff, GUILayout.ExpandWidth(false));

        }
        EditorGUILayout.Separator();
        #endregion

        #region AirbrakesFoldout
        // Airbrakes
        EditorStyles.foldout.fontSize = 16;
        EditorStyles.foldout.fontStyle = FontStyle.Italic;

        bAirbrakesExpanded = EditorGUILayout.Foldout(bAirbrakesExpanded, "Airbrakes Settings");
        
        if (bAirbrakesExpanded)
        {
            // Labels
            GUIContent Amount = new GUIContent("Airbrakes Amount", "How much the airbrakes cause the ship to bank over.");
            GUIContent Drag = new GUIContent("Airbrakes Drag", "How much air resistance the airbrakes cause.");
            GUIContent Gain = new GUIContent("Airbrakes Gain", "How quickly the airbrakes start having an effect on the ship.");
            GUIContent Falloff = new GUIContent("Airbrakes Falloff", "How quickly the airbrakes stop having an effect on the ship.");
            GUIContent Turn = new GUIContent("Airbrakes Turn", "How sensitive the airbrakes are.");
            GUIContent Slidegrip = new GUIContent("Airbrakes Slidegrip", "How much the airbrakes grip the ship to the track.");
            GUIContent Sideshift = new GUIContent("Airbrakes Sideshift", "How strong sideshifting is.");

            GUIContent VisualAmount = new GUIContent("Visual Amount", "The angle that the airbrakes on the ship raise to.");
            GUIContent UpSpeed = new GUIContent("Visual Up Speed", "How quickly the airbrake models on the ship raise.");
            GUIContent DownSpeed = new GUIContent("Visual Down Speed", "How quickly the airbrake models on the ship lower."); 

            EditorGUILayout.Separator();

            // Settings
            classTarget.airbrakesAmount = EditorGUILayout.FloatField(Amount, classTarget.airbrakesAmount, GUILayout.ExpandWidth(false));
            classTarget.airbrakesDrag = EditorGUILayout.FloatField(Drag, classTarget.airbrakesDrag, GUILayout.ExpandWidth(false));
            classTarget.airbrakesGain = EditorGUILayout.FloatField(Gain, classTarget.airbrakesGain, GUILayout.ExpandWidth(false));
            classTarget.airbrakesFalloff = EditorGUILayout.FloatField(Falloff, classTarget.airbrakesFalloff, GUILayout.ExpandWidth(false));
            classTarget.airbrakesTurn = EditorGUILayout.FloatField(Turn, classTarget.airbrakesTurn, GUILayout.ExpandWidth(false));
            classTarget.airbrakesSlidegrip = EditorGUILayout.FloatField(Slidegrip, classTarget.airbrakesSlidegrip, GUILayout.ExpandWidth(false));
            classTarget.airbrakesSideshift = EditorGUILayout.FloatField(Sideshift, classTarget.airbrakesSideshift, GUILayout.ExpandWidth(false));
            EditorGUILayout.Separator();
            classTarget.airbrakeAmount = EditorGUILayout.FloatField(VisualAmount, classTarget.airbrakeAmount, GUILayout.ExpandWidth(false));
            classTarget.airbrakeUpSpeed = EditorGUILayout.FloatField(UpSpeed, classTarget.airbrakeUpSpeed, GUILayout.ExpandWidth(false));
            classTarget.airbrakeDownSpeed = EditorGUILayout.FloatField(DownSpeed, classTarget.airbrakeDownSpeed, GUILayout.ExpandWidth(false));
        }
        EditorGUILayout.Separator();
        #endregion

        #region AntiGravityFoldout
        // Anti-Gravity Foldout
        EditorStyles.foldout.fontSize = 16;
        EditorStyles.foldout.fontStyle = FontStyle.Italic;

        bAntiGravityExpanded = EditorGUILayout.Foldout(bAntiGravityExpanded, "Anti-Gravity Settings");
        if (bAntiGravityExpanded)
        {
            // Labels
            GUIContent Rebound = new GUIContent("Rebound", "A force that keeps the ship grounded better.");
            GUIContent ReboundLanding = new GUIContent("Landing Rebound", "A damping multiplier to prevent the ship bouncing up high from great falls.");
            GUIContent ReboundJumpTime = new GUIContent("Rebound Jump Time", "How long the rebound force is active for.");
            GUIContent AirGrip = new GUIContent("Air Grip", "How much grip the ship has when in flight.");
            GUIContent TrackGrip = new GUIContent("Ground Grip", "How much grip the ship has when hovering above the track.");

            // Font Styles
            EditorStyles.foldout.fontSize = 12;
            EditorStyles.foldout.fontStyle = FontStyle.Bold;

            EditorGUILayout.Separator();

            // Others
            classTarget.agRebound = EditorGUILayout.FloatField(Rebound, classTarget.engineGain, GUILayout.ExpandWidth(false));
            classTarget.agReboundLanding = EditorGUILayout.FloatField(ReboundLanding, classTarget.engineFalloff, GUILayout.ExpandWidth(false));
            classTarget.agReboundJumpTime = EditorGUILayout.FloatField(ReboundJumpTime, classTarget.engineTurbo, GUILayout.ExpandWidth(false));

            EditorGUILayout.Separator();

            // D Class
            bAntiGravityDClassExpanded = EditorGUILayout.Foldout(bAntiGravityDClassExpanded, "D Class");
            if (bAntiGravityDClassExpanded)
            {
               classTarget.agGripAirD = EditorGUILayout.FloatField(AirGrip, classTarget.agGripAirD, GUILayout.ExpandWidth(false));
               classTarget.agGripGroundD = EditorGUILayout.FloatField(TrackGrip, classTarget.agGripGroundD, GUILayout.ExpandWidth(false));
            }

            // C Class
            bAntiGravityCClassExpanded = EditorGUILayout.Foldout(bAntiGravityCClassExpanded, "C Class");
            if (bAntiGravityCClassExpanded)
            {
                classTarget.agGripAirC = EditorGUILayout.FloatField(AirGrip, classTarget.agGripAirC, GUILayout.ExpandWidth(false));
                classTarget.agGripGroundC = EditorGUILayout.FloatField(TrackGrip, classTarget.agGripGroundC, GUILayout.ExpandWidth(false));
            }

            // B Class
            bAntiGravityBClassExpanded = EditorGUILayout.Foldout(bAntiGravityBClassExpanded, "B Class");
            if (bAntiGravityBClassExpanded)
            {
                classTarget.agGripAirB = EditorGUILayout.FloatField(AirGrip, classTarget.agGripAirB, GUILayout.ExpandWidth(false));
                classTarget.agGripGroundB = EditorGUILayout.FloatField(TrackGrip, classTarget.agGripGroundB, GUILayout.ExpandWidth(false));
            }

            // A Class
            bAntiGravityAClassExpanded = EditorGUILayout.Foldout(bAntiGravityAClassExpanded, "A Class");
            if (bAntiGravityAClassExpanded)
            {
                classTarget.agGripAirA = EditorGUILayout.FloatField(AirGrip, classTarget.agGripAirA, GUILayout.ExpandWidth(false));
                classTarget.agGripGroundA = EditorGUILayout.FloatField(TrackGrip, classTarget.agGripGroundA, GUILayout.ExpandWidth(false));
            }

            // AP Class
            bAntiGravityAPClassExpanded = EditorGUILayout.Foldout(bAntiGravityAPClassExpanded, "A+ Class");
            if (bAntiGravityAPClassExpanded)
            {
                classTarget.agGripAirAP = EditorGUILayout.FloatField(AirGrip, classTarget.agGripAirAP, GUILayout.ExpandWidth(false));
                classTarget.agGripGroundAP = EditorGUILayout.FloatField(TrackGrip, classTarget.agGripGroundAP, GUILayout.ExpandWidth(false));
            }

            // D Class
            bAntiGravityAPPClassExpanded = EditorGUILayout.Foldout(bAntiGravityAPPClassExpanded, "A++ Class");
            if (bAntiGravityAPPClassExpanded)
            {
                classTarget.agGripAirAPP = EditorGUILayout.FloatField(AirGrip, classTarget.agGripAirAPP, GUILayout.ExpandWidth(false));
                classTarget.agGripGroundAPP = EditorGUILayout.FloatField(TrackGrip, classTarget.agGripGroundAPP, GUILayout.ExpandWidth(false));
            }
        }
        EditorGUILayout.Separator();
        #endregion

        #region Camera
        // Camera Rollout
        EditorStyles.foldout.fontSize = 16;
        EditorStyles.foldout.fontStyle = FontStyle.Italic;

        bCameraExpanded = EditorGUILayout.Foldout(bCameraExpanded, "Camera Settings");
        if (bCameraExpanded)
        {
            // Labels
            GUIContent FoV = new GUIContent("Field of View", "How many degrees the camera can see.");

            // Font Styles
            EditorStyles.foldout.fontSize = 12;
            EditorStyles.foldout.fontStyle = FontStyle.Bold;

            EditorGUILayout.Separator();

            // Close Camera
            bCloseCamExpanded = EditorGUILayout.Foldout(bCloseCamExpanded, "Close Camera");
            if (bCloseCamExpanded)
            {
                classTarget.camCloseFoV = EditorGUILayout.FloatField(FoV, classTarget.camCloseFoV, GUILayout.ExpandWidth(false));

                GUIContent HorSpring = new GUIContent("Horizontal Spring", "How fast the camera chases the ship on the X axis.");
                classTarget.camCloseSpringHor = EditorGUILayout.FloatField(HorSpring, classTarget.camCloseSpringHor, GUILayout.ExpandWidth(false));

                GUIContent VertSpring = new GUIContent("Vertical Spring", "How fast the camera chases the ship on the Y axis.");
                classTarget.camCloseSpringVert = EditorGUILayout.FloatField(VertSpring, classTarget.camCloseSpringVert, GUILayout.ExpandWidth(false));

                GUIContent LA = new GUIContent("Look At", "The position relative to the ship the camera will look at.");
                classTarget.camCloseLA = EditorGUILayout.Vector3Field(LA, classTarget.camCloseLA);

                GUIContent POS = new GUIContent("Position", "The target position behind the ship the camera will chase.");
                classTarget.camCloseOffset = EditorGUILayout.Vector3Field(POS, classTarget.camCloseOffset);
            }

            // Far Camera
            bFarCamExpanded = EditorGUILayout.Foldout(bFarCamExpanded, "Far Camera");
            if (bFarCamExpanded)
            {
                classTarget.camFarFoV = EditorGUILayout.FloatField(FoV, classTarget.camFarFoV, GUILayout.ExpandWidth(false));

                GUIContent HorSpring = new GUIContent("Horizontal Spring", "How fast the camera chases the ship on the X axis.");
                classTarget.camFarSpringHor = EditorGUILayout.FloatField(HorSpring, classTarget.camFarSpringHor, GUILayout.ExpandWidth(false));

                GUIContent VertSpring = new GUIContent("Vertical Spring", "How fast the camera chases the ship on the Y axis.");
                classTarget.camFarSpringVert = EditorGUILayout.FloatField(VertSpring, classTarget.camFarSpringVert, GUILayout.ExpandWidth(false));

                GUIContent LA = new GUIContent("Look At", "The position relative to the ship the camera will look at.");
                classTarget.camFarLA = EditorGUILayout.Vector3Field(LA, classTarget.camFarLA);

                GUIContent POS = new GUIContent("Position", "The target position behind the ship the camera will chase.");
                classTarget.camFarOffset = EditorGUILayout.Vector3Field(POS, classTarget.camFarOffset);
            }

            // Internal Camera
            bInternalCamExpanded = EditorGUILayout.Foldout(bInternalCamExpanded, "Internal Camera");
            if (bInternalCamExpanded)
            {
                classTarget.camIntFoV = EditorGUILayout.FloatField(FoV, classTarget.camIntFoV, GUILayout.ExpandWidth(false));

                GUIContent POS = new GUIContent("Position", "The position relative to the ship the camera will sit.");
                classTarget.camIntOffset= EditorGUILayout.Vector3Field(POS, classTarget.camIntOffset);
            }

            // Backwards Camera
            bBackwardCamExpanded = EditorGUILayout.Foldout(bBackwardCamExpanded, "Backwards Camera");
            if (bBackwardCamExpanded)
            {
                classTarget.camBackFoV = EditorGUILayout.FloatField(FoV, classTarget.camBackFoV, GUILayout.ExpandWidth(false));

                GUIContent POS = new GUIContent("Position", "The position relative to the ship the camera will sit.");
                classTarget.camBackOffset = EditorGUILayout.Vector3Field(POS, classTarget.camBackOffset);
            }

            // Bonnet Camera
            bBonnetCamExpanded = EditorGUILayout.Foldout(bBonnetCamExpanded, "Bonnet Camera");
            if (bBonnetCamExpanded)
            {
                classTarget.camBonnetFoV = EditorGUILayout.FloatField(FoV, classTarget.camBonnetFoV, GUILayout.ExpandWidth(false));

                GUIContent POS = new GUIContent("Position", "The position relative to the ship the camera will sit.");
                classTarget.camBonnetOffset = EditorGUILayout.Vector3Field(POS, classTarget.camBonnetOffset);
            }
        }
        EditorGUILayout.Separator();
        #endregion

        #region Misc
        // Misc Rollout
        EditorStyles.foldout.fontSize = 16;
        EditorStyles.foldout.fontStyle = FontStyle.Italic;

        bMiscExpanded = EditorGUILayout.Foldout(bMiscExpanded, "Misc Settings");
        if (bMiscExpanded)
        {
            // Labels
            GUIContent ColliderSize = new GUIContent("Collider Scale", "The actual physical size of the ship which will be handled by the physics engine.");
            GUIContent ShieldAmount = new GUIContent("Shield Amount", "How strong the ship's shield generator is (from a range of 0 - 1).");
            GUIContent WeightDist = new GUIContent("Weight Distrubution", "Centre of Mass offset to help weight the ship.");

            GUIContent TiltInternalSpeed = new GUIContent("Internal Tilt Speed", "How fast the internal camera wobbles when the ship looses balance.");
            GUIContent TiltInternalAmount = new GUIContent("Internal Tilt Amount", "How much the internal camera wobbles when the ship looses balance.");
            GUIContent TiltShipSpeed = new GUIContent("Ship Tilt Speed", "How fast the ship wobbles when the ship looses balance.");
            GUIContent TiltShipAmount = new GUIContent("Ship Tilt Amount", "How much the ship wobbles when the ship looses balance.");

            EditorGUILayout.Separator();

            classTarget.physColliderSize = EditorGUILayout.Vector3Field(ColliderSize, classTarget.physColliderSize);
            classTarget.physShieldAmount = EditorGUILayout.Slider(ShieldAmount, classTarget.physShieldAmount, 0.0f, 1.0f);
            classTarget.physWeightDist = EditorGUILayout.Slider(WeightDist, classTarget.physWeightDist, -4.0f, 4.0f);

            EditorGUILayout.Separator();
            classTarget.tiltInternalSpeed = EditorGUILayout.FloatField(TiltInternalSpeed, classTarget.tiltInternalSpeed, GUILayout.ExpandWidth(false));
            classTarget.tiltInternalAmount = EditorGUILayout.FloatField(TiltInternalAmount, classTarget.tiltInternalAmount, GUILayout.ExpandWidth(false));
            classTarget.tiltShipSpeed = EditorGUILayout.FloatField(TiltShipSpeed, classTarget.tiltShipSpeed, GUILayout.ExpandWidth(false));
            classTarget.tiltShipAmount = EditorGUILayout.FloatField(TiltShipAmount, classTarget.tiltShipAmount, GUILayout.ExpandWidth(false));


            EditorGUILayout.Separator();
        }
        EditorGUILayout.Separator();
        #endregion

        #region FrontEnd
        // Frontend Rollout
        EditorStyles.foldout.fontSize = 16;
        EditorStyles.foldout.fontStyle = FontStyle.Italic;

        bFEExpanded = EditorGUILayout.Foldout(bFEExpanded, "Front-End Settings");
        if (bFEExpanded)
        {
            // Labels
            GUIContent Speed = new GUIContent("Speed", "The value (0 - 10) that is shown on the menu.");
            GUIContent Thrust = new GUIContent("Thrust", "The value (0 - 10) that is shown on the menu.");
            GUIContent Handling = new GUIContent("Handling", "The value (0 - 10) that is shown on the menu.");
            GUIContent Shield = new GUIContent("Shield", "The value (0 - 10) that is shown on the menu.");

            EditorGUILayout.Separator();

            classTarget.feSpeed = EditorGUILayout.IntSlider(Speed, classTarget.feSpeed, 0, 10);
            classTarget.feThrust = EditorGUILayout.IntSlider(Thrust, classTarget.feThrust, 0, 10);
            classTarget.feHandling = EditorGUILayout.IntSlider(Handling, classTarget.feHandling, 0, 10);
            classTarget.feShield = EditorGUILayout.IntSlider(Shield, classTarget.feShield, 0, 10);

        }
        EditorGUILayout.Separator();
        #endregion

        // Information
        EditorGUILayout.LabelField("This custom inspector makes it easier to edit the ship settings.");
        EditorGUILayout.LabelField("Attach this script to your ship model prefab!");
        // Reset editor styles
        EditorStyles.foldout.fontSize = origFoldoutFontSize;
        EditorStyles.foldout.fontStyle = origFoldoutFontStyle;
        
    }
}
