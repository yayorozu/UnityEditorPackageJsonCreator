using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTools.PackageJson
{
	[Serializable]
	internal class PackageSample : PackageBase<SampleData>
	{
		protected override string LabelName() => "Samples";

		protected override void DrawElement(int i)
		{
			list[i].DisplayName = EditorGUILayout.TextField("DisplayName", list[i].DisplayName);
			list[i].Description = EditorGUILayout.TextField("Description", list[i].Description);
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Path");
				EditorGUILayout.LabelField(list[i].Path);
				if (GUILayout.Button("Set Path"))
				{
					var path = EditorUtility.SaveFolderPanel("Select Save Folder", Data.CreatePath, "");
					if (!string.IsNullOrEmpty(path))
						list[i].Path = path.Replace(Application.dataPath, "Assets").Replace(Data.CreatePath + "/", "");
				}
			}
		}

		protected override SampleData NewT() => new SampleData();
	}

	[Serializable]
	internal class SampleData
	{
		public string DisplayName;
		public string Description;
		public string Path;
	}
}
