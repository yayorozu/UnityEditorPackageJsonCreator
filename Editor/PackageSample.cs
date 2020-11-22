using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTools.PackageJson
{
	[Serializable]
	internal class PackageSample : PackageBase<SampleData>
	{
		[SerializeField]
		private List<SampleData> list = new List<SampleData>();

		protected override int Length => list.Count;
		protected override string LabelName() => "Samples";

		internal override IEnumerable<SampleData> Stream => list;

		protected override void DrawElement(int i, PackageJsonCreateWindow window)
		{
			using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(window.CreatePath)))
			{
				list[i].DisplayName = EditorGUILayout.TextField("DisplayName", list[i].DisplayName);
				list[i].Description = EditorGUILayout.TextField("Description", list[i].Description);
				using (new EditorGUILayout.HorizontalScope())
				{
					EditorGUILayout.PrefixLabel("Path");
					EditorGUILayout.LabelField(list[i].Path);

					if (GUILayout.Button("Set Path"))
					{
						var path = EditorUtility.SaveFolderPanel("Select Save Folder", window.CreatePath, "");
						if (!string.IsNullOrEmpty(path))
						{
							if (path.StartsWith(Application.dataPath))
							{
								var pp = path.Replace(Application.dataPath, "Assets");
								if (pp.StartsWith(window.CreatePath))
								{
									list[i].Path = pp.Replace(window.CreatePath + "/", "");
								}
							}
						}
					}
				}
			}
		}

		protected override void Remove(int index)
		{
			list.RemoveAt(index);
		}

		protected override void Add()
		{
			list.Add(new SampleData());
		}
	}

	[Serializable]
	internal class SampleData
	{
		public string DisplayName;
		public string Description;
		public string Path;
	}
}
