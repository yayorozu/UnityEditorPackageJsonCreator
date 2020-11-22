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
		public string Name;
		public string DisplayName;
		public string Version;
		public string Unity;
		public string Description;

		public string AuthorName;
		public string AuthorEMail;
		public string AuthorURL;

		[SerializeField]
		private PackageDepend Depend;
		[SerializeField]
		private PackageSample Sample;
		[SerializeField]
		private PackageKeyword Keyword;

		internal void OnGUI(PackageJsonCreateWindow window)
		{
			if (Depend == null)
				Depend = new PackageDepend();
			if (Sample == null)
				Sample = new PackageSample();
			if (Keyword == null)
				Keyword = new PackageKeyword();

			Name = RegexTextField("Name", Name, "([a-z]|[0-9]|\\.|-)");
			DisplayName = EditorGUILayout.TextField("DisplayName", DisplayName);
			Version = RegexTextField("Version", Version, "([0-9]|\\.)*");
			Unity = RegexTextField("UnityVersion", Unity, "([0-9]|\\.)*");
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Description");
				Description = EditorGUILayout.TextArea(Description);
			}

			GUILayout.Space(10f);
			EditorGUILayout.LabelField("Author");
			using (new EditorGUI.IndentLevelScope())
			{
				AuthorName = EditorGUILayout.TextField("Name", AuthorName);
				AuthorEMail = EditorGUILayout.TextField("Mail", AuthorEMail);
				AuthorURL = EditorGUILayout.TextField("URL", AuthorURL);
			}

			GUILayout.Space(10f);
			Depend.OnGUI(window);
			GUILayout.Space(10f);
			Keyword.OnGUI(window);
			GUILayout.Space(10f);
			Sample.OnGUI(window);
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

		internal string ExportJson()
		{
			var depth = 0;

			var lines = new List<string>();

			const string format1 = "\"{0}\": \"{1}\"";
			const string format2 = "\"{0}\": {{\n";
			const string format3 = "\"{0}\"";
			const string format4 = "\"{0}\": [\n";

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

			string Join(IEnumerable<string> ie)
			{
				depth++;
				var body = string.Join(",\n", ie.Select(i => Indent() + i).ToArray());
				depth--;

				return body;
			}

			depth++;

			if (!string.IsNullOrEmpty(Name))
				AddText(format1, "name", Name);
			if (!string.IsNullOrEmpty(Version))
				AddText(format1, "version", Version);
			if (!string.IsNullOrEmpty(DisplayName))
				AddText(format1, "displayName", DisplayName);
			if (!string.IsNullOrEmpty(Description))
				AddText(format1, "description", Description);
			if (!string.IsNullOrEmpty(Unity))
				AddText(format1, "unity", Unity);

			if (!string.IsNullOrEmpty(AuthorName) ||
			    !string.IsNullOrEmpty(AuthorEMail) ||
			    !string.IsNullOrEmpty(AuthorURL))
			{
				var join = Join(new []
				{
					string.IsNullOrEmpty(AuthorName) ? "" : string.Format(format1, "name", AuthorName),
					string.IsNullOrEmpty(AuthorEMail) ? "" : string.Format(format1, "email", AuthorEMail),
					string.IsNullOrEmpty(AuthorURL) ? "" : string.Format(format1, "url", AuthorURL),
				}.Where(i => !string.IsNullOrEmpty(i)));
				lines.Add(
					Indent() + string.Format(format2, "author") +
					join + "\n" +
					Indent() + "}"
				);
			}

			if (Depend.Stream.Any())
			{
				var body = Join(Depend.Stream.Select(d => string.Format(format1, d.PackageName, d.URI)));

				lines.Add(
					Indent() +
					string.Format(format2, "dependencies") +
					body + "\n" +
					Indent() + "}"
				);
			}

			if (Keyword.Stream.Any())
			{
				var join = Join(Keyword.Stream.Select(k => string.Format(format3, k)));

				lines.Add(
					Indent() + string.Format(format4, "keywords") +
					join + "\n" +
					Indent() + "]"
				);
			}

			if (Sample.Stream.Any())
			{
				var body = Sample.Stream.Select(s =>
				{
					var b = "{\n" + Join(new List<string>
					{
						string.Format(format1, "displayName", s.DisplayName),
						string.Format(format1, "description", s.Description),
						string.Format(format1, "path", s.Path)
					}) + "\n" + Indent() + "}";
					return b;
				});
				var join = Join(body);

				lines.Add(
					Indent() + string.Format(format4, "samples") +
					join + "\n" +
					Indent() + "]"
				);
			}

			return "{\n" + string.Join(",\n", lines) + "\n}";
		}
	}
}

// "author": {
// 	"name": "Yorozu",
// 	"email": "hatch.tech.eng@gmail.com",
// 	"url": "https://github.com/yayorozu"
// },

