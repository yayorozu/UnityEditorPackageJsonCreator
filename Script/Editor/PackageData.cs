using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTools.PackageJson
{
	[Serializable]
	internal class PackageData
	{
		public string CreatePath;
		public string Name;
		public string DisplayName;
		public string Version;
		public string Unity;
		public string Description;

		[SerializeField]
		private PackageDepend Depend;
		[SerializeField]
		private PackageSample Sample;
		[SerializeField]
		private PackageKeyword Keyword;

		internal void OnGUI()
		{
			if (Depend == null)
				Depend = new PackageDepend {Data = this};
			if (Sample == null)
				Sample = new PackageSample {Data = this};
			if (Keyword == null)
				Keyword = new PackageKeyword {Data = this};

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("JsonCreatePath");
				EditorGUILayout.LabelField(CreatePath);
				if (GUILayout.Button("Set Path"))
				{
					var path = EditorUtility.SaveFolderPanel("Select Save Folder", "Assets", "");
					if (!string.IsNullOrEmpty(path))
						CreatePath = path.Replace(Application.dataPath, "Assets");
				}
			}

			Name = RegexTextField("Name", Name, "([a-z]|[A-Z]|[0-9]|\\.|-)");
			DisplayName = EditorGUILayout.TextField("DisplayName", DisplayName);
			Version = RegexTextField("Version", Version, "([0-9]|\\.)*");
			Unity = RegexTextField("UnityVersion", Unity, "([0-9]|\\.)*");
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Description");
				Description = EditorGUILayout.TextArea(Description);
			}

			GUILayout.Space(10f);
			Depend.OnGUI();
			GUILayout.Space(10f);
			Keyword.OnGUI();
			GUILayout.Space(10f);
			Sample.OnGUI();

			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Save"))
			{

			}
		}

		private string RegexTextField(string label, string value, string regex)
		{
			using (var check = new EditorGUI.ChangeCheckScope())
			{
				var t = EditorGUILayout.DelayedTextField(label, value);
				if (check.changed)
				{
					var builder = new StringBuilder();
					foreach (Match m in Regex.Matches(t, regex))
						builder.Append(m.Value);

					return builder.ToString();
				}
			}

			return value;
		}

		/// <summary>
		/// ちゃんとしたクラスにしたい
		/// </summary>
		private void ExportJson()
		{
			var builder = new StringBuilder();
			var depth = 0;

			var lines = new List<string>();

			const string format = "\"{0}\": \"{1}\"";
			const string format2 = "\"{0}\": {\n";
			const string format3 = "\"{0}\"";
			const string format4 = "\"{0}\": [";

			string Indent()
			{
				var ind = "";
				for (var i = 0; i < depth; i++)
					ind += "  ";

				return ind;
			}

			void AddText(string fmt, params object[] ps)
			{
				lines.Add(Indent() + string.Format(fmt, ps));
			}

			depth++;

			AddText(format, "name", Name);
			AddText(format, "version", Version);
			AddText(format, "displayName", DisplayName);
			AddText(format, "description", Description);
			AddText(format, "unity", Unity);

			if (Depend.List.Any())
			{
				depth++;
				var join = string.Join(",\n", Depend.List.Select(d => Indent() + string.Format(format, d.PackageName, d.URI)).ToArray());
				depth--;
				lines.Add(
					Indent() + string.Format(format2, "dependencies") +
					join + "\n" +
					"}"
					);
			}

			if (Keyword.List.Any())
			{
				depth++;
				var join = string.Join(",\n", Keyword.List.Select(k => Indent() + string.Format(format3, k)).ToArray());
				depth--;
				lines.Add(
					Indent() + string.Format(format4, "keywords") +
					join + "\n" +
					"]"
				);
			}

			if (Sample.List.Any())
			{
				depth++;
				var join = string.Join(",\n", Sample.List.Select(s =>
				{
					depth++;
					return Indent() + string.Format(format, "displayName", s.DisplayName) + ",\n" +
					       Indent() + string.Format(format, "description", s.Description) + ",\n" +
					       Indent() + string.Format(format, "path", s.Path) + "\n";
					depth++;
				}).ToArray());
				depth--;
			}
		}
	}
}

// "author": {
// 	"name": "Yorozu",
// 	"email": "hatch.tech.eng@gmail.com",
// 	"url": "https://github.com/yayorozu"
// },
// "samples": [
// {
// 	"displayName": "Sample",
// 	"description": "Simple",
// 	"path": "Samples/Sample1"
// }
// ]
// }
//
