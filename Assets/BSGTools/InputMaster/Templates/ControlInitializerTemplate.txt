using System.Collections.Generic;
using System.Linq;
using System.IO;
using BSGTools.IO;
using UnityEngine;
using IM = BSGTools.IO.InputMaster;

//Place this somewhere in your bootloader/first loaded scene!
public class InitializeControls : MonoBehaviour {
	//Modify for your config file path!
	public static string CFG_DIR = Application.dataPath;
	public static string CFG_PATH = CFG_DIR + @"\input.yaml";

	List<Control> controls = new List<Control>();

	void Start() {
		Init();
	}

	private void Init() {
		Directory.CreateDirectory(CFG_DIR);
		IM.controls.Clear();
		MakeDefaultControlList();
		try {
			var views = IMFileSystemTasks.ReadControls(CFG_PATH);
			CreateControlsFromConfig(views);
		}
		catch(System.Exception) {
			//In case of exception, recreates the list of default controls
			//To guarantee a working config
			MakeDefaultControlList();
		}
		IM.controls.AddRange(controls);
		IMFileSystemTasks.WriteControls(CFG_PATH);
		IM.Initialize();
		Destroy(this);
	}

	/// <summary>
	/// Attempts to modify control values using config file
	/// </summary>
	private void CreateControlsFromConfig(SimpleDataView[] views) {
		foreach(var v in views) {
			var control = controls.Single(c => c.identifier == v.identifier);
			if(control is ActionControl) {
				var ac = control as ActionControl;
				for(int i = 0;i < v.bindings.Length;i++)
					ac.bindings.Add(v.bindings[i], v.modifiers[i]);
			}
			else if(control is AxisControl) {
				var ax = control as AxisControl;
				var multipliers = new float[ax.bindings.Values.Count];
				ax.bindings.Values.CopyTo(multipliers, 0);
				ax.bindings.Clear();
				for(int i = 0;i < v.bindings.Length;i++)
					ax.bindings.Add(v.bindings[i], multipliers[i]);
			}
		}
	}

	/// <summary>
	/// Defines the default controls
	/// </summary>
	private void MakeDefaultControlList() {
		controls.Clear();

		//Create controls here!
		//Here's some sample controls
		//Consider using a static class of const strings
		//instead of hardcoding the IDs here.
		controls.AddRange(new Control[]{
			new ActionControl("PAUSE", Scope.ReleaseOnly) //Release only, because ESCAPE unlocks the mouse in the editor
				.AddBinding(Binding.Escape),
			new ActionControl("EDITOR_PAUSE", Scope.EditorOnly) //Editor only, because ESCAPE works properly in releases
				.AddBinding(Binding.Home),
			new AxisControl("STRAFE")
				.AddBinding(Binding.D, 1f)
				.AddBinding(Binding.RightArrow, 1f)
				.AddBinding(Binding.A, -1f)
				.AddBinding(Binding.LeftArrow, -1f),
			new ActionControl("NOCLIP", Scope.EditorOnly) // Example of using scope for dev-only cheats
				.AddBinding(Binding.F1),
			new ActionControl("BUILD")
				.AddBinding(Binding.B, ModifierFlags.Control | ModifierFlags.Shift) //Modifier example, this binding is CONTROL + SHIFT + B
		});
	}
}
