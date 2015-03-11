using UnityEngine;
using BSGTools.IO;
using IM = BSGTools.IO.InputMaster;
using System.Collections;
using System.Collections.Generic;

public class ControlsManager : MonoBehaviour {

    public static bool bHasLoadedControls;
    public static List<Control> controls = new List<Control>();

    public static void SetDefaultControls()
    {
        controls.Clear();
        IM.Initialize();

        controls.AddRange(new Control[]
        {
            new ActionControl("Thrust", Scope.All)
                .AddBinding(Binding.Space)
                .AddBinding(Binding.JoystickButton0), // A
            new ActionControl("Fire", Scope.All)
                .AddBinding(Binding.LeftShift)
                .AddBinding(Binding.JoystickButton2), // X
            new ActionControl("Absorb", Scope.All)
                .AddBinding(Binding.LeftControl)
                .AddBinding(Binding.JoystickButton1), // B
            new ActionControl("CameraToggle", Scope.All)
                .AddBinding(Binding.C)
                .AddBinding(Binding.JoystickButton3), // Y
            new ActionControl("LookBehind", Scope.All)
                .AddBinding(Binding.X)
                .AddBinding(Binding.JoystickButton4), // Shoulder Left
            new ActionControl("Special", Scope.All)
                .AddBinding(Binding.Z)
                .AddBinding(Binding.JoystickButton5), // Shoulder Right
            new ActionControl("Pause", Scope.All)
                .AddBinding(Binding.Escape)
                .AddBinding(Binding.JoystickButton7), // Start
            new AxisControl("Steer")
                .AddBinding(Binding.LeftArrow, -1f)
                .AddBinding(Binding.RightArrow, 1f)
                .AddBinding(Binding.LeftX, 1f)
                .AddBinding(Binding.DPadLeft, -1f)
                .AddBinding(Binding.DPadRight, 1f),
            new AxisControl("Pitch")
                .AddBinding(Binding.DownArrow, -1f)
                .AddBinding(Binding.UpArrow, 1f)
                .AddBinding(Binding.LeftY, 1f)
                .AddBinding(Binding.DPadDown, -1f)
                .AddBinding(Binding.DPadUp, 1),
            new AxisControl("LeftAirbrake")
                .AddBinding(Binding.Q, -1f)
                .AddBinding(Binding.TriggerLeft, -1f),
            new AxisControl("RightAirbrake")
                .AddBinding(Binding.E, 1f)
                .AddBinding(Binding.TriggerRight, 1f)
        });

        IM.controls.AddRange(controls);
    }
}
