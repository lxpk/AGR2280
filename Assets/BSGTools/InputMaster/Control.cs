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
using System;
using System.Linq;
using BSGTools.IO.XInput;
using XInputDotNetPure;


namespace BSGTools.IO {
	public enum Scope {
		All,
		ReleaseOnly,
		EditorOnly
	}

	[Serializable]
	public abstract class Control {
		/// <value>
		/// A required, unique identifiers.
		/// </value>
		public string identifier = "new_" + Guid.NewGuid().ToString().ToUpper().Split('-')[0];

		/// <value>
		/// This is used to block any control from receiving updates.
		/// Keep in mind that if you block a control, it maintains its values from it's most recent update.
		/// If you want to block and reset a control, you can use the <see cref="Reset(bool)"/> function.
		/// </value>
		[NonSerialized]
		public bool blocked = false;

		/// <value>
		/// Used to specify controls that automatically
		/// only work in the Editor or in Debug builds.
		/// </value>
		public Scope scope = Scope.All;

		public byte controllerIndex = 0;

		protected GamePadState gpState { get; private set; }

		internal Control() {
			Reset();
		}

		public Control(string identifier)
			: this(identifier, Scope.All, 0) { }
		public Control(string identifier, Scope scope)
			: this(identifier, scope, 0) { }
		public Control(string identifier, byte controllerIndex)
			: this(identifier, Scope.All, controllerIndex) { }
		public Control(string identifier, Scope scope, byte controllerIndex) {
			this.identifier = identifier;
			this.scope = scope;
			this.controllerIndex = controllerIndex;
		}

		/// <summary>
		/// Reset all non-configuration values and states for this control.
		/// </summary>
		/// <seealso cref="Reset(bool)"/>
		public void Reset() {
			Reset(false);
		}

		/// <summary>
		/// Reset all non-configuration values for this control. This is the best method to use for cutscenes.
		/// </summary>
		/// <param name="block">Whether or not to block this input after resetting.</param>
		/// <seealso cref="Reset"/>
		///	<seealso cref="blocked"/>
		public void Reset(bool block) {
			blocked = block;
			ResetControl();
		}

		protected abstract void ResetControl();

		/// <summary>
		/// Updates the control.
		/// This should never be used by any user-made script.
		/// This is public specifically for the use of <see cref="InputMaster"/>.
		/// </summary>
		public void Update() {
			Reset();
			if(blocked)
				return;

#if XBOX_ALLOWED
			gpState = XInputUtils.ControllerStates[controllerIndex];
#endif
			UpdateValues();

		}

		public SimpleDataView GetSimpleDataView() {
			return new SimpleDataView(this);
		}

		/// <summary>
		/// Internally used for updating the Up/Held/Down states of a control.
		/// </summary>
		protected abstract void UpdateValues();

		/// <summary>
		/// Internally used to give a random name to a control if one is not provided.
		/// </summary>
		/// <returns>A new random string.</returns>
		private static string GetRandomName() {
			return "UNNAMED_" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
		}
	}
}