#if (UNITY_STANDALONE_WIN || UNITY_METRO) && !UNITY_EDITOR_OSX
#define XBOX_ALLOWED
#endif

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BSGTools.IO {
	/// <summary>
	/// Used for all Keyboard and Mouse Button controls.
	/// Any binding present in Unity's KeyCode enumeration is valid here.
	/// </summary>
	[Serializable]
	public sealed class AxisControl : Control {
		public Dictionary<Binding, float> bindings = new Dictionary<Binding, float>();
		/// <value>
		/// Returns an analog representation of the current real value.
		/// This is functionally identical to calling Input.GetAxis() from Unity's native Input system.
		/// </value>
		public float rValue { get; protected set; }
		/// <value>
		/// The -1...1 clamped value.
		/// </value>
		public float value { get; protected set; }


		internal AxisControl()
			: base() { }
		public AxisControl(string identifier)
			: base(identifier) { }
		public AxisControl(string identifier, Scope scope)
			: base(identifier, scope) { }
		public AxisControl(string identifier, byte controllerIndex)
			: this(identifier, Scope.All, controllerIndex) { }
		public AxisControl(string identifier, Scope scope, byte controllerIndex)
			: base(identifier, scope, controllerIndex) { }

		public AxisControl AddBinding(Binding b, float multiplier) {
			bindings.Add(b, multiplier);
			return this;
		}

		/// <summary>
		/// Updates the current states of this control.
		/// </summary>
		protected override void UpdateValues() {
			foreach(var b in bindings) {
				if(BindingUtils.IsKeyCode(b.Key))
					rValue += BindingUtils.GetKCValue(b.Key) ? b.Value : 0f;
#if XBOX_ALLOWED
				else
					rValue += BindingUtils.GetXInputValue(b.Key, gpState) * b.Value;
#endif
			}
			value = Mathf.Clamp(rValue, -1f, 1f);
		}

		protected override void ResetControl() {
			value = rValue = 0f;
		}
	}
}