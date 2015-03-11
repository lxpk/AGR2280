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

using XInputDotNetPure;

#endif

namespace BSGTools.IO.XInput {

	/// <summary>
	/// A static utility class for minimal required updates for <see cref="XboxControl"/>s.
	/// </summary>
	public static class XInputUtils {
#if XBOX_ALLOWED
		public const int MAX_CONTROLLER_COUNT = 4;

		private static GamePadDeadZone[] _controllerDeadZones = new GamePadDeadZone[MAX_CONTROLLER_COUNT];
		private static GamePadState[] _controllerStates = new GamePadState[MAX_CONTROLLER_COUNT];

		/// <value>
		/// The current deadzone settings for each controller.
		/// </value>
		public static GamePadDeadZone[] ControllerDeadZones { get { return _controllerDeadZones; } }

		/// <value>
		/// The latest states of each controller.
		/// </value>
		public static GamePadState[] ControllerStates { get { return _controllerStates; } }

		/// <value>
		/// If true, vibration will automatically stop when the application has lost focus.
		/// </value>
		public static bool StopVibrateOnAppFocusLost { get; set; }

		/// <value>
		/// If true, vibration will automatically stop when the application is paused.
		/// </value>
		public static bool StopVibrateOnAppPause { get; set; }

		static XInputUtils() {
			StopVibrateOnAppFocusLost = true;
			StopVibrateOnAppPause = true;
		}

		/// <summary>
		/// Check if an individual controller is enabled.
		/// </summary>
		public static bool IsConnected(int controller) {
			return ControllerStates[controller].IsConnected;
		}

		/// <summary>
		/// Turn on vibration for both motors for a single controller.
		/// </summary>
		public static void SetVibration(int controller, float lrVib) {
			SetVibration(controller, lrVib, lrVib);
		}

		/// <summary>
		/// Turn on individual motor vibration for a single controller.
		/// </summary>
		public static void SetVibration(int controller, float lVib, float rVib) {
			GamePad.SetVibration((PlayerIndex)controller, lVib, rVib);
		}

		/// <summary>
		/// Turn on vibration for both motors for each controller.
		/// </summary>
		/// <param name="lrVib"></param>
		public static void SetVibrationAll(float lrVib) {
			SetVibrationAll(lrVib, lrVib);
		}

		/// <summary>
		/// Turn on individual motor vibration for each controller.
		/// </summary>
		public static void SetVibrationAll(float lVib, float rVib) {
			for(int i = 0;i < MAX_CONTROLLER_COUNT;i++)
				SetVibration(i, lVib, rVib);
		}

		/// <summary>
		/// Called by <see cref="InputMaster"/> to update the state of each controller.
		/// </summary>
		internal static void UpdateStates() {
			for(int i = 0;i < MAX_CONTROLLER_COUNT;i++) {
				var pi = (PlayerIndex)i;
				ControllerStates[i] = GamePad.GetState(pi, ControllerDeadZones[i]);
			}
		}

#endif
	}
}