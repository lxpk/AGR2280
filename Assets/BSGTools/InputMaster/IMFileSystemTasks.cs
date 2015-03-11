using UnityEngine;
using System.Collections;
using System.IO;
using YamlDotNet.Serialization;
using System.Linq;

namespace BSGTools.IO {
	public static class IMFileSystemTasks {

		public static SimpleDataView[] ReadControls(string cfgPath) {
			SimpleDataView[] views = null;
			var d = new Deserializer();
			using(var reader = new StreamReader(cfgPath)) {
				views = d.Deserialize<SimpleDataView[]>(reader);
			}
			return views;
		}

		public static void WriteControls(string cfgPath) {
			var s = new Serializer(SerializationOptions.EmitDefaults);
			var graph = InputMaster.controls.Where(c => c.scope != Scope.EditorOnly).Select(c => c.GetSimpleDataView()).ToArray();
			using(var writer = new StreamWriter(cfgPath)) {
				writer.AutoFlush = true;
				s.Serialize(writer, graph);
			}
		}

	}
}