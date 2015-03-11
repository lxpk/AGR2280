#if (UNITY_STANDALONE_WIN || UNITY_METRO) && !UNITY_EDITOR_OSX
#define XBOX_ALLOWED
#endif

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BSGTools.IO {

	public enum State {
		None,
		Down,
		Up,
		Held
	}

	[Flags]
	public enum ModifierFlags {
		None = 0,
		Control = 1,
		Alt = 2,
		Shift = 4
	}

	/// <summary>
	/// Used for all Keyboard and Mouse Button controls.
	/// Any binding present in Unity's KeyCode enumeration is valid here.
	/// </summary>
	[Serializable]
	public sealed class ActionControl : Control {
		internal Dictionary<Binding, ModifierFlags> bindings = new Dictionary<Binding, ModifierFlags>();

		public State state { get; private set; }
		State previousState;

		internal ActionControl()
			: base() { }
		public ActionControl(string identifier)
			: base(identifier) { }
		public ActionControl(string identifier, Scope scope)
			: base(identifier, scope) { }
		public ActionControl(string identifier, byte controllerIndex)
			: this(identifier, Scope.All, controllerIndex) { }
		public ActionControl(string identifier, Scope scope, byte controllerIndex)
			: base(identifier, scope, controllerIndex) { }

		public ActionControl AddBinding(Binding b) {
			return AddBinding(b, ModifierFlags.None);
		}

		public ActionControl AddBinding(Binding b, ModifierFlags flags) {
			bindings.Add(b, flags);
			return this;
		}

		/// <summary>
		/// Updates the current states of this control.
		/// </summary>
		protected override void UpdateValues() {
			var value = 0f;
			foreach(var b in bindings) {
				if((b.Value & ModifierFlags.Control) != 0 && !Input.GetKey(KeyCode.LeftControl))
					continue;
				if((b.Value & ModifierFlags.Alt) != 0 && !Input.GetKey(KeyCode.LeftAlt))
					continue;
				if((b.Value & ModifierFlags.Shift) != 0 && !Input.GetKey(KeyCode.LeftShift))
					continue;

				if(BindingUtils.IsKeyCode(b.Key))
					value += BindingUtils.GetKCValue(b.Key) ? 1f : 0f;
#if XBOX_ALLOWED
				else
					value += BindingUtils.GetXInputValue(b.Key, gpState);
#endif
			}

			if(previousState == State.None && value != 0f)
				state = State.Down;
			else if((previousState == State.Held || previousState == State.Down) && value == 0f)
				state = State.Up;
			else if(value != 0f)
				state = State.Held;
			else if(value == 0f)
				state = State.None;

			previousState = state;
		}

		protected override void ResetControl() {
			state = State.None;
		}
	}
}