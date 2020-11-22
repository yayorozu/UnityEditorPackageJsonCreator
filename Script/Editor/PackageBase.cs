using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTools.PackageJson
{
	[Serializable]
	internal abstract class PackageBase<T>
	{
		internal abstract IEnumerable<T> Stream { get; }

		private static GUIContent plus;
		private static GUIContent minus;
		private static GUIStyle button;
		private static GUILayoutOption width;

		protected abstract int Length { get; }


		internal void OnGUI(PackageJsonCreateWindow window)
		{
			if (plus == null)
				plus = EditorGUIUtility.TrIconContent("Toolbar Plus");
			if (minus == null)
				minus = EditorGUIUtility.TrIconContent("Toolbar minus");
			if (button == null)
				button = "RL FooterButton";
			if (width == null)
				width = GUILayout.Width(20);

			using (new GUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField(LabelName());
				GUILayout.FlexibleSpace();
				if (GUILayout.Button(plus, button, width))
					Add();
			}

			if (Length <= 0)
				return;

			using (new EditorGUILayout.VerticalScope("box"))
			{
				for (var i = 0; i < Length; i++)
				{
					using (new EditorGUILayout.VerticalScope("box"))
					{
						using (new GUILayout.HorizontalScope())
						{
							EditorGUILayout.LabelField(i.ToString(), width);
							using (new EditorGUILayout.VerticalScope())
							{
								DrawElement(i, window);
							}
							if (GUILayout.Button("x", button, width))
							{
								Remove(i);
								GUIUtility.ExitGUI();
							}
						}
					}
				}
			}
		}

		protected abstract string LabelName();
		protected abstract void DrawElement(int index, PackageJsonCreateWindow window);
		protected abstract void Remove(int index);
		protected abstract void Add();
	}
}
