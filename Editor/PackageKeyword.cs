using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTools.PackageJson
{
	[Serializable]
	internal class PackageKeyword : PackageBase<string>
	{
		[SerializeField]
		private List<string> list = new List<string>();

		protected override int Length => list.Count;
		protected override string LabelName() => "Keywords";

		internal override IEnumerable<string> Stream => list;

		protected override void DrawElement(int i, PackageJsonCreateWindow window)
		{
			list[i] = EditorGUILayout.TextField(list[i]);
		}

		protected override void Remove(int index)
		{
			list.RemoveAt(index);
		}

		protected override void Add()
		{
			list.Add("");
		}
	}
}
