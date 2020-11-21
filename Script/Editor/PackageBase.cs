using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTools.PackageJson
{
	[Serializable]
	internal abstract class PackageBase<T>
	{
		[SerializeField]
		internal PackageData Data;

		[SerializeField]
		protected List<T> list = new List<T>();

		internal List<T> List => List;

		private static GUIContent plus;
		private static GUIContent minus;
		private static GUIStyle button;
		private static GUILayoutOption width;

		internal void OnGUI()
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
					list.Add(NewT());
			}

			if (list.Count <= 0)
				return;

			using (new EditorGUILayout.VerticalScope("box"))
			{
				for (var i = 0; i < list.Count; i++)
				{

					using (new EditorGUILayout.VerticalScope("box"))
					{
						using (new GUILayout.HorizontalScope())
						{
							EditorGUILayout.LabelField(i.ToString(), width);
							using (new EditorGUILayout.VerticalScope())
							{
								DrawElement(i);
							}
							if (GUILayout.Button("x", button, width))
							{
								list.RemoveAt(i);
								GUIUtility.ExitGUI();
							}
						}
					}
				}
			}
		}

		protected abstract string LabelName();

		protected abstract void DrawElement(int index);

		protected abstract T NewT();
	}
}
