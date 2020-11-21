using System;
using UnityEditor;

namespace Yorozu.EditorTools.PackageJson
{
	[Serializable]
	internal class PackageKeyword : PackageBase<string>
	{
		protected override string LabelName() => "Keywords";

		protected override void DrawElement(int i)
		{
			list[i] = EditorGUILayout.TextField(list[i]);
		}

		protected override string NewT() => string.Empty;
	}
}
