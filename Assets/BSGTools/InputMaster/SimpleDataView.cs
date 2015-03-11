using UnityEngine;
using System.Collections;
using System.Linq;

namespace BSGTools.IO {
	public class SimpleDataView {
		public string identifier { get; set; }
		public Binding[] bindings { get; set; }
		public ModifierFlags[] modifiers { get; set; }

		public SimpleDataView(Control c) {
			this.identifier = c.identifier;

			if(c is ActionControl) {
				var ac = c as ActionControl;
				this.bindings = ac.bindings.Select(b => b.Key).ToArray();
				this.modifiers = ac.bindings.Values.ToArray();
			}
			else if(c is AxisControl) {
				var ax = c as AxisControl;
				this.bindings = ax.bindings.Select(b => b.Key).ToArray();
			}
		}

		// For YAML serialization
		public SimpleDataView() { }
	}
}