/**
Copyright (c) 2014, Michael Notarnicola
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

#if (UNITY_STANDALONE_WIN || UNITY_METRO) && !UNITY_EDITOR_OSX
#define XBOX_ALLOWED
#endif
#if UNITY_4_6
#define NEW_UI
#endif

using System.Collections.Generic;
using System.IO;
using System.Linq;
using BSGTools.IO.XInput;
using UnityEngine;
using YamlDotNet.Serialization;

namespace BSGTools.IO {
	public static class InputMaster {

		#region Fields
		public static List<Control> controls = new List<Control>();

		/// <value>
		/// Are any controls in an active Down state?
		/// </value>
		public static bool anyControlDown { get; internal set; }
		/// <value>
		/// Are any controls in an active Held state?
		/// </value>
		public static bool anyControlHeld { get; internal set; }
		/// <value>
		/// Are any controls in an active Up state?
		/// </value>
		public static bool anyControlUp { get; internal set; }

		public static bool mouseMovementBlocked = false;


		/// <value>
		/// The Mouse X Axis axis name in Unity's Input Manager
		/// </value>
		public static string mouseXAxisName;
		/// <value>
		/// The Mouse X Axis axis value from Unity's native Input system.
		/// </value>
		public static float mouseX { get; internal set; }
		/// <value>
		/// The Mouse X Axis raw axis value from Unity's native Input system.
		/// </value>
		public static float mouseXRaw { get; internal set; }

		/// <value>
		/// The Mouse Y Axis axis name in Unity's Input Manager
		/// </value>
		public static string mouseYAxisName;
		/// <value>
		/// The Mouse Y Axis axis value from Unity's native Input system.
		/// </value>
		public static float mouseY { get; internal set; }
		/// <value>
		/// The Mouse Y Axis raw axis value from Unity's native Input system.
		/// </value>
		public static float mouseYRaw { get; internal set; }

		/// <value>
		/// The MouseWheel Axis axis name in Unity's Input Manager
		/// </value>
		public static string mouseWheelAxisName;
		/// <value>
		/// The MouseWheel Axis axis value from Unity's native Input system.
		/// </value>
		public static float mouseWheel { get; internal set; }
		/// <value>
		/// The MouseWheel Axis raw axis value from Unity's native Input system.
		/// </value>
		public static float mouseWheelRaw { get; internal set; }
		#endregion

		static bool initialized;

		public static void Initialize() {
			if(initialized)
				return;
			var go = new GameObject("INPUT_MASTER");
			var updater = go.AddComponent<InputMasterUpdater>();
			Object.DontDestroyOnLoad(updater);
			initialized = true;
		}

		/// <summary>
		/// Resets all states/values on all controls.
		/// </summary>
		/// <seealso cref="ResetAll(bool)"/>
		public static void ResetAll() {
			foreach(var c in controls)
				c.Reset();
		}

		/// <summary>
		/// Blocks or unblocks all controls.
		/// This has the side effect of resetting all control states.
		/// </summary>
		/// <param name="blocked">To block/unblock.</param>
		/// <seealso cref="Control.blocked"/>
		public static void SetBlockAll(bool blocked) {
			foreach(var c in controls)
				c.blocked = blocked;
		}

		public static T GetControl<T>(string identifier) where T : Control {
			return controls.OfType<T>().FirstOrDefault(c => c.identifier == identifier);
		}

		public static bool TryGetControl<T>(string identifier, out T control) where T : Control {
			control = GetControl<T>(identifier);
			return control != null;
		}

		public static ActionControl GetAction(string identifier) {
			return controls.OfType<ActionControl>().FirstOrDefault(c => c.identifier == identifier);
		}

		public static bool TryGetAction(string identifier, out ActionControl control) {
			control = GetAction(identifier);
			return control != null;
		}

		public static AxisControl GetAxis(string identifier) {
			return controls.OfType<AxisControl>().FirstOrDefault(c => c.identifier == identifier);
		}

		public static bool TryGetAxis(string identifier, out AxisControl control) {
			control = GetAxis(identifier);
			return control != null;
		}
	}
}