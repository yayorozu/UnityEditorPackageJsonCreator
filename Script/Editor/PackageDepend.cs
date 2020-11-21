using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTools.PackageJson
{
	[Serializable]
	internal class PackageDepend : PackageBase<DependData>
	{
		protected override string LabelName() => "Dependency";

		protected override void DrawElement(int i)
		{
			list[i].PackageName = EditorGUILayout.TextField("PackageName", list[i].PackageName);
			list[i].URI = EditorGUILayout.TextField("URI", list[i].URI);
		}

		protected override DependData NewT() => new DependData();
	}

	[Serializable]
	internal class DependData
	{
		public string PackageName;
		public string URI;
	}


	[Serializable]
	internal class PackageDepend<TKey, TValue> : ISerializationCallbackReceiver
	{
		[SerializeField]
		List<TKey> keys;
		[SerializeField]
		List<TValue> values;

		Dictionary<TKey, TValue> target;
		public Dictionary<TKey, TValue> ToDictionary() { return target; }

		public Serialization(Dictionary<TKey, TValue> target)
		{
			this.target = target;
		}

		public void OnBeforeSerialize()
		{
			keys = new List<TKey>(target.Keys);
			values = new List<TValue>(target.Values);
		}

		public void OnAfterDeserialize()
		{
			var count = Math.Min(keys.Count, values.Count);
			target = new Dictionary<TKey, TValue>(count);
			for (var i = 0; i < count; ++i)
			{
				target.Add(keys[i], values[i]);
			}
		}
	}
}
