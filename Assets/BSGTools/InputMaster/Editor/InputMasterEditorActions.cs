using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace BSGTools.IO.Editor {
	public class InputMasterEditorActions : UnityEditor.Editor {
		public static readonly string CONTROL_INITIALIZER_TEMPLATE_PATH = Application.dataPath + "/BSGTools/InputMaster/Templates/ControlInitializerTemplate.txt";

		[MenuItem("Assets/Create/BSGTools/Control Initializer")]
		public static void CreateControlInitializer() {
			var newPath = Application.dataPath + "/InitializeControls.cs";
			File.Copy(CONTROL_INITIALIZER_TEMPLATE_PATH, newPath);
			AssetDatabase.Refresh();
			var newObj = AssetDatabase.LoadAssetAtPath(newPath, typeof(MonoBehaviour));
			Selection.activeObject = newObj;
		}
	}
}