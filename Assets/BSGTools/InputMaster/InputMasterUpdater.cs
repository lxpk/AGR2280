#if (UNITY_STANDALONE_WIN || UNITY_METRO) && !UNITY_EDITOR_OSX
#define XBOX_ALLOWED
using BSGTools.IO.XInput;
#endif

using UnityEngine;
using System.Collections;
using IM = BSGTools.IO.InputMaster;

namespace BSGTools.IO {
	public class InputMasterUpdater : MonoBehaviour {

		void Update() {
#if XBOX_ALLOWED
			XInputUtils.UpdateStates();
#endif
			if(InputMaster.mouseMovementBlocked) {
				IM.mouseX = 0f;
				IM.mouseY = 0f;
			}
			else {
				if(!string.IsNullOrEmpty(IM.mouseXAxisName)) {
					IM.mouseX = Input.GetAxis(IM.mouseXAxisName);
					IM.mouseXRaw = Input.GetAxisRaw(IM.mouseXAxisName);
				}
				if(!string.IsNullOrEmpty(IM.mouseYAxisName)) {
					IM.mouseY = Input.GetAxis(IM.mouseYAxisName);
					IM.mouseYRaw = Input.GetAxisRaw(IM.mouseYAxisName);
				}
				if(!string.IsNullOrEmpty(IM.mouseWheelAxisName)) {
					IM.mouseWheel = Input.GetAxis(IM.mouseWheelAxisName);
					IM.mouseWheelRaw = Input.GetAxisRaw(IM.mouseWheelAxisName);
				}
			}

			IM.anyControlDown = false;
			IM.anyControlHeld = false;
			IM.anyControlUp = false;

			if(IM.controls != null)
				foreach(var c in IM.controls)
					UpdateControl(c);
		}


		private static void UpdateControl(Control c) {
			if(CheckScope(c)) {
				c.Update();

				//if((c.down & ControlState.Either) != 0)
				//	anyControlDown = true;
				//if((c.held & ControlState.Either) != 0)
				//	anyControlHeld = true;
				//if((c.up & ControlState.Either) != 0)
				//	anyControlUp = true;
			}
		}

		private static bool CheckScope(Control c) {
			switch(c.scope) {
				case Scope.ReleaseOnly:
					return Application.isEditor == false;
				case Scope.EditorOnly:
					return Application.isEditor;
				default:
					return true;
			}
		}


		private void OnApplicationFocus(bool focused) {
#if XBOX_ALLOWED
			if(XInputUtils.StopVibrateOnAppFocusLost && focused == false)
				XInputUtils.SetVibrationAll(0f);
#endif
		}

		private void OnApplicationPause(bool paused) {
#if XBOX_ALLOWED
			if(XInputUtils.StopVibrateOnAppPause && paused == true)
				XInputUtils.SetVibrationAll(0f);
#endif
		}

		private void OnApplicationQuit() {
#if XBOX_ALLOWED
			XInputUtils.SetVibrationAll(0f);
#endif
			InputMaster.SetBlockAll(false);
		}
	}
}