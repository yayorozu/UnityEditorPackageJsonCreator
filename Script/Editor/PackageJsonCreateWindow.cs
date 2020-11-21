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

		private void OnGUI()
		{
			if (_data == null)
				_data = new PackageData();

			_data.OnGUI();


		}
	}
}
