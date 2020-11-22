using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTools.PackageJson
{
	public class PackageJsonCreateWindow : EditorWindow
	{
		[MenuItem("Tools/PackageJsonCreate")]
		private static void ShowWindow()
		{
			var window = GetWindow<PackageJsonCreateWindow>("PackageJson");
			window.Show();
		}

		[SerializeField]
		private PackageData _data;

		[SerializeField]
		private string _createPath;
		internal string CreatePath => _createPath;

		private void OnGUI()
		{
			if (_data == null)
				_data = new PackageData();

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("JsonCreatePath");
				EditorGUILayout.LabelField(_createPath);
				if (GUILayout.Button("Set Path"))
				{
					var path = EditorUtility.SaveFolderPanel("Select Save Folder", "Assets", "");
					if (!string.IsNullOrEmpty(path))
					{
						if (path.StartsWith(Application.dataPath))
						{
							_createPath = path.Replace(Application.dataPath, "Assets");
						}
					}
				}
			}

			_data.OnGUI(this);

			GUILayout.FlexibleSpace();
			using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_createPath)))
			{
				if (GUILayout.Button("Save"))
				{
					var savePath = _createPath + "/package.json";
					try
					{
						System.IO.File.WriteAllLines(savePath, _data.ExportJson().Split('\n'));
						EditorUtility.DisplayDialog("Info", $"Create package.json to {savePath}", "ok");
						AssetDatabase.Refresh();
					}
					catch (Exception e)
					{
						EditorUtility.DisplayDialog("Error", e.Message, "ok");
					}
				}
			}
		}
	}
}
