using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Yorozu.EditorTools.PackageJson
{
	[Serializable]
	internal class PackageDepend : PackageBase<DependData>
	{
		private List<DependData> list = new List<DependData>();
		protected override int Length => list.Count;
		protected override string LabelName() => "Dependency";

		internal override IEnumerable<DependData> Stream => list;

		protected override void DrawElement(int i, PackageJsonCreateWindow window)
		{
			list[i].PackageName = EditorGUILayout.TextField("PackageName", list[i].PackageName);
			list[i].URI = EditorGUILayout.TextField("URI", list[i].URI);
		}

		protected override void Remove(int index)
		{
			list.RemoveAt(index);
		}

		protected override void Add()
		{
			list.Add(new DependData());
		}

		internal string Body(string indent, string format)
		{
			var array = list.Select(d => indent + string.Format(format, d.PackageName, d.URI)).ToArray();
			return string.Join(",\n", array);
		}
	}

	[Serializable]
	internal class DependData
	{
		public string PackageName;
		public string URI;
	}
}
